import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { BehaviorSubject, combineLatest, map, Observable, of, switchMap, take, tap } from "rxjs";
import { EventDto } from "../../models/event.model";
import { EventService } from "../../services/event.service";
import { AsyncPipe, CommonModule, DatePipe, NgFor, NgIf } from "@angular/common";
import { Router } from "@angular/router";
import { EventTagsComponent } from "../../../../shared/components/tag/event-tag/event-tag.component";
import { TagSelectorComponent } from "../../../../shared/components/tag/tag-selector/tag-selector.component";
import { AppState } from "../../../../core/store/appState";
import { Store } from "@ngrx/store";
import { loadAllTags, loadPublicEvents } from "../../store/event.actions";
import { loadCurrentUser } from "../../../auth/store/auth.actions";
import { selectCurrentUser } from "../../../auth/store/auth.selectors";
import { selectPublicEvents, selectTags } from "../../store/event.selectors";

@Component({
    selector: 'app-event-card',
    imports: [FormsModule, DatePipe, AsyncPipe, NgIf, NgFor, CommonModule, EventTagsComponent, TagSelectorComponent],
    templateUrl: './event-card.component.html',
})

export class EventCardComponent {

    selectedTags: string[] = [];
    events$!: Observable<EventDto[]>;
    currentUserId$!: Observable<string | null>;
    availableTags$!: Observable<{ name: string }[]>;
    private searchTerm$ = new BehaviorSubject<string>('');

    constructor(private eventService: EventService, private router: Router, private store: Store<AppState>) { }

    ngOnInit() {

        this.store.dispatch(loadPublicEvents());
        this.store.dispatch(loadAllTags());
        this.store.dispatch(loadCurrentUser());

        this.currentUserId$ = this.store.select(selectCurrentUser).pipe(map(user => user?.id || null));

        const publicEvents$ = this.store.select(selectPublicEvents);

        this.events$ = combineLatest([publicEvents$, this.searchTerm$]).pipe(
            map(([events, term]) =>
                events.filter(e => e.title.toLowerCase().includes(term.toLowerCase()))
            )
        );

        this.availableTags$ = this.store.select(selectTags).pipe(
            map(tags => tags ? tags.map(t => ({ name: t.name })) : [])
        );
    }

    onSearch(term: string) {
        this.searchTerm$.next(term);
    }


    onToggleJoin(event: EventDto) {
        this.currentUserId$.pipe(take(1)).subscribe(userId => {
            if (!userId) return;

            const action$ = event.isJoined
                ? this.eventService.leaveEvent(event.id)
                : this.eventService.joinEvent(event.id);

            action$.pipe(
                switchMap(() => this.eventService.getPublicEvents())
            ).subscribe({
                next: events => this.events$ = of(events),
                error: err => console.error('Failed to update event', err)
            });
        })
    }

    goToEventDetails(event: EventDto) {
        this.router.navigate(['/events', event.id]);
    }

    onFilter() {
        this.events$ = this.store.select(selectPublicEvents).pipe(
            map(events => {
                if (!this.selectedTags || this.selectedTags.length === 0) {
                    return events;  
                }
                return events.filter(e =>
                    e.tags.some(tag => this.selectedTags.includes(tag.name))
                );
            })
        );
    }

}