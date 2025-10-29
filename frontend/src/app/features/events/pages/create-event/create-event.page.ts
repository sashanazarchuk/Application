import { Component } from "@angular/core";
import { CreateEventDto } from "../../models/event.model";
import { EventFormComponent } from "../../components/event-form/event-form.component";
import { EventHeaderComponent } from "../../components/event-header/event-header.component";
import { BackButtonComponent } from "../../../../shared/components/button/back-button/back-button.component";
import { AppState } from "../../../../core/store/appState";
import { Store } from "@ngrx/store";
import { createEvent } from "../../store/event.actions";
 
@Component({

    selector: 'app-create-event-form',
    imports: [EventFormComponent, EventHeaderComponent, BackButtonComponent],
    templateUrl: './create-event.page.html'
})

export class CreateEventPage {

    serverErrors: string | null = null;
 
    constructor(private store: Store<AppState>) { }

    handleCreate(eventData: CreateEventDto) {
         this.store.dispatch(createEvent({ dto: eventData }));
    }
}