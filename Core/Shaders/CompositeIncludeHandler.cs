using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    public class CompositeIncludeHandler : Include
    {
        private readonly IEnumerable<IIncludeHandler> handlers;

        private IIncludeHandler selectedHandler;

        public CompositeIncludeHandler(IIncludeHandler handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException("handlers");

        }

        public void Close(Stream stream)
        {
            if (this.selectedHandler != null)
            {
                this.selectedHandler.Close(stream);
            }
        }

        public Stream Open(IncludeType type, string fileName, System.IO.Stream parentStream)
        {
            foreach (IIncludeHandler handler in this.handlers)
            {
                var stream = handler.Open(type, fileName, parentStream);
                if (stream != null)
                {
                    this.selectedHandler = handler;
                    return stream;
                }
            }
            return null;
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