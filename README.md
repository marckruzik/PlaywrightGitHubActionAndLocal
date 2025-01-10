# PlayGal: Playwright.GitHub.Action.And.Local
You want to run your Playwright NUnit tests on local, and you want a GitHub Action to run your tests before merging to main? Without creating a whole pre-production environment?
PlayGal (Playwright GitHub Action and Local) is the solution for you!

With PlayGal, your Playwright NUnit local tests work with minimal change, and the GitHub Action creates its own headless server to run the tests directly during the pull request. Then it will either merge the pull request to main (if success), or close the pull request (if failure).

That's all! No need to deploy your project on a full online environment just to run your Playwright tests! No need to setup and manage a complete copy of your Azure setup to have a pre-production environment! 
Everything is done for you by PlayGal.

# What is PlayGal?
PlayGal is composed of 2 original creations:
* a class `PlayGalPageTest` with its own headless server to start the main project. Zero configuration needed, just inherit the class, then you can use "Page" and other Playwright elements as you are used to.
* a workflow yaml file, that installs Playwright and run tests. Zero configuration needed, the workflow yaml file is created in your GitHub Actions folder during the next Rebuild.

PlayGal is a very simple tool, intended for small projects. It relies on the philosophy "detection over configuration". Your main project is automatically detected (it's the project containing an "App.razor" file), and this main project is launched during local tests and GitHub Actions.

If you have a big project, with a custom configuration that cannot be detected by PlayGal, you surely don't need PlayGal.

# Installation
* Add the PlayGal nuget (`Playwright.GitHub.Action.And.Local`) to your Playwright NUnit test project.
* `Rebuild` your Playwright NUnit test project (use `Rebuild`, not Build) (in Visual Studio, right click the project and choose `Rebuild`).
* The Rebuild creates the workflow yaml file in the GitHub Actions folder at the root of your repository.
* Commit this workflow yaml file.

# Usage: tests
* In your Playwright test files, add `using PlayGal` at the top, replace inheritance `: PageTest` by `: PlayGalPageTest`, and use `Page.GotoAsync($"{RootUri.AbsoluteUri}MyPage")` to access a page on the local server.

Code example:

```csharp
using PlayGal;

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
```

In the repository, you can find a working Playwright NUnit tests project `TestPlaywrightDemoBlazorApp` [here](
https://github.com/marckruzik/PlaywrightGitHubActionAndLocal/tree/main/TestPlaywrightDemoBlazorApp) (and also a basic Blazor App project).

# Usage: GitHub Action
* Commit on your development branch (any branch other than `main` or `master`).
* Create a pull request from your development branch to your main branch (`main` or `master`).

If all tests run fine, the pull request will be merged into main. If not, the pull request is closed with a failure message.

Notes:
* If you are using another name than `main` or `master` for your main branch, you can edit the workflow yaml file.
* After creation, the workflow yaml file will not be overwritten, so you can safely edit it. But do not rename the file, or you will end up with multiple copies.

# Inner workings and shortcomings
Due to nuget limitations, and the strategy "detection over configuration", PlayGal has several shortcomings. The shortcomings are listed here, to help improve PlayGal, help debug, and allow customization.
* Installation: The nuget cannot directly create the workflow yaml file (that's against nuget philosophy). Hence the need for a rebuild and a .targets file to actually create the workflow yaml file.
* Installation: To find the root of the repo, the code of the targets file scans all directories upward, until it finds either a directory `.git`, next to it copies the workflow yaml file in directory `.github\workflows` (creates it if it does not exist).
* Installation: The targets file uses a RoslynCodeTaskFactory with a Code Fragment cs to scan directories upward, and a Copy
* Installation: The workflow yaml file can be edited by the user, but it cannot be renamed. If the workflow yaml file is renamed, it is no longer detected, so the code creates it again in the GitHub workflow directory, leading to multiple workflow yaml files.
* Running tests: To find the directory of the Blazor App, the code scans all directories upwards, until it finds a sub directory that contains a file `App.razor`.
* Running tests: The local URI `RootUri.AbsoluteUri` is defined by default as `https://localhost:7128`. It would be better to use the port from the configuration of the BlazorApp on `launchSettings.json`.
* Playwright installation during GitHub Action: The GitHub Action scans all folders to find the ps1 install file for Playwright.

# Contact, feedback and suggestions
The project is open to feedback, proposals, pull requests, and suggestions of new features.
* Marc Kruzik: https://www.linkedin.com/in/marckruzik marc.kruzik (at) gmail (dot) com

# Credits
PlayGal is an original creation of Marc Kruzik.
