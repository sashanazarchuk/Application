import logging
import requests
from ..core.config import settings

logger = logging.getLogger(__name__)

class AIClient:
    """
    A generic client for interacting with OpenAI-compatible chat completion APIs.
    """
    def __init__(self):
        # Initializes the client with configuration from the global settings object.
        self.api_url = settings.AI_API_URL
        self.api_key = settings.AI_API_KEY
        self.model = settings.AI_MODEL
        self.headers = {
            "Authorization": f"Bearer {self.api_key}",
            "Content-Type": "application/json"
        }

    def get_chat_completion(self, messages: list) -> dict:
        """
        Gets a chat completion from the configured AI API.

        Args:
            messages: A list of message dictionaries.

        Returns:
            The JSON response from the API as a dictionary.
        """
        data = {
            "model": self.model,
            "messages": messages
        }

        logger.info(f"Sending request to {self.api_url} with model: {self.model}")

        try:
            # Makes the actual HTTP request to the AI service.
            response = requests.post(self.api_url, headers=self.headers, json=data, timeout=60)
            response.raise_for_status()  # Raises an HTTPError for bad responses (4xx or 5xx)
            response_json = response.json()

            # Create a deep copy for logging to avoid modifying the original object.
            import copy
            loggable_response = copy.deepcopy(response_json)

            # Remove verbose fields for cleaner logs.
            if 'choices' in loggable_response:
                for choice in loggable_response['choices']:
                    choice.pop('reasoning', None)
                    choice.pop('reasoning_details', None)
            
            logger.info(f"Raw response from AI provider: {loggable_response}")
            
            return response_json
        except requests.exceptions.RequestException as e:
            logger.error(f"Error communicating with AI provider: {e}", exc_info=True)
            # Re-raise the exception to be handled by the service layer/API layer.
            raise
