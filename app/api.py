#!flask/bin/python

import errno
import logging
import os

from flask import Flask
from flask_cors import CORS
from flask import Flask, jsonify, request, make_response, abort
from flask_cors import CORS, cross_origin

import mysql.connector
from mysql.connector import pooling
from mysql.connector import errorcode

import socket, errno

import lib.docker_config as config

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
    filename="%s/%s.therowantree.client.api.log" % (config.LOGS_DIR, os.uname()[1])
)


app = Flask(__name__)
# cors = CORS(app, resources={r"/api/*": {"origins": "http://localhost:*"}})
cors = CORS(app, resources={r"/api/*": {"origins": "*"}})

try:
    cnxpool = pooling.MySQLConnectionPool(pool_name = "apicnxpool",
                                      pool_size = 32,
                                      user=config.API_DATABASE_USERNAME, password=config.API_DATABASE_PASSWORD,
                                      host=config.API_DATABASE_SERVER,
                                      database=config.API_DATABASE_NAME)
except socket.error, e:
    logging.debug(e)
except mysql.connector.Error as err:
    if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
        logging.debug("Something is wrong with your user name or password")
    elif err.errno == errorcode.ER_BAD_DB_ERROR:
        logging.debug("Database does not exist")
    else:
        logging.debug(err)


@app.route('/api/version', methods=['GET'])
@cross_origin()
def make_api_version_public():
    return make_response(jsonify({'version':  str(config.API_VERSION)}), 201)


@app.route('/health/plain', methods=['GET'])
@cross_origin()
def make_health_plain_public():
    return make_response('true', 200)


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
    result_args = None
    try:
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        result_args = cursor.callproc('getUserActivityStateByGUID', args)
        cursor.close()
    except socket.error, e:
        logging.debug(e)
    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()

    if result_args is None or result_args[1] is None:
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
            cnx = cnxpool.get_connection()
            cursor = cnx.cursor()
            cursor.callproc(proc, args)
            cursor.close()
        except socket.error, e:
            logging.debug(e)
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
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        cursor.callproc('getUserStoresByGUID', args)
        for result in cursor.stored_results():
            user_stores = result.fetchall()
        cursor.close()
    except socket.error, e:
        logging.debug(e)
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
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        cursor.callproc('getUserIncomeByGUID', args)
        for result in cursor.stored_results():
            user_incomes = result.fetchall()
        cursor.close()
    except socket.error, e:
        logging.debug(e)
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
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        cursor.callproc('deltaUserIncomeByNameAndGUID', args)
        cursor.close()
    except socket.error, e:
        logging.debug(e)
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
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        cursor.callproc('createUser')
        for result in cursor.stored_results():
            user_guid = result.fetchall()
        cursor.close()
        output = {'guid': user_guid[0][0]}
    except socket.error, e:
        logging.debug(e)
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
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        cursor.callproc('getUserFeaturesByGUID', args)
        for result in cursor.stored_results():
            user_features = result.fetchall()
        cursor.close()
        for feature in user_features:
            feature_list.append(feature[0])
        output = {'features': feature_list}
    except socket.error, e:
        logging.debug(e)
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
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        cursor.callproc('getUserPopulationByGUID', args)
        for result in cursor.stored_results():
            user_population = result.fetchall()
        cursor.close()
        if not user_population:
            user_population = 0
        population_obj = {'population': user_population}
    except socket.error, e:
        logging.debug(e)
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
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        cursor.callproc('getUserActiveFeatureByGUID', args)
        for result in cursor.stored_results():
            user_active_feature = result.fetchall()
        cursor.close()
        output = {'active_feature': user_active_feature[0][0]}
    except socket.error, e:
        logging.debug(e)
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


@app.route('/api/user/active/feature/details', methods=['POST'])
@cross_origin()
def make_user_active_feature_details_public():
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
    active_feature_state_details = {}
    result_object = {}
    try:
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        cursor.callproc('getUserActiveFeatureStateDetailsByGUID', args)
        for result in cursor.stored_results():
            active_feature_state_details = result.fetchall()
        cursor.close()

    except socket.error, e:
        logging.debug(e)
    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()

    if not active_feature_state_details:
        active_feature_state_details = {
            'name': None,
            'description': None
        }
    else:
        details_list = active_feature_state_details[0]
        active_feature_state_details = {
            'name': details_list[0],
            'description': details_list[1]
        }

    result_object['active_feature_state_details'] = active_feature_state_details
    return (jsonify(result_object), 201)


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
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        cursor.callproc('deleteUserByGUID', args)
        cursor.close()
    except socket.error, e:
        logging.debug(e)
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


@app.route('/api/merchant/transform', methods=['POST'])
@cross_origin()
def make_merchant_transform_public():
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
    if 'store_name' in request.json and type(request.json['store_name']) != unicode:
        abort(400)

    guid = request.json.get('guid')
    store_name = request.json.get('store_name')
    args = [guid, store_name]

    try:
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        cursor.callproc('peformMerchantTransformByGUID', args)
        cursor.close()
    except socket.error, e:
        logging.debug(e)
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


