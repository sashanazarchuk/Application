import { Component } from "@angular/core";
import { EventFormComponent } from "../../components/event-form/event-form.component";
import { EventHeaderComponent } from "../../components/event-header/event-header.component";
import { CreateEventDto, EventDto } from "../../models/event.model";
import { buildPatchDoc, mapEventToForm } from "../../../../core/utils/patch.utils";
import { ActivatedRoute, Router } from "@angular/router";
import { AsyncPipe, NgIf } from "@angular/common";
import { BackButtonComponent } from "../../../../shared/components/button/back-button/back-button.component";
import { Observable, take } from "rxjs";
import { AppState } from "../../../../core/store/appState";
import { Store } from "@ngrx/store";
import { loadEventById, updateEvent } from "../../store/event.actions";
import { selectSelectedEvent } from "../../store/event.selectors";

@Component({

    selector: 'app-edit-event-form',
    imports: [EventFormComponent, EventHeaderComponent, NgIf, BackButtonComponent, AsyncPipe],
    templateUrl: './edit-event.page.html'
})

export class EditEventPage {
    selectedEvent$!: Observable<EventDto | null>;
    modelForForm!: CreateEventDto;
    serverErrors: string | null = null;

    constructor(private route: ActivatedRoute, private store: Store<AppState>) { }

    ngOnInit(): void {
        const eventId = this.route.snapshot.paramMap.get('id');
        if (!eventId) return;

        this.store.dispatch(loadEventById({ eventId }));

        this.selectedEvent$ = this.store.select(selectSelectedEvent);

        this.selectedEvent$.pipe(take(1)).subscribe(event => {
            if (event) this.modelForForm = mapEventToForm(event);
        });
    }

    handleEdit(updatedData: CreateEventDto) {
        this.selectedEvent$.pipe(take(1)).subscribe(originalEvent => {
            if (!originalEvent) return;

            const patchDoc = buildPatchDoc(updatedData, originalEvent);

            this.store.dispatch(updateEvent({ eventId: originalEvent.id, patch: patchDoc }));
        });
    }
}