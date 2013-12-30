using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using FeralTic.DX11.Resources;
using SharpDX.Direct3D;

namespace FeralTic.DX11.Geometry
{
    public abstract class AbstractPrimitiveDescriptor
    {
        public abstract string PrimitiveType { get; }
        public abstract void Initialize(Dictionary<string, object> properties);
        public abstract IDxGeometry GetGeometry(RenderDevice device);
    }


    public class Box : AbstractPrimitiveDescriptor
    {
        public Box()
        {
            this.Size = new Vector3(1,1,1);
        }

        public Vector3 Size { get; set; }

        public override string PrimitiveType { get { return "Box"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector3)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Box(this);
        }
    }

    public class Cylinder : AbstractPrimitiveDescriptor
    {
        public Cylinder()
        {
            this.Radius1 = 0.5f;
            this.Radius2 = 0.5f;
            this.Length = 1.0f;
            this.Cycles = 1.0f;
            this.Caps = true;
            this.ResolutionX = 15;
            this.ResolutionY = 1;
        }

        public float Radius1 { get; set; }
        public float Radius2 { get; set; }
        public float Cycles { get; set; }
        public float Length { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
        public bool Caps { get; set; }

        public override string PrimitiveType { get { return "Cylinder"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius1 = (float)properties["Radius1"];
            this.Radius2 = (float)properties["Radius2"];
            this.Cycles = (float)properties["Cycles"];
            this.Length = (float)properties["Length"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];
            this.Caps = (bool)properties["Caps"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Cylinder(this);
        }
    }

    public class Grid : AbstractPrimitiveDescriptor
    {
        public Grid()
        {
            this.Size = new Vector2(1, 1);
            this.ResolutionX = 2;
            this.ResolutionY = 2;
        }

        public Vector2 Size { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }

        public override string PrimitiveType { get { return "Grid"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector2)properties["Size"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Grid(this);
        }
    }

    public class IcoGrid : AbstractPrimitiveDescriptor
    {
        public IcoGrid()
        {
            this.Size = new Vector2(1, 1);
            this.ResolutionX = 2;
            this.ResolutionY = 2;
        }

        public Vector2 Size { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }

        public override string PrimitiveType { get { return "IcoGrid"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector2)properties["Size"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.IcoGrid(this);
        }
    }

    public class IcoSphere : AbstractPrimitiveDescriptor
    {
        public IcoSphere()
        {
            this.Radius = 0.5f;
            this.SubDivisions = 1;
        }

        public float Radius { get; set; }
        public int SubDivisions { get; set; }

        public override string PrimitiveType { get { return "IcoSphere"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
            this.SubDivisions = (int)properties["SubDivisions"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.IcoSphere(this);
        }
    }

    public class Isocahedron : AbstractPrimitiveDescriptor
    {
        public Isocahedron()
        {
            this.Size = new Vector3(1,1,1);
        }

        public Vector3 Size { get; set; }

        public override string PrimitiveType { get { return "Isocahedron"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector3)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Isocahedron(this);
        }
    }

    public class Octahedron : AbstractPrimitiveDescriptor
    {
        public Octahedron()
        {
            this.Size = new Vector3(1,1,1);
        }

        public Vector3 Size { get; set; }

        public override string PrimitiveType { get { return "Octahedron"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector3)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Octahedron(this);
        }
    }

    public class Quad : AbstractPrimitiveDescriptor
    {
        public Quad()
        {
            this.Size = new Vector2(1,1);
        }

        public Vector2 Size { get; set; }

        public override string PrimitiveType { get { return "Quad"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector2)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.QuadNormals(this);
        }
    }

    public class RoundRect : AbstractPrimitiveDescriptor
    {
        public RoundRect()
        {
            this.InnerRadius = new Vector2(0.35f, 0.35f);
            this.OuterRadius = 0.15f;
            this.EnableCenter = true;
            this.CornerResolution = 20;
        }

        public Vector2 InnerRadius { get; set;}
        public float OuterRadius { get; set;}
        public int CornerResolution { get; set;}
        public bool EnableCenter { get; set; }

        public override string PrimitiveType { get { return "RoundRect"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.InnerRadius = (Vector2)properties["InnerRadius"];
            this.OuterRadius = (float)properties["OuterRadius"];
            this.CornerResolution = (int)properties["CornerResolution"];
            this.EnableCenter = (bool)properties["EnableCenter"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.RoundRect(this);
        }

    }

    public class Segment : AbstractPrimitiveDescriptor
    {
        public Segment()
        {
            this.Phase = 0.0f;
            this.Cycles = 1.0f;
            this.InnerRadius = 0.0f;
            this.Flat = true;
            this.Resolution = 20;
        }

        public float Phase { get; set; }
        public float Cycles { get; set; }
        public float InnerRadius { get; set; }
        public bool Flat { get; set; }
        public int Resolution { get; set; }

        public override string PrimitiveType { get { return "Segment"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Phase = (float)properties["Phase"];
            this.Cycles = (float)properties["Cycles"];
            this.InnerRadius = (float)properties["InnerRadius"];
            this.Resolution = (int)properties["Resolution"];
            this.Flat = (bool)properties["Flat"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Segment(this);
        }
    }

    public class SegmentZ : AbstractPrimitiveDescriptor
    {
        public SegmentZ()
        {
            this.Phase = 0.0f;
            this.Cycles = 1.0f;
            this.InnerRadius = 0.0f;
            this.Z = 0.5f;
            this.Resolution = 20;
        }

        public float Phase { get; set; }
        public float Cycles { get; set; }
        public float InnerRadius { get; set; }
        public float Z { get; set; }
        public int Resolution { get; set; }

        public override string PrimitiveType { get { return "SegmentZ"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Phase = (float)properties["Phase"];
            this.Cycles = (float)properties["Cycles"];
            this.InnerRadius = (float)properties["InnerRadius"];
            this.Resolution = (int)properties["Resolution"];
            this.Z = (float)properties["Z"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.SegmentZ(this);
        }
    }

    public class Sphere : AbstractPrimitiveDescriptor
    {
        public Sphere()
        {
            this.Radius = 0.5f;
            this.CyclesX = 1.0f;
            this.CyclesY = 1.0f;
            this.ResX = 15;
            this.ResY = 15;
        }

        public float Radius { get; set; }
        public int ResX { get; set; }
        public int ResY { get; set; }
        public float CyclesX { get; set; }
        public float CyclesY { get; set; }

        public override string PrimitiveType { get { return "Sphere"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
            this.ResX = (int)properties["ResX"];
            this.ResY = (int)properties["ResY"];
            this.CyclesX = (float)properties["CyclesX"];
            this.CyclesY = (float)properties["CyclesY"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Sphere(this);
        }
    }

    public class Tetrahedron : AbstractPrimitiveDescriptor
    {
        public Tetrahedron()
        {
            this.Size = new Vector3(1.0f, 1.0f, 1.0f);
        }

        public Vector3 Size { get; set; }

        public override string PrimitiveType { get { return "Tetrahedron"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Size = (Vector3)properties["Size"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Tetrahedron(this);
        }
    }

    public class Torus : AbstractPrimitiveDescriptor
    {
        public Torus()
        {
            this.Radius = 0.5f;
            this.Thickness = 0.1f;
            this.ResolutionX = 15;
            this.ResolutionY = 15;
            this.PhaseX = 1.0f;
            this.PhaseY = 1.0f;
            this.Rotation = 1.0f;
            this.CY = 1.0f;
        }



        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
        public float Radius { get; set; }
        public float Thickness { get; set; }
        public float PhaseY { get; set; }
        public float PhaseX { get; set; }
        public float Rotation { get; set; }
        public float CY { get; set; }

        public override string PrimitiveType { get { return "Torus"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.Radius = (float)properties["Radius"];
            this.ResolutionX = (int)properties["ResolutionX"];
            this.ResolutionY = (int)properties["ResolutionY"];

            this.Thickness = (float)properties["Thickness"];

            this.PhaseY = (float)properties["PhaseY"];
            this.PhaseX = (float)properties["PhaseX"];
            this.Rotation = (float)properties["Rotation"];
            this.CY = (float)properties["CY"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Torus(this);
        }
    }

    public class NullGeometry : AbstractPrimitiveDescriptor
    {
        public NullGeometry()
        {
            this.VertexCount = 1;
            this.InstanceCount = 1;
            this.Topology = PrimitiveTopology.PointList;
        }

        public int VertexCount { get; set; }
        public int InstanceCount { get; set; }
        public PrimitiveTopology Topology { get; set; }

        public override string PrimitiveType { get { return "Null"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
            this.VertexCount = (int)properties["VertexCount"];
            this.InstanceCount = (int)properties["InstanceCount"];
            this.Topology = (PrimitiveTopology)properties["Topology"];
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.NullDrawer(this);
        }
    }

    public class Dispatcher : AbstractPrimitiveDescriptor
    {
        public Dispatcher()
        {
            this.ThreadCountX = 1;
            this.ThreadCountY = 1;
            this.ThreadCountZ = 1;
        }

        public int ThreadCountX { get; set; }
        public int ThreadCountY { get; set; }
        public int ThreadCountZ { get; set; }
 
        public override string PrimitiveType { get { return "Dispatcher"; } }

        public override void Initialize(Dictionary<string, object> properties)
        {
        }

        public override IDxGeometry GetGeometry(RenderDevice device)
        {
            return device.Primitives.Dispatcher(this);
        }
    }
}
