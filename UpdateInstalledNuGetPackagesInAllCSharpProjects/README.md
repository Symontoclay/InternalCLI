This programm updates installed NuGet packages in all C# projects of the solutions listed in the `ProjectsDataSource.json` file.

You shoud input target nuget packageId and target version in command line.
The first argument should be target nuget packageId, next target version after space.
For example: 'NLog 5.1.4'");
The target version should only contain digits and point, for example: '5.1.4', '3.1.2'