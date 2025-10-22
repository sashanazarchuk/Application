import { Component } from "@angular/core";
import { EventFormComponent } from "../../components/event-form/event-form.component";
import { EventHeaderComponent } from "../../components/event-header/event-header.component";
import { CreateEventDto, EventDto } from "../../models/event.model";
import { buildPatchDoc, mapEventToForm } from "../../../../core/utils/patch.utils";
import { EventService } from "../../services/event.service";
import { ActivatedRoute, Router } from "@angular/router";
import { NgIf } from "@angular/common";
import { BackButtonComponent } from "../../../../shared/components/button/back-button/back-button.component";

@Component({

    selector: 'app-edit-event-form',
    imports: [EventFormComponent, EventHeaderComponent, NgIf, BackButtonComponent],
    templateUrl: './edit-event.page.html'
})

export class EditEventPage {
    originalEvent!: EventDto;
    modelForForm!: CreateEventDto;
    serverErrors: string | null = null;

    constructor(private route: ActivatedRoute, private eventService: EventService, private router: Router) { }

    ngOnInit(): void {
        const eventId = this.route.snapshot.paramMap.get('id');
        if (!eventId) return;

        this.eventService.getEventById(eventId).subscribe(event => {
            this.originalEvent = event;
            this.modelForForm = mapEventToForm(event);
        });
    }

    handleEdit(updatedData: CreateEventDto) {
        if (!this.originalEvent) return;

        const patchDoc =  buildPatchDoc(updatedData, this.originalEvent);

        this.eventService.editEvent(this.originalEvent.id, patchDoc).subscribe({
            next: updatedEvent => this.router.navigate(['/events', updatedEvent.id]),
            error: err => this.serverErrors = err.error?.errors?.[0] ?? 'Something went wrong'
        });
    }
}