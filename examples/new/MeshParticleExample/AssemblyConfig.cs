using Zenseless.Base;
using Zenseless.HLGL;

#if SOLUTION
[assembly: Solution]
#endif
// Sets the content search directory.
// This is needed if you want to do automatic runtime content reloading if the content source file changes.
// This feature is disabled otherwise.The execution time of this command is dependent on how many files are found inside the given directory.
// The default parameter will set path of this source file as content directory. 
// It would be faster if you only specify a subdirectory
[assembly: ContentSearchDirectory]
