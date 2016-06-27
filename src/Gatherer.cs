using NuGet.Packaging;
using NuGet.Packaging.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gatherer
{
    /// <summary>
    /// A gatherer examines a package and produces a set of directed nodes (package -> dependency), with the dependencies
    /// noted as part of the repo, or external.
    /// </summary>
    class Gatherer
    {
        public string RepoName { get; set; }
        public string PackageName { get; set; }
        public List<PackageNode> Nodes { get { return _nodes; } }

        private string _packagePath;

        List<PackageNode> _nodes;

        public Gatherer(string repoName, string filename)
        {
            RepoName = repoName;
            _packagePath = filename;
            _nodes = new List<PackageNode>();
        }

        public void GenerateNodes()
        {
            PackageArchiveReader archiveReader = new PackageArchiveReader(_packagePath);
            var identity = archiveReader.GetIdentity();
            PackageName = identity.Id;

            var dependencies = archiveReader.GetPackageDependencies();
            var dependencyPackages = dependencies.SelectMany(d => d.Packages);
            GenerateNodes(dependencyPackages);
        }

        private void GenerateNodes(IEnumerable<PackageDependency> dependencies)
        {
            if (dependencies.Count() > 0)
            {
                foreach (var dependency in dependencies)
                {
                    PackageNode packageNode = new PackageNode(PackageName, dependency.Id, RepoName);
                    _nodes.Add(packageNode);
                }
            }
            else
            {
                PackageNode packageNode = new PackageNode(PackageName, "", RepoName);
                _nodes.Add(packageNode);
            }
        }

        public void WriteNodes(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    foreach(var node in _nodes)
                    {
                        var line = string.Join(",", node.Name, node.Repo, node.Dependency, node.Version);
                        writer.WriteLine(line);
                    }
                }
            }
        }
    }
}
