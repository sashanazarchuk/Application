import { createReducer, on } from "@ngrx/store";
import { EventDto } from "../models/event.model";
import * as EventActions from './event.actions';
import { TagDto } from "../models/tag.model";

export interface EventState {
    publicEvents: EventDto[];
    myEvents: EventDto[];
    selectedEvent: EventDto | null;
    filteredEvents: EventDto[] | null;
    tags: TagDto[];
    loading: boolean;
    error: string | null;
}

export const initialEventState: EventState = {
    publicEvents: [],
    myEvents: [],
    selectedEvent: null,
    filteredEvents: null,
    tags: [],
    loading: false,
    error: null
};

export const eventReducer = createReducer(

    initialEventState,

    // Load Group Events
    on(EventActions.loadPublicEvents, EventActions.loadMyEvents,  EventActions.loadEventById, EventActions.loadAllTags, (state) => ({
        ...state,
        loading: true,
        error: null
    })),

    // Load Success
    on(EventActions.loadPublicEventsSuccess, (state, { events }) => ({
        ...state,
        publicEvents: events,
        error: null,
        loading: false
    })),
    on(EventActions.loadMyEventsSuccess, (state, { events }) => ({
        ...state,
        myEvents: events,
        error: null,
        loading: false
    })),
    on(EventActions.loadEventByIdSuccess, (state, { event }) => ({
        ...state,
        selectedEvent: event,
        error: null,
        loading: false
    })),
   
    // Load Failure
    on(EventActions.loadPublicEventsFailure, EventActions.loadMyEventsFailure, EventActions.loadEventByIdFailure, (state, { error }) => ({
        ...state,
        loading: false,
        error
    })),

    //Tag Group
    on(EventActions.loadAllTagsSuccess, (state, { tags }) => ({
        ...state,
        tags: tags,
        error: null,
        loading: false
    })),
    on(EventActions.loadAllTagsFailure, (state, { error }) => ({
        ...state,
        loading: false,
        error
    })),

    // Create, Update, Delete Event
    on(EventActions.createEvent, EventActions.updateEvent, EventActions.deleteEvent, (state) => ({
        ...state,
        loading: true,
        error: null
    })),

    // Create, Update, Delete Success
    on(EventActions.createEventSuccess, (state, { event }) => ({
        ...state,
        publicEvents: [...state.publicEvents, event],
        myEvents: [...state.myEvents, event],
        error: null,
        loading: false
    })),
    on(EventActions.updateEventSuccess, (state, { event }) => ({
        ...state,
        publicEvents: state.publicEvents.map(e => e.id === event.id ? event : e),
        myEvents: state.myEvents.map(e => e.id === event.id ? event : e),
        selectedEvent: state.selectedEvent?.id === event.id ? event : state.selectedEvent,
        error: null,
        loading: false
    })),
    on(EventActions.deleteEventSuccess, (state, { eventId }) => ({
        ...state,
        publicEvents: state.publicEvents.filter(e => e.id !== eventId),
        myEvents: state.myEvents.filter(e => e.id !== eventId),
        selectedEvent: state.selectedEvent?.id === eventId ? null : state.selectedEvent,
        error: null,
        loading: false
    })),

    // Create, Update, Delete Failure
    on(EventActions.createEventFailure, EventActions.updateEventFailure, EventActions.deleteEventFailure, (state, { error }) => ({
        ...state,
        loading: false,
        error
    })),

    //Filter Events by Tags
    on(EventActions.filterEventsByTags, (state) => ({
        ...state,
        loading: true,
        error: null
    })),
    on(EventActions.filterEventsByTagsSuccess, (state, { events }) => ({
        ...state,
        filteredEvents: events,
        loading: false,
        error: null
    })),
    on(EventActions.filterEventsByTagsFailure, (state, { error }) => ({
        ...state,
        loading: false,
        error
    })),
);