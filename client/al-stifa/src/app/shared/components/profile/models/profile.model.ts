export interface Profile{
  id: string;
  name: string;
  email: string;
  role: string; 
  bio?: string;
  profileImagePath?: string;
  maslakId?: number;
  isVerified: boolean;
  proofFilePath?: string;
  socialMediaLink?: string;
  createdAt: string;

}