import { Component } from '@angular/core';
import { HeaderComponent } from '../header/header.component';
import { FooterComponent } from '../footer/footer.component';
import { RouterOutlet } from '@angular/router';
import { LoadingSpinnerComponent } from "@shared/components/loading-spinner/loading-spinner.component";
import { CommonModule } from '@angular/common';
import { LoadingSpinnerService } from '@shared/services/loading-spinner.service';
import { debounceTime } from 'rxjs';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [HeaderComponent, FooterComponent, RouterOutlet, LoadingSpinnerComponent, CommonModule],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {
  loading$: any;

  constructor(private loadingSpinnerSvc: LoadingSpinnerService) {
    this.loading$ = this.loadingSpinnerSvc.loadingSpinner$.pipe(debounceTime(100));
  }
}
