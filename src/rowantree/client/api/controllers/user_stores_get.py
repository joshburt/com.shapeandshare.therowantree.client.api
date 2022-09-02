from .abstract_controller import AbstractController


class UserStoresGetController(AbstractController):
    def execute(self, user_guid: str):
        return self.dao.get_user_stores_by_guid(user_guid=user_guid)

    # TODO: update DTO creation ..
    # for result in user_stores:
    #     stores_obj[result[0]] = { 'amount': result[2], 'description': result[1] }
    #
    # return_results = {'stores': stores_obj}
