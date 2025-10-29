import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CreateEventDto } from "../../models/event.model";
import { FormsModule, NgForm } from "@angular/forms";
import { CommonModule, NgClass, NgIf } from "@angular/common";
import { TagSelectorComponent } from "../../../../shared/components/tag/tag-selector/tag-selector.component";
import { AiTagSuggestComponent } from "../../../ai-assistant/components/ai-tag-suggestion/ai-tag-suggestion.component";
import { combineDateTime } from "../../../../core/utils/date.utils";
import { map, Observable } from "rxjs";
import { loadAllTags } from "../../store/event.actions";
import { AppState } from "../../../../core/store/appState";
import { Store } from "@ngrx/store";
import { selectTags } from "../../store/event.selectors";

@Component({
    selector: 'app-event-form',
    imports: [FormsModule, NgClass, NgIf, CommonModule, TagSelectorComponent, AiTagSuggestComponent],
    templateUrl: './event-form.component.html',
})

export class EventFormComponent {
    @Input() initialData?: CreateEventDto;
    @Output() formSubmit = new EventEmitter<CreateEventDto>();
    @Input() serverError: string | null = null;

    availableTags$!: Observable<{ name: string }[]>;

    model: CreateEventDto = this.getEmptyModel();

    constructor(private store: Store<AppState>) { }

    ngOnInit(): void {
        if (this.initialData) {
            this.model = { ...this.initialData, tagNames: this.initialData.tagNames.map(t => t) };
        }

        this.store.dispatch(loadAllTags());
        this.availableTags$ = this.store.select(selectTags).pipe(
            map(tags => tags ? tags.map(t => ({ name: t.name })) : [])
        );
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