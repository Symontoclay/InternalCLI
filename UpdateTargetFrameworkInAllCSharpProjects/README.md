This programm updates target framework in all C# projects of the solutions listed in the `ProjectsDataSource.json` file.
The program will update only projects with the same target framework as specified in the command line arguments.
C# prjects with diffrefent target frameworks will be ignored.

You shoud input kind of target framework and target version in command line.
The first argument should be name of target framework, next target version after space.
For example: 'Net 11.0'
The kind of target framework is an option of enum KindOfTargetCSharpFramework.
The target version should only contain digits and point, for example: '8.0', '3.1.2'

If you want to run the program in read-only mode, please set "ReadOnlyMode" to true in "appsettings.json" of the program.