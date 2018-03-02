#!flask/bin/python

import errno
import logging
import os

from flask import Flask
from flask_cors import CORS
from flask import Flask, jsonify, request, make_response, abort
from flask_cors import CORS, cross_origin
import mysql.connector
from mysql.connector import errorcode

import lib.therowantree_config as config

# https://stackoverflow.com/questions/273192/how-can-i-create-a-directory-if-it-does-not-exist
def make_sure_path_exists(path):
    try:
        if os.path.exists(path) is False:
            os.makedirs(path)
    except OSError as exception:
        if exception.errno != errno.EEXIST:
            raise

# Setup logging.
make_sure_path_exists(config.LOGS_DIR)
logging.basicConfig(
    format='%(asctime)s - %(levelname)s - %(message)s',
    datefmt='%m/%d/%Y %I:%M:%S %p',
    level=logging.DEBUG,
    filemode='w',
    filename="%s/%s.therowantree.api.log" % (config.LOGS_DIR, os.uname()[1])
)


app = Flask(__name__)
# cors = CORS(app, resources={r"/api/*": {"origins": "http://localhost:*"}})
cors = CORS(app, resources={r"/api/*": {"origins": "*"}})


@app.route('/api/version', methods=['GET'])
@cross_origin()
def make_api_version_public():
    return make_response(jsonify({'version':  str(config.API_VERSION)}), 201)


@app.route('/health/plain', methods=['GET'])
@cross_origin()
def make_health_plain_public():
    return make_response('true', 201)


@app.route('/api/user/active/state', methods=['POST'])
@cross_origin()
def make_user_active_state_public():
    if request.headers['API-ACCESS-KEY'] != config.API_ACCESS_KEY:
        logging.debug('bad access key')
        abort(401)
    if request.headers['API-VERSION'] != config.API_VERSION:
        logging.debug('bad access version')
        abort(400)

    if not request.json:
        abort(400)
    if 'guid' in request.json and type(request.json['guid']) != unicode:
        abort(400)

    guid = request.json.get('guid')
    args = [guid, 0]

    try:
        cnx = mysql.connector.connect(user=config.API_DATABASE_USERNAME, password=config.API_DATABASE_PASSWORD,
                                      host=config.API_DATABASE_SERVER,
                                      database=config.API_DATABASE_NAME,
                                      use_pure=False)
        cursor = cnx.cursor()
        result_args = cursor.callproc('getUserActivityStateByGUID', args)
        cursor.close()
        cnx.close()
    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()

    if result_args[1] is None:
        result = 0
    else:
        result = result_args[1]

    return make_response(jsonify({'active': result}), 201)


@app.route('/api/user/active/set', methods=['POST'])
@cross_origin()
def make_user_active_state_set_public():
    if request.headers['API-ACCESS-KEY'] != config.API_ACCESS_KEY:
        logging.debug('bad access key')
        abort(401)
    if request.headers['API-VERSION'] != config.API_VERSION:
        logging.debug('bad access version')
        abort(400)
    if not request.json:
        abort(400)
    if 'guid' in request.json and type(request.json['guid']) != unicode:
        abort(400)
    if 'active' not in request.json:
        abort(400)

    guid = request.json.get('guid')
    active = int(request.json.get('active'))
    args = [guid,]

    if active == 1:
        proc = 'setUserActiveByGUID'
    elif active == 0:
        proc = 'setUserInactiveByGUID'
    else:
        proc = None

    if proc is not None:
        try:
            cnx = mysql.connector.connect(user=config.API_DATABASE_USERNAME, password=config.API_DATABASE_PASSWORD,
                                          host=config.API_DATABASE_SERVER,
                                          database=config.API_DATABASE_NAME,
                                          use_pure=False)
            cursor = cnx.cursor()
            cursor.callproc(proc, args)
            cursor.close()
            cnx.close()
        except mysql.connector.Error as err:
            if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
                logging.debug("Something is wrong with your user name or password")
            elif err.errno == errorcode.ER_BAD_DB_ERROR:
                logging.debug("Database does not exist")
            else:
                logging.debug(err)
        else:
            cnx.close()
    return ('', 201)


