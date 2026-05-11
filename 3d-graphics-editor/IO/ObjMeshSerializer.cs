using System.Globalization;
using System.Drawing;
using System.Text;
using System.Linq;
using _3d_graphics_editor.Geometry;

namespace _3d_graphics_editor.IO
{
    public static class ObjMeshSerializer
    {
        private static readonly CultureInfo DecimalCommaCulture = new("pt-BR");

        public static Mesh Load(string path)
        {
            ArgumentNullException.ThrowIfNull(path);

            var mesh = new Mesh();
            var lineNumber = 0;
            var currentMaterialName = (string?)null;

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
                        case "mtllib":
                            LoadMaterialLibrary(parts, path, mesh);
                            break;
                        case "usemtl":
                            currentMaterialName = ParseMaterialName(parts);
                            break;
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
                            mesh.Faces.Add(ParseFace(parts, mesh, currentMaterialName));
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

        private static Face ParseFace(string[] parts, Mesh mesh, string? materialName)
        {
            if (parts.Length < 4)
            {
                throw new FormatException("Face must have at least three vertices.");
            }

            var face = new Face
            {
                MaterialName = materialName
            };

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

        private static void LoadMaterialLibrary(string[] parts, string objPath, Mesh mesh)
        {
            if (parts.Length < 2)
            {
                return;
            }

            var materialFileName = string.Join(" ", parts.Skip(1));
            var objDirectory = Path.GetDirectoryName(objPath) ?? string.Empty;
            var materialPath = Path.Combine(objDirectory, materialFileName);

            if (!File.Exists(materialPath))
            {
                return;
            }

            Material? currentMaterial = null;

            foreach (var rawLine in File.ReadLines(materialPath))
            {
                var line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                var tokens = SplitTokens(line);
                if (tokens.Length == 0)
                {
                    continue;
                }

                switch (tokens[0])
                {
                    case "newmtl":
                        currentMaterial = new Material
                        {
                            Name = string.Join(" ", tokens.Skip(1))
                        };

                        if (currentMaterial.Name.Length > 0)
                        {
                            mesh.Materials[currentMaterial.Name] = currentMaterial;
                        }
                        break;
                    case "Kd" when currentMaterial is not null && tokens.Length >= 4:
                        currentMaterial.DiffuseColor = ColorFromObj(tokens[1], tokens[2], tokens[3]);
                        break;
                }
            }
        }

        private static string? ParseMaterialName(string[] parts)
        {
            return parts.Length < 2 ? null : string.Join(" ", parts.Skip(1));
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
            value = value.Trim();

            // Alguns arquivos OBJ exportados em ambiente local usam virgula
            // como separador decimal, como acontece no Cereja.obj.
            if (value.Contains(',') && !value.Contains('.'))
            {
                if (float.TryParse(value, NumberStyles.Float, DecimalCommaCulture, out var decimalCommaParsed))
                {
                    return decimalCommaParsed;
                }
            }

            if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var invariantParsed))
            {
                return invariantParsed;
            }

            var normalizedValue = value.Replace(',', '.');
            if (float.TryParse(normalizedValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var normalizedParsed))
            {
                return normalizedParsed;
            }

            throw new FormatException($"Invalid floating-point value: \"{value}\"");
        }

        private static Color ColorFromObj(string red, string green, string blue)
        {
            static int ToByte(string value)
            {
                var component = ParseFloat(value);
                return Math.Clamp((int)MathF.Round(component * 255f), 0, 255);
            }

            return Color.FromArgb(ToByte(red), ToByte(green), ToByte(blue));
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
