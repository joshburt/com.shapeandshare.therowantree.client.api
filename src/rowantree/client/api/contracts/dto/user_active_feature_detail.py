from typing import Optional

from pydantic import BaseModel


class UserActiveFeatureDetail(BaseModel):
    name: str
    description: Optional[str]
