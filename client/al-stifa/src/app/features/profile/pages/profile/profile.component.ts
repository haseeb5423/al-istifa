import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Profile } from '@features/profile/models/profile.model';
import { ProfileService } from '@features/profile/services/profile.service';
import { AuthService } from '@features/authentication/services/auth.service';
import { ToastService } from '@shared/services/toastr.service';
import { MASLAKS } from '@core/constants/maslaks.constants';
import { LoadingSpinnerService } from '@shared/services/loading-spinner.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  get selectedMaslakName(): string {
    const maslakId = this.profileData?.maslakId;
    const found = this.maslaks.find((m: { maslakId: number; name: string }) => m.maslakId === maslakId);
    return found ? found.name : 'Not selected';
  }
  profileData!: Profile;
  userId: string = '';
  maslaks = MASLAKS;
  isEditing = false;
  profileForm!: FormGroup;
  imagePreview: string | ArrayBuffer | null = null;
  selectedAvatarFile: File | null = null;
  selectedProofFile: File | null = null;

  private profileSvc = inject(ProfileService);
  private authSvc = inject(AuthService);
  private toastr = inject(ToastService);
  private fb = inject(FormBuilder);
  private cdr = inject(ChangeDetectorRef);
  private loadingSpinnerSvc = inject(LoadingSpinnerService);
  constructor() {}

  ngOnInit(): void {
    this.loadingSpinnerSvc.show();
    this.authSvc.UserId$.subscribe((id) => {
      if (id) {
        this.userId = id;
        this.profileSvc.getProfile(this.userId).subscribe({
          next: (res: any) => {
            this.profileData = res;
            this.initForm();
            this.loadingSpinnerSvc.hide();
          },
          error: (res: any) => {
            this.toastr.error('Error while fetching Profile', res.error.message);
            this.loadingSpinnerSvc.hide();
          },
        });
      }
    });
  }

  initForm() {
    this.profileForm = this.fb.group({
      name: [{ value: this.profileData?.name || '', disabled: true }, [Validators.required, Validators.minLength(4)]],
      email: [{ value: this.profileData?.email || '', disabled: true }, [Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$')]],
      role: [{ value: this.profileData?.role || '', disabled: true }],
      bio: [{ value: this.profileData?.bio || '', disabled: true }],
      maslakId: [{ value: this.profileData?.maslakId || '', disabled: true }],
      socialMediaLink: [{ value: this.profileData?.socialMediaLink || '', disabled: true }, [Validators.pattern(/^https?:\/\/.+/)]],
    });
  }

  onEditing() {
    this.isEditing = true;
    this.profileForm.enable();
  }

  saveChanges() {
    if (this.profileForm.invalid) {
      this.toastr.error('Form is invalid', 'Error');
      return;
    }
    
    const formData = this.formDataFormation();
    this.loadingSpinnerSvc.show();
    this.profileSvc.updateProfile(this.userId, formData).subscribe({
      next: (res: any) => {
        this.profileData = res;

        if (this.profileData.profileImagePath) {
          this.profileData.profileImagePath = 
            `${this.profileData.profileImagePath}?t=${new Date().getTime()}`;
        }

        this.profileForm.reset(this.profileData);

        this.isEditing = false;
        this.profileForm.disable(); 
        this.selectedAvatarFile = null;
        this.selectedProofFile = null;
        this.imagePreview = null;
        this.loadingSpinnerSvc.hide();
        
        this.toastr.success('Profile Updated Successfully', 'Success');
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.toastr.error('Error while updating profile', err.error);
      },
    });
  }

  onCancelEdit(): void {
    this.isEditing = false;
    this.profileForm.reset(this.profileData);
    this.profileForm.disable();
    
    this.imagePreview = null;
    this.selectedAvatarFile = null;
    this.selectedProofFile = null;
  }

  onAvatarSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedAvatarFile = file;
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }

  onProofFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedProofFile = file;
    }
  }

  formDataFormation(): FormData {
    const formData = new FormData();
    const formValues = this.profileForm.getRawValue();

    // Append form values, but only if they are not null/undefined
    Object.keys(formValues).forEach(key => {
      const value = formValues[key];
      if (value !== null && value !== undefined) {
        formData.append(key, value);
      }
    });

    // Use .set() to ensure files overwrite any other value
    if (this.selectedAvatarFile) {
      formData.set('profileImage', this.selectedAvatarFile);
    }

    if (this.selectedProofFile) {
      formData.set('proofFile', this.selectedProofFile);
    }

    return formData;
  }
}