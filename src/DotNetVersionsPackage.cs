using NuGet.Versioning;

namespace Gatherer
{
    public class DotNetVersionsPackage
    {
        public string Name { get; set; }
        public NuGetVersion Version { get; set; }

        public string Url { get; set; }
        public string Filename { get; set; }

        public DotNetVersionsPackage(string name, string version)
        {
            Name = name;
            Version = NuGetVersion.Parse(version);
            Url = string.Format("https://dotnet.myget.org/F/dotnet-core/api/v2/package/{0}/{1}", Name, Version.ToString());
            Filename = string.Format("package\\{0}-{1}.nupkg", Name, Version.ToString());

        }
    }
}
