using System.Drawing.Drawing2D;
using System.Numerics;
using _3d_graphics_editor.Geometry;

namespace _3d_graphics_editor.Rendering
{
    public sealed class MeshViewportRenderer
    {
        private const float CameraDistance = 4.2f;
        private static readonly Color BackgroundTopColor = Color.FromArgb(250, 252, 255);
        private static readonly Color BackgroundBottomColor = Color.FromArgb(224, 231, 239);
        private static readonly Color GridColor = Color.FromArgb(223, 228, 235);
        private static readonly Color BorderColor = Color.FromArgb(120, 138, 160);
        private static readonly Color EdgeColor = Color.FromArgb(34, 47, 66);
        private static readonly Color PlaceholderColor = Color.FromArgb(92, 108, 126);

        public void Render(
            Graphics graphics,
            Rectangle viewport,
            Mesh? mesh,
            float rotationX,
            float rotationY,
            float rotationZ,
            float zoom)
        {
            if (viewport.Width <= 0 || viewport.Height <= 0)
            {
                return;
            }

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.White);

            var renderBounds = Rectangle.Inflate(viewport, -18, -18);
            if (renderBounds.Width <= 0 || renderBounds.Height <= 0)
            {
                return;
            }

            DrawBackdrop(graphics, renderBounds);

            if (mesh is null || mesh.Vertices.Count == 0 || mesh.Faces.Count == 0)
            {
                DrawEmptyState(graphics, renderBounds);
                return;
            }

            var transformedVertices = TransformVertices(mesh, rotationX, rotationY, rotationZ);
            var projectedVertices = new PointF?[transformedVertices.Length];

            for (var i = 0; i < transformedVertices.Length; i++)
            {
                projectedVertices[i] = TryProject(transformedVertices[i], renderBounds, zoom, out var point)
                    ? point
                    : null;
            }

            DrawWireframe(graphics, mesh, projectedVertices);
        }

        private static void DrawBackdrop(Graphics graphics, Rectangle bounds)
        {
            using var gradientBrush = new LinearGradientBrush(bounds, BackgroundTopColor, BackgroundBottomColor, 90f);
            using var gridPen = new Pen(GridColor, 1f);
            using var borderPen = new Pen(BorderColor, 1.5f);
            using var axisPen = new Pen(Color.FromArgb(185, 194, 205), 1f);

            graphics.FillRectangle(gradientBrush, bounds);

            const int spacing = 34;
            for (var x = bounds.Left + spacing; x < bounds.Right; x += spacing)
            {
                graphics.DrawLine(gridPen, x, bounds.Top, x, bounds.Bottom);
            }

            for (var y = bounds.Top + spacing; y < bounds.Bottom; y += spacing)
            {
                graphics.DrawLine(gridPen, bounds.Left, y, bounds.Right, y);
            }

            var centerX = bounds.Left + bounds.Width / 2f;
            var centerY = bounds.Top + bounds.Height / 2f;
            graphics.DrawLine(axisPen, bounds.Left, centerY, bounds.Right, centerY);
            graphics.DrawLine(axisPen, centerX, bounds.Top, centerX, bounds.Bottom);
            graphics.DrawRectangle(borderPen, bounds);
        }

        private static void DrawEmptyState(Graphics graphics, Rectangle bounds)
        {
            const string title = "Nenhum modelo carregado";
            const string subtitle = "Use o botao Abrir .obj para visualizar um objeto 3D aqui.";

            using var titleFont = new Font("Segoe UI", 16f, FontStyle.Bold);
            using var subtitleFont = new Font("Segoe UI", 10f, FontStyle.Regular);
            var titleSize = graphics.MeasureString(title, titleFont);
            var subtitleSize = graphics.MeasureString(subtitle, subtitleFont);

            var titlePoint = new PointF(
                bounds.Left + (bounds.Width - titleSize.Width) / 2f,
                bounds.Top + (bounds.Height - titleSize.Height - subtitleSize.Height - 8f) / 2f);

            var subtitlePoint = new PointF(
                bounds.Left + (bounds.Width - subtitleSize.Width) / 2f,
                titlePoint.Y + titleSize.Height + 8f);

            using var titleBrush = new SolidBrush(PlaceholderColor);
            using var subtitleBrush = new SolidBrush(Color.FromArgb(115, 126, 139));
            graphics.DrawString(title, titleFont, titleBrush, titlePoint);
            graphics.DrawString(subtitle, subtitleFont, subtitleBrush, subtitlePoint);
        }

