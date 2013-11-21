using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.Primitives
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple=false)]
    public class PrimitiveAttribute : Attribute
    {
        public string Name { get; set; }
    }

    [Primitive(Name="Box")]
    public class Box
    {
        public Vector3 Size { get; set; }
    }

    [Primitive(Name = "Cylinder")]
    public class Cylinder
    {
        public float Radius1 { get; set; }
        public float Radius2 { get; set; }
        public float Cycles { get; set; }
        public float Length { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
        public bool Caps { get; set; }
    }

    [Primitive(Name = "Grid")]
    public class Grid
    {
        public Vector2 Size { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
    }

    [Primitive(Name = "IcoGrid")]
    public class IcoGrid
    {
        public Vector2 Size { get; set; }
        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
    }

    [Primitive(Name = "IcoSphere")]
    public class IcoSphere
    {
        public float Radius { get; set; }
        public int SubDivisions { get; set; }
    }

    [Primitive(Name = "Isocahedron")]
    public class Isocahedron 
    {
        public Vector3 Size { get; set; }
    }

    [Primitive(Name = "Octahedron")]
    public class Octahedron
    {
        public Vector3 Size { get; set; }
    }

    [Primitive(Name = "Tetrahedron")]
    public class Tetrahedron
    {
        public Vector3 Size { get; set; }
    }

    [Primitive(Name = "Quad")]
    public class Quad
    {
        public Vector2 Size { get; set; }
    }

    [Primitive(Name = "RoundRect")]
    public class RoundRect
    {
        public Vector2 InnerRadius { get; set; }
        public float OuterRadius { get; set; }
        public int CornerResolution { get; set; }
        public bool EnableCenter { get; set; }
    }

    [Primitive(Name = "Segment")]
    public class Segment
    {
        public float Phase { get; set; }
        public float Cycles { get; set; }
        public float InnerRadius { get; set; }
        public bool Flat { get; set; }
        public int Resolution { get; set; }
    }

    [Primitive(Name = "SegmentZ")]
    public class SegmentZ
    {
        public float Phase { get; set; }
        public float Cycles { get; set; }
        public float InnerRadius { get; set; }
        public float Z { get; set; }
        public int Resolution { get; set; }
    }

    [Primitive(Name = "Sphere")]
    public class Sphere
    {
        public float Radius { get; set; }
        public int ResX { get; set; }
        public int ResY { get; set; }
        public float CyclesX { get; set; }
        public float CyclesY { get; set; }
    }
}
