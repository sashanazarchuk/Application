import { Component } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { map, Observable } from "rxjs";
import { EventDto } from "../../models/event.model";
import { EventService } from "../../services/event.service";
import { AsyncPipe, DatePipe, NgFor, NgIf } from "@angular/common";

@Component({
    selector: 'app-event-card',
    imports: [FormsModule, DatePipe, AsyncPipe, NgIf, NgFor],
    templateUrl: './event-card.component.html',
})

export class EventCardComponent {

    events$: Observable<EventDto[]>;

    constructor(private eventService: EventService) {
        this.events$ = this.eventService.getPublicEvents().pipe(
            map(events => events || [])
        );
    }

    onEventClick(event: EventDto) {
        console.log('Clicked event:', event);

    }


}