using System;
using NUnit.Framework;

namespace Tradeas.Security.Tests
{
    [TestFixture]
    public class CrypterTest
    {
        [Test]
        public void Test1()
        {
            var passPhrase = "tr@d3@s.as1n";
            var password = "Calv1nc3nt!";
            var encryptedPassword = Crypter.EncryptString(password, passPhrase);
            var decryptedPassword = Crypter.DecryptString(encryptedPassword, passPhrase);
            Assert.AreEqual(password, decryptedPassword);
            
            var username = "1300-8645";
            var encryptedUsername = Crypter.EncryptString(username, passPhrase);
            var decryptedUsername = Crypter.DecryptString(encryptedUsername, passPhrase);
            Assert.AreEqual(username, decryptedUsername);
        }
    }
}