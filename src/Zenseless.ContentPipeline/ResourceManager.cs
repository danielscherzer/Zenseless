using System;
using Zenseless.ContentPipeline;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Zenseless.HLGL;

namespace Zenseless.ContentPipeline
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Zenseless.ContentPipeline.IShaderProvider" />
	/// <seealso cref="Zenseless.HLGL.IResourceProvider" />
	[Export(typeof(IResourceProvider))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class ResourceManager : IShaderProvider, IResourceProvider
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceManager"/> class.
		/// </summary>
		public ResourceManager()
		{
			Instance = this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="shader">The shader.</param>
		public delegate void ShaderChangedHandler(string name, IShader shader);
		/// <summary>
		/// Occurs when [shader changed].
		/// </summary>
		public event ShaderChangedHandler ShaderChanged;

		/// <summary>
		/// Adds the shader.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="vertexFile">The vertex file.</param>
		/// <param name="fragmentFile">The fragment file.</param>
		/// <param name="vertexShaderResource">The vertex shader resource.</param>
		/// <param name="fragmentShaderResource">The fragment shader resource.</param>
		public void AddShader(string name, string vertexFile, string fragmentFile,
			byte[] vertexShaderResource = null, byte[] fragmentShaderResource = null)
		{
			var sfd = new ShaderFileDebugger(vertexFile, fragmentFile, vertexShaderResource, fragmentShaderResource);
			shaderWatcher.Add(name, sfd);
		}

		/// <summary>
		/// Checks for shader change.
		/// </summary>
		public void CheckForShaderChange()
		{
			foreach(var item in shaderWatcher)
			{
				if(item.Value.CheckForShaderChange())
				{
					ShaderChanged?.Invoke(item.Key, item.Value.Shader);
				}
			}
		}

		/// <summary>
		/// Gets the shader.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public IShader GetShader(string name)
		{
			if (shaderWatcher.TryGetValue(name, out ShaderFileDebugger shaderFD))
			{
				return shaderFD.Shader;
			}
			return null;
		}

		/// <summary>
		/// Adds the specified name.
		/// </summary>
		/// <typeparam name="RESOURCE_TYPE">The type of the esource type.</typeparam>
		/// <param name="name">The name.</param>
		/// <param name="resource">The resource.</param>
		public void Add<RESOURCE_TYPE>(string name, IResource<RESOURCE_TYPE> resource) where RESOURCE_TYPE : IDisposable
		{
			resources.Add(name, resource); //throws exception if key exists
		}

		/// <summary>
		/// Gets the specified name.
		/// </summary>
		/// <typeparam name="RESOURCE_TYPE">The type of the esource type.</typeparam>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public IResource<RESOURCE_TYPE> Get<RESOURCE_TYPE>(string name) where RESOURCE_TYPE : IDisposable
		{
			if (resources.TryGetValue(name, out object resource))
			{
				return resource as IResource<RESOURCE_TYPE>;
			}
			return null;
		}

		/// <summary>
		/// The shader watcher
		/// </summary>
		private Dictionary<string, ShaderFileDebugger> shaderWatcher = new Dictionary<string, ShaderFileDebugger>();
		/// <summary>
		/// The resources
		/// </summary>
		private Dictionary<string, object> resources = new Dictionary<string, object>();

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>
		/// The instance.
		/// </value>
		public static IResourceProvider Instance { get; private set; }
	}
}