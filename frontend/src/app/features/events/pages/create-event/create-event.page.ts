import { Component } from "@angular/core";
import { EventService } from "../../services/event.service";
import { CreateEventDto } from "../../models/event.model";
import { Router } from "@angular/router";
import { EventFormComponent } from "../../components/event-form/event-form.component";
import { EventHeaderComponent } from "../../components/event-header/event-header.component";
import { ErrorService } from "../../../../core/services/error.service";
import { BackButtonComponent } from "../../../../shared/components/button/back-button/back-button.component";
 
@Component({

    selector: 'app-create-event-form',
    imports: [EventFormComponent, EventHeaderComponent, BackButtonComponent],
    templateUrl: './create-event.page.html'
})

export class CreateEventPage {

    serverErrors: string | null = null;
 
    constructor(private eventService: EventService, private errorService: ErrorService, private router: Router) { }

    handleCreate(eventData: CreateEventDto) {
        this.eventService.createEvent(eventData).subscribe({
            next: (createdEvent) => {
                 if (createdEvent?.id) {
                this.router.navigate(['/events', createdEvent.id])};
            },
            error: err => {
                const msg = this.errorService.parseValidationErrors(err);
                this.serverErrors = msg;
            }
        });
    }

}