using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using _3d_graphics_editor.Geometry;

namespace _3d_graphics_editor.Rendering
{
    public sealed class MeshViewportRenderer
    {
        private const float CameraDistance = 4.2f;
        private const float PerspectiveScaleFactor = 0.78f;
        private const float ParallelProjectionScaleFactor = 0.22f;
        private const float OnePointPerspectiveScreenDivisor = 1400f;
        private const float FixedOnePointFocalDistance = 300f;
        private const float ObliqueAngleDegrees = 45f;
        private const float CavalierDepthFactor = 1f;
        private const float CabinetDepthFactor = 0.5f;
        private const float PerspectiveBaseDepthOffset = 2f;
        private const float DepthEpsilon = 0.0001f;
        private const int ThumbnailGap = 10;
        private const int TransparentPixel = 0;
        private static readonly Color BackgroundTopColor = Color.FromArgb(250, 252, 255);
        private static readonly Color BackgroundBottomColor = Color.FromArgb(224, 231, 239);
        private static readonly Color GridColor = Color.FromArgb(223, 228, 235);
        private static readonly Color BorderColor = Color.FromArgb(120, 138, 160);
        private static readonly Color EdgeColor = Color.FromArgb(34, 47, 66);
        private static readonly Color PlaceholderColor = Color.FromArgb(92, 108, 126);
        private static readonly Color DefaultFaceColor = Color.FromArgb(188, 205, 226);
        private static readonly Color CavalierProjectionColor = Color.FromArgb(181, 58, 66);
        private static readonly Color CabinetProjectionColor = Color.FromArgb(41, 141, 77);
        private static readonly Color OnePointPerspectiveColor = Color.FromArgb(46, 101, 185);

        private float[] _zBuffer = Array.Empty<float>();
        private int[] _colorBuffer = Array.Empty<int>();
        private Bitmap? _rasterBitmap;

        public void Render(
            Graphics graphics,
            Rectangle viewport,
            Mesh? mesh,
            TransformState transform,
            ViewportRenderOptions options)
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

            var transformedVertices = TransformVertices(mesh, transform);
            var mainProjection = options.Mode == ViewportMode.Projection
                ? options.Projection
                : ProjectionView.Normal;
            var projectionVertices = ApplyProjectionViewTransform(transformedVertices, mainProjection, options.Parameters);

            var renderableFaces = CreateRenderableFaces(
                mesh,
                projectionVertices,
                renderBounds,
                mainProjection,
                options.ShowBackFaces,
                options.Parameters);

            DrawProjectedMesh(graphics, renderableFaces, renderBounds, options.FillFaces, EdgeColor, null);

            if (mainProjection == ProjectionView.OnePointPerspective)
            {
                DrawOnePointPerspectiveGuides(graphics, renderBounds, OnePointPerspectiveColor);
            }

            if (options.Mode == ViewportMode.Projection)
            {
                DrawProjectionModeBadge(graphics, renderBounds, mainProjection, options.Parameters);
                DrawProjectionThumbnails(graphics, mesh, transformedVertices, renderBounds, options);
            }
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

        private static Vector3D[] TransformVertices(Mesh mesh, TransformState transform)
        {
            var bounds = ComputeBounds(mesh.Vertices);
            var center = new Vector3D(
                (bounds.Min.X + bounds.Max.X) * 0.5f,
                (bounds.Min.Y + bounds.Max.Y) * 0.5f,
                (bounds.Min.Z + bounds.Max.Z) * 0.5f);

            var size = new Vector3D(
                bounds.Max.X - bounds.Min.X,
                bounds.Max.Y - bounds.Min.Y,
                bounds.Max.Z - bounds.Min.Z);

            var maxDimension = MathF.Max(size.X, MathF.Max(size.Y, size.Z));
            var normalizationScale = maxDimension <= 0.0001f ? 1f : 2f / maxDimension;

            // Mantemos toda a cadeia de transformacao 4x4 em um unico produto
            // para que translacao, escala e rotacoes sejam aplicadas juntas.
            var combinedMatrix = Transform3D.Compose(
                Transform3D.CreateTranslation(transform.TranslationX, transform.TranslationY, transform.TranslationZ),
                Transform3D.CreateRotationZ(transform.RotationZ),
                Transform3D.CreateRotationY(transform.RotationY),
                Transform3D.CreateRotationX(transform.RotationX),
                Transform3D.CreateScale(transform.ScaleX, transform.ScaleY, transform.ScaleZ),
                Transform3D.CreateScale(normalizationScale, normalizationScale, normalizationScale),
                Transform3D.CreateTranslation(-center.X, -center.Y, -center.Z));

            var transformed = new Vector3D[mesh.Vertices.Count];
            for (var i = 0; i < mesh.Vertices.Count; i++)
            {
                var transformedVertex = Transform3D.TransformVertex(mesh.Vertices[i], combinedMatrix);
                transformed[i] = new Vector3D(transformedVertex.X, transformedVertex.Y, transformedVertex.Z);
            }

            return transformed;
        }

