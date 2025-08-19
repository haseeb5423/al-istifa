namespace Q_A_Backend.Services.Interfaces
{
    public interface IPasswordService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string hashedPassword, string password);
    }
}