@app.route('/api/user/merchants', methods=['POST'])
@cross_origin()
def make_user_merchant_transforms_public():
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
    user_merchants = {}
    merchants_obj = []

    try:
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        cursor.callproc('getUserMerchantTransformsByGUID', args)
        for result in cursor.stored_results():
            user_merchants = result.fetchall()
        cursor.close()
    except socket.error, e:
        logging.debug(e)
    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()
    for transform in user_merchants:
        merchants_obj.append(transform[0])

    return_results = {'merchants': merchants_obj}
    return (jsonify(return_results), 201)


@app.route('/api/user/transport', methods=['POST'])
@cross_origin()
def make_user_transport_public():
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
    if 'location' in request.json and type(request.json['location']) != unicode:
        abort(400)

    guid = request.json.get('guid')
    location = request.json.get('location')
    args = [guid, location]

    try:
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()
        cursor.callproc('transportUserByGUID', args)
        cursor.close()
    except socket.error, e:
        logging.debug(e)
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


@app.route('/api/user/state', methods=['POST'])
@cross_origin()
def make_user_state_public():
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
    user_object = {}

    stores_obj = {}
    user_stores = {}
    user_activity_state = None
    incomes_obj = {}
    user_incomes = {}
    user_features = {}
    feature_list = []
    population_obj = {}
    user_population = 0
    user_active_feature = {}
    user_merchants = {}
    merchants_obj = []
    active_feature_state_details = {}
    user_notifications = {}
    note_obj = []

    try:
        cnx = cnxpool.get_connection()
        cursor = cnx.cursor()

        # User Game State
        user_activity_state = cursor.callproc('getUserActivityStateByGUID', [guid, 0])

        # User Stores (Inventory)
        cursor.callproc('getUserStoresByGUID', args)
        for result in cursor.stored_results():
            user_stores = result.fetchall()

        # User Income
        cursor.callproc('getUserIncomeByGUID', args)
        for result in cursor.stored_results():
            user_incomes = result.fetchall()

        # Features
        cursor.callproc('getUserFeaturesByGUID', args)
        for result in cursor.stored_results():
            user_features = result.fetchall()

        # Population
        cursor.callproc('getUserPopulationByGUID', args)
        for result in cursor.stored_results():
            user_population = result.fetchall()

        # Active Feature
        cursor.callproc('getUserActiveFeatureByGUID', args)
        for result in cursor.stored_results():
            user_active_feature = result.fetchall()

        # Active Feature Details
        cursor.callproc('getUserActiveFeatureStateDetailsByGUID', args)
        for result in cursor.stored_results():
            active_feature_state_details = result.fetchall()

        # Merchants
        cursor.callproc('getUserMerchantTransformsByGUID', args)
        for result in cursor.stored_results():
            user_merchants = result.fetchall()

        # Notifications
        cursor.callproc('getUserNotificationByGUID', args)
        for result in cursor.stored_results():
            user_notifications = result.fetchall()

        cursor.close()
    except socket.error, e:
        logging.debug(e)
    except mysql.connector.Error as err:
        if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
            logging.debug("Something is wrong with your user name or password")
        elif err.errno == errorcode.ER_BAD_DB_ERROR:
            logging.debug("Database does not exist")
        else:
            logging.debug(err)
    else:
        cnx.close()


    # Build up our response #

    # User Activity Status
    user_object['user_activity_state'] = 'Unknown'
    if user_activity_state is None or user_activity_state[1] is None:
        user_object[user_activity_state] = 0
    else:
        user_object['user_activity_state'] = user_activity_state[1]

    # User Stores (Inventory)
    for result in user_stores:
        stores_obj[result[0]] = { 'amount': result[2], 'description': result[1] }
    user_object['stores'] = stores_obj

    # User Income
    for result in user_incomes:
        incomes_obj[result[1]] = { 'amount': result[0], 'description': result[2] }
    user_object['income'] = incomes_obj

    # Features
    for feature in user_features:
        feature_list.append(feature[0])
    user_object['features'] = feature_list

    # Population
    if not user_population:
        user_population = 0
    else:
        user_population = user_population[0][0]
    user_object['population'] = user_population

    # Active Feature
    user_object['active_feature'] = user_active_feature[0][0]

    # Active Feature Details
    if not active_feature_state_details:
        active_feature_state_details = {
            'name': None,
            'description': None
        }
    else:
        active_feature_state_details = {
            'name': active_feature_state_details[0][0],
            'description': active_feature_state_details[0][1]
        }
    user_object['active_feature_state_details'] = active_feature_state_details

    # Merchants
    for transform in user_merchants:
        merchants_obj.append(transform[0])
    user_object['merchants'] = merchants_obj

    # User Notifications
    for notification in user_notifications:
        note_obj.append(notification)
        # note_obj.append(notification[2])
    user_object['notifications'] = note_obj



    return_results = {'user': user_object}
    return (jsonify(return_results), 201)



@app.errorhandler(404)
def not_found(error):
    return make_response(jsonify({'error': 'Not found'}), 404)


if __name__ == '__main__':
    logging.debug('starting flask app')
    app.run(debug=config.FLASK_DEBUG, host=config.LISTENING_HOST, threaded=True)
    # port=80,