        private static Vector3D[] ApplyProjectionViewTransform(
            Vector3D[] transformedVertices,
            ProjectionView projection,
            ProjectionParameters parameters)
        {
            var adjustedVertices = new Vector3D[transformedVertices.Length];

            for (var i = 0; i < transformedVertices.Length; i++)
            {
                var point = transformedVertices[i];

                if (projection == ProjectionView.Cavalier || projection == ProjectionView.Cabinet)
                {
                    point = RotateY(point, DegreesToRadians(parameters.ObliqueRotationYDegrees));
                }
                else if (projection == ProjectionView.OnePointPerspective)
                {
                    point = RotateX(point, DegreesToRadians(parameters.PerspectiveRotationXDegrees));
                    point = RotateY(point, DegreesToRadians(parameters.PerspectiveRotationYDegrees));
                    point.Z += MapPerspectiveZOffsetToDepth(parameters.PerspectiveZOffset);
                }

                adjustedVertices[i] = point;
            }

            return adjustedVertices;
        }

        private static List<RenderableFace> CreateRenderableFaces(
            Mesh mesh,
            Vector3D[] transformedVertices,
            Rectangle bounds,
            ProjectionView projection,
            bool showBackFaces,
            ProjectionParameters parameters)
        {
            var projectedVertices = ProjectVertices(transformedVertices, bounds, projection, parameters);
            return BuildRenderableFaces(mesh, transformedVertices, projectedVertices, projection, showBackFaces, parameters);
        }

        private static PointF?[] ProjectVertices(
            Vector3D[] transformedVertices,
            Rectangle bounds,
            ProjectionView projection,
            ProjectionParameters parameters)
        {
            var projectedVertices = new PointF?[transformedVertices.Length];

            for (var i = 0; i < transformedVertices.Length; i++)
            {
                projectedVertices[i] = TryProject(transformedVertices[i], bounds, projection, parameters, out var point)
                    ? point
                    : null;
            }

            return projectedVertices;
        }

        private static List<RenderableFace> BuildRenderableFaces(
            Mesh mesh,
            Vector3D[] transformedVertices,
            PointF?[] projectedVertices,
            ProjectionView projection,
            bool showBackFaces,
            ProjectionParameters parameters)
        {
            var renderableFaces = new List<RenderableFace>(mesh.Faces.Count);

            foreach (var face in mesh.Faces)
            {
                if (face.Vertices.Count < 3)
                {
                    continue;
                }

                if (!TryCreateRenderableFace(
                        mesh,
                        face,
                        transformedVertices,
                        projectedVertices,
                        projection,
                        parameters,
                        out var renderableFace))
                {
                    continue;
                }

                if (!showBackFaces && !renderableFace.IsFrontFace)
                {
                    continue;
                }

                renderableFaces.Add(renderableFace);
            }

            return renderableFaces;
        }

        private static bool TryCreateRenderableFace(
            Mesh mesh,
            Face face,
            Vector3D[] transformedVertices,
            PointF?[] projectedVertices,
            ProjectionView projection,
            ProjectionParameters parameters,
            out RenderableFace renderableFace)
        {
            var screenVertices = new ScreenVertex[face.Vertices.Count];
            var faceVertices = new Vector3D[face.Vertices.Count];

            for (var i = 0; i < face.Vertices.Count; i++)
            {
                var vertexIndex = face.Vertices[i].VertexIndex;
                if (!IsValidIndex(vertexIndex, transformedVertices.Length) ||
                    projectedVertices[vertexIndex] is null)
                {
                    renderableFace = default;
                    return false;
                }

                faceVertices[i] = transformedVertices[vertexIndex];

                var depth = GetDepthValue(transformedVertices[vertexIndex], projection, parameters);
                screenVertices[i] = new ScreenVertex(projectedVertices[vertexIndex]!.Value, depth);
            }

            var geometricNormal = ComputeFaceNormal(faceVertices[0], faceVertices[1], faceVertices[2]);
            if (LengthSquared(geometricNormal) <= 0.0000001f)
            {
                renderableFace = default;
                return false;
            }

            var isFrontFace = IsFrontFace(faceVertices[0], geometricNormal, projection, parameters);
            var fillColor = GetFaceColor(mesh, face);
            var useReciprocalDepthInterpolation = projection is ProjectionView.Normal or ProjectionView.OnePointPerspective;

            renderableFace = new RenderableFace(
                screenVertices,
                fillColor,
                isFrontFace,
                useReciprocalDepthInterpolation);
            return true;
        }

