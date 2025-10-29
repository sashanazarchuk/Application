import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { AuthService } from "../../../core/services/auth.service";
import { ErrorService } from "../../../core/services/error.service";
import { Router } from "@angular/router";
import { catchError, map, of, switchMap, tap } from "rxjs";
import * as AuthActions from './auth.actions';
import { UserService } from "../../../core/services/user.service";


@Injectable()
export class AuthEffects {

    private actions$ = inject(Actions);
    private authService = inject(AuthService);
    private userService = inject(UserService);
    private errorService = inject(ErrorService);
    private router = inject(Router);

    login$ = createEffect(() =>
        this.actions$.pipe(
            ofType(AuthActions.login),
            switchMap(action =>
                this.authService.login(action.dto).pipe(
                    map(() => AuthActions.loginSuccess()),
                    catchError(err => of(AuthActions.loginFailure({ error: this.errorService.parseValidationErrors(err) })))
                )
            )
        )
    );

    register$ = createEffect(() =>
        this.actions$.pipe(
            ofType(AuthActions.register),
            switchMap(action =>
                this.authService.register(action.dto).pipe(
                    map(() => AuthActions.registerSuccess()),
                    catchError(err => of(AuthActions.registerFailure({ error: this.errorService.parseValidationErrors(err) })))
                )
            )
        )
    );


    loadCurrentUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(AuthActions.loadCurrentUser),
            switchMap(() =>
                this.userService.loadCurrentUser().pipe(
                    map(user => AuthActions.loadCurrentUserSuccess({ user })),
                    catchError(error => of(AuthActions.loadCurrentUserFailure({ error })))
                )
            )
        )
    );


    navigateAfterAuth$ = createEffect(() =>
        this.actions$.pipe(
            ofType(AuthActions.loginSuccess, AuthActions.registerSuccess),
            tap(() => this.router.navigate(['']))
        ),
        { dispatch: false }  
    );
}