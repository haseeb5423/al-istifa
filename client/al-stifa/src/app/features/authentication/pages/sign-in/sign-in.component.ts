import { CommonModule } from '@angular/common';
import { Component, RESPONSE_INIT } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import {  MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { AuthService } from '@features/authentication/services/auth.service';
import { LoadingSpinnerService } from '@shared/services/loading-spinner.service';
import { LoadingSpinnerComponent } from '@shared/components/loading-spinner/loading-spinner.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-sign-in',
  imports: [MatIconModule, MatCheckboxModule, MatFormFieldModule,MatInputModule, ReactiveFormsModule, CommonModule, LoadingSpinnerComponent],
  templateUrl: './sign-in.component.html',
  styleUrl: './sign-in.component.scss'
})
export class SignInComponent {
 loginForm: FormGroup;
  hidePassword = true;

  constructor(
    private fb: FormBuilder,
    private authSvc: AuthService,
    private toastr :ToastrService,
    private router: Router,
  public loadingSpinnerSvc: LoadingSpinnerService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [false]
    });
  }

  ngOnInit(): void {}

  onSubmit(): void {
    this.loadingSpinnerSvc.show();
    if (this.loginForm.valid) {
      this.authSvc.login(this.loginForm.value).subscribe({
        next: (response) => {
          this.toastr.success('Login successful');
          this.loadingSpinnerSvc.hide();
          this.router.navigate(['/profile']);
        },
        error: (response) => {
          this.toastr.error('Login failed', response.error );
          this.loadingSpinnerSvc.hide();
        }
      })
    }
  }

  onGoogleSignIn(): void {
    console.log('Google sign in clicked');
    // Handle Google sign in
  }

  navigateToRegister(): void {
    // this.router.navigate(['/register']);
  }
}
