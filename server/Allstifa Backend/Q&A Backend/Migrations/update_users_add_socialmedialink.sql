-- Add SocialMediaLink column to Users table
ALTER TABLE Users
ADD SocialMediaLink NVARCHAR(255) NULL;

-- Example: Insert initial data with SocialMediaLink
INSERT INTO Users (Id, Name, Email, PasswordHash, Role, Bio, ProfileImagePath, MaslakId, IsVerified, ProofFilePath, SocialMediaLink, CreatedAt)
VALUES
  (NEWID(), 'Scholar With Degree', 'scholar1@example.com', 'HASHED_PASSWORD', 'Scholar', 'Has degree', NULL, 1, 1, 'proof1.png', NULL, GETDATE()),
  (NEWID(), 'Scholar Without Degree', 'scholar2@example.com', 'HASHED_PASSWORD', 'Scholar', 'No degree, but active on YouTube', NULL, 2, 0, NULL, 'https://youtube.com/scholar2', GETDATE());
