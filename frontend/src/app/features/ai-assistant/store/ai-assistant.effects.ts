import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { AIAssistantService } from "../service/ai-assistant.service";
import * as AIAssistantActions from './ai-assistant.actions';
import { catchError, map, of, switchMap } from "rxjs";



@Injectable()
export class AIAssistantEffects {

    private actions$ = inject(Actions);
    private aiService = inject(AIAssistantService);

    requestTags$ = createEffect(() =>
        this.actions$.pipe(
            ofType(AIAssistantActions.requestTags),
            switchMap(action =>
                this.aiService.askAI(`Recommend up to 5 tags for an event with title "${action.title}"`).pipe(
                    map(response => {
                        const match = response.reply.match(/\[.*?\]/s);
                        if (!match) throw new Error("No JSON array found");
                        const tags = JSON.parse(match[0]) as string[];
                        return AIAssistantActions.requestTagsSuccess({ tags });
                    }),
                    catchError(() => of(AIAssistantActions.requestTagsFailure()))
                )
            )
        )
    );

    sendMessage$ = createEffect(() =>
        this.actions$.pipe(
            ofType(AIAssistantActions.sendMessage),
            switchMap(action =>
                this.aiService.askAI(action.userMessage).pipe(
                    map(response => AIAssistantActions.sendMessageSuccess({ reply: response.reply })),
                    catchError(() => of(AIAssistantActions.sendMessageFailure()))
                )
            )
        )
    );
}