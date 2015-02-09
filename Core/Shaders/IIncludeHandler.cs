using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    /// <summary>
    /// Shortened interface for include handlers, does remove callback and disposable
    /// </summary>
    public interface IIncludeHandler
    {
        void Close(Stream stream);
        Stream Open(IncludeType type, string fileName, Stream parentStream);
    }
}
