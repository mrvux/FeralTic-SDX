using Assimp;
using FeralTic.DX11;
using FeralTic.DX11.Resources;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Addons.AssetImport
{
    public class DxAssimpModelLoader
    {
        private DxDevice device;

        public DxAssimpModelLoader(DxDevice device)
        {
            this.device = device;
        }

        public DX11IndexedGeometry LoadFromMesh(Assimp.Mesh mesh, AssimpLoadInformation loadInfo)
        {
            uint[] inds = mesh.GetIndices();

            if (inds.Length > 0 && mesh.VertexCount > 0)
            {
                int vertexsize;
                var layout = mesh.InputLayout(loadInfo, out vertexsize);

                BoundingBox bb;
                DataStream ds = mesh.LoadVertices(loadInfo, vertexsize, out bb);

                DX11IndexedGeometry geom = new DX11IndexedGeometry(device)
                {
                    HasBoundingBox = true,
                    BoundingBox = bb,
                    IndexBuffer = DX11IndexBuffer.CreateImmutable(device, inds),
                    InputLayout = layout,
                    PrimitiveType = "AssimpModel",
                    Tag = null,
                    Topology = SharpDX.Direct3D.PrimitiveTopology.TriangleList,
                    VertexBuffer = DX11VertexBuffer.CreateImmutable(device, mesh.VertexCount, vertexsize, ds)
                };

                ds.Dispose();
                return geom;
            }

            return null;
        }

        public List<DX11IndexedGeometry> LoadModelsFromFile(string path, AssimpLoadInformation loadInfo)
        {
            Assimp.AssimpImporter importer = new AssimpImporter();
            Scene scene = importer.ImportFile(path, PostProcessPreset.TargetRealTimeQuality | PostProcessPreset.TargetRealTimeQuality);

            List<DX11IndexedGeometry> result = new List<DX11IndexedGeometry>();
            for (int j = 0; j < scene.MeshCount; j++)
            {
                Assimp.Mesh mesh = scene.Meshes[j];

                if (mesh.HasFaces && mesh.HasVertices)
                {
                    result.Add(this.LoadFromMesh(mesh, loadInfo));
                }
            }
            return result;
        }
    }
}
