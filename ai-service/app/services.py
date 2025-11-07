import json
import logging
from .clients.ai_client import AIClient
from .schemas import AIRequest

logger = logging.getLogger(__name__)

def get_ai_reply(request: AIRequest) -> str:
    """
    Prepares the prompt and uses the AI client to get a reply.
    This function orchestrates the business logic.
    """
    # Prepare the prompt by combining the system message and the data snapshot.
    snapshot_json = json.dumps(request.snapshot, indent=2, ensure_ascii=False)
    full_system_content = (
        f"{request.system_prompt}\n\n"
        "--- SNAPSHOT START ---\n"
        f"{snapshot_json}\n"
        "--- SNAPSHOT END ---\n\n"
    )

    messages = [
        {"role": "system", "content": full_system_content},
        {"role": "user", "content": request.message},
    ]

    # Use the dedicated client to communicate with the AI API.
    client = AIClient()
    response_json = client.get_chat_completion(messages)

    # Safely parse the response to extract the content.
    reply_content = response_json.get("choices", [{}])[0].get("message", {}).get("content")

    if not reply_content:
        logger.warning("AI service returned empty content. Full response: %s", response_json)
        return ""

    logger.info(f"Reply content: '{reply_content}'")
    return reply_content