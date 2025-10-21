import { Component, Input } from "@angular/core";
import { ButtonComponent } from "../../../../shared/components/button/button.component";
import { RouterModule } from "@angular/router";
import { NgIf } from "@angular/common";

@Component({
    selector: 'app-event-header',
    imports: [ButtonComponent, RouterModule, NgIf],
    templateUrl: './event-header.component.html'
})

export class EventHeaderComponent {
    @Input() title = '';
    @Input() subtitle = '';
    @Input() showCreateButton = false;
}