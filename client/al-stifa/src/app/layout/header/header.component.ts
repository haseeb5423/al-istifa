import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {

  /**
   *
   */
  constructor(private router: Router) {
    
  }
onLogin(): void {
    this.router.navigate(['/login']);
    console.log('Navigate to login');
  }

  onRegister(): void {
    this.router.navigate(['register/step1']);
    console.log('Navigate to register');
  }
}