        private static Vector3[] TransformVertices(Mesh mesh, float rotationX, float rotationY, float rotationZ)
        {
            var bounds = ComputeBounds(mesh.Vertices);
            var center = (bounds.Min + bounds.Max) * 0.5f;
            var size = bounds.Max - bounds.Min;
            var maxDimension = MathF.Max(size.X, MathF.Max(size.Y, size.Z));
            var scale = maxDimension <= 0.0001f ? 1f : 2f / maxDimension;

            var rotationMatrix =
                Matrix4x4.CreateRotationX(rotationX) *
                Matrix4x4.CreateRotationY(rotationY) *
                Matrix4x4.CreateRotationZ(rotationZ);

            var transformed = new Vector3[mesh.Vertices.Count];
            for (var i = 0; i < mesh.Vertices.Count; i++)
            {
                var vertex = mesh.Vertices[i];
                var normalized = new Vector3(vertex.X, vertex.Y, vertex.Z) - center;
                normalized *= scale;
                transformed[i] = Vector3.Transform(normalized, rotationMatrix);
            }

            return transformed;
        }

        private static (Vector3 Min, Vector3 Max) ComputeBounds(IReadOnlyList<Vertex> vertices)
        {
            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (var vertex in vertices)
            {
                min.X = MathF.Min(min.X, vertex.X);
                min.Y = MathF.Min(min.Y, vertex.Y);
                min.Z = MathF.Min(min.Z, vertex.Z);

                max.X = MathF.Max(max.X, vertex.X);
                max.Y = MathF.Max(max.Y, vertex.Y);
                max.Z = MathF.Max(max.Z, vertex.Z);
            }

            return (min, max);
        }

        private static bool TryProject(Vector3 point, Rectangle bounds, float zoom, out PointF projectedPoint)
        {
            var depth = point.Z + CameraDistance;
            if (depth <= 0.01f)
            {
                projectedPoint = PointF.Empty;
                return false;
            }

            var projectionScale = Math.Min(bounds.Width, bounds.Height) * 0.78f * zoom;
            var perspective = projectionScale / depth;
            var centerX = bounds.Left + bounds.Width / 2f;
            var centerY = bounds.Top + bounds.Height / 2f;

            projectedPoint = new PointF(
                centerX + point.X * perspective,
                centerY - point.Y * perspective);

            return true;
        }

        private static void DrawWireframe(Graphics graphics, Mesh mesh, PointF?[] projectedVertices)
        {
            using var edgePen = new Pen(EdgeColor, 1.15f);
            var drawnEdges = new HashSet<Edge>();

            foreach (var face in mesh.Faces)
            {
                if (face.Vertices.Count < 2)
                {
                    continue;
                }

                for (var i = 0; i < face.Vertices.Count; i++)
                {
                    var fromIndex = face.Vertices[i].VertexIndex;
                    var toIndex = face.Vertices[(i + 1) % face.Vertices.Count].VertexIndex;

                    if (!IsValidIndex(fromIndex, projectedVertices.Length) ||
                        !IsValidIndex(toIndex, projectedVertices.Length) ||
                        projectedVertices[fromIndex] is null ||
                        projectedVertices[toIndex] is null)
                    {
                        continue;
                    }

                    var edge = new Edge(fromIndex, toIndex);
                    if (!drawnEdges.Add(edge))
                    {
                        continue;
                    }

                    graphics.DrawLine(edgePen, projectedVertices[fromIndex]!.Value, projectedVertices[toIndex]!.Value);
                }
            }
        }

        private static bool IsValidIndex(int index, int count)
        {
            return index >= 0 && index < count;
        }

        private readonly record struct Edge
        {
            public Edge(int first, int second)
            {
                if (first <= second)
                {
                    First = first;
                    Second = second;
                }
                else
                {
                    First = second;
                    Second = first;
                }
            }

            public int First { get; }

            public int Second { get; }
        }
    }
}
