import { Component } from "@angular/core";
import { AIAssistantService } from "../service/ai-assistant.service";
import { ExampleButtonComponent } from "../../../shared/components/button/example-button/example-button.component";
import { FormsModule } from "@angular/forms";
import { NgIf } from "@angular/common";

@Component({
    selector: 'app-ai-assistant',
    imports: [ExampleButtonComponent, FormsModule, NgIf],
    templateUrl: './ai-assistant.page.html',
})

export class AIAssistantPage {
    userMessage: string = '';
    aiReply: string = '';
    loading = false;

    constructor(private aiService: AIAssistantService) { }

    sendToAI() {

        this.loading = true;
        this.aiService.askAI(this.userMessage).subscribe({
            next: (res) => {
                this.aiReply = res.reply;
                this.loading = false;
            },
            error: () => {
                this.aiReply = 'An error occurred. Please try again.';
                this.loading = false;
            }
        });
    }

    setExample(messageToAI: string): void {
        this.userMessage = messageToAI;
        this.sendToAI();
    }


}