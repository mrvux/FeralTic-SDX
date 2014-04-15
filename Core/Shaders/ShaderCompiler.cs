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

        public static VertexShader CompileFromResource(DxDevice device, Assembly assembly, string path, string entrypoint, out ShaderSignature inputsignature)
        {
            ShaderBytecode sb = CompileFromResource(assembly, path, GetShaderProfile<VertexShader>(device), entrypoint);
            inputsignature = ShaderSignature.GetInputSignature(sb);
            return GetShaderInstance<VertexShader>(device, sb);
        }

        public static T CompileFromResource<T>(DxDevice device, Assembly assembly, string path, string entrypoint) where T : class
        {
            ShaderBytecode sb = CompileFromResource(assembly,path,GetShaderProfile<T>(device),entrypoint);
            return GetShaderInstance<T>(device, sb);
        }

        public static T CompileFromResource<T>(DxDevice device, Assembly assembly, string path, string entrypoint, out ShaderReflection reflectiondata) where T : class
        {
            ShaderBytecode sb = CompileFromResource(assembly, path, GetShaderProfile<T>(device), entrypoint);
            reflectiondata = new ShaderReflection(sb);
            return GetShaderInstance<T>(device, sb);
        }

        private static string GetShaderProfile<T>(DxDevice device) where T : class
        {
            return GetProfileType<T>() + GetFeature(device);
        }

        private static string GetProfileType<T>() where T : class
        {
            if (typeof(T) == typeof(ComputeShader))
            {
                return "cs_";
            }
            else if (typeof(T) == typeof(VertexShader))
            {
                return "vs_";
            }
            else if (typeof(T) == typeof(PixelShader))
            {
                return "ps_";
            }
            else if (typeof(T) == typeof(DomainShader))
            {
                return "ds_";
            }
            else if (typeof(T) == typeof(HullShader))
            {
                return "hs_";
            }
            else if (typeof(T) == typeof(GeometryShader))
            {
                return "gs_";
            }
            throw new Exception("Invalid Shader type");
        }

        private static string GetFeature(DxDevice device)
        {
            switch (device.FeatureLevel)
            {
                case SharpDX.Direct3D.FeatureLevel.Level_10_0:
                    return "4_0";
                case SharpDX.Direct3D.FeatureLevel.Level_10_1:
                    return "4_1";
                case SharpDX.Direct3D.FeatureLevel.Level_11_0:
                    return "5_0";
                case SharpDX.Direct3D.FeatureLevel.Level_11_1:
                    return "5_0";
                case SharpDX.Direct3D.FeatureLevel.Level_9_3:
                    return "";
                case SharpDX.Direct3D.FeatureLevel.Level_9_2:
                    return "";
                case SharpDX.Direct3D.FeatureLevel.Level_9_1:
                    return "";
            }
            throw new ArgumentException("Unknown feature level");
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
