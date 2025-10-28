import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CreateEventDto } from "../../models/event.model";
import { FormsModule, NgForm } from "@angular/forms";
import { CommonModule, NgClass, NgIf } from "@angular/common";
import { TagService } from "../../services/tag.service";
import { TagSelectorComponent } from "../../../../shared/components/tag/tag-selector/tag-selector.component";
import { AiTagSuggestComponent } from "../../../ai-assistant/components/ai-tag-suggestion/ai-tag-suggestion.component";
import { combineDateTime } from "../../../../core/utils/date.utils";

@Component({
    selector: 'app-event-form',
    imports: [FormsModule, NgClass, NgIf, CommonModule, TagSelectorComponent, AiTagSuggestComponent],
    templateUrl: './event-form.component.html',
})

export class EventFormComponent {
    @Input() initialData?: CreateEventDto;
    @Output() formSubmit = new EventEmitter<CreateEventDto>();
    @Input() serverError: string | null = null;

    availableTags: { name: string }[] = [];

    model: CreateEventDto = this.getEmptyModel();

    constructor(private tagService: TagService) { }

    ngOnInit(): void {
        if (this.initialData) {
            this.model = { ...this.initialData, tagNames: this.initialData.tagNames.map(t => t) };
        }

        this.tagService.getAllTags().subscribe(tags => {
            this.availableTags = tags.map(tag => ({ name: tag.name }));
        });

    }

    onSubmit(form: NgForm) {
        if (form.invalid || !this.model.tagNames || this.model.tagNames.length === 0) {
            form.control.markAllAsTouched();
            return;
        }

        const eventDate = combineDateTime(this.model.date, this.model.time);

        const event = {
            ...this.model,
            date: eventDate
        };

        this.formSubmit.emit(event);
    }

    onCancel(form?: NgForm) {
        this.model = this.getEmptyModel();
        form?.resetForm(this.model);
    }

    applySuggestedTag(tag: string) {
        if (!this.model.tagNames.includes(tag) && this.model.tagNames.length < 5) {
            this.model.tagNames = [...this.model.tagNames, tag];
        }
    }

    private getEmptyModel(): CreateEventDto {
        return {
            title: '',
            description: '',
            date: '',
            time: '',
            location: '',
            capacity: null,
            type: 'Public',
            tagNames: []
        };
    }
}