        private static Color GetFaceColor(Mesh mesh, Face face)
        {
            var baseColor = DefaultFaceColor;

            if (face.MaterialName is not null && mesh.Materials.TryGetValue(face.MaterialName, out var material))
            {
                baseColor = material.DiffuseColor;
            }

            return baseColor;
        }

        private static (Vector3D Min, Vector3D Max) ComputeBounds(IReadOnlyList<Vertex> vertices)
        {
            var min = new Vector3D(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3D(float.MinValue, float.MinValue, float.MinValue);

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

        private static bool TryProject(
            Vector3D point,
            Rectangle bounds,
            ProjectionView projection,
            ProjectionParameters parameters,
            out PointF projectedPoint)
        {
            return projection switch
            {
                ProjectionView.Cavalier => TryProjectOblique(point, bounds, CavalierDepthFactor, parameters.ObliqueAlphaDegrees, out projectedPoint),
                ProjectionView.Cabinet => TryProjectOblique(point, bounds, CabinetDepthFactor, parameters.ObliqueAlphaDegrees, out projectedPoint),
                ProjectionView.OnePointPerspective => TryProjectOnePointPerspective(point, bounds, FixedOnePointFocalDistance, out projectedPoint),
                _ => TryProjectStandardPerspective(point, bounds, out projectedPoint)
            };
        }

        private static bool TryProjectStandardPerspective(
            Vector3D point,
            Rectangle bounds,
            out PointF projectedPoint)
        {
            var depth = point.Z + CameraDistance;
            if (depth <= 0.01f)
            {
                projectedPoint = PointF.Empty;
                return false;
            }

            var projectionScale = Math.Min(bounds.Width, bounds.Height) * PerspectiveScaleFactor;
            var perspective = projectionScale / depth;
            var centerX = bounds.Left + bounds.Width / 2f;
            var centerY = bounds.Top + bounds.Height / 2f;

            projectedPoint = new PointF(
                centerX + point.X * perspective,
                centerY - point.Y * perspective);

            return true;
        }

        private static bool TryProjectOnePointPerspective(
            Vector3D point,
            Rectangle bounds,
            float focalDistance,
            out PointF projectedPoint)
        {
            // Formula do material:
            // x' = x * d / z
            // y' = y * d / z
            // com COP na origem e plano de projecao em z = d.
            var depth = point.Z;
            if (depth <= 0.01f)
            {
                projectedPoint = PointF.Empty;
                return false;
            }

            var centerX = bounds.Left + bounds.Width / 2f;
            var centerY = bounds.Top + bounds.Height / 2f;
            var projectedX = point.X * focalDistance / depth;
            var projectedY = point.Y * focalDistance / depth;
            var screenScale = Math.Min(bounds.Width, bounds.Height) / OnePointPerspectiveScreenDivisor;

            projectedPoint = new PointF(
                centerX + projectedX * screenScale,
                centerY - projectedY * screenScale);

            return true;
        }

        private static bool TryProjectOblique(
            Vector3D point,
            Rectangle bounds,
            float depthFactor,
            float alphaDegrees,
            out PointF projectedPoint)
        {
            // Formula da projecao obliqua do material:
            // xp = x + z * L * cos(a) | yp = y + z * L * sin(a)
            var alpha = DegreesToRadians(alphaDegrees);
            var horizontal = point.X + (point.Z * depthFactor * MathF.Cos(alpha));
            var vertical = point.Y + (point.Z * depthFactor * MathF.Sin(alpha));
            var projectionScale = Math.Min(bounds.Width, bounds.Height) * ParallelProjectionScaleFactor;
            var centerX = bounds.Left + bounds.Width / 2f;
            var centerY = bounds.Top + bounds.Height / 2f;

            projectedPoint = new PointF(
                centerX + horizontal * projectionScale,
                centerY - vertical * projectionScale);

            return true;
        }

        private static bool IsFrontFace(
            Vector3D facePoint,
            Vector3D geometricNormal,
            ProjectionView projection,
            ProjectionParameters parameters)
        {
            if (projection == ProjectionView.Normal)
            {
                var observerToFace = new Vector3D(facePoint.X, facePoint.Y, facePoint.Z + CameraDistance);
                return Dot(observerToFace, geometricNormal) < 0f;
            }

            if (projection == ProjectionView.OnePointPerspective)
            {
                return Dot(facePoint, geometricNormal) < 0f;
            }

            var viewVector = GetObliqueProjectionDirection(projection, parameters);

            return Dot(viewVector, geometricNormal) < 0f;
        }

        private static float GetDepthValue(Vector3D point, ProjectionView projection, ProjectionParameters parameters)
        {
            if (projection == ProjectionView.Cavalier || projection == ProjectionView.Cabinet)
            {
                return Dot(point, GetObliqueProjectionDirection(projection, parameters));
            }

            if (projection == ProjectionView.Normal)
            {
                return point.Z + CameraDistance;
            }

            return point.Z;
        }

        private void DrawProjectedMesh(
            Graphics graphics,
            IReadOnlyList<RenderableFace> renderableFaces,
            Rectangle bounds,
            bool fillFaces,
            Color edgeColor,
            Color? uniformFillColor)
        {
            if (renderableFaces.Count == 0 || bounds.Width <= 0 || bounds.Height <= 0)
            {
                return;
            }

            if (fillFaces)
            {
                DrawZBufferedMesh(graphics, renderableFaces, bounds, true, edgeColor, uniformFillColor);
                return;
            }

            DrawWireframe(graphics, renderableFaces, bounds, edgeColor);
        }

        private void DrawZBufferedMesh(
            Graphics graphics,
            IReadOnlyList<RenderableFace> renderableFaces,
            Rectangle bounds,
            bool fillFaces,
            Color edgeColor,
            Color? uniformFillColor)
        {
            var width = bounds.Width;
            var height = bounds.Height;
            var pixelCount = width * height;

            EnsureRasterResources(width, height);
            Array.Fill(_zBuffer, float.PositiveInfinity, 0, pixelCount);
            Array.Fill(_colorBuffer, TransparentPixel, 0, pixelCount);

            foreach (var face in renderableFaces)
            {
                var faceColor = (uniformFillColor ?? face.FillColor).ToArgb();
                RasterizeFace(face, bounds, width, height, fillFaces, faceColor);
            }

            CopyColorBufferToBitmap(width, height);
            DrawEdgeOverlay(renderableFaces, bounds, width, height, edgeColor);

            graphics.DrawImage(
                _rasterBitmap!,
                bounds,
                new Rectangle(0, 0, width, height),
                GraphicsUnit.Pixel);
        }

        private unsafe void DrawWireframe(
            Graphics graphics,
            IReadOnlyList<RenderableFace> renderableFaces,
            Rectangle bounds,
            Color edgeColor)
        {
            var width = bounds.Width;
            var height = bounds.Height;
            if (width <= 0 || height <= 0)
            {
                return;
            }

            EnsureRasterResources(width, height);
            using (var bitmapGraphics = Graphics.FromImage(_rasterBitmap!))
            {
                bitmapGraphics.CompositingMode = CompositingMode.SourceCopy;
                bitmapGraphics.Clear(Color.Transparent);
            }

            var bitmapData = _rasterBitmap!.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            var drawnEdges = new HashSet<Edge2D>();

            try
            {
                var scan0 = (byte*)bitmapData.Scan0;
                var stride = bitmapData.Stride;

                foreach (var face in renderableFaces)
                {
                    for (var i = 0; i < face.ScreenVertices.Length; i++)
                    {
                        var nextIndex = (i + 1) % face.ScreenVertices.Length;
                        var start = face.ScreenVertices[i].Point;
                        var end = face.ScreenVertices[nextIndex].Point;
                        var edge = new Edge2D(start, end);

                        if (!drawnEdges.Add(edge))
                        {
                            continue;
                        }

                        DrawLineMidpoint(
                            scan0,
                            stride,
                            width,
                            height,
                            ToBitmapPoint(start, bounds),
                            ToBitmapPoint(end, bounds),
                            edgeColor);
                    }
                }
            }
            finally
            {
                _rasterBitmap.UnlockBits(bitmapData);
            }

            graphics.DrawImage(
                _rasterBitmap!,
                bounds,
                new Rectangle(0, 0, width, height),
                GraphicsUnit.Pixel);
        }

        private void EnsureRasterResources(int width, int height)
        {
            var pixelCount = width * height;
            if (_zBuffer.Length < pixelCount || _colorBuffer.Length < pixelCount)
            {
                _zBuffer = new float[pixelCount];
                _colorBuffer = new int[pixelCount];
            }

            if (_rasterBitmap is null ||
                _rasterBitmap.Width < width ||
                _rasterBitmap.Height < height)
            {
                _rasterBitmap?.Dispose();
                _rasterBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            }
        }

        private void RasterizeFace(
            RenderableFace face,
            Rectangle bounds,
            int width,
            int height,
            bool writeColor,
            int colorArgb)
        {
            if (face.ScreenVertices.Length < 3)
            {
                return;
            }

            var first = face.ScreenVertices[0];
            for (var i = 1; i < face.ScreenVertices.Length - 1; i++)
            {
                RasterizeTriangle(
                    first,
                    face.ScreenVertices[i],
                    face.ScreenVertices[i + 1],
                    bounds,
                    width,
                    height,
                    writeColor,
                    colorArgb,
                    face.UseReciprocalDepthInterpolation);
            }
        }

        private void RasterizeTriangle(
            ScreenVertex first,
            ScreenVertex second,
            ScreenVertex third,
            Rectangle bounds,
            int width,
            int height,
            bool writeColor,
            int colorArgb,
            bool useReciprocalDepthInterpolation)
        {
            var x0 = first.Point.X - bounds.Left;
            var y0 = first.Point.Y - bounds.Top;
            var x1 = second.Point.X - bounds.Left;
            var y1 = second.Point.Y - bounds.Top;
            var x2 = third.Point.X - bounds.Left;
            var y2 = third.Point.Y - bounds.Top;

            var area = EdgeFunction(x0, y0, x1, y1, x2, y2);
            if (MathF.Abs(area) <= 0.000001f)
            {
                return;
            }

            var minX = Math.Clamp((int)MathF.Floor(MathF.Min(x0, MathF.Min(x1, x2))), 0, width - 1);
            var maxX = Math.Clamp((int)MathF.Ceiling(MathF.Max(x0, MathF.Max(x1, x2))), 0, width - 1);
            var minY = Math.Clamp((int)MathF.Floor(MathF.Min(y0, MathF.Min(y1, y2))), 0, height - 1);
            var maxY = Math.Clamp((int)MathF.Ceiling(MathF.Max(y0, MathF.Max(y1, y2))), 0, height - 1);

            if (minX > maxX || minY > maxY)
            {
                return;
            }

            var inverseArea = 1f / area;
            for (var y = minY; y <= maxY; y++)
            {
                var sampleY = y + 0.5f;
                for (var x = minX; x <= maxX; x++)
                {
                    var sampleX = x + 0.5f;
                    var weight0 = EdgeFunction(x1, y1, x2, y2, sampleX, sampleY) * inverseArea;
                    var weight1 = EdgeFunction(x2, y2, x0, y0, sampleX, sampleY) * inverseArea;
                    var weight2 = EdgeFunction(x0, y0, x1, y1, sampleX, sampleY) * inverseArea;

                    if (weight0 < -DepthEpsilon || weight1 < -DepthEpsilon || weight2 < -DepthEpsilon)
                    {
                        continue;
                    }

                    var depth = InterpolateDepth(
                        weight0,
                        weight1,
                        weight2,
                        first.Depth,
                        second.Depth,
                        third.Depth,
                        useReciprocalDepthInterpolation);

                    TryWritePixel(x, y, width, height, depth, writeColor, colorArgb, DepthEpsilon);
                }
            }
        }

        private unsafe void DrawEdgeOverlay(
            IReadOnlyList<RenderableFace> renderableFaces,
            Rectangle bounds,
            int width,
            int height,
            Color edgeColor)
        {
            var bitmapData = _rasterBitmap!.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            var drawnEdges = new HashSet<Edge2D>();

            try
            {
                var scan0 = (byte*)bitmapData.Scan0;
                var stride = bitmapData.Stride;

                foreach (var face in renderableFaces)
                {
                    for (var i = 0; i < face.ScreenVertices.Length; i++)
                    {
                        var nextIndex = (i + 1) % face.ScreenVertices.Length;
                        var start = face.ScreenVertices[i].Point;
                        var end = face.ScreenVertices[nextIndex].Point;
                        var edge = new Edge2D(start, end);

                        if (!drawnEdges.Add(edge))
                        {
                            continue;
                        }

                        DrawLineMidpoint(
                            scan0,
                            stride,
                            width,
                            height,
                            ToBitmapPoint(start, bounds),
                            ToBitmapPoint(end, bounds),
                            edgeColor);
                    }
                }
            }
            finally
            {
                _rasterBitmap.UnlockBits(bitmapData);
            }
        }

        private void TryWritePixel(
            int x,
            int y,
            int width,
            int height,
            float depth,
            bool writeColor,
            int colorArgb,
            float depthEpsilon)
        {
            if (x < 0 || x >= width || y < 0 || y >= height || float.IsNaN(depth))
            {
                return;
            }

            var index = (y * width) + x;
            if (depth > _zBuffer[index] + depthEpsilon)
            {
                return;
            }

            _zBuffer[index] = depth;
            if (writeColor)
            {
                _colorBuffer[index] = colorArgb;
            }
        }

        private void CopyColorBufferToBitmap(int width, int height)
        {
            var bitmapData = _rasterBitmap!.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            try
            {
                if (bitmapData.Stride == width * 4)
                {
                    Marshal.Copy(_colorBuffer, 0, bitmapData.Scan0, width * height);
                    return;
                }

                for (var y = 0; y < height; y++)
                {
                    var rowPointer = bitmapData.Stride > 0
                        ? IntPtr.Add(bitmapData.Scan0, y * bitmapData.Stride)
                        : IntPtr.Add(bitmapData.Scan0, (height - 1 - y) * -bitmapData.Stride);

                    Marshal.Copy(_colorBuffer, y * width, rowPointer, width);
                }
            }
            finally
            {
                _rasterBitmap.UnlockBits(bitmapData);
            }
        }

        private static float EdgeFunction(
            float ax,
            float ay,
            float bx,
            float by,
            float px,
            float py)
        {
            return ((px - ax) * (by - ay)) - ((py - ay) * (bx - ax));
        }

        private static float InterpolateDepth(
            float weight0,
            float weight1,
            float weight2,
            float depth0,
            float depth1,
            float depth2,
            bool useReciprocalDepthInterpolation)
        {
            if (!useReciprocalDepthInterpolation)
            {
                return (weight0 * depth0) + (weight1 * depth1) + (weight2 * depth2);
            }

            var reciprocalDepth = (weight0 / depth0) + (weight1 / depth1) + (weight2 / depth2);
            return reciprocalDepth <= 0.000001f
                ? float.PositiveInfinity
                : 1f / reciprocalDepth;
        }

        public static unsafe void LineMidpoint(Bitmap bitmap, Point p1, Point p2, Color color)
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            try
            {
                DrawLineMidpoint((byte*)data.Scan0, data.Stride, bitmap.Width, bitmap.Height, p1, p2, color);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
        }

        private static unsafe void DrawLineMidpoint(
            byte* scan0,
            int stride,
            int width,
            int height,
            Point p1,
            Point p2,
            Color color)
        {
            var x1 = p1.X;
            var y1 = p1.Y;
            var x2 = p2.X;
            var y2 = p2.Y;

            var dx = x2 - x1;
            var dy = y2 - y1;

            var passoX = 1;
            var passoY = 1;

            if (dx < 0)
            {
                passoX = -1;
                dx = -dx;
            }

            if (dy < 0)
            {
                passoY = -1;
                dy = -dy;
            }

            var x = x1;
            var y = y1;

            if (dx >= dy)
            {
                var d = (2 * dy) - dx;
                var incE = 2 * dy;
                var incNE = 2 * (dy - dx);

                for (var i = 0; i <= dx; i++)
                {
                    SetPixelSafe(scan0, stride, width, height, x, y, color);

                    if (d < 0)
                    {
                        d += incE;
                    }
                    else
                    {
                        d += incNE;
                        y += passoY;
                    }

                    x += passoX;
                }
            }
            else
            {
                var d = (2 * dx) - dy;
                var incE = 2 * dx;
                var incNE = 2 * (dx - dy);

                for (var i = 0; i <= dy; i++)
                {
                    SetPixelSafe(scan0, stride, width, height, x, y, color);

                    if (d < 0)
                    {
                        d += incE;
                    }
                    else
                    {
                        d += incNE;
                        x += passoX;
                    }

                    y += passoY;
                }
            }
        }

        private static unsafe void SetPixelSafe(
            byte* scan0,
            int stride,
            int width,
            int height,
            int x,
            int y,
            Color color)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }

            var row = stride >= 0
                ? scan0 + (y * stride)
                : scan0 + ((height - 1 - y) * -stride);
            var pixel = row + (x * 4);

            pixel[0] = color.B;
            pixel[1] = color.G;
            pixel[2] = color.R;
            pixel[3] = color.A;
        }

