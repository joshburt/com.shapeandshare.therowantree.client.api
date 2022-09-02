import logging
from typing import Any

from ..contracts.user_active_get_response import UserActiveGetResponse
from ..contracts.user_active_set_request import UserActiveSetRequest
from ..db.dao import DBDAO
from ..db.incorrect_row_count_error import IncorrectRowCountError
from .abstract_controller import AbstractController


class UserActiveSetController(AbstractController):
    def __init__(self, dao: DBDAO):
        super().__init__(dao=dao)

    def execute(self, user_guid: str, request: UserActiveSetRequest) -> UserActiveSetRequest:
        self.dao.set_user_active_state(user_guid=user_guid, active=request.active)
        return request
