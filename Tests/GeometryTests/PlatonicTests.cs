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
        public void ValidDodecaheron()
        {
            var solid = new Dodecahedron()
            {
                Size = new SharpDX.Vector3(1.0f,1.0f,1.0f)
            };
        }

        [TestMethod()]
        public void DodecaheronRaiseEventsProperly()
        {
            int count = 0;

            var solid = new Dodecahedron()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
            solid.PropertyChanged += (s, e) => count++;

            solid.Size = new SharpDX.Vector3(2.0f, 0.0f, 0.0f);
            Assert.AreEqual<int>(count, 1);
        }

        [TestMethod()]
        public void DodecaheronShouldNotRaiseEvent()
        {
            int count = 0;

            var solid = new Dodecahedron()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
            solid.PropertyChanged += (s, e) => count++;

            solid.Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f);
            Assert.AreEqual<int>(count, 0);
        }

        [TestMethod()]
        public void ValidOctahedron()
        {
            var solid = new Octahedron()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
        }

        [TestMethod()]
        public void OctahedronShouldNotRaiseEvent()
        {
            int count = 0;

            var solid = new Octahedron()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
            solid.PropertyChanged += (s, e) => count++;

            solid.Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f);
            Assert.AreEqual<int>(count, 0);
        }

        [TestMethod()]
        public void OctahedronRaiseEventsProperly()
        {
            int count = 0;

            var solid = new Octahedron()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
            solid.PropertyChanged += (s, e) => count++;
            solid.Size = new SharpDX.Vector3(2.0f, 0.0f, 0.0f);
            Assert.AreEqual<int>(count, 1);
        }

        [TestMethod()]
        public void ValidTetrahedron()
        {
            var solid = new Tetrahedron()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
        }

        [TestMethod()]
        public void TetrahedronRaiseEventsProperly()
        {
            int count = 0;
            var solid = new Tetrahedron()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
            solid.PropertyChanged += (s, e) => count++;
            solid.Size = new SharpDX.Vector3(2.0f, 0.0f, 0.0f);
            Assert.AreEqual<int>(count, 1);
        }

        [TestMethod()]
        public void TetrahedronShouldNotRaiseEvent()
        {
            int count = 0;

            var solid = new Tetrahedron()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
            solid.PropertyChanged += (s, e) => count++;
            solid.Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f);
            Assert.AreEqual<int>(count, 0);
        }


        [TestMethod()]
        public void ValidIsocahedron()
        {
            var solid = new Isocahedron()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
        }

        [TestMethod()]
        public void IsocahedronRaiseEventsProperly()
        {
            int count = 0;
            var solid = new Isocahedron()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
            solid.PropertyChanged += (s, e) => count++;
            solid.Size = new SharpDX.Vector3(2.0f, 0.0f, 0.0f);
            Assert.AreEqual<int>(count, 1);
        }

        [TestMethod()]
        public void IsocahedronShouldNotRaiseEvent()
        {
            int count = 0;

            var solid = new Isocahedron()
            {
                Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f)
            };
            solid.PropertyChanged += (s, e) => count++;
            solid.Size = new SharpDX.Vector3(1.0f, 1.0f, 1.0f);
            Assert.AreEqual<int>(count, 0);
        }
    }
}
