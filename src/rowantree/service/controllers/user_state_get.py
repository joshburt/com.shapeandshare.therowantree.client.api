from rowantree.contracts import UserFeature, UserIncome, UserNotification, UserStore
from rowantree.contracts.dto.user.active import UserActive
from rowantree.contracts.dto.user.features import UserFeatures
from rowantree.contracts.dto.user.incomes import UserIncomes
from rowantree.contracts.dto.user.merchants import UserMerchants
from rowantree.contracts.dto.user.notifications import UserNotifications
from rowantree.contracts.dto.user.population import UserPopulation
from rowantree.contracts.dto.user.state import UserState
from rowantree.contracts.dto.user.stores import UserStores
from starlette import status
from starlette.exceptions import HTTPException

from ..db.incorrect_row_count_error import IncorrectRowCountError
from .abstract_controller import AbstractController


class UserStateGetController(AbstractController):
    def execute(self, user_guid: str) -> UserState:
        # User Game State
        try:
            active: UserActive = self.dao.user_active_state_get(user_guid=user_guid)
        except IncorrectRowCountError as error:
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="User not found") from error

        # User Stores (Inventory)
        stores: UserStores = self.dao.user_stores_get(user_guid=user_guid)

        # User Income
        incomes: UserIncomes = self.dao.user_income_get(user_guid=user_guid)

        # Features
        features: UserFeatures = self.dao.user_features_get(user_guid=user_guid)

        # Population
        population: UserPopulation = self.dao.user_population_by_guid_get(user_guid=user_guid)

        # Active Feature w/ Details
        active_feature: UserFeature = self.dao.user_active_feature_state_details_get(user_guid=user_guid)

        # Merchants
        merchants: UserMerchants = self.dao.user_merchant_transforms_get(user_guid=user_guid)

        # Notifications
        notifications: UserNotifications = self.dao.user_notifications_get(user_guid=user_guid)

        user_state: UserState = UserState(
            active=active.active,
            stores=stores.stores,
            incomes=incomes.incomes,
            features=features.features,
            active_feature=active_feature,
            population=population.population,
            merchants=merchants.merchants,
            notifications=notifications.notifications,
        )
        return user_state
