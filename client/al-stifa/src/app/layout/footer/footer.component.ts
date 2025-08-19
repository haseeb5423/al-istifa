import { Component } from '@angular/core';
import { FOOTER_LINKS } from '../../core/constants/footer.constants';
import { FooterLink } from '../../shared/models/landing.model';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-footer',
  imports: [RouterModule, CommonModule],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss'
})
export class FooterComponent {
footerLinks: FooterLink[] = FOOTER_LINKS;
  currentYear: number = new Date().getFullYear()
}
