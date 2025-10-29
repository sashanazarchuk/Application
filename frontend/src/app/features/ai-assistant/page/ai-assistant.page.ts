import { Component } from "@angular/core";
import { ExampleButtonComponent } from "../../../shared/components/button/example-button/example-button.component";
import { FormsModule } from "@angular/forms";
import { AsyncPipe, NgIf } from "@angular/common";
import { AppState } from "../../../core/store/appState";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { selectAiReply, selectIsLoading } from "../store/ai-assistant.selectors";
import { sendMessage } from "../store/ai-assistant.actions";

@Component({
    selector: 'app-ai-assistant',
    imports: [ExampleButtonComponent, FormsModule, NgIf, AsyncPipe],
    templateUrl: './ai-assistant.page.html',
})

export class AIAssistantPage {

    aiReply$: Observable<string>;
    loading$: Observable<boolean>;
    userMessage: string = '';

    constructor(private store: Store<AppState>) {
        this.aiReply$ = this.store.select(selectAiReply);
        this.loading$ = this.store.select(selectIsLoading);
    }

    sendToAI(userMessage: string) {
        this.store.dispatch(sendMessage({ userMessage }));
    }

    setExample(messageToAI: string): void {
        this.sendToAI(messageToAI);
    }
}