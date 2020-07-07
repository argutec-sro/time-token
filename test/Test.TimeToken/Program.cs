using System;
using Argutec.TimeToken;

namespace Test.TimeToken
{
    class Program
    {
        static void Main(string[] args)
        {
            TokenService lTokenService = new TokenService();

            string lToken1 = lTokenService.CreateToken("1", "password");
            string lToken2 = lTokenService.CreateToken("1", "password");
            string lToken3 = lTokenService.CreateToken("1", "password");
            string lToken4 = lTokenService.CreateToken("1", "password");

            Console.WriteLine(lTokenService.CheckToken("1", "passwordx", lToken1));
            Console.WriteLine(lTokenService.CheckToken("1", "password", lToken2));
            Console.WriteLine(lTokenService.CheckToken("1", "passwordx", lToken3));
            Console.WriteLine(lTokenService.CheckToken("1", "password", lToken4));
        }
    }
}
