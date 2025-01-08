using PlaywrightGitHubLocal;

namespace TestPlaywrightDemoBlazorApp
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class TestsCounterPage : PlayGalPageTest
    {
        // This Setup is using "new", to correctly replace the parent's class Setup and avoid warnings.
        // Note: Playwright calls all SetUp, from parent to child PageTest.
        [SetUp]
        public async new Task Setup()
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
