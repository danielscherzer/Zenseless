+ `Example window` hosts  the `IContentLoader` interface that can be used to load shaders, textures, sound and other content in a file system agnostic manner.
+ Content loading can be configured to enable on-the-fly editing-and-continue of resources by setting the attribute `[assembly: ContentSearchDirectory]`. For many examples this is already configured in the file `AssemblyConfig.cs`. By default this attribute will set the directory in which the file `AssemblyConfig.cs` resides as root directory for content file searches, but by giving the attribute constructor a different path you can configure different scenarios.
+ This works for all content types. If you make an error in a shader an error dialog will pop-up were you can correct the error.


