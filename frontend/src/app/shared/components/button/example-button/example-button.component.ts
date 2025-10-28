import { Component, EventEmitter, Input, Output } from "@angular/core";

@Component({
    selector: 'app-example-button',
    templateUrl: './example-button.component.html',
})

export class ExampleButtonComponent {
    @Input() text!: string;
    @Output() selectExample = new EventEmitter<string>();

    onClick() {
        this.selectExample.emit(this.text);
    }
}