@app.route('/api/user/stores', methods=['POST'])
@cross_origin()
def make_user_stores_public():
    if request.headers['API-ACCESS-KEY'] != config.API_ACCESS_KEY:
        logging.debug('bad access key')
        abort(401)
    if request.headers['API-VERSION'] != config.API_VERSION:
        logging.debug('bad access version')
        abort(400)
    if not request.json:
        abort(400)
    if 'guid' in request.json and type(request.json['guid']) != unicode:
        abort(400)

    guid = request.json.get('guid')
    args = [guid, ]
    stores_obj = {}
    user_stores = {}

    try:
        cnx = mysql.connector.connect(user=config.API_DATABASE_USERNAME, password=config.API_DATABASE_PASSWORD,
                                      host=config.API_DATABASE_SERVER,
                                      database=config.API_DATABASE_NAME,
                                      use_pure=False)
        cursor = cnx.cursor()
        cursor.callproc('getUserStoresByGUID', args)
        for result in cursor.stored_results():
            user_stores = result.fetchall()
        cursor.close()
        cnx.close()
    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()

    for result in user_stores:
        stores_obj[result[0]] = { 'amount': result[2], 'description': result[1] }

    return_results = {'stores': stores_obj}
    return (jsonify(return_results), 201)


@app.route('/api/user/income', methods=['POST'])
@cross_origin()
def make_user_income_public():
    if request.headers['API-ACCESS-KEY'] != config.API_ACCESS_KEY:
        logging.debug('bad access key')
        abort(401)
    if request.headers['API-VERSION'] != config.API_VERSION:
        logging.debug('bad access version')
        abort(400)
    if not request.json:
        abort(400)
    if 'guid' in request.json and type(request.json['guid']) != unicode:
        abort(400)

    guid = request.json.get('guid')
    args = [guid,]
    incomes_obj = {}
    user_incomes = {}

    try:
        cnx = mysql.connector.connect(user=config.API_DATABASE_USERNAME, password=config.API_DATABASE_PASSWORD,
                                      host=config.API_DATABASE_SERVER,
                                      database=config.API_DATABASE_NAME,
                                      use_pure=False)
        cursor = cnx.cursor()
        cursor.callproc('getUserIncomeByGUID', args)
        for result in cursor.stored_results():
            user_incomes = result.fetchall()
        cursor.close()
        cnx.close()

    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()

    for result in user_incomes:
        incomes_obj[result[1]] = { 'amount': result[0], 'description': result[2] }

    return_results = { 'income': incomes_obj }
    return (jsonify(return_results), 201)


@app.route('/api/user/income/set', methods=['POST'])
@cross_origin()
def make_user_income_set_public():
    if request.headers['API-ACCESS-KEY'] != config.API_ACCESS_KEY:
        logging.debug('bad access key')
        abort(401)
    if request.headers['API-VERSION'] != config.API_VERSION:
        logging.debug('bad access version')
        abort(400)
    if not request.json:
        abort(400)
    if 'guid' in request.json and type(request.json['guid']) != unicode:
        abort(400)
    if 'income_source_name' in request.json and type(request.json['income_source_name']) != unicode:
        abort(400)
    if 'amount' not in request.json:
        abort(400)

    guid = request.json.get('guid')
    income_source_name = request.json.get('income_source_name')
    amount = int(request.json.get('amount'))
    args = [guid, income_source_name, amount]

    try:
        cnx = mysql.connector.connect(user=config.API_DATABASE_USERNAME, password=config.API_DATABASE_PASSWORD,
                                      host=config.API_DATABASE_SERVER,
                                      database=config.API_DATABASE_NAME,
                                      use_pure=False)
        cursor = cnx.cursor()
        cursor.callproc('deltaUserIncomeByNameAndGUID', args)
        cursor.close()
        cnx.close()
    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()

    return ('', 201)


@app.route('/api/user/create', methods=['GET'])
@cross_origin()
def make_user_create_public():
    if request.headers['API-ACCESS-KEY'] != config.API_ACCESS_KEY:
        logging.debug('bad access key')
        abort(401)
    if request.headers['API-VERSION'] != config.API_VERSION:
        logging.debug('bad access version')
        abort(400)

    user_guid = {}
    output = {}
    try:
        cnx = mysql.connector.connect(user=config.API_DATABASE_USERNAME, password=config.API_DATABASE_PASSWORD,
                                      host=config.API_DATABASE_SERVER,
                                      database=config.API_DATABASE_NAME,
                                      use_pure=False)
        cursor = cnx.cursor()
        cursor.callproc('createUser')
        for result in cursor.stored_results():
            user_guid = result.fetchall()
        cursor.close()
        cnx.close()
        output = {'guid': user_guid[0][0]}

    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()

    return (jsonify(output), 201)


