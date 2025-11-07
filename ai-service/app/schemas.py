from pydantic import BaseModel

# Defines the structure and validation for the incoming request body.
class AIRequest(BaseModel):
    message: str
    snapshot: dict
    system_prompt: str
