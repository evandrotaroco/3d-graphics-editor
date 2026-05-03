using System.Globalization;
using System.Text;
using _3d_graphics_editor.Geometry;

namespace _3d_graphics_editor.IO
{
    public static class ObjMeshSerializer
    {
        public static Mesh Load(string path)
        {
            ArgumentNullException.ThrowIfNull(path);

            var mesh = new Mesh();
            var lineNumber = 0;

            foreach (var rawLine in File.ReadLines(path))
            {
                lineNumber++;

                var line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                var parts = SplitTokens(line);
                if (parts.Length == 0)
                {
                    continue;
                }

                try
                {
                    switch (parts[0])
                    {
                        case "v":
                            mesh.Vertices.Add(ParseVertex(parts));
                            break;
                        case "vt":
                            mesh.TexCoords.Add(ParseTexCoord(parts));
                            break;
                        case "vn":
                            mesh.Normals.Add(ParseNormal(parts));
                            break;
                        case "f":
                            mesh.Faces.Add(ParseFace(parts, mesh));
                            break;
                    }
                }
                catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                {
                    throw new FormatException($"Invalid OBJ data at line {lineNumber}: \"{rawLine}\"", ex);
                }
            }

            return mesh;
        }

        
        private static string[] SplitTokens(string line)
        {
            return line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static Vertex ParseVertex(string[] parts)
        {
            if (parts.Length < 4)
            {
                throw new FormatException("Vertex must have three coordinates.");
            }

            return new Vertex(
                ParseFloat(parts[1]),
                ParseFloat(parts[2]),
                ParseFloat(parts[3]));
        }

        private static TexCoord ParseTexCoord(string[] parts)
        {
            if (parts.Length < 2)
            {
                throw new FormatException("Texture coordinate must have at least one coordinate.");
            }

            var u = ParseFloat(parts[1]);
            var v = parts.Length > 2 ? ParseFloat(parts[2]) : 0f;

            return new TexCoord(u, v);
        }

        private static Normal ParseNormal(string[] parts)
        {
            if (parts.Length < 4)
            {
                throw new FormatException("Normal must have three coordinates.");
            }

            return new Normal(
                ParseFloat(parts[1]),
                ParseFloat(parts[2]),
                ParseFloat(parts[3]));
        }

        private static Face ParseFace(string[] parts, Mesh mesh)
        {
            if (parts.Length < 4)
            {
                throw new FormatException("Face must have at least three vertices.");
            }

            var face = new Face();

            for (var i = 1; i < parts.Length; i++)
            {
                face.Vertices.Add(ParseFaceVertex(
                    parts[i],
                    mesh.Vertices.Count,
                    mesh.TexCoords.Count,
                    mesh.Normals.Count));
            }

            return face;
        }

        private static FaceVertex ParseFaceVertex(
            string token,
            int vertexCount,
            int texCoordCount,
            int normalCount)
        {
            var parts = token.Split('/');
            if (parts.Length == 0 || string.IsNullOrWhiteSpace(parts[0]))
            {
                throw new FormatException("Face vertex must reference a vertex index.");
            }

            return new FaceVertex(
                ResolveIndex(parts[0], vertexCount),
                parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]) ? ResolveIndex(parts[1], texCoordCount) : null,
                parts.Length > 2 && !string.IsNullOrWhiteSpace(parts[2]) ? ResolveIndex(parts[2], normalCount) : null);
        }

        private static int ResolveIndex(string rawValue, int itemCount)
        {
            var parsed = int.Parse(rawValue, CultureInfo.InvariantCulture);
            if (parsed == 0)
            {
                throw new FormatException("OBJ indices are 1-based and cannot be zero.");
            }

            var resolved = parsed > 0 ? parsed - 1 : itemCount + parsed;
            if (resolved < 0 || resolved >= itemCount)
            {
                throw new ArgumentOutOfRangeException(nameof(rawValue), "OBJ index is outside the available data range.");
            }

            return resolved;
        }

        private static float ParseFloat(string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture);
        }

        private static string FormatFaceVertex(FaceVertex faceVertex)
        {
            var vertexIndex = (faceVertex.VertexIndex + 1).ToString(CultureInfo.InvariantCulture);

            if (faceVertex.TexCoordIndex is null && faceVertex.NormalIndex is null)
            {
                return vertexIndex;
            }

            if (faceVertex.TexCoordIndex is not null && faceVertex.NormalIndex is null)
            {
                var texCoordIndex = (faceVertex.TexCoordIndex.Value + 1).ToString(CultureInfo.InvariantCulture);
                return $"{vertexIndex}/{texCoordIndex}";
            }

            if (faceVertex.TexCoordIndex is null && faceVertex.NormalIndex is not null)
            {
                var normalIndex = (faceVertex.NormalIndex.Value + 1).ToString(CultureInfo.InvariantCulture);
                return $"{vertexIndex}//{normalIndex}";
            }

            var formattedTexCoordIndex = (faceVertex.TexCoordIndex!.Value + 1).ToString(CultureInfo.InvariantCulture);
            var formattedNormalIndex = (faceVertex.NormalIndex!.Value + 1).ToString(CultureInfo.InvariantCulture);
            return $"{vertexIndex}/{formattedTexCoordIndex}/{formattedNormalIndex}";
        }

        private static string FormatFloat(float value)
        {
            return value.ToString("G9", CultureInfo.InvariantCulture);
        }
    }
}
