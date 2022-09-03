from typing import Any, Tuple

from pydantic import BaseModel

from .user_notification import UserNotification


class UserState(BaseModel):
    active: bool
    stores: Any
    incomes: Any
    features: list[Tuple[str]]
    active_features: list[Tuple[str]]
    active_features_details: list[Tuple[str, Any]]
    population: int
    merchants: list[Tuple[str]]
    notifications: list[UserNotification]
