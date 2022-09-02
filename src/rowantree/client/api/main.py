import logging
import os
import socket
from pathlib import Path
from typing import Any, Optional, Tuple

from fastapi import Body, FastAPI, Header, HTTPException, status
from mysql.connector import Error, errorcode
from mysql.connector.pooling import MySQLConnectionPool

from .config.server import ServerConfig
from .contracts.merchant_transform_request import MerchantTransformRequest
from .contracts.user_active_get_response import UserActiveGetResponse
from .contracts.user_active_set_request import UserActiveSetRequest
from .contracts.user_create_response import UserCreateResponse
from .contracts.user_income_set_request import UserIncomeSetRequest
from .controllers.user_active_get import UserActiveGetController
from .controllers.user_active_set import UserActiveSetController
from .controllers.user_income_get import UserIncomeGetController
from .controllers.user_income_set import UserIncomeSetController
from .controllers.user_stores_get import UserStoresGetController
from .db.dao import DBDAO
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
user_active_get_controller = UserActiveGetController(dao=dao)
user_active_set_controller = UserActiveSetController(dao=dao)
user_stores_get_controller = UserStoresGetController(dao=dao)
user_income_get_controller = UserIncomeGetController(dao=dao)
user_income_set_controller = UserIncomeSetController(dao=dao)

user_create_controller = UserCreateController(dao=dao)
user_features_get_controller = UserFeaturesGetController(dao=dao)
user_population_get_controller = UserPopulationGetController(dao=dao)
user_features_active_get_controller = UserFeaturesActiveGetController(dao=dao)
user_delete_controller = UserDeleteController(dao=dao)
merchant_transforms_perform_controller = MerchantTransformPerformController(dao=dao)
user_merchant_transforms_get_controller = UserMerchantTransformsGetController(dao=dao)
user_transport_controller = UserTransportController(dao=dao)
user_state_get_controller = UserStateGetController(dao=dao)


app = FastAPI()


# Application Health Endpoint
@app.get("/health/plain", status_code=status.HTTP_200_OK)
async def health_plain():
    return True


# Get User's Active State
@app.get("/v1/user/{user_guid}/active", status_code=status.HTTP_200_OK)
async def user_active_get_handler(user_guid: str, api_access_key: str = Header(default=None)) -> UserActiveGetResponse:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_active_get_controller.execute(user_guid=user_guid)


# Set User's Active State
@app.post("/v1/user/{user_guid}/active", status_code=status.HTTP_200_OK)
async def user_active_set_handler(
    request: UserActiveSetRequest, user_guid: str, api_access_key: str = Header(default=None)
) -> UserActiveSetRequest:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_active_set_controller.execute(user_guid=user_guid, request=request)


@app.get("/v1/user/{user_guid}/stores", status_code=status.HTTP_200_OK)
async def user_stores_get_handler(user_guid: str, api_access_key: str = Header(default=None)) -> Any:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_stores_get_controller.execute(user_guid=user_guid)


@app.get("/v1/user/{user_guid}/income", status_code=status.HTTP_200_OK)
async def user_income_get_handler(user_guid: str, api_access_key: str = Header(default=None)) -> Any:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_income_get_controller.execute(user_guid=user_guid)


@app.post("/v1/user/{user_guid}/income", status_code=status.HTTP_200_OK)
async def user_income_set_handler(
    request: UserIncomeSetRequest, user_guid: str, api_access_key: str = Header(default=None)
) -> Any:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_income_set_controller.execute(user_guid=user_guid, request=request)


@app.post("/v1/user", status_code=status.HTTP_201_CREATED)
async def user_create_handler(api_access_key: str = Header(default=None)) -> UserCreateResponse:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_create_controller.execute()


@app.delete("/v1/user/{user_guid}", status_code=status.HTTP_200_OK)
async def user_delete_handler(user_guid: str, api_access_key: str = Header(default=None)) -> Any:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_delete_controller.execute(user_guid=user_guid)


@app.get("/v1/user/{user_guid}/features", status_code=status.HTTP_200_OK)
async def user_features_get_handler(user_guid: str, api_access_key: str = Header(default=None)) -> Any:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_features_get_controller.execute(user_guid=user_guid)


@app.get("/v1/user/{user_guid}/features/active", status_code=status.HTTP_200_OK)
async def user_features_active_get_handler(
    user_guid: str, api_access_key: str = Header(default=None), details: bool = False
) -> Any:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_features_active_get_controller.execute(user_guid=user_guid, details=details)


@app.get("/v1/user/{user_guid}/population", status_code=status.HTTP_200_OK)
async def user_population_get_handler(user_guid: str, api_access_key: str = Header(default=None)) -> Any:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_population_get_controller.execute(user_guid=user_guid)


# Consider reducing to just /v1/merchant
@app.post("/v1/merchant/transforms", status_code=status.HTTP_202_ACCEPTED)
async def merchant_transforms_perform_handler(
    request: MerchantTransformRequest, api_access_key: str = Header(default=None)
) -> Any:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return merchant_transforms_perform_controller.execute(request=request)


@app.get("/v1/user/{user_id}/merchant", status_code=status.HTTP_200_OK)
async def user_merchant_transforms_get_handler(user_guid: str, api_access_key: str = Header(default=None)) -> Any:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_merchant_transforms_get_controller.execute(user_guid=user_guid)


@app.post("/v1/user/{user_guid}/transport", status_code=status.HTTP_200_OK)
async def user_transport_handler(
    user_guid: str, location: str = Body(), api_access_key: str = Header(default=None)
) -> Any:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_transport_controller.execute(user_guid=user_guid, location=location)


@app.get("/v1/user/{user_guid}/state", status_code=status.HTTP_200_OK)
async def user_state_get_handler(user_guid: str, api_access_key: str = Header(default=None)) -> Any:
    if api_access_key != config.access_key:
        logging.debug("bad access key")
        raise HTTPException(status_code=status.HTTP_401_UNAUTHORIZED, detail="Bad Access Key")

    return user_state_get_controller.execute(user_guid=user_guid)
