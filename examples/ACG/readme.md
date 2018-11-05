## Best Practices
#### Automatic conversion of the data type of a uniform
**Example:**
application code: `GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "time"), timeSource.Elapsed.TotalSeconds);`
shader code: `uniform float time;`

**Problem:** `timeSource.Elapsed.TotalSeconds` returns a `double`. the shader expects a `float`. 
does not work on for instance Intel HW, `uniform float time` stays on `0`.

**Solution:** explicit conversion: `GL.Uniform1(shader.GetResourceLocation(ShaderResourceType.Uniform, "time"), (float)timeSource.Elapsed.TotalSeconds);`
