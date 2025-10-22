import { NgIf } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ActionButtonComponent } from "../button/action-button/action-button.component";

@Component({
    selector: 'app-confirm-modal',
    imports:[NgIf, ActionButtonComponent],
    templateUrl: './confirm-modal.component.html',
})

export class ConfirmModalComponent {
    @Input() show = false;
    @Input() title = 'Confirm';
    @Input() message = 'Are you sure?';

    @Output() confirm = new EventEmitter<void>();
    @Output() cancel = new EventEmitter<void>();

    onConfirm() {
        this.confirm.emit();
    }

    onCancel() {
        this.cancel.emit();
    }
}