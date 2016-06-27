using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;

namespace Gatherer
{
    class Program
    {
        static readonly string versionsUrl = "https://raw.githubusercontent.com/dotnet/versions/master/build-info/dotnet/[repo]/master/Latest_Packages.txt";

        static int Main(string[] args)
        {
            if(args.Length == 0 || 
                args[0] == "/?" ||
                args[0] == "help" ||
                args[0] == "-?")
            {
                ShowUsage();
                return 1;
            }
            string [] repos = args;
            Dictionary<string, DotNetVersions> allDotNetVersions = new Dictionary<string, DotNetVersions>();
            foreach (string repo in repos)
            {
                // Gather all packages by repo
                string repoUrl = versionsUrl.Replace("[repo]", repo);
                DotNetVersions dotNetVersions = new DotNetVersions(repo, repoUrl);
                allDotNetVersions.Add(repo, dotNetVersions);
            }

            // Download all packages
            var packages = allDotNetVersions.SelectMany(a => a.Value.Packages.Values);

            List<string> packagePaths = new List<string>();

            CombinedGatherer combinedGatherer = new CombinedGatherer();

            foreach (var dotNetVersion in allDotNetVersions)
            {
                string repo = dotNetVersion.Key;
                foreach(var package in dotNetVersion.Value.Packages)
                {
                    using (var client = new WebClient())
                    {
                        string version = package.Value.Version.ToString();
                        if (!Directory.Exists(Path.GetDirectoryName(package.Value.Filename)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(package.Value.Filename));
                        }
                        if (!File.Exists(package.Value.Filename))
                        {
                            client.DownloadFile(package.Value.Url, package.Value.Filename);
                        }
                    }
                    Gatherer g = new Gatherer(repo, package.Value.Filename);
                    g.GenerateNodes();
                    combinedGatherer.Add(g);
                }
            }
            combinedGatherer.WriteNodes(combinedGatherer.Name + ".nodes.txt");
            return 0;
        }
        private static void ShowUsage()
        {
            Console.WriteLine("Usage: Gatherer [repo] ... ([repo])");
            Console.WriteLine("  [repo] is a space delimitted list of repos to examine and generate nodes for.");
            Console.WriteLine("  The repo name is used in the URL for accessing the versions repo, so it must ");
            Console.WriteLine("  be one of the folders listed at");
            Console.WriteLine("  https://github.com/dotnet/versions/tree/master/build-info/dotnet");
            Console.WriteLine();
            Console.WriteLine("  Gatherer produces a [repo](_[repo]...).nodes.txt file which is used by the");
            Console.WriteLine("  https://chcosta.github.io/ package dependency graph.");
            Console.WriteLine();
            Console.WriteLine("Example: Gatherer coreclr corefx wcf projectk-tfs");
        }
    }
}
