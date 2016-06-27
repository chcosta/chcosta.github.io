using System;

namespace Gatherer
{
    public class PackageNode
    {
        public string Repo { get; set; }
        public string Name { get; set; }

        public string Dependency { get; set; }

        public string Version { get; set; }

        public PackageNode(string name, string dependency, string repo)
        {
            Repo = repo;
            Name = name;
            Version = "";
            if (dependency.Contains(" >= "))
            {
                string[] tokens = dependency.Split(new string[] { " >= " }, StringSplitOptions.RemoveEmptyEntries);
                Dependency = tokens[0];
                Version = tokens[1];
            }
            else
            {
                Dependency = dependency;
            }
        }
    }
}
