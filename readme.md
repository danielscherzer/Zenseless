[![Build status](https://ci.appveyor.com/api/projects/status/6yeqn2p92vd8rddx?svg=true)](https://ci.appveyor.com/project/danielscherzer/zenseless)

# Zenseless
A Framework for real-time computer graphics using OpenGL. This framework is created for a university course in computer graphics. Documentation inside the package is incomplete! See the [change log](CHANGELOG.md) for changes, features and road map.

### Setup of an empty stand-alone project
1. Create a Console App (.Net Framework) in Visual Studio
1. Install the Nuget package [OpenTK](hhttps://www.nuget.org/packages/OpenTK/3.0.0-pre). An [OpenTK Manual](https://github.com/mono/opentk/blob/master/Documentation/Manual.pdf).
1. Install either Nuget package [Zenseless](https://www.nuget.org/packages/Zenseless/) or [Zenseless.sources](https://www.nuget.org/packages/Zenseless.sources/)
	+ [Zenseless](https://www.nuget.org/packages/Zenseless/) is a .Net Framework 4.6 assembly package.
	+ [Zenseless.sources](https://www.nuget.org/packages/Zenseless.sources/) is a source package. The sources of Zenseless will be compiled along-side your project.

### Setup of full framework
1. download framework
	1. create empty dir
	1. change into empty dir
	+ repository clone [TortoiseGit](https://tortoisegit.org/)
		1. right click `git clone...`
		1. URL: https://github.com/danielscherzer/Zenseless
	+ repository clone (shell)
		1. open `git cmd` or `git bash`
		1. `git clone https://github.com/danielscherzer/Zenseless`
1. compile and run
	1. open solution file (`all.sln`)
	1. build solution


## Contribute
Check out the [contribution guidelines](CONTRIBUTING.md)
if you want to contribute to this project.

## License
[Apache 2.0](LICENSE)