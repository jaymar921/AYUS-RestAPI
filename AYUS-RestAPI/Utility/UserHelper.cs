using AYUS_RestAPI.Entity.Metadata;

namespace AYUS_RestAPI.Utility
{
    public static class UserHelper
    {
        public static User CreateUser(this User user, PersonalInformation personal, Credential credential, Wallet wallet, AccountStatus account, List<Vehicle> vehicles)
        {
            string uuid = personal.UUID;
            credential.UUID = uuid;
            wallet.UUID = uuid;
            account.UUID = uuid;

            return new User(personal, credential, wallet, account, vehicles);
        }

        public static string HashMD5(this string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +

                // Convert the byte array to hexadecimal string prior to .NET 5
                // StringBuilder sb = new System.Text.StringBuilder();
                // for (int i = 0; i < hashBytes.Length; i++)
                // {
                //     sb.Append(hashBytes[i].ToString("X2"));
                // }
                // return sb.ToString();
            }
        }
    }
}
