import os
from pydantic_settings import BaseSettings

# Centralized configuration management for the application.
# Pydantic's BaseSettings automatically reads and validates environment variables from a .env file.
class Settings(BaseSettings):
    AI_API_KEY: str
    AI_MODEL: str
    AI_API_URL: str

    class Config:
        # Specifies the .env file to load variables from.
        env_file = ".env"
        env_file_encoding = 'utf-8'

# A single, globally accessible instance of the settings.
settings = Settings()
