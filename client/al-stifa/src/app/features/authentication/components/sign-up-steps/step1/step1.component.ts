import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { MASLAKS } from '@core/constants/maslaks.constants';
import { ROLES } from '@core/constants/role.constants';
import { AuthService } from '@features/authentication/services/auth.service';
import { ToastService } from '@shared/services/toastr.service';

@Component({
  selector: 'app-step1',
  imports: [MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatCheckboxModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule, ReactiveFormsModule,CommonModule],
  templateUrl: './step1.component.html',
  styleUrls: ['./step1.component.scss'],
})
export class Step1Component {
  registerForm: FormGroup;
  hidePassword = true;
  hideConfirmPassword = true;
   maslaks = MASLAKS;
   roles = ROLES;

  constructor(private fb: FormBuilder, private router: Router, private authSvc: AuthService, private toastr: ToastService) {
    this.registerForm = this.fb.group(
      {
        name: ['', [Validators.required, Validators.minLength(2)]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
        role: ['', [Validators.required]],
        maslakId: [''],
      },
      { validators: this.passwordMatchValidator }
    );
  }
 
  ngOnInit(): void {
  this.registerForm.get('role')!.valueChanges.subscribe(selectedRole => {
  const maslakControl = this.registerForm.get('maslakId');

  if (selectedRole === 'Scholar') {
    maslakControl?.setValidators(Validators.required);
  } else {
    maslakControl?.clearValidators();
    maslakControl?.setValue(null);  
  }
  maslakControl?.updateValueAndValidity();
});
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');

    if (
      password &&
      confirmPassword &&
      password.value !== confirmPassword.value
    ) {
      confirmPassword.setErrors({ mismatch: true });
    } else {
      confirmPassword?.setErrors(null);
    }

    return null;
  }

  onSubmit() {
    console.log(this.registerForm.value);
    if (this.registerForm.valid) {
      this.authSvc.registerStep1(this.registerForm.value).subscribe({
        next:(response:any) => {
          this.authSvc.setUserId(response.userId)
          this.toastr.success('Registration Step 1 completed successfully!');
          this.router.navigate(['/register/step2']);
        },
        error: (response) => {
          this.toastr.error(response.error);
          console.error('Registration Step 1 error:', response);
        }
      })
    }
  }

  navigateToLogin(): void {
    // this.router.navigate(['/login']);
  }
}
