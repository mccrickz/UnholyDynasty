using NUnit.Framework;

namespace uMMORPG.Tests
{
    public class PlayerPartyTests
    {
        [Test]
        public void CalculateExperienceShare()
        {
            // 100 exp => 1 members, 0% bonus/extra member, monster & member same level
            Assert.That(PlayerParty.CalculateExperienceShare(1000, 1, 0, 10, 10), Is.EqualTo(1000));

            // 100 exp => 1 members, 0% bonus/extra member, monster 1 lvl higher than member
            // => 5% more exp per level difference gives 1000 * 5% = 50 extra
            Assert.That(PlayerParty.CalculateExperienceShare(1000, 1, 0, 9, 10), Is.EqualTo(1050));

            // 100 exp => 5 members, 0% bonus/extra member, monster & member same level
            Assert.That(PlayerParty.CalculateExperienceShare(1000, 5, 0, 10, 10), Is.EqualTo(200));

            // 100 exp => 5 members, 10% bonus/extra member, monster & member same level
            // => 40% bonus gives 200*0.4 = 80 extra
            Assert.That(PlayerParty.CalculateExperienceShare(1000, 5, 0.1f, 10, 10), Is.EqualTo(280));

            // finally one complicated case where everything is applied:
            // 100 exp => 5 members, 10% bonus/extra member, monster 1 lvl higher than member
            // => 200 per member
            // => 5% more exp per level difference gives 200 * 5% = 210
            // => 40% bonus gives 210*0.4 = 84 extra
            // => 294 total
            Assert.That(PlayerParty.CalculateExperienceShare(1000, 5, 0.1f, 9, 10), Is.EqualTo(294));
        }
    }
}