        private static Point ToBitmapPoint(PointF point, Rectangle bounds)
        {
            return new Point(
                (int)MathF.Round(point.X - bounds.Left),
                (int)MathF.Round(point.Y - bounds.Top));
        }

        private static void DrawProjectionModeBadge(
            Graphics graphics,
            Rectangle renderBounds,
            ProjectionView projection,
            ProjectionParameters parameters)
        {
            var badgeBounds = new Rectangle(renderBounds.Left + 18, renderBounds.Top + 18, 278, 32);
            using var backgroundBrush = new SolidBrush(Color.FromArgb(218, 255, 255, 255));
            using var borderPen = new Pen(Color.FromArgb(175, 197, 214), 1f);
            using var textBrush = new SolidBrush(Color.FromArgb(42, 56, 74));
            using var font = new Font("Segoe UI", 9f, FontStyle.Bold);

            graphics.FillRectangle(backgroundBrush, badgeBounds);
            graphics.DrawRectangle(borderPen, badgeBounds);
            graphics.DrawString(
                BuildProjectionBadgeText(projection, parameters),
                font,
                textBrush,
                badgeBounds.Left + 10,
                badgeBounds.Top + 7);
        }

        private void DrawProjectionThumbnails(
            Graphics graphics,
            Mesh mesh,
            Vector3D[] transformedVertices,
            Rectangle renderBounds,
            ViewportRenderOptions options)
        {
            var previews = new[]
            {
                new ProjectionThumbnailDefinition(ProjectionView.Cavalier, "Cavaleira", CavalierProjectionColor),
                new ProjectionThumbnailDefinition(ProjectionView.Cabinet, "Gabinete", CabinetProjectionColor),
                new ProjectionThumbnailDefinition(ProjectionView.OnePointPerspective, "1 PF", OnePointPerspectiveColor)
            };

            var thumbnailWidth = Math.Clamp(renderBounds.Width / 6, 92, 124);
            var thumbnailHeight = Math.Clamp(renderBounds.Height / 5, 76, 108);
            var totalWidth = (thumbnailWidth * previews.Length) + (ThumbnailGap * (previews.Length - 1));
            var startX = renderBounds.Right - 18 - totalWidth;
            var top = renderBounds.Bottom - 18 - thumbnailHeight;

            for (var i = 0; i < previews.Length; i++)
            {
                var thumbnailBounds = new Rectangle(
                    startX + i * (thumbnailWidth + ThumbnailGap),
                    top,
                    thumbnailWidth,
                    thumbnailHeight);

                DrawProjectionThumbnail(
                    graphics,
                    mesh,
                    transformedVertices,
                    thumbnailBounds,
                    previews[i],
                    options);
            }
        }

