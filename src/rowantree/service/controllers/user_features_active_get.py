""" User Features Active Get Controller Definition """
import logging

from starlette import status
from starlette.exceptions import HTTPException

from rowantree.contracts import UserFeature

from ..services.db.incorrect_row_count_error import IncorrectRowCountError
from .abstract_controller import AbstractController


class UserFeaturesActiveGetController(AbstractController):
    """
    User Features Active Get Controller
    Gets the active user feature.

    Methods
    -------
    execute(self, user_guid: str, details: bool) -> UserFeature
        Executes the command.
    """

    def execute(self, user_guid: str, details: bool) -> UserFeature:
        """
        Gets the active user feature.

        Parameters
        ----------
        user_guid: str
            The target user guid.
        details: bool
            Whether to include details of the feature.

        Returns
        -------
        user_feature: UserFeature
            The active UserFeature.
        """

        try:
            if details:
                feature: UserFeature = self.dao.user_active_feature_state_details_get(user_guid=user_guid)
            else:
                feature: UserFeature = self.dao.user_active_feature_get(user_guid=user_guid)
            return feature
        except IncorrectRowCountError as error:
            # User did not exist (received an empty tuple)
            logging.debug("caught: {%s}", str(error))
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="Unable to find user") from error
