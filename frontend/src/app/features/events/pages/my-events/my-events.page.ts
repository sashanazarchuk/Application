import { Component } from '@angular/core';
import { CalendarComponent } from '../../components/calendar/calendar.component';
import { CommonModule } from '@angular/common';
import { EventDto } from '../../models/event.model';
import { EventService } from '../../services/event.service';
import { EventHeaderComponent } from '../../components/event-header/event-header.component';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Component({
    selector: 'app-my-events',
    standalone: true,
    imports: [CommonModule, CalendarComponent, EventHeaderComponent],
    templateUrl: './my-events.page.html',

})
export class MyEventsPage {

    events$!: Observable<EventDto[]>;

    constructor(private eventService: EventService, private router: Router) { }

    ngOnInit() {
        this.events$ = this.eventService.getMyEvents();
    }

    onEventClick(event: any) {
        const eventId = event.id;
        if (eventId) {
            this.router.navigate(['/events', eventId]);
        }
    }

}