import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
 
@Component({
    selector: 'app-back-button',
    imports:[RouterModule],
    templateUrl: './back-button.component.html',
})
export class BackButtonComponent {
    @Input() link: string = '/events';
    @Input() text: string = 'Back';
}