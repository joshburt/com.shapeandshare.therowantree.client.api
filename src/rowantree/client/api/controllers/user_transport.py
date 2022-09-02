from typing import Any

from .abstract_controller import AbstractController


class UserTransportController(AbstractController):
    def execute(self, user_guid: str, location: str) -> Any:
        return self.dao.user_transport(user_guid=user_guid, location=location)
