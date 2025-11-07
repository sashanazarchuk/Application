from fastapi import APIRouter, HTTPException
from . import services
from .schemas import AIRequest

router = APIRouter()

# This endpoint is the main entry point for the AI service.
# It handles the HTTP request/response and delegates the business logic to the services layer.
@router.post("/ask")
def ask_ai_endpoint(request: AIRequest):
    try:
        reply_content = services.get_ai_reply(request)
        # The response is formatted to be compatible with the C# client's expectations.
        return {
            "choices": [
                {"message": {"content": reply_content}}
            ]
        }
    except Exception as e:
        # Catches any exceptions from the service layer and returns a generic error.
        raise HTTPException(status_code=502, detail=f"An error occurred while communicating with the AI service: {e}")
