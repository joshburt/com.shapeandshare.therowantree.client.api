from rowantree.contracts import WorldStatus

from ..controllers.abstract_controller import AbstractController


class WorldStatusGetController(AbstractController):
    def execute(self) -> WorldStatus:
        return WorldStatus(active_players=self.dao.users_active_get())
