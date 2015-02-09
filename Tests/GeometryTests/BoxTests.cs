using FeralTic.DX11.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Tests.GeometryTests
{
    [TestClass]
    public partial class GeometryTests
    {
        [TestMethod()]
        public void ValidBox()
        {
            var box = new Box()
            {
                Size = new SharpDX.Vector3(1.0f,1.0f,1.0f)
            };
        }

        [TestMethod()]
        public void BoxRaiseEventsProperly()
        {
            int count = 0;

            var box = new Box()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
            box.PropertyChanged += (s, e) => count++;

            box.Size = new SharpDX.Vector3(2.0f, 0.0f, 0.0f);
            Assert.AreEqual<int>(count, 1);
        }


        [TestMethod()]
        public void BoxShouldNotRaiseEvent()
        {
            int count = 0;

            var box = new Box()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
            box.PropertyChanged += (s, e) => count++;

            box.Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f);
            Assert.AreEqual<int>(count, 0);
        }
    }
}
