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
        [TestMethod(), TestCategory("Geometry")]
        public void ValidSphere()
        {
            var sphere = new Sphere()
            {
                CyclesX = 0.5f,
                CyclesY = 0.0f,
                Radius = 0.5f,
                ResX = 16,
                ResY = 16
            };
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidSphereResolutionX()
        {
            var sphere = new Sphere()
            {
                CyclesX = 0.5f,
                CyclesY = 0.0f,
                Radius = 0.5f,
                ResX = -5,
                ResY = 16
            };
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidSphereResolutionY()
        {
            var sphere = new Sphere()
            {
                CyclesX = 0.5f,
                CyclesY = 0.0f,
                Radius = 0.5f,
                ResX = 16,
                ResY = -8
            };
        }

        [TestMethod()]
        public void SphereRaiseEventsProperly()
        {
            int count = 0;

            var sphere = new Sphere()
            {
                CyclesX = 0.0f,
                CyclesY = 0.0f,
                Radius = 0.5f,
                ResX = 16,
                ResY = 16
            };

            sphere.PropertyChanged += (s, e) => count++;
            sphere.CyclesX = 1.0f;
            sphere.CyclesY = 1.0f;
            sphere.Radius = 1.0f;
            sphere.ResX = 32;
            sphere.ResY = 32;

            Assert.AreEqual<int>(count, 5);
        }




    }
}
