using FeralTic.DX11.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    public abstract class FileTextureLoadTask<T> : AbstractLoadTask<T> where T : IDxResource
    {
        protected string path;

        public FileTextureLoadTask(RenderDevice device, string path)
            : base(device)
        {
            this.path = path;
        }

        public override void Dispose()
        {
            if (this.Resource != null) { this.Resource.Dispose(); }
        }
    }

    public class FileTexture2dLoadTask : FileTextureLoadTask<IDxTexture2D>
    {
        private bool domips;

        public FileTexture2dLoadTask(RenderDevice device, string path, bool domips)
            : base(device, path)
        {
            this.domips = domips;
        }

        protected override void DoProcess()
        {
            this.Resource = TextureLoader.LoadFromFile(this.Device, path);
        }
    }

    public class FileTexture1dLoadTask : FileTextureLoadTask<IDxTexture1D>
    {
        public FileTexture1dLoadTask(RenderDevice device, string path)
            : base(device, path)
        {
        }

        protected override void DoProcess()
        {
            //this.Resource = DX11Texture1D.FromFile(this.Context, path);
        }
    }

    public class FileTexture3dLoadTask : FileTextureLoadTask<IDxTexture3D>
    {
        public FileTexture3dLoadTask(RenderDevice device, string path)
            : base(device, path)
        {
        }

        protected override void DoProcess()
        {
            //this.Resource = DX11Texture3D.FromFile(this.Context, path);
        }
    }
}
