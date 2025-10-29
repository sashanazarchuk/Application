import { createReducer, on } from "@ngrx/store";
import * as AIAssistantActions from './ai-assistant.actions';

export interface AIAssistantState {
    suggestedTags: string[];
    aiSuggestedOnce: boolean;
    aiReply: string;
    isLoading: boolean;
}

export const initialAIState: AIAssistantState = {
    suggestedTags: [],
    aiSuggestedOnce: false,
    aiReply: '',
    isLoading: false
};

export const aiAssistantReducer = createReducer(

    initialAIState,

    // Handlers for AI tag suggestion actions
    on(AIAssistantActions.requestTagsSuccess, (state, { tags }) => ({
        ...state,
        suggestedTags: tags,
        aiSuggestedOnce: true
    })),
    on(AIAssistantActions.removeSuggestedTag, (state, { tag }) => ({
        ...state,
        suggestedTags: state.suggestedTags.filter(t => t !== tag)
    })),
    on(AIAssistantActions.resetAiSuggestedOnce, state => ({
        ...state,
        aiSuggestedOnce: false
    })),


    // Handlers for AI message actions
    on(AIAssistantActions.sendMessage, (state) => ({
        ...state,
        isLoading: true,
    })),
    on(AIAssistantActions.sendMessageSuccess, (state, { reply }) => ({
        ...state,
        aiReply: reply,
        isLoading: false
    })),
    on(AIAssistantActions.sendMessageFailure, state => ({
        ...state,
        aiReply: 'Error',
        isLoading: false
    }))
)