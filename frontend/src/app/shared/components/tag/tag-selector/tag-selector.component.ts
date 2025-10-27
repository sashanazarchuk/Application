import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NgSelectComponent } from "@ng-select/ng-select";

@Component({
    selector: 'app-tag-selector',
    imports: [NgSelectComponent, FormsModule, CommonModule],
    templateUrl: './tag-selector.component.html',
    styleUrl: './tag-selector.component.css'
})

export class TagSelectorComponent {
  @Input() availableTags: { name: string }[] = [];
  @Input() selectedTags: string[] = [];
  @Input() allowAdd = false;
  @Input() maxTags = 5;

  @Output() selectedTagsChange = new EventEmitter<string[]>();

  onChange(tags: string[]) {
    this.selectedTagsChange.emit(tags);
  }
}