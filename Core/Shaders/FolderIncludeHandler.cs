using SharpDX;
using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FeralTic.DX11
{
    public class FolderIncludeHandler : CallbackBase, Include
    {
        public string BaseShaderPath { get; set; }

        public void Close(Stream stream)
        {
        }

        public Stream Open(IncludeType type, string fileName, Stream parentStream)
        {
            Stream stream;
            if (type == IncludeType.Local)
            {
                string path = Path.Combine(this.BaseShaderPath, fileName);
                if (File.Exists(path))
                {
                    string content = File.ReadAllText(path);
                    stream = new MemoryStream();
                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(content);
                    writer.Flush();
                    stream.Position = 0;
                }
                else
                {
                    stream = null;
                }
            }
            else
            {
                stream = null;
            }

            return stream;
        }
    }
}
