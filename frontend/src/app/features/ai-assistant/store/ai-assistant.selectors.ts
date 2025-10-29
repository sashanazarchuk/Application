import { createSelector } from "@ngrx/store";
import { AppState } from "../../../core/store/appState";

export const selectAiAssistant = (state: AppState) => state.aiAssistant;

export const selectSuggestedTags = createSelector(
    selectAiAssistant,
    state => state.suggestedTags
);

export const selectAiSuggestedOnce = createSelector(
    selectAiAssistant,
    state => state.aiSuggestedOnce
);

export const selectAiReply = createSelector(
    selectAiAssistant,
    state => state.aiReply
);

export const selectIsLoading = createSelector(
    selectAiAssistant,
    state => state.isLoading
);