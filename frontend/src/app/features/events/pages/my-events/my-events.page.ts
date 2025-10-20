import { Component } from '@angular/core';
import { CalendarComponent } from '../../components/calendar/calendar.component';
import { CommonModule } from '@angular/common';
import { EventDto } from '../../models/event.model';
import { EventService } from '../../services/event.service';
import { ButtonComponent } from '../../../../shared/components/button/button.component';

@Component({
    selector: 'app-my-events',
    standalone: true,
    imports: [CommonModule, CalendarComponent, ButtonComponent],
    templateUrl: './my-events.page.html',

})
export class MyEventsPage {

    events: EventDto[] = [];
    constructor(private eventService: EventService) { }

    ngOnInit() {
        this.eventService.getMyEvents().subscribe({
            next: (res) => this.events = res,
            error: (err) => console.error('Failed to load events', err)
        });

    }
}