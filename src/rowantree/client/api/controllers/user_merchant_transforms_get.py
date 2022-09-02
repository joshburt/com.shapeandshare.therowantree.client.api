from typing import Any

from .abstract_controller import AbstractController


class UserMerchantTransformsGetController(AbstractController):
    def execute(self, user_guid: str) -> Any:
        return self.dao.user_merchant_transforms_get(user_guid=user_guid)
