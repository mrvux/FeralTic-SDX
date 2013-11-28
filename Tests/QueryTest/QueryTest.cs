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
            OcclusionQuery occlusion = new OcclusionQuery(this.Device);
            occlusion.Dispose();           
        }

        [TestMethod()]
        public void CreatePipelineStats()
        {
            OcclusionQuery query = new OcclusionQuery(this.Device);
            query.Dispose();
        }

        [TestMethod()]
        public void CreateStreamOut()
        {
            StreamOutQuery query = new StreamOutQuery(this.Device);
            query.Dispose();
        }

        [TestMethod()]
        public void CreateTimeStamp()
        {
            TimeStampQuery query = new TimeStampQuery(this.Device);
            query.Dispose();
        }

        



    }
}
