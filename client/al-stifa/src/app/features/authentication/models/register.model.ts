export interface Step1{
    fullName: string;
    email: string;
    password: string;
    role: string;
    maslakId: number;
}

export interface Step2 {
    userId: number;
    bio: string;
    profileImage: File | null;
    proofFile: File | null;
    socialMediaLink: string;
  }
  