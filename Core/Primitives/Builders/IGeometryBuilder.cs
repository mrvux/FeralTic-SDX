﻿using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11.Geometry
{
    public class PrimitiveInfo
    {
        public bool PrimitivesKnown { get; private set; }
        public bool IsBoundingBoxKnown { get; private set; }

        public int VerticesCount { get; private set; }

        public int IndicesCount { get; private set; }

        public BoundingBox BoundingBox { get; private set; }

        private PrimitiveInfo()
        {

        }

        public PrimitiveInfo(int verticescount, int indicescount)
        {
            this.PrimitivesKnown = true;
            this.VerticesCount = verticescount;
            this.IndicesCount = indicescount;
            this.IsBoundingBoxKnown = false;
        }

        public PrimitiveInfo(int verticescount, int indicescount, BoundingBox box)
        {
            this.PrimitivesKnown = true;
            this.VerticesCount = verticescount;
            this.IndicesCount = indicescount;
            this.IsBoundingBoxKnown = true;
            this.BoundingBox = box;
        }

        public PrimitiveInfo(BoundingBox box)
        {
            this.PrimitivesKnown = false;
            this.VerticesCount = 0;
            this.IndicesCount = 0;
            this.IsBoundingBoxKnown = true;
            this.BoundingBox = box;
        }

        public static PrimitiveInfo UnKnown
        {
            get
            {
                PrimitiveInfo result = new PrimitiveInfo();
                result.PrimitivesKnown = false;
                result.IndicesCount = 0;
                result.VerticesCount = 0;
                result.IsBoundingBoxKnown = false;
                return result;
            }
        }
    }


    public interface IGeometryBuilder
    {
        void Construct(Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex);
    }

    public interface IGeometryBuilder<T> where T : AbstractPrimitiveDescriptor
    {
        PrimitiveInfo GetPrimitiveInfo(T settings);

        void Construct(T settings, Action<Vector3, Vector3, Vector2> appendVertex, Action<Int3> appendIndex);
    }
}
