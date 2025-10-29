import { AIAssistantState } from "../../features/ai-assistant/store/ai-assistant.reducer";
import { AuthState } from "../../features/auth/store/auth.reducer";
import { EventState } from "../../features/events/store/event.reducer";

 
export interface AppState{
    aiAssistant: AIAssistantState;
    auth: AuthState;
    event: EventState;
}