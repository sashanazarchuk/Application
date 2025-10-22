import { Component, Input } from "@angular/core";
import { RouterModule } from "@angular/router";
import { NgIf } from "@angular/common";
import { CreateButtonComponent } from "../../../../shared/components/button/create-button/create-button.component";
  
@Component({
    selector: 'app-event-header',
    imports: [CreateButtonComponent, RouterModule, NgIf],
    templateUrl: './event-header.component.html'
})

export class EventHeaderComponent {
    @Input() title = '';
    @Input() subtitle = '';
    @Input() showCreateButton = false;
}