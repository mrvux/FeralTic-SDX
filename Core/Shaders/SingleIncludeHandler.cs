using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    public class SingleIncludeHandler : Include
    {
        private readonly IIncludeHandler handler;

        public SingleIncludeHandler(IIncludeHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            this.handler = handler;
        }

        public void Close(Stream stream)
        {
            this.handler.Close(stream);
        }

        public Stream Open(IncludeType type, string fileName, System.IO.Stream parentStream)
        {
            return this.handler.Open(type, fileName, parentStream);
        }

        public IDisposable Shadow
        {
            get;
            set;
        }

        public void Dispose()
        {

        }
    }
}
