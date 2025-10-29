import { createAction, props } from "@ngrx/store";
import { LoginDto, RegisterDto } from "../../../core/models/auth.model";
import { UserDto } from "../../../core/models/user.model";

// Login Actions
export const login = createAction('[Auth] Login', props<{ dto: LoginDto }>());
export const loginSuccess = createAction('[Auth] Login Success');
export const loginFailure = createAction('[Auth] Login Failure', props<{ error: string }>());

// Register Actions
export const register = createAction('[Auth] Register', props<{ dto: RegisterDto }>());
export const registerSuccess = createAction('[Auth] Register Success');
export const registerFailure = createAction('[Auth] Register Failure', props<{ error: string }>());

// Get Current User Actions
export const loadCurrentUser = createAction('[Auth] Load Current User');
export const loadCurrentUserSuccess = createAction('[Auth] Load Current User Success', props<{ user: UserDto }>());
export const loadCurrentUserFailure = createAction('[Auth] Load Current User Failure', props<{ error: string }>());
