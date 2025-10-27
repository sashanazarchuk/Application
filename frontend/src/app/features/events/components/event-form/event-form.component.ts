import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CreateEventDto } from "../../models/event.model";
import { FormsModule } from "@angular/forms";
import { CommonModule, NgClass, NgIf } from "@angular/common";
import { TagService } from "../../services/tag.service";
import { TagSelectorComponent } from "../../../../shared/components/tag/tag-selector/tag-selector.component";

@Component({

    selector: 'app-event-form',
    imports: [FormsModule, NgClass, NgIf, CommonModule, TagSelectorComponent],
    templateUrl: './event-form.component.html',
})

export class EventFormComponent {
    @Input() initialData?: CreateEventDto;
    @Output() formSubmit = new EventEmitter<CreateEventDto>();
    @Input() serverError: string | null = null;

    availableTags: { name: string }[] = [];

    model: CreateEventDto = {
        title: '',
        description: '',
        date: '',
        time: '',
        location: '',
        capacity: null,
        type: 'Public',
        tagNames: []
    };


    constructor(private tagService: TagService) { }

    ngOnInit(): void {
        if (this.initialData) {
            this.model = { ...this.initialData, tagNames: this.initialData.tagNames.map(t => t) };
        }

        this.tagService.getAllTags().subscribe(tags => {
            this.availableTags = tags.map(tag => ({ name: tag.name }));
        });

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
            tagNames: []
        };

        //this.router.navigate(['/events']);
    }
}