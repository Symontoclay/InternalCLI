The program checks the solutions listed in the `ProjectsDataSource.json` file to ensure that the projects they contain are present in `ProjectsDataSource.json`.
The list of solutions to be checked can be configured in the program's configuration file.
For adding solution to list of chackable solutions, please copy the "Name" of target solution to "CheckedSolutions" in "appsettings.json" of the program.

The program also provides information about solutions that are present in `ProjectsDataSource.json` but are not being checked by the program.