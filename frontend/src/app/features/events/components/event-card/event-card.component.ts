import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { BehaviorSubject, combineLatest, map, Observable, of, switchMap, tap } from "rxjs";
import { EventDto } from "../../models/event.model";
import { EventService } from "../../services/event.service";
import { AsyncPipe, CommonModule, DatePipe, NgFor, NgIf } from "@angular/common";
import { UserService } from "../../../../core/services/user.service";
import { Router } from "@angular/router";

@Component({
    selector: 'app-event-card',
    imports: [FormsModule, DatePipe, AsyncPipe, NgIf, NgFor, CommonModule],
    templateUrl: './event-card.component.html',
})

export class EventCardComponent {

    events$!: Observable<EventDto[]>;
    currentUserId: string | null = null;
    private searchTerm$ = new BehaviorSubject<string>('');

    constructor(private eventService: EventService, private userService: UserService, private router: Router) { }

    ngOnInit() {

        const user$ = this.userService.getCurrentUser().pipe(
            tap(user => this.currentUserId = user?.id || null)
        );

        const allEvents$ = user$.pipe(
            switchMap(() => this.eventService.getPublicEvents())
        );

        this.events$ = combineLatest([allEvents$, this.searchTerm$]).pipe(
            map(([events, term]) =>
                events.filter(e => e.title.toLowerCase().includes(term.toLowerCase()))
            )
        );
    }

    onSearch(term: string) {
        this.searchTerm$.next(term);
    }


    onToggleJoin(event: EventDto) {
        if (!this.currentUserId) return;

        const action$ = event.isJoined
            ? this.eventService.leaveEvent(event.id)
            : this.eventService.joinEvent(event.id);

        action$.pipe(
            switchMap(() => this.eventService.getPublicEvents())
        ).subscribe({
            next: events => this.events$ = of(events),
            error: err => console.error('Failed to update event', err)
        });
    }

    goToEventDetails(event: EventDto) {
        this.router.navigate(['/events', event.id]);
    }
}