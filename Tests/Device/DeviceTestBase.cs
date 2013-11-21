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

        [TestInitialize()]
        public void Initialize()
        {
            this.Device = new RenderDevice();
        }

        [TestCleanup()]
        public void CleanUp()
        {
            if (Device != null) { Device.Dispose(); }
        }
    }
}
