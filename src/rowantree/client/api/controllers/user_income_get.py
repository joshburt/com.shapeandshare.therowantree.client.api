from ..contracts.dto.user_income import UserIncome
from .abstract_controller import AbstractController


class UserIncomeGetController(AbstractController):
    def execute(self, user_guid: str) -> list[UserIncome]:
        return self.dao.user_income_by_guid_get(user_guid=user_guid)
