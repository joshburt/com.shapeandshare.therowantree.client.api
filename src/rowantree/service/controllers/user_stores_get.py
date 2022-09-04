from rowantree.contracts.dto.user.stores import UserStores

from ..contracts.responses.user_stores_get_response import UserStoresGetResponse
from .abstract_controller import AbstractController


class UserStoresGetController(AbstractController):
    def execute(self, user_guid: str) -> UserStores:
        return self.dao.user_stores_get(user_guid=user_guid)
