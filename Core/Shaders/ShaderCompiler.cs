using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.D3DCompiler;

namespace FeralTic.DX11
{
    public class ShaderCompiler
    {
        #region Compile (from string)
        private static ShaderBytecode Compile(string content, bool isfile, Include include,string profile,string entrypoint)
        {
            try
            {
                ShaderFlags flags = ShaderFlags.PackMatrixRowMajor | ShaderFlags.OptimizationLevel1;

                if (isfile)
                {
                    CompilationResult result = ShaderBytecode.CompileFromFile(content, entrypoint, profile, flags, EffectFlags.None, null, include);
                    return result.Bytecode;
                }
                else
                {
                    CompilationResult result = ShaderBytecode.Compile(content, entrypoint, profile, flags, EffectFlags.None, null, include);
                    return result.Bytecode;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        public static ShaderBytecode CompileFromString(string code, string profile, string entrypoint)
        {
            return Compile(code, false, null, profile, entrypoint);
        }

        public static ShaderBytecode CompileFromResource(Assembly assembly, string path, string profile, string entrypoint)
        {
            var textStreamReader = new StreamReader(assembly.GetManifestResourceStream(path));
            string code = textStreamReader.ReadToEnd();
            textStreamReader.Dispose();
            return Compile(code, false, null,profile,entrypoint);
        }

        public static ShaderBytecode LoadFromResource(Assembly assembly, string path)
        {
            var ms = new BinaryReader(assembly.GetManifestResourceStream(path));
            byte[] data = new byte[ms.BaseStream.Length];
            ms.Read(data, 0, (int)ms.BaseStream.Length);
            return new ShaderBytecode(data);
        }

        public static T GetShaderInstance<T>(DxDevice device, Assembly assembly, string path) where T : class
        {
            ShaderBytecode sb = LoadFromResource(assembly, path);
            return GetShaderInstance<T>(device, sb);
        }

        public static T GetShaderInstance<T>(DxDevice device, ShaderBytecode sb) where T: class
        {
             if(typeof(T) == typeof(ComputeShader))
             {
                 return new ComputeShader(device.Device, sb) as T;
             }
             else if (typeof(T) == typeof(VertexShader))
             {
                 return new VertexShader(device.Device, sb) as T;
             }
             else if (typeof(T) == typeof(PixelShader))
             {
                 return new PixelShader(device.Device, sb) as T;
             }
             else if (typeof(T) == typeof(DomainShader))
             {
                 return new DomainShader(device.Device, sb) as T;
             }
             else if (typeof(T) == typeof(HullShader))
             {
                 return new HullShader(device.Device, sb) as T;
             }
             else if (typeof(T) == typeof(GeometryShader))
             {
                 return new GeometryShader(device.Device, sb) as T;
             }
             throw new Exception("Invalid Shader type");
        }

        






    }
}
