using System;
using System.Text;

namespace Argutec.TimeToken.Common
{
    public class Sha1Hasher : IHasher
    {
        public string Hash(string aInput)
        {
            var lEncoding = Encoding.UTF8;
            byte[] lBuffer = lEncoding.GetBytes(aInput);
            
            var lSha1 = System.Security.Cryptography.SHA1.Create();
            var lHash = lSha1.ComputeHash(lBuffer);

            return Convert.ToBase64String(lHash);
        }
    }
}