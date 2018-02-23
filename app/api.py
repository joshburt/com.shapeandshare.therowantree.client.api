#!flask/bin/python

import errno
import logging
import os

from flask import Flask
from flask_cors import CORS
from flask import Flask, jsonify, request, make_response, abort
from flask_cors import CORS, cross_origin

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
cors = CORS(app, resources={r"/api/*": {"origins": "http://localhost:*"}})

@app.route('/api/version', methods=['GET'])
def make_api_version_public():
    return make_response(jsonify({'version':  str(config.API_VERSION)}), 201)

@app.route('/health/plain', methods=['GET'])
@cross_origin()
def make_health_plain_public():
    return make_response('true', 201)


@app.errorhandler(404)
def not_found(error):
    return make_response(jsonify({'error': 'Not found'}), 404)

if __name__ == '__main__':
    logging.debug('starting flask app')
    app.run(debug=config.FLASK_DEBUG, host=config.LISTENING_HOST, threaded=True)
