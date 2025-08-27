import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@features/authentication/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-header',
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  authCheck : boolean = false;
  /**
   *
   */
  constructor(private router: Router, private authSvc: AuthService, private toastr: ToastrService) {
    this.authSvc.isAuthenticated$.subscribe(isAuth => {
      console.log('Authentication status changed:', isAuth);
      this.authCheck = isAuth;
    });
    
  }
onLogin(): void {
    this.router.navigate(['/login']);
    console.log('Navigate to login');
  }
  onLogout(): void {
    this.authSvc.logout();
    this.router.navigate(['/login']);
    this.toastr.success('Logged out successfully');
    
  }

  onRegister(): void {
    this.router.navigate(['register/step1']);
    console.log('Navigate to register');
  }
}
