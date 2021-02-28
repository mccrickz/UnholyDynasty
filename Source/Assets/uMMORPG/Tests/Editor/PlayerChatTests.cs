using NUnit.Framework;

namespace uMMORPG.Tests
{
    public class PlayerChatTests
    {
        [Test]
        public void ParseGeneral()
        {
            string message = PlayerChat.ParseGeneral("/command", "/command This is a test message.");
            Assert.That(message, Is.EqualTo("This is a test message."));
        }

        [Test]
        public void ParsePM()
        {
            (string user, string message) = PlayerChat.ParsePM("/w", "/w SOMEONE This is a test message.");
            Assert.That(user, Is.EqualTo("SOMEONE"));
            Assert.That(message, Is.EqualTo("This is a test message."));
        }
    }
}
