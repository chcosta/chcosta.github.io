# chcosta.github.io

This graph displays the packages produced in the core-setup, coreclr, corefx, wcf, and 
projectk-tfs (not on GitHub) repositories.  Click on a node to see a line connecting that node
to the packages it depends on.

The node data is gathered from the https://github.com/dotnet/versions repo by the Gathere tool.  
The Latest_Packages.txt file is examined from each specified repo to determine what packages 
are available from a given repo.  Those packages are downloaded locally and then NuGet API's
are used to open the nupkg's and examine the nuspec's.  We just gather dependency names 
from the nuspec files, we don't take into account target framework, package versions, etc...

You can view the graph at https://chcosta.github.io/.