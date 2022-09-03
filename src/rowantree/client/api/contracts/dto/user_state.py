from typing import Any, Tuple

from pydantic import BaseModel

from .user_active_feature_detail import UserActiveFeatureDetail
from .user_notification import UserNotification
from .user_store import UserStore


class UserState(BaseModel):
    active: bool
    stores: list[UserStore]
    incomes: Any
    features: list[str]
    active_features: list[str]
    active_features_details: list[UserActiveFeatureDetail]
    population: int
    merchants: list[str]
    notifications: list[UserNotification]
