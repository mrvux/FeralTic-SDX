using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.DXGI;

using FeralTic.DX11;
using FeralTic.DX11.Resources;

namespace FeralTic.RenderLayers.TextureFX
{
    public class TextureFXSettings
    {
        public class DefaultTextureSettings
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public Format Format { get; set; }
        }

        public TextureFXSettings()
        {
            this.DefaultSettings = new DefaultTextureSettings();
            this.TemporaryResults = new List<ResourcePoolEntry<DX11RenderTarget2D>>();
        }

        public DefaultTextureSettings DefaultSettings { get; set; }

        public RenderDevice Device { get; set; }
        public RenderContext RenderContext { get; set; }

        public bool ForceDefaultSize { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Format Format { get; set; }
        public bool Mips { get; set; }

        public IDxTexture2D InitialTexture { get; set; }
        public IDxTexture2D PreviousTexture { get; set; }
        public ResourcePoolEntry<DX11RenderTarget2D> CurrentTargetEntry { get; set; }

        public DX11RenderTarget2D CurrentTarget
        {
            get { return this.CurrentTargetEntry; }
        }

        public List<ResourcePoolEntry<DX11RenderTarget2D>> TemporaryResults { get; set; }

        public void CleanUp()
        {
            foreach (ResourcePoolEntry<DX11RenderTarget2D> target in TemporaryResults)
            {
                target.UnLock();
            }
            this.CurrentTargetEntry = null;
        }

        public void ApplyInitial()
        {
            if (this.InitialTexture == null || this.ForceDefaultSize)
            {
                this.Width = this.DefaultSettings.Width;
                this.Height = this.DefaultSettings.Height;
                this.Format = Format.R8G8B8A8_UNorm;
            }
            else
            {
                this.Width = this.InitialTexture.Width;
                this.Height = this.InitialTexture.Height;
                this.Format = this.InitialTexture.Format;
            }
        }

        public void EndPass()
        {
            this.TemporaryResults.Add(this.CurrentTargetEntry);
            this.PreviousTexture = this.CurrentTarget;
        }

        public void GetRenderTarget()
        {
            this.CurrentTargetEntry = this.Device.ResourcePool.LockRenderTarget(Width, Height, Format, Mips, 0);
        }

        public void PushRenderTarget()
        {
            this.RenderContext.RenderTargetStack.Push(this.CurrentTarget);
        }

        public void ApplyMips()
        {
            this.RenderContext.Context.GenerateMips(this.CurrentTarget.ShaderView);
        }

        public void Scale(float factor)
        {
            this.Width = (int)((float)this.Width * factor);
            this.Height = (int)((float)this.Height * factor);
        }
    }
}
