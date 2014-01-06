#if DIRECTX11_1
using FeralTic.DX11;
using FeralTic.DX11.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using System.Threading;

namespace FeralTic.Resources
{
    public class FileTextureLoader
    {
        public static IDxTexture2D LoadFromFile(RenderDevice device, string path, CancellationToken ct, IProgress<bool> started = null)
        {
            if (ct.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            if (started != null) { started.Report(true); }
            //Thread.Sleep(4000);

            IDxTexture2D texture = TextureLoader.LoadFromFile(device, path);
            if (ct.IsCancellationRequested)
            {
                texture.Dispose();
                throw new OperationCanceledException();
            }
            return texture;
        }

        public static Task<IDxTexture2D> LoadFromFileAsync(RenderDevice device, string path, CancellationToken ct, IProgress<bool> started = null)
        {
            return Task.Run<IDxTexture2D>(() => (LoadFromFile(device, path, ct,started)));
        }
   }
}
#endif