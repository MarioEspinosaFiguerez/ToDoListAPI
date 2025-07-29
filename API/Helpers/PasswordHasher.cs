namespace API.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        // El coste de BCrypt (también llamado work factor o log rounds) es el número de iteraciones exponenciales que el algoritmo usa para derivar el hash.
        private int bCryptWorkFactor = 12;

        public string Hash(string password)
        {
            string preHashSha256 = PreHashPassword(password);
            return BCrypt.Net.BCrypt.HashPassword(preHashSha256, bCryptWorkFactor);
        }

        public bool Verify(string password, string hash)
        {
            string preHashSha256 = PreHashPassword(password);
            return BCrypt.Net.BCrypt.Verify(preHashSha256, hash);
        }

        private string PreHashPassword(string password)
        {
            var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}