import { Component, inject, OnInit, ChangeDetectorRef, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Profile } from '@shared/components/profile/models/profile.model';
import { ProfileService } from '@shared/components/profile/services/profile.service'; 
import { AuthService } from '@features/authentication/services/auth.service';
import { ToastService } from '@shared/services/toastr.service';
import { MASLAKS } from '@core/constants/maslaks.constants';
import { LoadingSpinnerService } from '@shared/services/loading-spinner.service';
import { LoadingSpinnerComponent } from "@shared/components/loading-spinner/loading-spinner.component";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  @Input() profileData!: Profile;
  @Input() profileForm!: FormGroup;
  @Input() maslaks = MASLAKS;
  @Input() isEditing! : boolean;
  @Input() proofFile: File | null = null;
  @Input() imagePreview: string | ArrayBuffer | null = null;
  @Output() editClicked = new EventEmitter<boolean>();
  @Output() cancelEditClicked = new EventEmitter<boolean>();
  @Output() avtarSelected = new EventEmitter<any>();
  @Output() proofFileSelected = new EventEmitter<any>();
  @Output() saveClicked = new EventEmitter<boolean>();
  constructor() {}
  
  ngOnInit(): void {
    
  }
  
  
  get selectedMaslakName(): string {
    const maslakId = this.profileData?.maslakId;
    const found = this.maslaks.find((m: { maslakId: number; name: string }) => m.maslakId === maslakId);
    return found ? found.name : 'Not selected';
  }
  
  onEditing() {
    this.editClicked.emit(true);
  }
  
  
  
  
  
  onCancelEdit(): void {
    this.cancelEditClicked.emit(false);
  }
  onSaveClicked(): void {
    this.saveClicked.emit();
  }
  
  onAvatarSelected(event: any) : void {
    const file = event.target.files[0];
    if (file) {
      this.avtarSelected.emit(file);
    }
  }
  
  onProofFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.proofFileSelected.emit(file);
    }
  }
  
  
}