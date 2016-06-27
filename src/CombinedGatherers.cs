using System.Collections.Generic;
using System.IO;

namespace Gatherer
{
    /// <summary>
    /// A CombinedGatherer takes multiple gatherers, combines them into a single list of merged nodes, and updates dependencies to determine if
    /// they are part of a known repo, or external..
    /// </summary>
    class CombinedGatherer
    {
        public string Name { get { return string.Join("_", _repos); } }

        public List<PackageNode> Nodes { get { return _nodes; } }

        List<PackageNode> _nodes;

        HashSet<string> _repos;

        public CombinedGatherer(params Gatherer[] gatherers)
        {
            _repos = new HashSet<string>();
            _nodes = new List<PackageNode>();
            foreach (var gatherer in gatherers)
            {
                _nodes.AddRange(gatherer.Nodes);
                _repos.Add(gatherer.RepoName);
            }
        }
        public void Add(Gatherer gatherer)
        {
            _nodes.AddRange(gatherer.Nodes);
            _repos.Add(gatherer.RepoName);
        }

        public void WriteNodes(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    foreach (var node in _nodes)
                    {
                        var line = string.Join(",", node.Name, node.Repo, node.Dependency, node.Version);
                        writer.WriteLine(line);
                    }
                }
            }
        }

    }
}
