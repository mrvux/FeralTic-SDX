using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Shaders
{
    public class FolderInclude
    {
        private readonly string path;
        private readonly ShaderIncludeFlags flags;

        public FolderInclude(string path, ShaderIncludeFlags flags)
        {
            this.path = path;
            this.flags = flags;
        }

        public bool Accept(string fileName, IncludeType includeType)
        {
            if (includeType == IncludeType.Local && !this.flags.HasFlag(ShaderIncludeFlags.Local))
                return false;

            if (includeType == IncludeType.System && !this.flags.HasFlag(ShaderIncludeFlags.System))
                return false;

            string fullPath = Path.Combine(this.path, fileName);

            return File.Exists(fullPath);
        }
    }
}
