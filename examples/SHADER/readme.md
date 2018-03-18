## Examples
1. [MinimalShaderExample](MinimalShaderExample) 
1. [ShaderDebugDialogExample](ShaderDebugDialogExample)
1. [GeometryExample](GeometryExample)
1. [MeshExample](MeshExample)
1. [InstancingExample](InstancingExample)
1. [BasicTransformations3D](BasicTransformations3D)
1. [CameraTransformationExample](CameraTransformationExample)
1. [CameraExample](CameraExample)
1. [PhongLightingExample](PhongLightingExample)
1. [EnvMappingExample](EnvMappingExample)
1. [RenderToTextureExample](RenderToTextureExample)
1. [ShadowMappingExample](ShadowMappingExample)
1. [PhysicsExample](PhysicsExample)
1. [ParticleSystemExample](ParticleSystemExample)
1. [SSBOExample](SSBOExample)

## Best Practices
#### Automatic conversion of the data type of a uniform
**Example:**
application code: `GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "time"), timeSource.Elapsed.TotalSeconds);`
shader code: `uniform float time;`

**Problem:** `timeSource.Elapsed.TotalSeconds` returns a `double`. the shader expects a `float`. 
does not work on for instance Intel HW, `uniform float time` stays on `0`.

**Solution:** explicit conversion: `GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "time"), (float)timeSource.Elapsed.TotalSeconds);`
