namespace _3d_graphics_editor.Geometry
{
    public class FaceVertex
    {
        public int VertexIndex;
        public int? TexCoordIndex;
        public int? NormalIndex;

        public FaceVertex()
        {
        }

        public FaceVertex(int vertexIndex, int? texCoordIndex = null, int? normalIndex = null)
        {
            VertexIndex = vertexIndex;
            TexCoordIndex = texCoordIndex;
            NormalIndex = normalIndex;
        }
    }
}
