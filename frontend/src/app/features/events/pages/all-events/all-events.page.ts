import { Component } from "@angular/core";
import { EventHeaderComponent } from "../../components/event-header/event-header.component";
import { EventCardComponent } from "../../components/event-card/event-card.component";


@Component({
    selector: 'app-all-event',
    imports: [EventHeaderComponent, EventCardComponent],
    templateUrl: './all-events.page.html'
})

export class AllEventPage {

}