import { CommonModule, NgIf } from '@angular/common';
import { Component, Input } from '@angular/core';
import { } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-create-button',
  imports: [RouterModule, NgIf, CommonModule],
  templateUrl: './create-button.component.html',
})
export class CreateButtonComponent {
  @Input() text = '';
  @Input() link: string | null = null;
  @Input() bgColor = 'bg-violet-500';
  @Input() hoverColor = 'hover:bg-blue-700';
}
