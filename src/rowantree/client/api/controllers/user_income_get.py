from typing import Any

from .abstract_controller import AbstractController


class UserIncomeGetController(AbstractController):
    def execute(self, user_guid: str) -> Any:
        # TODO: complete with DTO
        return self.dao.user_income_by_guid_get(user_guid=user_guid)
