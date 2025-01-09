# PlayGal: Playwright.GitHub.Action.And.Local
 
You want to run your Playwright NUnit tests on local, and you want a GitHub Action to run your tests before merging to main? Without creating a whole pre-production environment?
PlayGal (Playwright GitHub Action and Local) is the solution for you!

With PlayGal, your Playwright NUnit local tests work with minimal change, and the GitHub Action creates its own headless server to run the tests directly during the pull request. Then it will either merge the pull request to main (if success), or close the pull request (if failure).

That's all! No need to deploy your project on a full online environment just to run your Playwright tests! No need to setup and manage a complete copy of your Azure setup to have a pre-production environment! 
Everything is done for you by PlayGal.

# What is PlayGal?
PlayGal is composed of 2 original creations:
* a `PlayGalPageTest` class with its own process to start the main project. Zero configuration needed, just inherit the class, then you can use "Page" and other Playwright elements as you are used to.
* a yaml file, which installs Playwright and run tests. Zero configuration needed, the yaml file is created in your GitHub Actions folder at first compilation.

PlayGal is a very simple tool, intended for small projects. It relies on the philosophy "detection over configuration". Your main project is automatically detected (it's the project containing an "App.razor" file), and this main project is launched during local tests and GitHub Actions.

If you have a big project, with a custom configuration that cannot be detected by PlayGal, you surely don't need PlayGal.

# Installation
* Add the PlayGal nuget (Playwright.GitHub.Action.And.Local) to your Playwright NUnit test project.
* In your Playwright test files, replace inheritance `: PageTest` by `: PlayGalPageTest`, and use `Page.GotoAsync($"{RootUri.AbsoluteUri}MyPage")` to access a page on the local server.
* Compile the project (it creates the yaml file in the GitHub Actions folder at the root of your repository).

Feel free to look at included examples on the GitHub repo.

# Usage
* Commit on your development branch (any branch other than `main`).
* Create a pull request from your development branch to the branch `main`.

If all tests run fine, the pull request will be merged into main. If not, the pull request is closed with a failure message.

If you are using another name than `main`, you can edit the yaml file.

# Contact, feedback and suggestions
The project is open to feedback, proposals, pull requests, and suggestions of new features.
* Marc Kruzik: https://www.linkedin.com/in/marckruzik marc.kruzik (at) gmail (dot) com

# Shortcomings
Due to nuget limitations, and the strategy "detection over configuration", PlayGal has several shortcomings. The shortcomings are listed here, to help improve PlayGal, help debug, and allow customization.
* Installation: The nuget cannot directly create the yaml file (that's against nuget philosophy). Hence the need for a compilation to actually create the yaml file.
* Installation: To find the root of the repo, the code scans all directories upward, until it finds either a directory `.github`, or a directory `.git` next to it will create a directory `.github`.
* Test run: To find the directory of the Blazor App, the code scans all directories upwards, until it finds a sub directory that contains a file `App.razor`.
* Test run: The local URI `RootUri.AbsoluteUri` is defined by default as `https://localhost:7128`. It would be better to use the port from the configuration of the BlazorApp on `launchSettings.json`.
* Playwright Installation during GitHub Action: The GitHub Action scans all folders to find the ps1 install file for Playwright.

# Credits
PlayGal is an original creation of Marc Kruzik.
