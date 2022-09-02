from typing import Any

from .abstract_controller import AbstractController


class UserStateGetController(AbstractController):
    def execute(self, user_guid: str) -> Any:
        pass
        # TODO: fill in once other response structures have been verified and defined
