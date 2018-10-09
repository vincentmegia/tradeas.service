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
            var passPhrase = "asdfasd";
            var password = "asdfsdf";
            var encryptedPassword = Crypter.EncryptString(password, passPhrase);
            var decryptedPassword = Crypter.DecryptString(encryptedPassword, passPhrase);
            Assert.AreEqual(password, decryptedPassword);
            
            var username = "sdfasdf";
            var encryptedUsername = Crypter.EncryptString(username, passPhrase);
            var decryptedUsername = Crypter.DecryptString(encryptedUsername, passPhrase);
            Assert.AreEqual(username, decryptedUsername);
        }
    }
}