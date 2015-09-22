using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Reflection;

using SharpDX.Direct3D11;
using SharpDX.D3DCompiler;
using SharpDX;
using SharpDX.Direct3D;
using System.Threading.Tasks;

namespace FeralTic.DX11
{

    public class DX11Effect : IDisposable
    {
        private static FolderIncludeHandler folderhandler = new FolderIncludeHandler();

        public CompilationResult CompilationResult { get; private set; }

        public ShaderBytecode ByteCode
        {
            get { return this.CompilationResult.Bytecode; }
        }
           
        public string ErrorMessage
        {
            get { return this.CompilationResult.Message; }
        }

        public bool IsCompiled
        {
            get { return this.ByteCode != null; }
        }

        public Effect DefaultEffect { get; private set; }

        public static implicit operator Effect(DX11Effect effect)
        {
            return effect.DefaultEffect;
        }

        private DX11Effect()
        {
            //To prevent instancing
        }

        #region Compile
        private static DX11Effect Compile(string content, bool isfile, Include include, ShaderMacro[] defines)
        {
            DX11Effect shader = new DX11Effect();
            ShaderFlags flags = ShaderFlags.OptimizationLevel1;// | ShaderFlags.PackMatrixRowMajor;

            try
            {
                if (isfile)
                {
                    shader.CompilationResult = ShaderBytecode.CompileFromFile(content, "fx_5_0", flags, EffectFlags.None, defines, include);
                }
                else
                {
                    shader.CompilationResult = ShaderBytecode.Compile(content, "fx_5_0", flags, EffectFlags.None, defines, include);
                }

                if (shader.IsCompiled)
                {
                    shader.DefaultEffect = new Effect(NullDevice.Device, shader.CompilationResult.Bytecode);
                }
            }
            catch (Exception ex)
            {
                shader.CompilationResult = new CompilationResult(null, Result.Fail, ex.Message);
            }
            return shader;
        }
        #endregion

        #region compile from string
        public static DX11Effect CompileFromString(string code)
        {
            return Compile(code, false, null, null);
        }

        public static DX11Effect CompileFromString(string code, Include include)
        {
            return Compile(code, false, include, null);
        }

        public static DX11Effect CompileFromString(string code, Include include, ShaderMacro[] defines)
        {
            return Compile(code, false, include, defines);
        }
        #endregion

        #region Compile From File
        public static DX11Effect CompileFromFile(string path, ShaderMacro[] defines)
        {
            folderhandler.BaseShaderPath = Path.GetDirectoryName(path);
            return Compile(path, true, folderhandler, defines);
        }

        public static DX11Effect CompileFromFile(string path)
        {
            folderhandler.BaseShaderPath = Path.GetDirectoryName(path);
            return Compile(path, true, folderhandler, null);
        }

        public static DX11Effect CompileFromFile(string path, Include include, ShaderMacro[] defines)
        {
            return Compile(path, true, include, defines);
        }

        public static DX11Effect CompileFromFile(string path, Include include)
        {
            return Compile(path, true, include, null);
        }
        #endregion

        #region Compile From Resource
        public static DX11Effect CompileFromResource(Assembly assembly, string path)
        {
            var textStreamReader = new StreamReader(assembly.GetManifestResourceStream(path));
            string code = textStreamReader.ReadToEnd();
            textStreamReader.Dispose();
            return Compile(code, false, null, null);
        }

        public static DX11Effect CompileFromResource(Assembly assembly, string path, ShaderMacro[] macros = null, Include include = null)
        {
            var textStreamReader = new StreamReader(assembly.GetManifestResourceStream(path));
            string code = textStreamReader.ReadToEnd();
            textStreamReader.Dispose();
            return Compile(code, false, include, macros);
        }
        #endregion

        #region Loac Binary Form
        public static DX11Effect LoadFromFile(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            return LoadFromByteCode(data);         
        }

        public static DX11Effect LoadFromByteCode(byte[] data)
        {
            DX11Effect shader = new DX11Effect();
            shader.CompilationResult = new CompilationResult(new ShaderBytecode(data), Result.Ok, "");
            shader.DefaultEffect = new Effect(NullDevice.Device, shader.CompilationResult.Bytecode);
            return shader;
        }

        public static DX11Effect LoadFromResource(Assembly assembly, string path)
        {
            var ms = new BinaryReader(assembly.GetManifestResourceStream(path));
            byte[] data = new byte[ms.BaseStream.Length];
            ms.Read(data, 0, (int)ms.BaseStream.Length);
            return LoadFromByteCode(data);
        }
        #endregion

        #region Save Byte code
        public bool SaveByteCode(string path)
        {
            File.WriteAllBytes(path, this.ByteCode.Data);
            return true;
        }
        #endregion

        /*public bool[] TechniqueValids { get; private set; }
        public string[] TechniqueNames { get; private set; }*/

        public void Dispose()
        {
            if (this.DefaultEffect != null) { this.DefaultEffect.Dispose(); }
            if (this.CompilationResult != null) { this.CompilationResult.Dispose(); }
        }

        /*private void Preprocess()
        {
            //Set techniques
            int techcnt = this.DefaultEffect.Description.TechniqueCount;

            this.TechniqueNames = new string[techcnt];
            this.TechniqueValids = new bool[techcnt];

            for (int i = 0; i < techcnt; i++)
            {
                EffectTechnique technique = this.DefaultEffect.GetTechniqueByIndex(i);

                this.TechniqueNames[i] = technique.Description.Name;
                this.TechniqueValids[i] = technique.IsValid;
            }
        }*/
    }
}
