import { Component, EventEmitter, Input, Output } from "@angular/core";
import { NgModel } from "@angular/forms";
import { AsyncPipe, NgFor, NgIf } from "@angular/common";
import { AppState } from "../../../../core/store/appState";
import { Store } from "@ngrx/store";
import { firstValueFrom, Observable } from "rxjs";
import { removeSuggestedTag, requestTags, resetAiSuggestedOnce } from "../../store/ai-assistant.actions";
import { selectAiSuggestedOnce, selectSuggestedTags } from "../../store/ai-assistant.selectors";

@Component({
    selector: 'app-ai-tag-suggest',
    imports: [NgIf, NgFor, AsyncPipe],
    templateUrl: './ai-tag-suggestion.component.html'
})

export class AiTagSuggestComponent {

    @Input() title: string = '';
    @Output() tagSelected = new EventEmitter<string>();

    suggestedTags$: Observable<string[]>;
    aiSuggestedOnce$: Observable<boolean>;

    constructor(private store: Store<AppState>) {
        this.suggestedTags$ = this.store.select(selectSuggestedTags);
        this.aiSuggestedOnce$ = this.store.select(selectAiSuggestedOnce);
    }

    async onTitleBlur(titleControl: NgModel) {
        if (!titleControl.valid) return;

        const once = await firstValueFrom(this.aiSuggestedOnce$);
        if (once) return;

        this.store.dispatch(requestTags({ title: this.title }));
    }

    onTitleFocus(titleControl: NgModel) {
        this.store.dispatch(resetAiSuggestedOnce());
    }

    applySuggestedTag(tag: string) {
        this.store.dispatch(removeSuggestedTag({ tag }));
        this.tagSelected.emit(tag);
    }

}