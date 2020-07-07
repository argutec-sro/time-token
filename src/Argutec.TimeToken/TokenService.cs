using System;
using System.Globalization;
using Argutec.TimeToken.Common;

namespace Argutec.TimeToken
{
    public class TokenService
    {
        private string mNowFormat = "yyyyMMddHHmmssfff";

        public IHasher Hasher { get; set; }
        public TokenServiceOptions Options { get; set; }

        public TokenService()
            : this(new TokenServiceOptions(), new Sha1Hasher())
        {
        }
        public TokenService(TokenServiceOptions aOptions) 
            : this(aOptions, new Sha1Hasher())
        {
        }
        public TokenService(TokenServiceOptions aOptions, IHasher aHasher)
        {
            Hasher = aHasher;
            Options = aOptions;
        }

        public string CreateToken(string aClientID, string aSecret)
        {
            DateTime lNow = DateTime.Now;
            string lRandomPart = Guid.NewGuid().ToString();

            return CreateToken(aClientID, aSecret, lNow, lRandomPart);
        }
        private string CreateToken(string aClientID, string aSecret, DateTime aDateTime, string aRandomPart)
        {
            DateTime lNow = DateTime.UtcNow;
            string lRandomPart = Guid.NewGuid().ToString();

            string lPayload = GetTokenPayload(aClientID, aSecret,  lRandomPart, lNow);

            string lHash = Hasher.Hash(lPayload);
            string lNowSerialized = lNow.ToString(mNowFormat);

            return $"{lHash};{lNowSerialized};{lRandomPart}";
        }

        public bool CheckToken(string aClientID, string aSecret, string aHash)
        {
            if (!aHash.Contains(";"))
                throw new ArgumentException("Invalid hash format.", nameof(aHash));

            string[] lHashParts = aHash.Split(';');
            if (lHashParts.Length != 3)
                throw new ArgumentException("Invalid hash format.", nameof(aHash));

            DateTime lNow = DateTime.UtcNow;

            if (!DateTime.TryParseExact(lHashParts[1], mNowFormat, 
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lDate))
            {
                throw new ArgumentException("Invalid time stamp in hash.", nameof(aHash));
            }

            string lRandomPart = lHashParts[2];
            string lPayload = GetTokenPayload(aClientID, aSecret, lRandomPart, lNow);
            string lHash = Hasher.Hash(lPayload);

            // TODO: +- minuta
            // TODO: event to set time on client

            return lHash == lHashParts[0];
        }

        private string GetTokenPayload(string aClientID, string aSecret, string aRandom, DateTime aNow)
        {
            string lDateFormat = "yyyy-MM-ddHHmm";
            string lSerializedDate = aNow.ToString(lDateFormat);

            return $"{Options.Salt};{aClientID};{aSecret};{lSerializedDate};{aRandom}";
        }
    }
}
