import { createSelector } from "@ngrx/store";
import { AppState } from "../../../core/store/appState";

export const selectAuth = (state: AppState) => state.auth;

export const selectAuthLoading = createSelector(
    selectAuth,
    state => state.loading
);

export const selectAuthError = createSelector(
    selectAuth,
    state => state.error
);

export const selectCurrentUser = createSelector(
    selectAuth,
    state => state.currentUser
);