        private void DrawProjectionThumbnail(
            Graphics graphics,
            Mesh mesh,
            Vector3D[] transformedVertices,
            Rectangle bounds,
            ProjectionThumbnailDefinition preview,
            ViewportRenderOptions options)
        {
            var isSelected = options.Projection == preview.View;
            var borderColor = isSelected
                ? preview.AccentColor
                : Color.FromArgb(170, preview.AccentColor);
            var backgroundColor = isSelected
                ? Color.FromArgb(242, 255, 255, 255)
                : Color.FromArgb(220, 255, 255, 255);

            using var backgroundBrush = new SolidBrush(backgroundColor);
            using var borderPen = new Pen(borderColor, isSelected ? 2f : 1.2f);
            using var labelBrush = new SolidBrush(preview.AccentColor);
            using var labelFont = new Font("Segoe UI", 8.5f, FontStyle.Bold);

            graphics.FillRectangle(backgroundBrush, bounds);
            graphics.DrawRectangle(borderPen, Rectangle.Inflate(bounds, -1, -1));
            graphics.DrawString(preview.Label, labelFont, labelBrush, bounds.Left + 8, bounds.Top + 6);

            var previewCanvas = Rectangle.FromLTRB(bounds.Left + 8, bounds.Top + 26, bounds.Right - 8, bounds.Bottom - 8);
            if (previewCanvas.Width <= 0 || previewCanvas.Height <= 0)
            {
                return;
            }

            var previewVertices = ApplyProjectionViewTransform(transformedVertices, preview.View, options.Parameters);
            var renderableFaces = CreateRenderableFaces(
                mesh,
                previewVertices,
                previewCanvas,
                preview.View,
                options.ShowBackFaces,
                options.Parameters);

            var overlayFillColor = Color.FromArgb(52, preview.AccentColor);
            DrawProjectedMesh(
                graphics,
                renderableFaces,
                previewCanvas,
                options.FillFaces,
                preview.AccentColor,
                overlayFillColor);

            if (preview.View == ProjectionView.OnePointPerspective)
            {
                DrawOnePointPerspectiveGuides(graphics, previewCanvas, preview.AccentColor);
            }
        }

