import { createAction, props } from "@ngrx/store";
import { CreateEventDto, EventDto } from "../models/event.model";
import { TagDto } from "../models/tag.model";

// Actions for loading all public events
export const loadPublicEvents = createAction('[Event] Load Public Events');
export const loadPublicEventsSuccess = createAction('[Event] Load Public Events Success', props<{ events: EventDto[] }>());
export const loadPublicEventsFailure = createAction('[Event] Load Public Events Failure', props<{ error: string }>());

// Actions for loading event by Id
export const loadEventById = createAction('[Event] Load Event By Id', props<{ eventId: string }>());
export const loadEventByIdSuccess = createAction('[Event] Load Event By Id Success', props<{ event: EventDto }>());
export const loadEventByIdFailure = createAction('[Event] Load Event By Id Failure', props<{ error: string }>());

// Actions for creating an event
export const createEvent = createAction('[Event] Create Event', props<{ dto: CreateEventDto }>());
export const createEventSuccess = createAction('[Event] Create Event Success', props<{ event: EventDto }>());
export const createEventFailure = createAction('[Event] Create Event Failure', props<{ error: string }>());

// Actions for updating an event
export const updateEvent = createAction('[Event] Update Event', props<{ eventId: string, patch:any }>());
export const updateEventSuccess = createAction('[Event] Update Event Success', props<{ event: EventDto }>());
export const updateEventFailure = createAction('[Event] Update Event Failure', props<{ error: string }>());

// Actions for deleting an event
export const deleteEvent = createAction('[Event] Delete Event', props<{ eventId: string }>());
export const deleteEventSuccess = createAction('[Event] Delete Event Success', props<{ eventId: string }>());
export const deleteEventFailure = createAction('[Event] Delete Event Failure', props<{ error: string }>());

// Action for load events created by the current user
export const loadMyEvents = createAction('[Event] Load My Events');
export const loadMyEventsSuccess = createAction('[Event] Load My Events Success', props<{ events: EventDto[] }>());
export const loadMyEventsFailure = createAction('[Event] Load My Events Failure', props<{ error: string }>());

// Action for load all tags
export const loadAllTags = createAction('[Event] Load All Tags');
export const loadAllTagsSuccess = createAction('[Event] Load All Tags Success', props<{ tags: TagDto[] }>());
export const loadAllTagsFailure = createAction('[Event] Load All Tags Failure', props<{ error: string }>());

// Action for filtering events by tags
export const filterEventsByTags = createAction('[Event] Filter Events By Tags', props<{ tags: string[] }>());
export const filterEventsByTagsSuccess = createAction('[Event] Filter Events By Tags Success', props<{ events: EventDto[] }>());
export const filterEventsByTagsFailure = createAction('[Event] Filter Events By Tags Failure', props<{ error: string }>());