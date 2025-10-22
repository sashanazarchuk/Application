import { Component, EventEmitter, Input, Output } from "@angular/core";

@Component({
    selector: 'app-action-button',
    templateUrl: './action-button.component.html',
})
export class ActionButtonComponent {
    @Input() text: string = '';
    @Input() bgColor: string = 'bg-blue-500';
    @Input() hoverColor: string = 'hover:bg-blue-600';
    @Input() disabled: boolean = false;
    @Input() extraClasses: string = '';

    @Output() clickEvent = new EventEmitter<void>();

    onClick() {
        this.clickEvent.emit();
    }
}