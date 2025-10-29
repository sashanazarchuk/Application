import { createSelector } from "@ngrx/store";
import { AppState } from "../../../core/store/appState";


export const selectEventState = (state: AppState) => state.event;

export const selectPublicEvents = createSelector(
    selectEventState,
    state => state.publicEvents
);

export const selectMyEvents = createSelector(
    selectEventState,
    state => state.myEvents
);

export const selectSelectedEvent = createSelector(
    selectEventState,
    state => state.selectedEvent
);

export const selectTags = createSelector(
    selectEventState,
    state => state.tags
);

export const selectEventLoading = createSelector(
    selectEventState,
    state => state.loading
);

export const selectEventError = createSelector(
    selectEventState,
    state => state.error
);

export const selectEventById = (id: string) => createSelector(
    selectEventState,
    state => state.selectedEvent?.id === id
        ? state.selectedEvent
        : state.publicEvents.find(e => e.id === id)
        || state.myEvents.find(e => e.id === id)
        || null
);

export const selectFilteredEvents = createSelector(
    selectEventState,
    state => state.filteredEvents
);