@app.route('/api/user/feature', methods=['POST'])
@cross_origin()
def make_user_feature_public():
    if request.headers['API-ACCESS-KEY'] != config.API_ACCESS_KEY:
        logging.debug('bad access key')
        abort(401)
    if request.headers['API-VERSION'] != config.API_VERSION:
        logging.debug('bad access version')
        abort(400)
    if not request.json:
        abort(400)
    if 'guid' in request.json and type(request.json['guid']) != unicode:
        abort(400)

    guid = request.json.get('guid')
    args = [guid, ]
    user_features = {}
    feature_list = []
    output = {}
    try:
        cnx = mysql.connector.connect(user=config.API_DATABASE_USERNAME, password=config.API_DATABASE_PASSWORD,
                                      host=config.API_DATABASE_SERVER,
                                      database=config.API_DATABASE_NAME,
                                      use_pure=False)
        cursor = cnx.cursor()
        cursor.callproc('getUserFeaturesByGUID', args)
        for result in cursor.stored_results():
            user_features = result.fetchall()
        cursor.close()
        cnx.close()
        for feature in user_features:
            feature_list.append(feature[0])
        output = {'features': feature_list}

    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()

    return (jsonify(output), 201)


@app.route('/api/user/population', methods=['POST'])
@cross_origin()
def make_user_population_public():
    if request.headers['API-ACCESS-KEY'] != config.API_ACCESS_KEY:
        logging.debug('bad access key')
        abort(401)
    if request.headers['API-VERSION'] != config.API_VERSION:
        logging.debug('bad access version')
        abort(400)
    if not request.json:
        abort(400)
    if 'guid' in request.json and type(request.json['guid']) != unicode:
        abort(400)

    guid = request.json.get('guid')
    args = [guid,]
    population_obj = {}
    user_population = 0

    try:
        cnx = mysql.connector.connect(user=config.API_DATABASE_USERNAME, password=config.API_DATABASE_PASSWORD,
                                      host=config.API_DATABASE_SERVER,
                                      database=config.API_DATABASE_NAME,
                                      use_pure=False)
        cursor = cnx.cursor()
        cursor.callproc('getUserPopulationByGUID', args)
        for result in cursor.stored_results():
            user_population = result.fetchall()
        cursor.close()
        cnx.close()
        if not user_population:
            user_population = 0
        population_obj = {'population': user_population}

    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()

    return (jsonify(population_obj), 201)


@app.route('/api/user/active/feature', methods=['POST'])
@cross_origin()
def make_user_active_feature_public():
    if request.headers['API-ACCESS-KEY'] != config.API_ACCESS_KEY:
        logging.debug('bad access key')
        abort(401)
    if request.headers['API-VERSION'] != config.API_VERSION:
        logging.debug('bad access version')
        abort(400)
    if not request.json:
        abort(400)
    if 'guid' in request.json and type(request.json['guid']) != unicode:
        abort(400)

    guid = request.json.get('guid')
    args = [guid, ]
    user_active_feature = {}
    output = {}
    try:
        cnx = mysql.connector.connect(user=config.API_DATABASE_USERNAME, password=config.API_DATABASE_PASSWORD,
                                      host=config.API_DATABASE_SERVER,
                                      database=config.API_DATABASE_NAME,
                                      use_pure=False)
        cursor = cnx.cursor()
        cursor.callproc('getUserActiveFeatureByGUID', args)
        for result in cursor.stored_results():
            user_active_feature = result.fetchall()
        cursor.close()
        cnx.close()
        output = {'active_feature': user_active_feature[0][0]}

    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()

    return (jsonify(output), 201)


@app.route('/api/user/delete', methods=['POST'])
@cross_origin()
def make_user_delete_public():
    if request.headers['API-ACCESS-KEY'] != config.API_ACCESS_KEY:
        logging.debug('bad access key')
        abort(401)
    if request.headers['API-VERSION'] != config.API_VERSION:
        logging.debug('bad access version')
        abort(400)
    if not request.json:
        abort(400)
    if 'guid' in request.json and type(request.json['guid']) != unicode:
        abort(400)

    guid = request.json.get('guid')
    args = [guid,]

    try:
        cnx = mysql.connector.connect(user=config.API_DATABASE_USERNAME, password=config.API_DATABASE_PASSWORD,
                                      host=config.API_DATABASE_SERVER,
                                      database=config.API_DATABASE_NAME,
                                      use_pure=False)
        cursor = cnx.cursor()
        cursor.callproc('deleteUserByGUID', args)
        cursor.close()
        cnx.close()

    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()

    return ('', 201)

@app.errorhandler(404)
def not_found(error):
    return make_response(jsonify({'error': 'Not found'}), 404)


if __name__ == '__main__':
    logging.debug('starting flask app')
    app.run(debug=config.FLASK_DEBUG, host=config.LISTENING_HOST, threaded=False)

