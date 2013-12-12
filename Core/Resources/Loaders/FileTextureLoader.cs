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
            SharpDX.WIC.BitmapSource src = FileLoader.LoadBitmap(device.WICFactory, path);

            if (ct.IsCancellationRequested)
            {
                src.Dispose();
                throw new OperationCanceledException();
            }

            SharpDX.Direct3D11.Texture2D tex = FileLoader.CreateTexture2DFromBitmap(device, src, false);
            ShaderResourceView srv = new ShaderResourceView(device, tex);

            if (ct.IsCancellationRequested)
            {
                srv.Dispose();
                tex.Dispose();
                src.Dispose();
                throw new OperationCanceledException();
            }
            return DX11Texture2D.FromReference(device, tex, srv);
        }

        public static Task<IDxTexture2D> LoadFromFileAsync(RenderDevice device, string path, CancellationToken ct, IProgress<bool> started = null)
        {
            return Task.Run<IDxTexture2D>(() => (LoadFromFile(device, path, ct,started)));
        }
    }
}
