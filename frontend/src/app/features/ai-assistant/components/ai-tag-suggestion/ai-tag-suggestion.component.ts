import { Component, EventEmitter, Input, Output } from "@angular/core";
import { AIAssistantService } from "../../service/ai-assistant.service";
import { NgModel } from "@angular/forms";
import { NgFor, NgIf } from "@angular/common";

@Component({
    selector: 'app-ai-tag-suggest',
    imports: [NgIf, NgFor],
    templateUrl: './ai-tag-suggestion.component.html'
})

export class AiTagSuggestComponent {

    @Input() title: string = '';
    @Output() tagSelected = new EventEmitter<string>();

    suggestedTags: string[] = [];
    aiSuggestedOnce = false;

    constructor(private aiService: AIAssistantService) { }

    onTitleBlur(titleControl: NgModel) {
        if (!titleControl.valid || this.aiSuggestedOnce) return;
        this.requestTagRecommendations(this.title);
    }

    onTitleFocus(titleControl: NgModel) {
        this.aiSuggestedOnce = false;
    }

    applySuggestedTag(tag: string) {
        this.suggestedTags = this.suggestedTags.filter(t => t !== tag);
        this.tagSelected.emit(tag);
        this.aiSuggestedOnce = true;
    }

    private requestTagRecommendations(title: string) {
        const message = `Recommend up to 5 tags for an event with title "${title}"`;
        this.aiService.askAI(message).subscribe({
            next: res => this.handleAIResponse(res.reply),
            error: () => console.error('AI tag recommendation failed')
        });
    }

    private handleAIResponse(reply: string) {
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