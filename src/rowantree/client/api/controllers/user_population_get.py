from typing import Any

from .abstract_controller import AbstractController


class UserPopulationGetController(AbstractController):
    def execute(self, user_guid: str) -> Any:
        return self.dao.get_user_population(target_user=user_guid)
