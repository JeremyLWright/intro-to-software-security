using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OTPHelpers
{
    public class OTPChecker
    {
        public static string UserVerificationKey = "mBBK6FBmy28tWcgmCwrvfvtq";

        public static String GetOTP(String password, String nonce)
        {
            HMACSHA1 hmac_sha1 = new HMACSHA1(Encoding.ASCII.GetBytes(password));
            var salt = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(nonce));
            hmac_sha1.Initialize();

            var hashBytes = hmac_sha1.ComputeHash(salt);

            // Use a bitwise operation to get a representative binary code from the hash
            // Refer section 5.4 at http://tools.ietf.org/html/rfc4226#page-7            
            int offset = hashBytes[19] & 0xf;
            int binaryCode = (hashBytes[offset] & 0x7f) << 24
                | (hashBytes[offset + 1] & 0xff) << 16
                | (hashBytes[offset + 2] & 0xff) << 8
                | (hashBytes[offset + 3] & 0xff);

            int otp = binaryCode % (int)Math.Pow(10, 8); // where 8 is the password length

            return otp.ToString().PadLeft(8, '0');
        }

        public static bool VerifyUser(String password, String nonce, String users_otp_response)
        {
            return (users_otp_response == GetOTP(password, nonce));
        }

    }
}
