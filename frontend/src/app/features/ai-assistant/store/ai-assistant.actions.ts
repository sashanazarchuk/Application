import { createAction, props } from "@ngrx/store";

// Actions for requesting AI-generated tags based on event title
export const requestTags = createAction('[AI] Request Tags', props<{ title: string }>());
export const requestTagsSuccess = createAction('[AI] Request Tags Success', props<{ tags: string[] }>());
export const requestTagsFailure = createAction('[AI] Request Tags Failure>()');
export const resetAiSuggestedOnce = createAction('[AI] Reset AI Suggested Once');
export const removeSuggestedTag = createAction('[AI] Remove Suggested Tag', props<{ tag: string }>());

// Actions for sending messages to the AI assistant
export const sendMessage = createAction('[AI] Send Message', props<{ userMessage: string }>());
export const sendMessageSuccess = createAction('[AI] Send Message Success', props<{ reply: string }>());
export const sendMessageFailure = createAction('[AI] Send Message Failure>()');