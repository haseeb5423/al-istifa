import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastService } from '@shared/services/toastr.service';
import { ProfileService } from '@features/profile/services/profile.service';
import { MASLAKS } from '@core/constants/maslaks.constants';
import { Profile } from '@features/profile/models/profile.model';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  profile!:Profile
  maslaks = MASLAKS;

  private profileSvc = inject(ProfileService)
  private toastr = inject(ToastService);



  isEditing = false;
  editableProfile: Profile = { ...this.profile };
  selectedFile: File | null = null;

  ngOnInit() {
    this.getProfile('44490265-07B6-4A8E-8838-5E3C3D3F70D2');
  }
  getProfile(id: string) {
    this.profileSvc.getProfile(id).subscribe({
      next: (res: any) => {
        this.profile = res
    this.resetEditableProfile();

      },
      error: (res: any) => {
        console.log(res.error);
        this.toastr.error(res.error)
      }
    })
  }

  toggleEdit() {
    if (this.isEditing) {
      this.resetEditableProfile();
    }
    this.isEditing = !this.isEditing;
  }

  saveProfile() {
    // Here you would typically call your API service
    this.profile = { ...this.editableProfile };
    this.isEditing = false;
    // Simulate API call success
    console.log('Profile updated:', this.profile);
  }

  resetEditableProfile() {
    this.editableProfile = { ...this.profile };
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      // Create preview URL
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.editableProfile.profileImagePath = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  getMaslakName(id?: number): string {
    if (!id) return 'Not specified';
    const maslak = this.maslaks.find(m => m.maslakId === id);
    return maslak ? maslak.name : 'Unknown';
  }

  getRoleColor(role: string): string {
    switch (role.toLowerCase()) {
      case 'scholar': return 'role-scholar';
      case 'student': return 'role-student';
      case 'admin': return 'role-admin';
      default: return 'role-user';
    }
  }

  formatDate(dateStr: string | null | undefined): string {
  if (!dateStr) return 'N/A';

  const date = new Date(dateStr);
  if (isNaN(date.getTime())) return 'Invalid date';

  return date.toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  });
}

}