        private static string GetProjectionLabel(ProjectionView projection)
        {
            return projection switch
            {
                ProjectionView.Normal => "Normal (XYZ)",
                ProjectionView.Cavalier => "Cavaleira",
                ProjectionView.Cabinet => "Gabinete",
                ProjectionView.OnePointPerspective => "Perspectiva 1 PF",
                _ => "Normal (XYZ)"
            };
        }

        private static string BuildProjectionBadgeText(ProjectionView projection, ProjectionParameters parameters)
        {
            return projection switch
            {
                ProjectionView.Cavalier =>
                    $"Cavaleira | L=1.0 | a={parameters.ObliqueAlphaDegrees:F0} deg",
                ProjectionView.Cabinet =>
                    $"Gabinete | L=0.5 | a={parameters.ObliqueAlphaDegrees:F0} deg",
                ProjectionView.OnePointPerspective =>
                    $"Perspectiva 1 PF | d={FixedOnePointFocalDistance:F0} | z={parameters.PerspectiveZOffset:F0}",
                _ => "Projecao: Normal (XYZ)"
            };
        }

        private static float MapPerspectiveZOffsetToDepth(float zOffset)
        {
            // O slider vai de 100 a 400. Mantemos uma profundidade-base fixa
            // para que a 1 PF nunca encoste demais no plano de projecao, mas
            // ainda preserve o efeito visual do z offset.
            return PerspectiveBaseDepthOffset + (zOffset / 100f);
        }

