import logging
from fastapi import FastAPI
import uvicorn
from app.api import router as api_router

# Standard basic logging configuration.
logging.basicConfig(level=logging.INFO, format="%(asctime)s - %(levelname)s - %(message)s")

# Create the main FastAPI application instance.
app = FastAPI(title="AI Service")

# Include the API router. All routes defined in api.py will be included
# with the prefix /api/ai.
app.include_router(api_router, prefix="/api/ai", tags=["AI"])

# Standard entry point for running the application with uvicorn.
if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8001)
