from pydantic import BaseModel


class GetUserActiveStateResponse(BaseModel):
    active: int