        private static Vector3D GetObliqueProjectionDirection(ProjectionView projection, ProjectionParameters parameters)
        {
            var depthFactor = projection == ProjectionView.Cabinet
                ? CabinetDepthFactor
                : CavalierDepthFactor;
            var alpha = DegreesToRadians(parameters.ObliqueAlphaDegrees);

            // A formula x' = x + L*z*cos(a), y' = y + L*z*sin(a)
            // implica raios projetores chegando ao objeto na direcao
            // oposta ao vetor usado para "derrubar" o ponto no plano XY.
            // Por isso, para visibilidade e ordenacao usamos o vetor do
            // observador para o objeto: (-L*cos(a), -L*sin(a), 1).
            return new Vector3D(
                -depthFactor * MathF.Cos(alpha),
                -depthFactor * MathF.Sin(alpha),
                1f);
        }

        private static void DrawOnePointPerspectiveGuides(Graphics graphics, Rectangle bounds, Color accentColor)
        {
            using var horizonPen = new Pen(Color.FromArgb(108, accentColor), 1f);
            using var vanishingPointBrush = new SolidBrush(accentColor);

            var centerX = bounds.Left + (bounds.Width / 2f);
            var centerY = bounds.Top + (bounds.Height / 2f);
            graphics.DrawLine(horizonPen, bounds.Left, centerY, bounds.Right, centerY);
            graphics.FillEllipse(vanishingPointBrush, centerX - 4f, centerY - 4f, 8f, 8f);
        }

