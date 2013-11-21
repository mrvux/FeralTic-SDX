using FeralTic.DX11;
using FeralTic.DX11.Queries;
using FeralTic.DX11.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.Tests
{
    [TestClass]
    public class QueryTests : RenderDeviceTestBase
    {
        [TestMethod()]
        public void CreateOcclusion()
        {
            DX11OcclusionQuery occlusion = new DX11OcclusionQuery(this.Device);
            occlusion.Dispose();           
        }

        [TestMethod()]
        public void CreatePipelineStats()
        {
            DX11PipelineQuery query = new DX11PipelineQuery(this.Device);
            query.Dispose();
        }

        [TestMethod()]
        public void CreateStreamOut()
        {
            DX11StreamOutQuery query = new DX11StreamOutQuery(this.Device);
            query.Dispose();
        }

        [TestMethod()]
        public void CreateTimeStamp()
        {
            DX11TimeStampQuery query = new DX11TimeStampQuery(this.Device);
            query.Dispose();
        }

        



    }
}
