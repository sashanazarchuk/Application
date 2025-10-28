import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CreateEventDto } from "../../models/event.model";
import { FormsModule, NgModel } from "@angular/forms";
import { CommonModule, NgClass, NgIf } from "@angular/common";
import { TagService } from "../../services/tag.service";
import { TagSelectorComponent } from "../../../../shared/components/tag/tag-selector/tag-selector.component";
import { AIAssistantService } from "../../../ai-assistant/service/ai-assistant.service";

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

    suggestedTags: string[] = [];
    aiSuggestedOnce = false;

    constructor(private tagService: TagService, private aiService: AIAssistantService) { }

    ngOnInit(): void {
        if (this.initialData) {
            this.model = { ...this.initialData, tagNames: this.initialData.tagNames.map(t => t) };
        }

        this.tagService.getAllTags().subscribe(tags => {
            this.availableTags = tags.map(tag => ({ name: tag.name }));
        });

    }

    onSubmit(form: any) {
        if (form.invalid || !this.model.tagNames || this.model.tagNames.length === 0) {
            form.control.markAllAsTouched();
            return;
        }

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

    }

    onTitleBlur(titleControl: NgModel) {
        if (!titleControl.valid || !this.shouldAskAI(titleControl)) return;

        this.requestTagRecommendations(this.model.title);
    }

    
    onTitleFocus(titleControl: NgModel) {
        
        this.aiSuggestedOnce = false;
    }

    applySuggestedTag(tag: string) {

        if (!this.model.tagNames.includes(tag) && this.model.tagNames.length < 5) {
            this.model.tagNames = [...this.model.tagNames, tag];
        }
        
        this.suggestedTags = this.suggestedTags.filter(t => t !== tag);
    }
    
    private shouldAskAI(titleControl: NgModel): boolean {
        return !this.aiSuggestedOnce || titleControl.touched === false;
    }

    private requestTagRecommendations(title: string) {
        const message = `Recommend up to 5 tags for an event with title "${title}"`;

        this.aiService.askAI(message).subscribe({
            next: res => this.handleAIResponse(res.reply),
            error: () => console.error('AI tag recommendation failed')
        });
    }

    private handleAIResponse(reply: string) {
        this.suggestedTags = [];
        try {

            const match = reply.match(/\[.*?\]/s);
            if (!match) throw new Error("No JSON array found");

            const tags = JSON.parse(match[0]) as string[];
            this.suggestedTags = tags;
            this.aiSuggestedOnce = true;
        } catch (err) {
            console.warn('AI response is not a valid JSON array:', reply, err);
        }
    }
}