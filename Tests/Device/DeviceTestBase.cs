using FeralTic.DX11;
using FeralTic.DX11.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.Tests
{
    [TestClass]
    public class RenderDeviceTestBase
    {
        protected RenderDevice Device { get; set; }
        protected RenderContext RenderContext { get; set; }

        [TestInitialize()]
        public void Initialize()
        {
            this.Device = new RenderDevice();
            this.RenderContext = new RenderContext(this.Device);
        }

        [TestCleanup()]
        public void CleanUp()
        {
            if (RenderContext != null) { RenderContext.Dispose(); }
            if (Device != null) { Device.Dispose(); }
        }
    }
}
