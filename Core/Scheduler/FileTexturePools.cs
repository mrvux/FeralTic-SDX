using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    public class FileTexture1dPoolElement : FileTexturePoolElement<IDxTexture1D, FileTexture1dLoadTask>
    {
        public FileTexture1dPoolElement(RenderDevice device, string path, bool mips, bool async = false)
            : base(device, path, mips, async) { }

        protected override FileTexture1dLoadTask CreateTask(RenderDevice device)
        {
            return new FileTexture1dLoadTask(device, this.FileName);
        }
    }

    public class FileTexture2dPoolElement : FileTexturePoolElement<IDxTexture2D, FileTexture2dLoadTask>
    {
        public FileTexture2dPoolElement(RenderDevice device, string path, bool mips, bool async = false)
            : base(device, path, mips, async) { }

        protected override FileTexture2dLoadTask CreateTask(RenderDevice device)
        {
            return new FileTexture2dLoadTask(device, this.FileName, this.DoMips);
        }
    }

    public class FileTexture3dPoolElement : FileTexturePoolElement<IDxTexture3D, FileTexture3dLoadTask>
    {
        public FileTexture3dPoolElement(RenderDevice device, string path, bool mips, bool async = false)
            : base(device, path, mips, async) { }

        protected override FileTexture3dLoadTask CreateTask(RenderDevice device)
        {
            return new FileTexture3dLoadTask(device, this.FileName);
        }
    }

    public class FileTexture1dPool : FileTexturePool<FileTexture1dPoolElement,IDxTexture1D,FileTexture1dLoadTask>
    {
        protected override FileTexture1dPoolElement CreateElement(RenderDevice device, string path, bool mips, bool async = false)
        {
            return new FileTexture1dPoolElement(device, path, mips, async);
        }
    }

    public class FileTexture2dPool : FileTexturePool<FileTexture2dPoolElement, IDxTexture2D, FileTexture2dLoadTask>
    {
        protected override FileTexture2dPoolElement CreateElement(RenderDevice device, string path, bool mips, bool async = false)
        {
            return new FileTexture2dPoolElement(device, path, mips, async);
        }
    }

    public class FileTexture3dPool : FileTexturePool<FileTexture3dPoolElement, IDxTexture3D, FileTexture3dLoadTask>
    {
        protected override FileTexture3dPoolElement CreateElement(RenderDevice device, string path, bool mips, bool async = false)
        {
            return new FileTexture3dPoolElement(device, path, mips, async);
        }
    }

}
