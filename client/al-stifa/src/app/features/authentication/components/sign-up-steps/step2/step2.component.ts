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
import { AuthService } from '@features/authentication/services/auth.service';
import { ToastService } from '@shared/services/toastr.service';

@Component({
  selector: 'app-step2',
  imports: [MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatCheckboxModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule, ReactiveFormsModule,CommonModule],
    templateUrl: './step2.component.html',
    styleUrl: './step2.component.scss'
  })
  export class Step2Component {
    profileForm: FormGroup;
    selectedFile: File | null = null;
    userId : string = ''
    
    bioWordCount: number = 0;
    wordCountExceeded: boolean = false;
    
    profileImagePreview: string | null = null;
    selectedProfileImage: File | null = null;
    
    constructor(private fb: FormBuilder, private authSvc:AuthService, private toastr : ToastService, private router :Router)
    {
      this.profileForm = this.fb.group({
        
        bio: ['', [Validators.maxLength(500)]],
        socialLink: ['', [Validators.pattern(/^https?:\/\/.+/)]],
      });
      
      
    }
    
    ngOnInit(): void {
      this.userId= this.authSvc.getUserId
    }
    
    onFileSelected(event: any): void {
      const file = event.target.files[0];
      if (file) {
        this.selectedFile = file;
        console.log('File selected:', file.name);
      }
    }
    
    onProfileImageSelected(event: Event): void {
      const file = (event.target as HTMLInputElement).files?.[0];
      if (file) {
        this.selectedProfileImage = file;
        
        const reader = new FileReader();
        reader.onload = () => {
          this.profileImagePreview = reader.result as string;
        };
        reader.readAsDataURL(file);
      }
    }
    onUpload(): void {
      if (this.selectedFile) {
        console.log('Uploading file:', this.selectedFile.name);
      }
    }
    formDataFormation(){
      this.userId = this.authSvc.getUserId
      const formData = new FormData();
      formData.append('userId', this.userId)
      formData.append('bio', this.profileForm.get('bio')?.value),
      formData.append('socialLink', this.profileForm.get('socialLink')?.value)
      
      if(this.selectedFile){
        formData.append('proofFile', this.selectedFile)
      }
      if(this.selectedProfileImage){
        formData.append('profileImage', this.selectedProfileImage)
      }
      return formData;
      
    }
    
    onSubmit(): void {
      if (this.profileForm.valid) {
        const formData = this.formDataFormation();
        this.authSvc.registerStep2(formData).subscribe({
          next:(response) =>{
            this.toastr.success('Profile Created Successfully!')
            this.router.navigate(['/login']);
          }, 
          error : (response)=>{
            console.log('Res:' , response)
            this.toastr.error('Error Occured!', response.error)
          }
        })
        
        // Handle profile setup completion
        // this.router.navigate(['/login']);
      }
    }
    
    updateWordCount() {
      const bio = this.profileForm.get('bio')?.value || '';
      const words = bio.trim().split(/\s+/).filter((word: any) => word.length > 0);
      this.bioWordCount = words.length;
      this.wordCountExceeded = this.bioWordCount > 100;
      
      // Set a custom error if the word count exceeds 100, else remove the error
      const bioControl = this.profileForm.get('bio');
      if (this.wordCountExceeded) {
        bioControl?.setErrors({ maxWordsExceeded: true });
      } else {
        bioControl?.setErrors(null); // Remove the error
      }
    }
    
  }
  