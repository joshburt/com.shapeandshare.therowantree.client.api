import logging
import os
import socket
from pathlib import Path
from typing import Optional, Tuple

from fastapi import Body, FastAPI, Header, HTTPException, status
from mysql.connector import Error, errorcode
from mysql.connector.pooling import MySQLConnectionPool

from .config.server import ServerConfig
from .contracts.get_user_active_state_response import GetUserActiveStateResponse
from .controllers.get_user_active_state import GetUserActiveStateController
from .db.dao import DBDAO
from .db.incorrect_row_count_error import IncorrectRowCountError
from .db.utils import get_connect_pool

# Generating server configuration
config: ServerConfig = ServerConfig()

# Setup logging
Path(config.log_dir).mkdir(parents=True, exist_ok=True)
logging.basicConfig(
    format="%(asctime)s - %(levelname)s - %(message)s",
    datefmt="%m/%d/%Y %I:%M:%S %p",
    level=logging.DEBUG,
    filemode="w",
    filename=f"{config.log_dir}/{os.uname()[1]}.therowantree.client.api.log",
)

logging.debug("Starting server")

logging.debug(config.json(by_alias=True, exclude_unset=True))

# Creating database connection pool, and DAO
cnxpool: MySQLConnectionPool = get_connect_pool(config=config)
dao: DBDAO = DBDAO(cnxpool=cnxpool)

# create controllers
get_user_active_state_controller = GetUserActiveStateController(dao=dao)

app = FastAPI()


@app.get("/health/plain", status_code=status.HTTP_200_OK)
# @cross_origin()
async def health_plain():
    return True


@app.post("/api/user/active/state", status_code=status.HTTP_200_OK)
# @cross_origin()
async def get_user_active_state(user_guid: str, api_access_key: str = Header(default=None)):
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return get_user_active_state_controller.execute(user_guid=user_guid)
