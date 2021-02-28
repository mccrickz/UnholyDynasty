using NUnit.Framework;

namespace uMMORPG.Tests
{
    public class ExperienceTests
    {
        [Test]
        public void BalanceExperienceReward()
        {
            // max difference 10 tests first: those are easiest to understand

            // max difference 10, same level should get 100%
            Assert.That(Experience.BalanceExperienceReward(100, 10, 10, 10), Is.EqualTo(100));

            // max difference 10, monster 1 level higher, should get 10% more
            Assert.That(Experience.BalanceExperienceReward(100, 9, 10, 10), Is.EqualTo(110));

            // max difference 10, monster 2 level higher, should get 20% more
            Assert.That(Experience.BalanceExperienceReward(100, 8, 10, 10), Is.EqualTo(120));

            // max difference 10, monster 9 level higher, should get 90% more
            Assert.That(Experience.BalanceExperienceReward(100, 1, 10, 10), Is.EqualTo(190));

            // max difference 10, monster 10 level higher, should get 100% more
            Assert.That(Experience.BalanceExperienceReward(100, 0, 10, 10), Is.EqualTo(200));

            // max difference 10, monster 11 level higher, should get 100%=limit more. not 110%.
            Assert.That(Experience.BalanceExperienceReward(100, 0, 11, 10), Is.EqualTo(200));

            // max difference 10, monster 1 level lower, should get 10% less
            Assert.That(Experience.BalanceExperienceReward(100, 10, 9, 10), Is.EqualTo(90));

            // max difference 10, monster 2 level lower, should get 20% less
            Assert.That(Experience.BalanceExperienceReward(100, 10, 8, 10), Is.EqualTo(80));

            // max difference 10, monster 9 level lower, should get 90% less
            Assert.That(Experience.BalanceExperienceReward(100, 10, 1, 10), Is.EqualTo(10));

            // max difference 10, monster 10 level lower, should get 0% = nothing anymore
            Assert.That(Experience.BalanceExperienceReward(100, 10, 0, 10), Is.EqualTo(0));

            // max difference 10, monster 11 level lower, should get 0%. not negative.
            Assert.That(Experience.BalanceExperienceReward(100, 11, 0, 10), Is.EqualTo(0));

            ////////////////////////////////////////////////////////////////////
            // max difference 20 tests. those need to work as well, and instead
            // of 10% per level, it should make a 5% per level difference now!

            // max difference 20, same level should get 100%
            Assert.That(Experience.BalanceExperienceReward(100, 20, 20, 20), Is.EqualTo(100));

            // max difference 20, monster 1 level higher, should get 5% more
            // instead of 10% more like with maxLevelDiff=10.
            // => prevents: https://github.com/vis2k/uMMORPG/issues/35
            Assert.That(Experience.BalanceExperienceReward(100, 19, 20, 20), Is.EqualTo(105));

            // max difference 20, monster 2 level higher, should get 10% more
            Assert.That(Experience.BalanceExperienceReward(100, 18, 20, 20), Is.EqualTo(110));

            // max difference 20, monster 19 level higher, should get 95% more
            Assert.That(Experience.BalanceExperienceReward(100, 1, 20, 20), Is.EqualTo(195));

            // max difference 20, monster 20 level higher, should get 100% more
            Assert.That(Experience.BalanceExperienceReward(100, 0, 20, 20), Is.EqualTo(200));

            // max difference 20, monster 21 level higher, should get 100%=limit more. not 110%.
            Assert.That(Experience.BalanceExperienceReward(100, 0, 21, 20), Is.EqualTo(200));

            // max difference 20, monster 1 level lower, should get 5% less
            Assert.That(Experience.BalanceExperienceReward(100, 20, 19, 20), Is.EqualTo(95));

            // max difference 20, monster 2 level lower, should get 10% less
            Assert.That(Experience.BalanceExperienceReward(100, 20, 18, 20), Is.EqualTo(90));

            // max difference 20, monster 19 level lower, should get 95% less
            Assert.That(Experience.BalanceExperienceReward(100, 20, 1, 20), Is.EqualTo(5));

            // max difference 20, monster 20 level lower, should get 0% = nothing anymore
            Assert.That(Experience.BalanceExperienceReward(100, 20, 0, 20), Is.EqualTo(0));

            // max difference 20, monster 21 level lower, should get 0%. not negative.
            Assert.That(Experience.BalanceExperienceReward(100, 21, 0, 20), Is.EqualTo(0));
        }
    }
}
