using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Gatherer
{

    public class DotNetVersions
    {
        string dotnetVersionsRegEx = @"(?<name>[^\s]+)\s+(?<version>[^\s]+)";

        public Dictionary<string, DotNetVersionsPackage> Packages { get; set; }
        public string Repo { get; set; }

        /// <summary>
        /// Given a url to a versions file located on dotnet/versions, read it into an object
        /// </summary>
        /// <param name="url"></param>
        public DotNetVersions(string repo, string url)
        {

            Packages = new Dictionary<string, DotNetVersionsPackage>();
            string strContent = string.Empty;
            Regex versionsRegex = new Regex(dotnetVersionsRegEx);
            var webRequest = WebRequest.Create(url);
            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {
                strContent = reader.ReadToEnd();
            }
            string[] lines = strContent.Split('\n');
            foreach(string line in lines)
            {
                Match m = versionsRegex.Match(line);
                if (m.Success)
                {
                    string name = m.Groups["name"].Value;
                    DotNetVersionsPackage package = new DotNetVersionsPackage(name, m.Groups["version"].Value);
                    Packages.Add(name, package);
                }
            }

            Repo = repo;
        }
    }
}
