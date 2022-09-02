from ..contracts.user_income_set_request import UserIncomeSetRequest
from ..controllers.abstract_controller import AbstractController


class UserIncomeSetController(AbstractController):
    def execute(self, user_guid: str, request: UserIncomeSetRequest):
        return self.dao.set_user_income(user_guid=user_guid, transaction=request)
