import { UserDto } from '../../../core/models/user.model';
import * as AuthActions from './auth.actions';
import { createReducer, on } from '@ngrx/store';

export interface AuthState {
    currentUser: UserDto | null;
    loading: boolean;
    error: string | null;
}

export const initialAuthState: AuthState = {
    currentUser: null,
    loading: false,
    error: null
};

export const authReducer = createReducer(
    initialAuthState,

    on(AuthActions.login, AuthActions.register, AuthActions.loadCurrentUser, state => ({
        ...state,
        loading: true,
        error: null
    })),

    on(AuthActions.loginSuccess, AuthActions.registerSuccess, state => ({
        ...state,
        loading: false,
        error: null
    })),

    on(AuthActions.loginFailure, AuthActions.registerFailure, (state, { error }) => ({
        ...state,
        loading: false,
        error
    })),

    // Load current user success
    on(AuthActions.loadCurrentUserSuccess, (state, { user }) => ({
        ...state,
        currentUser: user,
        loading: false,
        error: null
    })),

    // Load current user failure
    on(AuthActions.loadCurrentUserFailure, (state, { error }) => ({
        ...state,
        currentUser: null,
        loading: false,
        error
    }))
);