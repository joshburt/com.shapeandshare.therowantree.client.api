from typing import Any

from .abstract_controller import AbstractController


class UserFeaturesGetController(AbstractController):
    def execute(self, user_guid: str) -> Any:
        return self.dao.user_features_get(user_guid=user_guid)
