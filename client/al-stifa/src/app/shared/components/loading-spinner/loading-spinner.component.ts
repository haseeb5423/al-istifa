import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading-spinner',
  imports: [CommonModule],
  templateUrl: './loading-spinner.component.html',
  styleUrl: './loading-spinner.component.scss'
})
export class LoadingSpinnerComponent {
 @Input() size: 'small' | 'medium' | 'large' = 'large';
  @Input() message: string = 'Loading...';
  @Input() showMessage: boolean = true;
  @Input() overlay: boolean = true;
  @Input() theme: 'light' | 'dark' = 'light';
}
