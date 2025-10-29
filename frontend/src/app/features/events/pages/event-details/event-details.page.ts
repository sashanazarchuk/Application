import { Component } from "@angular/core";
import { map, Observable, switchMap, take, tap } from "rxjs";
import { EventDto } from "../../models/event.model";
import { EventService } from "../../services/event.service";
import { ActivatedRoute, Router } from "@angular/router";
import { AsyncPipe, CommonModule, DatePipe, NgFor, NgIf } from "@angular/common";
import { ActionButtonComponent } from "../../../../shared/components/button/action-button/action-button.component";
import { BackButtonComponent } from "../../../../shared/components/button/back-button/back-button.component";
import { ConfirmModalComponent } from "../../../../shared/components/modal/confirm-modal.component";
import { EventTagsComponent } from "../../../../shared/components/tag/event-tag/event-tag.component";
import { AppState } from "../../../../core/store/appState";
import { Store } from "@ngrx/store";
import { selectCurrentUser } from "../../../auth/store/auth.selectors";
import { deleteEvent, loadEventById } from "../../store/event.actions";
import { selectSelectedEvent } from "../../store/event.selectors";

@Component({

    selector: 'app-event-details',
    imports: [NgFor, DatePipe, AsyncPipe, NgIf, ActionButtonComponent, CommonModule, BackButtonComponent, ConfirmModalComponent, EventTagsComponent],
    templateUrl: './event-details.page.html'
})

export class EventDetailsPage {

    event$!: Observable<EventDto | null>;
    currentUserId$!: Observable<string | null>;

    showDeleteModal = false;
    eventToDelete: EventDto | null = null;

    constructor(private eventService: EventService, private store: Store<AppState>, private route: ActivatedRoute, private router: Router) { }

    ngOnInit() {
        const eventId = this.route.snapshot.paramMap.get('id')!;
        this.currentUserId$ = this.store.select(selectCurrentUser).pipe(map(user => user?.id || null));
        this.store.dispatch(loadEventById({ eventId }));
        this.event$ = this.store.select(selectSelectedEvent);
    }

    goToEditEvent(eventId: string) {
        this.router.navigate(['/events', eventId, 'edit']);
    }

    onDeleteEvent(event: EventDto) {
        this.eventToDelete = event;
        this.showDeleteModal = true;
    }

    confirmDelete() {
        if (!this.eventToDelete) return;
        this.store.dispatch(deleteEvent({ eventId: this.eventToDelete.id }));
        this.showDeleteModal = false;
    }

    cancelDelete() {
        this.showDeleteModal = false;
        this.eventToDelete = null;
    }

    onToggleJoin(event: EventDto) {
        this.currentUserId$.pipe(take(1)).subscribe(userId => {
            if (!userId) return;

            const action$ = event.isJoined
                ? this.eventService.leaveEvent(event.id)
                : this.eventService.joinEvent(event.id);

            this.event$ = action$.pipe(
                switchMap(() => this.eventService.getEventById(event.id))
            );
        })
    }
}