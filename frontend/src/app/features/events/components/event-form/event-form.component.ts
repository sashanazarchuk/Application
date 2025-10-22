import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CreateEventDto } from "../../models/event.model";
import { FormsModule } from "@angular/forms";
import { NgClass, NgIf } from "@angular/common";

@Component({

    selector: 'app-event-form',
    imports: [FormsModule, NgClass, NgIf],
    templateUrl: './event-form.component.html'
})

export class EventFormComponent {
    @Input() initialData?: CreateEventDto;
    @Output() formSubmit = new EventEmitter<CreateEventDto>();
    @Input() serverError: string | null = null;


    model: CreateEventDto = {
        title: '',
        description: '',
        date: '',
        time: '',
        location: '',
        capacity: null,
        type: 'Public'
    };

    ngOnInit(): void {
        if (this.initialData) {
            this.model = { ...this.initialData };
        }
    }

    onSubmit(form: any) {
        if (form.invalid) return;

        const [year, month, day] = this.model.date.split('-');
        const [hours, minutes] = this.model.time.split(':');

        const eventDate = new Date(+year, +month - 1, +day, +hours, +minutes).toISOString();

        const event = {
            ...this.model,
            date: eventDate
        };

        this.formSubmit.emit(event);
    }

    onCancel() {
        this.model = {
            title: '',
            description: '',
            date: '',
            time: '',
            location: '',
            capacity: null,
            type: 'Public',
        };

        //this.router.navigate(['/events']);
    }
}