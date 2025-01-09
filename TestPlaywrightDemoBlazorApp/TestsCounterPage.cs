using PlaywrightGitHubLocal;

namespace TestPlaywrightDemoBlazorApp
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class TestsCounterPage : PlayGalPageTest
    {
        [SetUp]
        public async Task Setup()
        {
            await Page.GotoAsync($"{RootUri.AbsoluteUri}Counter");
        }

        [Test]
        public async Task title_exist()
        {
            await Expect(Page).ToHaveTitleAsync(new Regex("Counter"));
        }
    }
}
