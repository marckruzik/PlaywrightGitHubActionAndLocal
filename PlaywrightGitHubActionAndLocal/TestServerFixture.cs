using System.Diagnostics;
using System.Reflection;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightGitHubLocal
{
    public partial class TestServerFixture : PageTest, IDisposable
    {
        public Uri RootUri { get; private set; } = new("https://localhost:7128/");

        public static int max_total_wait_seconds = 120;
        public static int wait_between_tries_milliseconds = 100;

        private Process? _serverProcess;

        [OneTimeSetUp]
        public void StartServer()
        {
            string project_path = from_environment_get_app_folderpath();

            _serverProcess = Process.Start(new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project {project_path} --urls {RootUri}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            });

            WaitForServerAsync().GetAwaiter().GetResult();
            
			Display();
		}

        public static string from_environment_get_app_folderpath()
        {
            string current_folderpath =
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                ?? throw new InvalidOperationException("Cannot find assembly directory.");

            string[] arr_current_directory = current_folderpath.Split(Path.DirectorySeparatorChar);

            for (int i = arr_current_directory.Length - 1; i > 0; i--)
            {
                current_folderpath = String.Join(Path.DirectorySeparatorChar,  arr_current_directory[..i]);
                foreach (var candidate_folderpath in Directory.GetDirectories(current_folderpath))
                {
                    if (File.Exists(Path.Combine(candidate_folderpath, "App.razor")))
                    {
                        return candidate_folderpath;
                    }
                }
            }

            throw new Exception("Cannot find a directory containing App.razor.");
        }

        private async Task WaitForServerAsync()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            };

            using var client = new HttpClient(handler);

            DateTime datetime_max = DateTime.Now + TimeSpan.FromSeconds(max_total_wait_seconds);
            while(DateTime.Now < datetime_max)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(RootUri);
                    if (response != null)
                    {
                        break;
                    }
                }
                catch (HttpRequestException) { } // ignore


                if (_serverProcess?.HasExited ?? true)
                {
                    throw new InvalidOperationException("Server process stoped.");

                }

                TestContext.Progress.WriteLine("Waiting for server to respond...");
                await Task.Delay(wait_between_tries_milliseconds);
            }
        }

		private void Display()
		{
            if (_serverProcess?.HasExited == false) return;
            if (_serverProcess == null) return;
           
            TestContext.Progress.WriteLine("Server process has exited");
            string output = _serverProcess.StandardOutput.ReadToEnd();
            TestContext.Progress.WriteLine("Standard Output");
            TestContext.Progress.WriteLine(output);

            string error = _serverProcess.StandardError.ReadToEnd();
            if (string.IsNullOrEmpty(error) == false)
            {
                TestContext.Progress.WriteLine("Error Output");
                TestContext.Progress.WriteLine(error);
            }
		}

		[OneTimeTearDown]
        public void StopServer()
        {
            if (!_serverProcess?.HasExited == true)
            {
                _serverProcess?.Kill();
                Dispose();
            }
        }

        public void Dispose()
        {
            _serverProcess?.Dispose();
        }
    }

    public partial class TestServerFixture : PageTest
    {
        protected IPage page;

        [SetUp]
        public virtual async Task Setup()
        {
            var contextOptions = new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true
            };
            var context = await Browser.NewContextAsync(contextOptions);
            page = await context.NewPageAsync();
        }
    }
}
