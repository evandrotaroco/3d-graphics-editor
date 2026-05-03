using System.Collections.Generic;

namespace _3d_graphics_editor.Geometry
{
    public class Mesh
    {
        public List<Vertex> Vertices = new();
        public List<TexCoord> TexCoords = new();
        public List<Normal> Normals = new();
        public List<Face> Faces = new();
    }
}
