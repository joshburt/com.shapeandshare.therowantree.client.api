""" Database DAO Definition """

import logging
import socket
from typing import Any, Tuple

import mysql.connector
from mysql.connector import errorcode
from mysql.connector.pooling import MySQLConnectionPool

from ..contracts.user_income_set_request import UserIncomeSetRequest
from .incorrect_row_count_error import IncorrectRowCountError


class DBDAO:
    """
    Database DAO

    Attributes
    ----------
    cnxpool: Any
        MySQL Connection Pool
    """

    cnxpool: MySQLConnectionPool

    def __init__(self, cnxpool: MySQLConnectionPool):
        self.cnxpool = cnxpool

    def transport_user(self, user_guid: str, location: str) -> Any:
        args: list = [user_guid, location]
        rows: list[Tuple[str]] = self._call_proc("transportUserByGUID", args)
        return rows

    def get_user_merchant_transforms(self, user_guid: str) -> Any:
        args: list = [
            user_guid,
        ]
        rows: list[Tuple[str]] = self._call_proc("getUserMerchantTransformsByGUID", args)
        return rows

    def perform_merchant_transform(self, user_guid: str, store_name: str) -> Any:
        args: list = [user_guid, store_name]
        rows: list[Tuple[str]] = self._call_proc("peformMerchantTransformByGUID", args)
        return rows

    def get_user_active_feature_state_details(self, user_guid: str) -> Any:
        args: list = [
            user_guid,
        ]
        rows: list[Tuple[str]] = self._call_proc("getUserActiveFeatureStateDetailsByGUID", args)
        return rows

    def get_user_active_feature(self, user_guid: str) -> Any:
        args: list = [
            user_guid,
        ]
        rows: list[Tuple[str]] = self._call_proc("getUserActiveFeatureByGUID", args)
        return rows

    def get_user_features(self, user_guid: str) -> Any:
        args: list = [
            user_guid,
        ]
        rows: list[Tuple[str]] = self._call_proc("getUserFeaturesByGUID", args)
        return rows

    def user_create(self) -> str:
        rows: list[Tuple[str]] = self._call_proc("createUser", [])
        if len(rows) != 1:
            raise IncorrectRowCountError(f"Result count was not exactly one. Received: {rows}")
        return rows[0][0]

    def delete_user(self, user_guid: str) -> Any:
        args: list = [
            user_guid,
        ]
        rows: list[Tuple[str]] = self._call_proc("deleteUserByGUID", args)
        return rows

    def get_user_active_state(self, user_guid: str) -> int:
        args: list[str, int] = [
            user_guid,
        ]
        rows: list[Tuple[int]] = self._call_proc("getUserActivityStateByGUID", args)
        if len(rows) != 1:
            raise IncorrectRowCountError(f"Result count was not exactly one. Received: {rows}")
        return rows[0][0]

    def set_user_active_state(self, user_guid: str, active: bool) -> None:
        args = [
            user_guid,
        ]
        if active:
            proc = "setUserActiveByGUID"
        else:
            proc = "setUserInactiveByGUID"
        self._call_proc(name=proc, args=args)

    def get_user_stores_by_guid(self, user_guid: str) -> Any:
        # Used by client api
        args: list[str, int] = [
            user_guid,
        ]
        rows: list[Tuple[Any]] = self._call_proc("getUserStoresByGUID", args)
        return rows

    def get_user_income_by_guid(self, user_guid: str) -> Any:
        args: list[str] = [
            user_guid,
        ]
        rows: list[Tuple[Any]] = self._call_proc("getUserIncomeByGUID", args)
        return rows

    def set_user_income(self, user_guid: str, transaction: UserIncomeSetRequest) -> Any:
        args = [user_guid, transaction.income_source_name, transaction.amount]
        rows: list[Tuple[Any]] = self._call_proc("deltaUserIncomeByNameAndGUID", args)
        return rows

    def get_active_users(self) -> list[str]:
        my_active_users: list[str] = []
        rows: list[Tuple] = self._call_proc("getActiveUsers", [])
        for response_tuple in rows:
            my_active_users.append(response_tuple[0])
        return my_active_users

    def get_user_population(self, target_user) -> int:
        rows: list[Tuple] = self._call_proc(
            "getUserPopulationByID",
            [
                target_user,
            ],
        )
        return rows[0][0]

    def get_user_stores_by_id(self, target_user) -> dict[str, Any]:
        # used by server
        user_stores: dict[str, Any] = {}
        rows: list[Tuple] = self._call_proc(
            "getUserStoresByID",
            [
                target_user,
            ],
        )
        for response_tuple in rows:
            user_stores[response_tuple[0]] = response_tuple[2]
        return user_stores

    def process_action_queue(self, action_queue) -> None:
        # logging.debug(action_queue)
        for action in action_queue:
            self._call_proc(action[0], action[1])

    def _call_proc(self, name: str, args: list) -> list[Tuple]:
        rows: list[Tuple] = []
        try:
            cnx = self.cnxpool.get_connection()
            cursor = cnx.cursor()
            cursor.callproc(name, args)
            for result in cursor.stored_results():
                rows = result.fetchall()
            cursor.close()
        except socket.error as error:
            logging.debug(error)
        except mysql.connector.Error as err:
            if err.errno == errorcode.ER_ACCESS_DENIED_ERROR:
                logging.debug("Something is wrong with your user name or password")
            elif err.errno == errorcode.ER_BAD_DB_ERROR:
                logging.debug("Database does not exist")
            else:
                logging.debug(err)
        else:
            cnx.close()

        if rows is None:
            raise Exception("Failure getting database information")

        return rows
