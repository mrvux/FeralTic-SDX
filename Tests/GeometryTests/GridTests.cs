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
        public void ValidGrid()
        {
            var grid = new Grid()
            {
                Size = new SharpDX.Vector2(2, 2),
                ResolutionX = 16,
                ResolutionY = 16
            };
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidGridResolutionX()
        {
            var grid = new Grid()
            {
                Size = new SharpDX.Vector2(2, 2),
                ResolutionX = -2,
                ResolutionY = 4
            };
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidGridResolutionY()
        {
            var grid = new Grid()
            {
                Size = new SharpDX.Vector2(2, 2),
                ResolutionX = 4,
                ResolutionY = -2
            };
        }

        [TestMethod()]
        public void GridRaiseEventsProperly()
        {
            int count = 0;

            var grid = new Grid()
            {
                Size = new SharpDX.Vector2(2, 2),
                ResolutionX = 16,
                ResolutionY = 16
            };

            grid.PropertyChanged += (s, e) => count++;
            grid.Size = new SharpDX.Vector2(0.5f, 0.5f);
            grid.ResolutionX = 4;
            grid.ResolutionY = 4;

            Assert.AreEqual<int>(count, 3);
        }

        [TestMethod(), TestCategory("Geometry")]
        public void ValidIcoGrid()
        {
            var grid = new IcoGrid()
            {
                Size = new SharpDX.Vector2(2, 2),
                ResolutionX = 16,
                ResolutionY = 16
            };
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidIcoGridResolutionX()
        {
            var grid = new IcoGrid()
            {
                Size = new SharpDX.Vector2(2, 2),
                ResolutionX = -2,
                ResolutionY = 4
            };
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidIcoGridResolutionY()
        {
            var grid = new IcoGrid()
            {
                Size = new SharpDX.Vector2(2, 2),
                ResolutionX = 4,
                ResolutionY = -2
            };
        }

        [TestMethod()]
        public void IcoGridRaiseEventsProperly()
        {
            int count = 0;

            var grid = new IcoGrid()
            {
                Size = new SharpDX.Vector2(2, 2),
                ResolutionX = 16,
                ResolutionY = 16
            };

            grid.PropertyChanged += (s, e) => count++;
            grid.Size = new SharpDX.Vector2(0.5f, 0.5f);
            grid.ResolutionX = 4;
            grid.ResolutionY = 4;

            Assert.AreEqual<int>(count, 3);
        }
    }
}
