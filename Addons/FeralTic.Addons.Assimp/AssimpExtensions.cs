using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Addons.AssetImport
{
    public static class AssimpExtensions
    {
        public static InputElement[] InputLayout(this Assimp.Mesh mesh,AssimpLoadInformation loadInfo, out int vertexsize)
        {
            List<InputElement> result = new List<InputElement>();
            int offset = 0;

            result.Add(new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0));
            offset += 12;

            if (mesh.HasNormals && loadInfo.Normals)
            {
                result.Add(new InputElement("NORMAL", 0, Format.R32G32B32_Float, offset, 0));
                offset += 12;
            }

            if (mesh.HasTangentBasis && loadInfo.Tangents)
            {
                result.Add(new InputElement("TANGENT", 0, Format.R32G32B32_Float, offset, 0));
                offset += 12;
                result.Add(new InputElement("BINORMAL", 0, Format.R32G32B32_Float, offset, 0));
                offset += 12;
            }

            if (loadInfo.VertexColors)
            {
                for (int i = 0; i < mesh.VertexColorChannelCount; i++)
                {
                    result.Add(new InputElement("COLOR", i, Format.R32G32B32A32_Float, offset, 0));
                    offset += 16;
                }
            }


            if (loadInfo.TexCoords)
            {
                for (int i = 0; i < mesh.TextureCoordsChannelCount; i++)
                {
                    int numcomp = mesh.GetUVComponentCount(i);
                    int stride = 4;
                    Format fmt = Format.R32_Float;

                    if (numcomp == 2)
                    {
                        fmt = Format.R32G32_Float;
                        stride = 8;
                    }
                    if (numcomp == 3)
                    {
                        fmt = Format.R32G32B32_Float;
                        stride = 12;
                    }
                    result.Add(new InputElement("TEXCOORD", i, fmt, offset, 0));
                    offset += stride;
                }
            }

            if (mesh.HasBones && loadInfo.Bones)
            {
                result.Add(new InputElement("BLENDINDICES", 0, Format.R32G32B32A32_Float, offset, 0));
                offset += 16;
                result.Add(new InputElement("BLENDWEIGHT", 0, Format.R32G32B32A32_Float, offset, 0));
                offset += 16;
            }

            vertexsize = offset;
            return result.ToArray();
        }

        public static DataStream LoadVertices(this Assimp.Mesh mesh,AssimpLoadInformation loadInfo, int vertexsize)
        {
            DataStream ds = new DataStream(mesh.VertexCount * vertexsize, true, true);
            for (int i = 0; i < mesh.VertexCount; i++)
            {
                ds.Write<Assimp.Vector3D>(mesh.Vertices[i]);

                if (mesh.HasNormals && loadInfo.Normals)
                {
                    ds.Write<Assimp.Vector3D>(mesh.Normals[i]);
                }

                if (mesh.HasTangentBasis && loadInfo.Tangents)
                {
                    ds.Write<Assimp.Vector3D>(mesh.Tangents[i]);
                    ds.Write<Assimp.Vector3D>(mesh.BiTangents[i]);
                }

                if (loadInfo.VertexColors)
                {
                    for (int j = 0; j < mesh.VertexColorChannelCount; j++)
                    {
                        ds.Write<Assimp.Color4D>(mesh.GetVertexColors(j)[i]);
                    }
                }

                if (loadInfo.TexCoords)
                {
                    for (int j = 0; j < mesh.TextureCoordsChannelCount; j++)
                    {
                        int numcomp = mesh.GetUVComponentCount(j);
                        if (numcomp == 1)
                        {
                            ds.Write<float>(mesh.GetTextureCoords(j)[i].X);
                        }
                        if (numcomp == 2)
                        {
                            ds.Write<float>(mesh.GetTextureCoords(j)[i].X);
                            ds.Write<float>(mesh.GetTextureCoords(j)[i].Y);
                        }
                        if (numcomp == 3)
                        {
                            ds.Write<Assimp.Vector3D>(mesh.GetTextureCoords(j)[i]);
                        }
                    }
                }
            }
            return ds;
        }


    }
}
