import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { FooterLink, Step } from '@shared/models/landing.model';
import { FOOTER_LINKS } from '@core/constants/footer.constants';
import { HOW_IT_WORKS_STEPS, TRUST_FEATURES } from '@core/constants/landing.constants';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  imports: [MatIconModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})


export class HomeComponent {
 howItWorksSteps: Step[] = HOW_IT_WORKS_STEPS;
  trustFeatures: string[] = TRUST_FEATURES;
  footerLinks: FooterLink[] = FOOTER_LINKS;
  currentYear: number = new Date().getFullYear();

  ngOnInit() {}

  // Navigation methods
  onAskQuestion(): void {
    console.log('Navigate to ask question');
  }

  onLogin(): void {
    console.log('Navigate to login');
  }

  onRegister(): void {
    console.log('Navigate to register');
  }

  // Track by functions for ngFor
  trackByStep(index: number, step: Step): string {
    return step.title;
  }

  trackByFeature(index: number, feature: string): string {
    return feature;
  }

  trackByLink(index: number, link: FooterLink): string {
    return link.url;
  }
}
