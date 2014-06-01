using FeralTic.DX11.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Tests.GeometryTests
{
    public partial class GeometryTests
    {
        [TestMethod()]
        public void ValidCylinder()
        {
            var cylinder = new Cylinder()
            {
                Caps = true,
                Cycles = 1.0f,
                Length = 1.0f,
                Radius1 = 0.5f,
                Radius2 = 0.5f,
                ResolutionX = 16,
                ResolutionY = 16      
            };
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidCylinderResolutionX()
        {
            var cylinder = new Cylinder()
            {
                Caps = true,
                Cycles = 1.0f,
                Length = 1.0f,
                Radius1 = 0.5f,
                Radius2 = 0.5f,
                ResolutionX = -5,
                ResolutionY = 16
            };
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidCylinderResolutionY()
        {
            var cylinder = new Cylinder()
            {
                Caps = true,
                Cycles = 1.0f,
                Length = 1.0f,
                Radius1 = 0.5f,
                Radius2 = 0.5f,
                ResolutionX = 16,
                ResolutionY = -5
            };
        }

        [TestMethod()]
        public void CylinderRaiseEventsProperly()
        {
            int count = 0;

            var cylinder = new Cylinder()
            {
                Caps = true,
                Cycles = 1.0f,
                Length = 1.0f,
                Radius1 = 0.5f,
                Radius2 = 0.5f,
                ResolutionX = 16,
                ResolutionY = 16
            };

            cylinder.PropertyChanged += (s, e) => count++;
            cylinder.Caps = false;
            cylinder.Cycles = 0.0f;
            cylinder.Length = 0.5f;
            cylinder.Radius1 = 0;
            cylinder.Radius2 = 0;
            cylinder.ResolutionX = 32;
            cylinder.ResolutionY = 32;

            Assert.AreEqual<int>(count, 7);
        }




    }
}
