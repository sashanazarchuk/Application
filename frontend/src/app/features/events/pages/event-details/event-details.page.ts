import { Component } from "@angular/core";
import { Observable, switchMap, tap } from "rxjs";
import { EventDto } from "../../models/event.model";
import { EventService } from "../../services/event.service";
import { ActivatedRoute, Router } from "@angular/router";
import { AsyncPipe, CommonModule, DatePipe, NgFor, NgIf } from "@angular/common";
import { UserService } from "../../../../core/services/user.service";
import { ActionButtonComponent } from "../../../../shared/components/button/action-button/action-button.component";
import { BackButtonComponent } from "../../../../shared/components/button/back-button/back-button.component";
import { ConfirmModalComponent } from "../../../../shared/components/modal/confirm-modal.component";

@Component({

    selector: 'app-event-details',
    imports: [NgFor, DatePipe, AsyncPipe, NgIf, ActionButtonComponent, CommonModule, BackButtonComponent, ConfirmModalComponent],
    templateUrl: './event-details.page.html'
})

export class EventDetailsPage {

    event$!: Observable<EventDto>;
    currentUserId: string | null = null;

    showDeleteModal = false;
    eventToDelete: EventDto | null = null;


    constructor(private eventService: EventService, private userService: UserService, private route: ActivatedRoute, private router: Router) { }

    ngOnInit() {

        const eventId = this.route.snapshot.paramMap.get('id')!;

        this.event$ = this.userService.getCurrentUser().pipe(
            tap(user => this.currentUserId = user?.id || null),
            switchMap(() => this.eventService.getEventById(eventId))
        );
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

        this.eventService.deleteEvent(this.eventToDelete.id).subscribe({
            next: () => {
                console.log('Event deleted');
                this.showDeleteModal = false;
                this.router.navigate(['/events']);
            },
            error: err => {
                console.error('Failed to delete event', err);
                this.showDeleteModal = false;
            }
        });
    }

    cancelDelete() {
        this.showDeleteModal = false;
        this.eventToDelete = null;
    }

    onToggleJoin(event: EventDto) {
        if (!this.currentUserId) return;

        const action$ = event.isJoined
            ? this.eventService.leaveEvent(event.id)
            : this.eventService.joinEvent(event.id);

        this.event$ = action$.pipe(
            switchMap(() => this.eventService.getEventById(event.id))
        );
    }
}