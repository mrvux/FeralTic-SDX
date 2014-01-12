using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Direct3D11;

using FeralTic.DX11.Resources;
using FeralTic.DX11.Utils;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Geometry
{
    internal static class BoxData
    {
        public static Vector4 BottomLeftFront { get { return new Vector4(-1.0f, -1.0f, 1.0f, 1.0f); } }
        public static Vector4 BottomRightFront { get { return new Vector4(1.0f, -1.0f, 1.0f, 1.0f); } }
        public static Vector4 TopRightFront { get { return new Vector4(1.0f, 1.0f, 1.0f, 1.0f); } }
        public static Vector4 TopLeftFront { get { return new Vector4(-1.0f, 1.0f, 1.0f, 1.0f); } }

        public static Vector4 BottomLeftBack { get { return new Vector4(-1.0f, -1.0f, -1.0f, 1.0f); } }
        public static Vector4 BottomRightBack { get { return new Vector4(1.0f, -1.0f, -1.0f, 1.0f); } }
        public static Vector4 TopRightBack { get { return new Vector4(1.0f, 1.0f, -1.0f, 1.0f); } }
        public static Vector4 TopLeftBack { get { return new Vector4(-1.0f, 1.0f, -1.0f, 1.0f); } }

        public static Vector3 FrontNormal { get { return new Vector3(0.0f, 0.0f, -1.0f); } }
        public static Vector3 BackNormal { get { return new Vector3(0.0f, 0.0f, 1.0f); } }
        public static Vector3 TopNormal { get { return new Vector3(0.0f, 1.0f, 0.0f); } }
        public static Vector3 BottomNormal { get { return new Vector3(0.0f, -1.0f, 0.0f); } }
        public static Vector3 LeftNormal { get { return new Vector3(-1.0f, 0.0f, 0.0f); } }
        public static Vector3 RightNormal { get { return new Vector3(1.0f, 0.0f, 0.0f); } }

        public static Vector4 MulComp(this Vector4 v, Vector4 v2)
        {
            v.X *= v2.X;
            v.Y *= v2.Y;
            v.Z *= v2.Z;
            v.W = 1.0f;

            return v;
        }

        public static Vector2[] QuadUvLayout = new Vector2[] 
        {
            new Vector2(1.0f,1.0f),
            new Vector2(0.0f,1.0f),
            new Vector2(0.0f,0.0f),
            new Vector2(1.0f,0.0f),
        };
    }

    public partial class PrimitivesManager
    {
        public DX11IndexedGeometry Box(Box settings)
        {
            DX11IndexedGeometry geom = new DX11IndexedGeometry(device);
            geom.Tag = settings;
            geom.PrimitiveType = settings.PrimitiveType;

            DataStream vertexstream = new DataStream(24 * Pos4Norm3Tex2Vertex.VertexSize, true, true);
            vertexstream.Position = 0;

            var indexstream = new DataStream(36 * 4, true, true);

            float sx = 0.5f * settings.Size.X;
            float sy = 0.5f * settings.Size.Y;
            float sz = 0.5f * settings.Size.Z;

            Vector3 s3 = new Vector3(sx, sy, sz);
            Vector4 size = new Vector4(sx, sy, sz,1.0f);

            this.WriteFrontFace(vertexstream, indexstream, size);
            this.WriteBackFace(vertexstream, indexstream, size);
            this.WriteRightFace(vertexstream, indexstream, size);
            this.WriteLeftFace(vertexstream, indexstream, size);
            this.WriteTopFace(vertexstream, indexstream, size);
            this.WriteBottomFace(vertexstream, indexstream, size);

            geom.VertexBuffer = DX11VertexBuffer.CreateImmutable(device, 24, Pos4Norm3Tex2Vertex.VertexSize, vertexstream);
            geom.IndexBuffer = DX11IndexBuffer.CreateImmutable(device, 36, indexstream, true); 
            geom.InputLayout = Pos4Norm3Tex2Vertex.Layout;
            geom.VertexBuffer.InputLayout = geom.InputLayout;
            geom.Topology = PrimitiveTopology.TriangleList;
            geom.HasBoundingBox = true;
            geom.BoundingBox = new BoundingBox(-s3, s3);

            vertexstream.Dispose();
            indexstream.Dispose();

            return geom;
        }

        #region Faces
        private void WriteFrontFace(DataStream verts,DataStream inds, Vector4 size)
        {
            verts.Write<Vector4>(BoxData.BottomLeftFront.MulComp(size));
            verts.Write<Vector3>(BoxData.BackNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[0]);

            verts.Write<Vector4>(BoxData.BottomRightFront.MulComp(size));
            verts.Write<Vector3>(BoxData.BackNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[1]);

            verts.Write<Vector4>(BoxData.TopRightFront.MulComp(size));
            verts.Write<Vector3>(BoxData.BackNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[2]);

            verts.Write<Vector4>(BoxData.TopLeftFront.MulComp(size));
            verts.Write<Vector3>(BoxData.BackNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[3]);

            inds.WriteRange(new int[] { 0, 1, 2, 2, 3, 0 });
        }

        private void WriteBackFace(DataStream verts, DataStream inds, Vector4 size)
        {
            verts.Write<Vector4>(BoxData.BottomRightBack.MulComp(size));
            verts.Write<Vector3>(BoxData.FrontNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[0]);

            verts.Write<Vector4>(BoxData.BottomLeftBack.MulComp(size));
            verts.Write<Vector3>(BoxData.FrontNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[1]);

            verts.Write<Vector4>(BoxData.TopLeftBack.MulComp(size));
            verts.Write<Vector3>(BoxData.FrontNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[2]);

            verts.Write<Vector4>(BoxData.TopRightBack.MulComp(size));
            verts.Write<Vector3>(BoxData.FrontNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[3]);

            inds.WriteRange(new int[] { 4, 5, 6, 6, 7, 4 });
        }

        private void WriteRightFace(DataStream verts, DataStream inds, Vector4 size)
        {
            verts.Write<Vector4>(BoxData.BottomRightFront.MulComp(size));
            verts.Write<Vector3>(BoxData.RightNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[0]);

            verts.Write<Vector4>(BoxData.BottomRightBack.MulComp(size));
            verts.Write<Vector3>(BoxData.RightNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[1]);

            verts.Write<Vector4>(BoxData.TopRightBack.MulComp(size));
            verts.Write<Vector3>(BoxData.RightNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[2]);

            verts.Write<Vector4>(BoxData.TopRightFront.MulComp(size));
            verts.Write<Vector3>(BoxData.RightNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[3]);

            inds.WriteRange(new int[] { 8, 9, 10, 10, 11, 8 });
        }

        private void WriteLeftFace(DataStream verts, DataStream inds, Vector4 size)
        {
            verts.Write<Vector4>(BoxData.BottomLeftBack.MulComp(size));
            verts.Write<Vector3>(BoxData.LeftNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[0]);

            verts.Write<Vector4>(BoxData.BottomLeftFront.MulComp(size));
            verts.Write<Vector3>(BoxData.LeftNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[1]);

            verts.Write<Vector4>(BoxData.TopLeftFront.MulComp(size));
            verts.Write<Vector3>(BoxData.LeftNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[2]);

            verts.Write<Vector4>(BoxData.TopLeftBack.MulComp(size));
            verts.Write<Vector3>(BoxData.LeftNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[3]);

            inds.WriteRange(new int[] { 12, 13, 14, 14, 15, 12 });
        }

        private void WriteTopFace(DataStream verts, DataStream inds, Vector4 size)
        {
            verts.Write<Vector4>(BoxData.TopLeftFront.MulComp(size));
            verts.Write<Vector3>(BoxData.TopNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[2]);

            verts.Write<Vector4>(BoxData.TopRightFront.MulComp(size));
            verts.Write<Vector3>(BoxData.TopNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[3]);

            verts.Write<Vector4>(BoxData.TopRightBack.MulComp(size));
            verts.Write<Vector3>(BoxData.TopNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[0]);

            verts.Write<Vector4>(BoxData.TopLeftBack.MulComp(size));
            verts.Write<Vector3>(BoxData.TopNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[1]);

            inds.WriteRange(new int[] { 16, 17, 18, 18, 19, 16 });
        }

        private void WriteBottomFace(DataStream verts, DataStream inds, Vector4 size)
        {
            verts.Write<Vector4>(BoxData.BottomLeftBack.MulComp(size));
            verts.Write<Vector3>(BoxData.BottomNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[0]);

            verts.Write<Vector4>(BoxData.BottomRightBack.MulComp(size));
            verts.Write<Vector3>(BoxData.BottomNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[1]);

            verts.Write<Vector4>(BoxData.BottomRightFront.MulComp(size));
            verts.Write<Vector3>(BoxData.BottomNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[2]);

            verts.Write<Vector4>(BoxData.BottomLeftFront.MulComp(size));
            verts.Write<Vector3>(BoxData.BottomNormal);
            verts.Write<Vector2>(BoxData.QuadUvLayout[3]);

            inds.WriteRange(new int[] { 20, 21, 22, 22, 23, 20 });
        }
        #endregion
    }

}
