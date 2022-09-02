from typing import Any, Tuple

from .abstract_controller import AbstractController


class UserFeaturesActiveGetController(AbstractController):
    def execute(self, user_guid: str, details: bool) -> Any:
        if details:
            rows: list[Tuple[str]] = self.dao.user_active_feature_state_details_get(user_guid=user_guid)
        else:
            rows: list[Tuple[str]] = self.dao.user_active_feature_get(user_guid=user_guid)
        return rows