        private static float DegreesToRadians(float degrees)
        {
            return degrees * (MathF.PI / 180f);
        }

        private static Vector3D RotateX(Vector3D point, float radians)
        {
            var cosine = MathF.Cos(radians);
            var sine = MathF.Sin(radians);

            return new Vector3D(
                point.X,
                (point.Y * cosine) - (point.Z * sine),
                (point.Y * sine) + (point.Z * cosine));
        }

        private static Vector3D RotateY(Vector3D point, float radians)
        {
            var cosine = MathF.Cos(radians);
            var sine = MathF.Sin(radians);

            return new Vector3D(
                (point.X * cosine) + (point.Z * sine),
                point.Y,
                (-point.X * sine) + (point.Z * cosine));
        }

        private static Vector3D ComputeFaceNormal(Vector3D first, Vector3D second, Vector3D third)
        {
            var edgeA = Subtract(second, first);
            var edgeB = Subtract(third, first);
            return Cross(edgeA, edgeB);
        }

        private static Vector3D Subtract(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        private static Vector3D Cross(Vector3D left, Vector3D right)
        {
            return new Vector3D(
                (left.Y * right.Z) - (left.Z * right.Y),
                (left.Z * right.X) - (left.X * right.Z),
                (left.X * right.Y) - (left.Y * right.X));
        }

        private static float Dot(Vector3D left, Vector3D right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        private static float LengthSquared(Vector3D vector)
        {
            return Dot(vector, vector);
        }

        private static bool IsValidIndex(int index, int count)
        {
            return index >= 0 && index < count;
        }

        private readonly record struct RenderableFace(
            ScreenVertex[] ScreenVertices,
            Color FillColor,
            bool IsFrontFace,
            bool UseReciprocalDepthInterpolation);

        private readonly record struct ScreenVertex(PointF Point, float Depth);

        private readonly record struct ProjectionThumbnailDefinition(
            ProjectionView View,
            string Label,
            Color AccentColor);

        private readonly record struct Edge2D(PointF First, PointF Second)
        {
            public bool Equals(Edge2D other)
            {
                return (PointsEqual(First, other.First) && PointsEqual(Second, other.Second)) ||
                       (PointsEqual(First, other.Second) && PointsEqual(Second, other.First));
            }

            public override int GetHashCode()
            {
                var firstHash = HashPoint(First);
                var secondHash = HashPoint(Second);
                return firstHash <= secondHash
                    ? HashCode.Combine(firstHash, secondHash)
                    : HashCode.Combine(secondHash, firstHash);
            }

            private static bool PointsEqual(PointF first, PointF second)
            {
                return first.X.Equals(second.X) && first.Y.Equals(second.Y);
            }

            private static int HashPoint(PointF point)
            {
                return HashCode.Combine(point.X, point.Y);
            }
        }

        private struct Vector3D
        {
            public float X;
            public float Y;
            public float Z;

            public Vector3D(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }
    }
}
