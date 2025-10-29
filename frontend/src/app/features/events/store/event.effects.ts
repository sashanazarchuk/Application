import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { EventService } from "../services/event.service";
import { TagService } from "../services/tag.service";
import * as EventActions from './event.actions';
import { catchError, map, of, switchMap, tap } from "rxjs";
import { Router } from "@angular/router";


@Injectable()
export class EventEffects {

    private actions$ = inject(Actions);
    private eventService = inject(EventService);
    private tagService = inject(TagService);
    private router = inject(Router);

    getPublicEvents$ = createEffect(() =>
        this.actions$.pipe(
            ofType(EventActions.loadPublicEvents),
            switchMap(() =>
                this.eventService.getPublicEvents().pipe(
                    map(events => EventActions.loadPublicEventsSuccess({ events })),
                    catchError(error => of(EventActions.loadPublicEventsFailure({ error })))
                )
            )
        )
    );

    getMyEvents$ = createEffect(() =>
        this.actions$.pipe(
            ofType(EventActions.loadMyEvents),
            switchMap(() =>
                this.eventService.getMyEvents().pipe(
                    map(events => EventActions.loadMyEventsSuccess({ events })),
                    catchError(error => of(EventActions.loadMyEventsFailure({ error })))
                )
            )
        )
    );

    loadAllTags$ = createEffect(() =>
        this.actions$.pipe(
            ofType(EventActions.loadAllTags),
            switchMap(() =>
                this.tagService.getAllTags().pipe(
                    map(tags => EventActions.loadAllTagsSuccess({ tags })),
                    catchError(error => of(EventActions.loadAllTagsFailure({ error })))
                )
            )
        )
    );

    getEventById$ = createEffect(() =>
        this.actions$.pipe(
            ofType(EventActions.loadEventById),
            switchMap(action =>
                this.eventService.getEventById(action.eventId).pipe(
                    map(event => EventActions.loadEventByIdSuccess({ event })),
                    catchError(error => of(EventActions.loadEventByIdFailure({ error })))
                )
            )
        )
    );


    createEvent$ = createEffect(() =>
        this.actions$.pipe(
            ofType(EventActions.createEvent),
            switchMap(action =>
                this.eventService.createEvent(action.dto).pipe(
                    map(event => EventActions.createEventSuccess({ event })),
                    catchError(error => of(EventActions.createEventFailure({ error })))
                )
            )
        )
    );
    
    updateEvent$ = createEffect(() =>
        this.actions$.pipe(
            ofType(EventActions.updateEvent),
            switchMap(action =>
                this.eventService.editEvent(action.eventId, action.patch).pipe(
                    map(event => EventActions.updateEventSuccess({ event })),
                    catchError(error => of(EventActions.updateEventFailure({ error })))
                )
            )
        )
    );
    
    deleteEvent$ = createEffect(() =>
        this.actions$.pipe(
            ofType(EventActions.deleteEvent),
            switchMap(action =>
                this.eventService.deleteEvent(action.eventId).pipe(
                    map(() => EventActions.deleteEventSuccess({ eventId: action.eventId })),
                    catchError(error => of(EventActions.deleteEventFailure({ error })))
                )
            )
        )
    );
    
    filterEventsByTags$ = createEffect(() =>
        this.actions$.pipe(
            ofType(EventActions.filterEventsByTags),
            switchMap(action =>
                this.eventService.getEventsByTags(action.tags).pipe(
                    map(events => EventActions.filterEventsByTagsSuccess({ events })),
                    catchError(error => of(EventActions.filterEventsByTagsFailure({ error })))
                )
            )
        )
    );
    
    navigateAfterCreate$ = createEffect(() =>
        this.actions$.pipe(
            ofType(EventActions.createEventSuccess),
            tap(({ event }) => this.router.navigate(['/events', event.id]))
        ),
        { dispatch: false }
    );

    navigateAfterUpdate$ = createEffect(() =>
        this.actions$.pipe(
            ofType(EventActions.updateEventSuccess),
            tap(({ event }) => this.router.navigate(['/events', event.id]))
        ),
        { dispatch: false }
    );

    navigateAfterDelete$ = createEffect(() =>
        this.actions$.pipe(
            ofType(EventActions.deleteEventSuccess),
            tap(() => this.router.navigate(['/events']))
        ),
        { dispatch: false }
    );
}