import { CommonModule, NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';
import { } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-button',
  imports: [RouterModule, NgIf, CommonModule],
  templateUrl: './button.component.html',
})
export class ButtonComponent {
  @Input() text = '';
  @Input() link: string | null = null;
  @Input() bgColor = 'bg-violet-500';
  @Input() hoverColor = 'hover:bg-blue-700';
}
