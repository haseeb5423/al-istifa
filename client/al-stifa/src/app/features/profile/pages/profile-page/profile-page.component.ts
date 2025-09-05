import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MASLAKS } from '@core/constants/maslaks.constants';
import { AuthService } from '@features/authentication/services/auth.service';
import { Profile } from '@shared/components/profile/models/profile.model';
import { ProfileComponent } from "@shared/components/profile/pages/profile/profile.component";
import { ProfileService } from '@shared/components/profile/services/profile.service';
import { LoadingSpinnerService } from '@shared/services/loading-spinner.service';
import { ToastService } from '@shared/services/toastr.service';

@Component({
  selector: 'app-profile-page',
  imports: [ProfileComponent],
  templateUrl: './profile-page.component.html',
  styleUrl: './profile-page.component.scss'
})
export class ProfilePageComponent {

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
    this.authSvc.UserId$.subscribe((id) => {
      if (id) {
        this.userId = id;
        this.loadingSpinnerSvc.show();
        this.profileSvc.getProfile(this.userId).subscribe({
          next: (res: any) => {
            this.profileData = res;
            this.loadingSpinnerSvc.hide();
            this.initForm();
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

 onSaveChanges() {
   this.saveChanges();
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
        this.isEditing = false;
        
      },
      error: (err: any) => {
        this.toastr.error('Error while updating profile', err.error);
      },
    });
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

  handleEditClick(event: boolean) {
    this.isEditing = event;
    this.profileForm.enable();
  }
  handleCancelEdit(event: boolean) {
    this.isEditing = event;
    this.profileForm.reset(this.profileData);
    this.profileForm.disable();
    this.imagePreview = null;
    this.selectedAvatarFile = null;
    this.selectedProofFile = null;
  }
  handleAvatarSelection(file: any) {
    this.selectedAvatarFile = file;
    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview = reader.result;
    };
    reader.readAsDataURL(file);
  } 

}
