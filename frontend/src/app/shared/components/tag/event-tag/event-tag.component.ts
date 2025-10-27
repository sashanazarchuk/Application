import { Component, Input } from "@angular/core";
import { CommonModule, NgFor, NgIf } from "@angular/common";
import { TagDto } from "../../../../features/events/models/tag.model";

@Component({

    selector: 'app-event-tags',
    imports: [NgIf, NgFor, CommonModule],
    templateUrl: './event-tag.component.html',

})

export class EventTagsComponent {
    @Input() tags: TagDto[] | null | undefined;


    getTagBgColor(tagName: string): string {
        const colors = ['bg-blue-500', 'bg-green-500', 'bg-red-500', 'bg-yellow-500', 'bg-purple-500'];
        const index = tagName.charCodeAt(0) % colors.length;
        return colors[index];
    }
}