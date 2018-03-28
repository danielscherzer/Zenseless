1. When using the `IContentLoader` you can enable on-the-fly editing by calling `window.SetContentSearchDirectory(Path.GetDirectoryName(PathTools.GetSourceFilePath()));`. This will search for content source files in the given directory and all sub-directories. This works for all content types. If you make an error in a shader an error dialog will pop-up were you can correct the error.


