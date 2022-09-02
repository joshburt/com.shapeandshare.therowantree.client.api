from pydantic import BaseModel


class MerchantTransformRequest(BaseModel):
    user_guid: str
    store_name: str
