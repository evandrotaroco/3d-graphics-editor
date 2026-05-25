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
        private const float CavalierDepthFactor = 1f;
        private const float CabinetDepthFactor = 0.5f;
        private const float PerspectiveBaseDepthOffset = 2f;
        private const float DepthEpsilon = 0.0001f;
        private const float EdgeDepthEpsilon = 0.01f;
        private const int TransparentPixel = 0;
        private static readonly Color ViewportBackgroundColor = Color.White;
        private static readonly Color BorderColor = Color.FromArgb(120, 138, 160);
        private static readonly Color EdgeColor = Color.FromArgb(34, 47, 66);
        private static readonly Color PlaceholderColor = Color.FromArgb(92, 108, 126);
        private static readonly Color DefaultFaceColor = Color.FromArgb(188, 205, 226);
        private static readonly Color FrontalProjectionColor = Color.FromArgb(55, 113, 196);
        private static readonly Color SuperiorProjectionColor = Color.FromArgb(133, 90, 181);
        private static readonly Color LateralProjectionColor = Color.FromArgb(189, 116, 34);
        private static readonly Color CavalierProjectionColor = Color.FromArgb(181, 58, 66);
        private static readonly Color CabinetProjectionColor = Color.FromArgb(41, 141, 77);
        private static readonly Color OnePointPerspectiveColor = Color.FromArgb(46, 101, 185);

        private float[] _zBuffer = Array.Empty<float>();
        private int[] _colorBuffer = Array.Empty<int>();
        private Bitmap? _rasterBitmap;
        // Desenha o viewport inteiro: fundo, modelo, projecao e extras da tela.
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
            var transformedNormals = TransformNormals(mesh, transform);
            var generatedVertexNormals = BuildGeneratedVertexNormals(mesh, transformedVertices);
            var mainProjection = options.Mode == ViewportMode.Projection
                ? options.Projection
                : ProjectionView.Normal;
            var projectionVertices = ApplyProjectionViewTransform(transformedVertices, mainProjection, options.Parameters);
            var projectionNormals = ApplyProjectionViewNormalTransform(transformedNormals, mainProjection, options.Parameters);
            var projectionGeneratedVertexNormals = ApplyProjectionViewNormalTransform(generatedVertexNormals, mainProjection, options.Parameters);

            var renderableFaces = CreateRenderableFaces(
                mesh,
                projectionVertices,
                renderBounds,
                mainProjection,
                options.ShowBackFaces,
                options.Parameters,
                projectionNormals,
                projectionGeneratedVertexNormals);

            DrawProjectedMesh(
                graphics,
                renderableFaces,
                renderBounds,
                options.FillFaces,
                options.ShowEdges,
                options.ShowBackFaces,
                EdgeColor,
                null,
                options.ShadingMode,
                options.Lighting);

            if (options.ShowLightMarker)
            {
                DrawLightMarker(
                    graphics,
                    projectionVertices,
                    renderBounds,
                    mainProjection,
                    options.Parameters,
                    options.Lighting);
            }

            if (mainProjection == ProjectionView.OnePointPerspective)
            {
                DrawOnePointPerspectiveGuides(graphics, renderBounds, OnePointPerspectiveColor);
            }

            if (options.Mode == ViewportMode.Projection)
            {
                DrawProjectionModeBadge(graphics, renderBounds, mainProjection, options.Parameters);
            }
        }

        // Gera bitmaps pequenos para os PictureBox nativos das miniaturas.
        public IReadOnlyDictionary<ProjectionView, Bitmap> RenderProjectionThumbnails(
            Mesh mesh,
            TransformState transform,
            ViewportRenderOptions options,
            Size thumbnailSize)
        {
            var images = new Dictionary<ProjectionView, Bitmap>();
            if (thumbnailSize.Width <= 0 || thumbnailSize.Height <= 0)
            {
                return images;
            }

            var transformedVertices = TransformVertices(mesh, transform);
            foreach (var preview in CreateProjectionThumbnailDefinitions())
            {
                var bitmap = new Bitmap(thumbnailSize.Width, thumbnailSize.Height, PixelFormat.Format32bppArgb);
                using var graphics = Graphics.FromImage(bitmap);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.Transparent);

                DrawProjectionThumbnail(
                    graphics,
                    mesh,
                    transformedVertices,
                    new Rectangle(Point.Empty, thumbnailSize),
                    preview,
                    options);

                images[preview.View] = bitmap;
            }

            return images;
        }

        // Desenha o fundo uniforme e a borda do viewport.
        private static void DrawBackdrop(Graphics graphics, Rectangle bounds)
        {
            using var backgroundBrush = new SolidBrush(ViewportBackgroundColor);
            using var borderPen = new Pen(BorderColor, 1.5f);

            graphics.FillRectangle(backgroundBrush, bounds);
            graphics.DrawRectangle(borderPen, bounds);
        }

        // Mostra uma mensagem quando nao existe modelo carregado.
        private static void DrawEmptyState(Graphics graphics, Rectangle bounds)
        {
            const string title = "Nenhum modelo carregado";
            const string subtitle = "Use o botão Abrir .obj para visualizar um objeto 3D aqui.";

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

        // Aplica centralizacao, normalizacao, escala, rotacao e translacao aos vertices.
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

            // Mantemos toda a cadeia de transformação 4x4 em um único produto
            // para que translação, escala e rotações sejam aplicadas juntas.
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

        // Rotaciona as normais para elas acompanharem a orientacao do modelo.
        private static Vector3D[] TransformNormals(Mesh mesh, TransformState transform)
        {
            if (mesh.Normals.Count == 0)
            {
                return [];
            }

            var normalMatrix = Transform3D.Compose(
                Transform3D.CreateRotationZ(transform.RotationZ),
                Transform3D.CreateRotationY(transform.RotationY),
                Transform3D.CreateRotationX(transform.RotationX));

            var transformed = new Vector3D[mesh.Normals.Count];
            for (var i = 0; i < mesh.Normals.Count; i++)
            {
                var normal = mesh.Normals[i];
                var vector = normalMatrix.Transform(new Vector4(normal.X, normal.Y, normal.Z, 0f));
                transformed[i] = Normalize(new Vector3D(vector.X, vector.Y, vector.Z));
            }

            return transformed;
        }

        // Gera normais por vertice quando o arquivo nao fornece normais prontas.
        private static Vector3D[] BuildGeneratedVertexNormals(Mesh mesh, Vector3D[] transformedVertices)
        {
            var normals = new Vector3D[transformedVertices.Length];

            foreach (var face in mesh.Faces)
            {
                if (face.Vertices.Count < 3)
                {
                    continue;
                }

                var firstIndex = face.Vertices[0].VertexIndex;
                var secondIndex = face.Vertices[1].VertexIndex;
                var thirdIndex = face.Vertices[2].VertexIndex;
                if (!IsValidIndex(firstIndex, transformedVertices.Length) ||
                    !IsValidIndex(secondIndex, transformedVertices.Length) ||
                    !IsValidIndex(thirdIndex, transformedVertices.Length))
                {
                    continue;
                }

                var faceNormal = Normalize(ComputeFaceNormal(
                    transformedVertices[firstIndex],
                    transformedVertices[secondIndex],
                    transformedVertices[thirdIndex]));

                if (LengthSquared(faceNormal) <= 0.0000001f)
                {
                    continue;
                }

                foreach (var faceVertex in face.Vertices)
                {
                    if (!IsValidIndex(faceVertex.VertexIndex, normals.Length))
                    {
                        continue;
                    }

                    normals[faceVertex.VertexIndex] = Add(normals[faceVertex.VertexIndex], faceNormal);
                }
            }

            for (var i = 0; i < normals.Length; i++)
            {
                normals[i] = Normalize(normals[i]);
            }

            return normals;
        }

        // Faz ajustes extras nos vertices antes de aplicar a projecao escolhida.
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

        // Faz os mesmos ajustes de projecao nas normais usadas pela iluminacao.
        private static Vector3D[] ApplyProjectionViewNormalTransform(
            Vector3D[] normals,
            ProjectionView projection,
            ProjectionParameters parameters)
        {
            if (normals.Length == 0)
            {
                return normals;
            }

            var adjustedNormals = new Vector3D[normals.Length];
            for (var i = 0; i < normals.Length; i++)
            {
                var normal = normals[i];

                if (projection == ProjectionView.Cavalier || projection == ProjectionView.Cabinet)
                {
                    normal = RotateY(normal, DegreesToRadians(parameters.ObliqueRotationYDegrees));
                }
                else if (projection == ProjectionView.OnePointPerspective)
                {
                    normal = RotateX(normal, DegreesToRadians(parameters.PerspectiveRotationXDegrees));
                    normal = RotateY(normal, DegreesToRadians(parameters.PerspectiveRotationYDegrees));
                }

                adjustedNormals[i] = Normalize(normal);
            }

            return adjustedNormals;
        }

        // Cria a lista de faces ja prontas para serem desenhadas na tela.
        private static List<RenderableFace> CreateRenderableFaces(
            Mesh mesh,
            Vector3D[] transformedVertices,
            Rectangle bounds,
            ProjectionView projection,
            bool showBackFaces,
            ProjectionParameters parameters,
            Vector3D[]? transformedNormals = null,
            Vector3D[]? generatedVertexNormals = null)
        {
            var projectedVertices = ProjectVertices(transformedVertices, bounds, projection, parameters);
            return BuildRenderableFaces(
                mesh,
                transformedVertices,
                projectedVertices,
                projection,
                showBackFaces,
                parameters,
                transformedNormals ?? [],
                generatedVertexNormals ?? BuildGeneratedVertexNormals(mesh, transformedVertices));
        }

        // Projeta todos os vertices 3D para pontos 2D do viewport.
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

        // Monta as faces renderizaveis e remove as que nao devem aparecer.
        private static List<RenderableFace> BuildRenderableFaces(
            Mesh mesh,
            Vector3D[] transformedVertices,
            PointF?[] projectedVertices,
            ProjectionView projection,
            bool showBackFaces,
            ProjectionParameters parameters,
            Vector3D[] transformedNormals,
            Vector3D[] generatedVertexNormals)
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
                        transformedNormals,
                        generatedVertexNormals,
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

        // Tenta converter uma face do modelo em uma face com pontos de tela.
        private static bool TryCreateRenderableFace(
            Mesh mesh,
            Face face,
            Vector3D[] transformedVertices,
            PointF?[] projectedVertices,
            ProjectionView projection,
            ProjectionParameters parameters,
            Vector3D[] transformedNormals,
            Vector3D[] generatedVertexNormals,
            out RenderableFace renderableFace)
        {
            var screenVertices = new ScreenVertex[face.Vertices.Count];
            var faceVertices = new Vector3D[face.Vertices.Count];

            for (var i = 0; i < face.Vertices.Count; i++)
            {
                var faceVertex = face.Vertices[i];
                var vertexIndex = faceVertex.VertexIndex;
                if (!IsValidIndex(vertexIndex, transformedVertices.Length) ||
                    projectedVertices[vertexIndex] is null)
                {
                    renderableFace = default;
                    return false;
                }

                faceVertices[i] = transformedVertices[vertexIndex];

                var depth = GetDepthValue(transformedVertices[vertexIndex], projection, parameters);
                screenVertices[i] = new ScreenVertex(
                    projectedVertices[vertexIndex]!.Value,
                    depth,
                    transformedVertices[vertexIndex],
                    GetFaceVertexNormal(faceVertex, transformedNormals, generatedVertexNormals));
            }

            var geometricNormal = Normalize(ComputeFaceNormal(faceVertices[0], faceVertices[1], faceVertices[2]));
            if (LengthSquared(geometricNormal) <= 0.0000001f)
            {
                renderableFace = default;
                return false;
            }

            var isFrontFace = IsFrontFace(faceVertices[0], geometricNormal, projection, parameters);
            var fillColor = GetFaceColor(mesh, face);
            var faceCenter = ComputeCenter(faceVertices);
            var useReciprocalDepthInterpolation = projection is ProjectionView.Normal or ProjectionView.OnePointPerspective;

            renderableFace = new RenderableFace(
                screenVertices,
                fillColor,
                geometricNormal,
                faceCenter,
                isFrontFace,
                useReciprocalDepthInterpolation);
            return true;
        }

        // Escolhe a normal correta de um vertice da face.
        private static Vector3D GetFaceVertexNormal(
            FaceVertex faceVertex,
            Vector3D[] transformedNormals,
            Vector3D[] generatedVertexNormals)
        {
            if (faceVertex.NormalIndex is { } normalIndex && IsValidIndex(normalIndex, transformedNormals.Length))
            {
                return Normalize(transformedNormals[normalIndex]);
            }

            if (IsValidIndex(faceVertex.VertexIndex, generatedVertexNormals.Length))
            {
                return Normalize(generatedVertexNormals[faceVertex.VertexIndex]);
            }

            return new Vector3D(0f, 0f, -1f);
        }

        // Calcula o ponto medio de uma lista de vertices.
        private static Vector3D ComputeCenter(IReadOnlyList<Vector3D> vertices)
        {
            var center = new Vector3D();
            foreach (var vertex in vertices)
            {
                center = Add(center, vertex);
            }

            return Divide(center, vertices.Count);
        }

        // Descobre a cor da face a partir do material do modelo.
        private static Color GetFaceColor(Mesh mesh, Face face)
        {
            var baseColor = DefaultFaceColor;

            if (face.MaterialName is not null && mesh.Materials.TryGetValue(face.MaterialName, out var material))
            {
                baseColor = material.DiffuseColor;
            }

            return baseColor;
        }

        // Calcula os limites minimo e maximo ocupados pelo modelo.
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

        // Escolhe qual formula de projecao deve ser usada para um ponto.
        private static bool TryProject(
            Vector3D point,
            Rectangle bounds,
            ProjectionView projection,
            ProjectionParameters parameters,
            out PointF projectedPoint)
        {
            return projection switch
            {
                ProjectionView.Frontal => TryProjectOrthographic(point, bounds, projection, out projectedPoint),
                ProjectionView.Superior => TryProjectOrthographic(point, bounds, projection, out projectedPoint),
                ProjectionView.Lateral => TryProjectOrthographic(point, bounds, projection, out projectedPoint),
                ProjectionView.Cavalier => TryProjectOblique(point, bounds, CavalierDepthFactor, parameters.ObliqueAlphaDegrees, out projectedPoint),
                ProjectionView.Cabinet => TryProjectOblique(point, bounds, CabinetDepthFactor, parameters.ObliqueAlphaDegrees, out projectedPoint),
                ProjectionView.OnePointPerspective => TryProjectOnePointPerspective(point, bounds, FixedOnePointFocalDistance, out projectedPoint),
                _ => TryProjectStandardPerspective(point, bounds, out projectedPoint)
            };
        }

        // Faz projecao ortografica: frontal, superior ou lateral.
        private static bool TryProjectOrthographic(
            Vector3D point,
            Rectangle bounds,
            ProjectionView projection,
            out PointF projectedPoint)
        {
            var (horizontal, vertical) = projection switch
            {
                ProjectionView.Superior => (point.X, point.Z),
                ProjectionView.Lateral => (point.Y, point.Z),
                _ => (point.X, point.Y)
            };

            var projectionScale = Math.Min(bounds.Width, bounds.Height) * ParallelProjectionScaleFactor;
            projectedPoint = ProjectToViewportAxes(bounds, horizontal, vertical, projectionScale);

            return true;
        }

        // Faz a perspectiva padrao usada na visualizacao normal.
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
            projectedPoint = ProjectToViewportAxes(bounds, point.X, point.Y, perspective);

            return true;
        }

        // Faz a perspectiva com um ponto de fuga.
        private static bool TryProjectOnePointPerspective(
            Vector3D point,
            Rectangle bounds,
            float focalDistance,
            out PointF projectedPoint)
        {
            // Fórmula do material:
            // x' = x * d / z
            // y' = y * d / z
            // com COP na origem e plano de projeção em z = d.
            var depth = point.Z;
            if (depth <= 0.01f)
            {
                projectedPoint = PointF.Empty;
                return false;
            }

            var projectedX = point.X * focalDistance / depth;
            var projectedY = point.Y * focalDistance / depth;
            var screenScale = Math.Min(bounds.Width, bounds.Height) / OnePointPerspectiveScreenDivisor;

            projectedPoint = ProjectToViewportAxes(bounds, projectedX, projectedY, screenScale);

            return true;
        }

        // Faz projecao obliqua, usada na cavaleira e na gabinete.
        private static bool TryProjectOblique(
            Vector3D point,
            Rectangle bounds,
            float depthFactor,
            float alphaDegrees,
            out PointF projectedPoint)
        {
            // Fórmula da projeção oblíqua do material:
            // xp = x + z * L * cos(a) | yp = y + z * L * sin(a)
            var alpha = DegreesToRadians(alphaDegrees);
            var horizontal = point.X + (point.Z * depthFactor * MathF.Cos(alpha));
            var vertical = point.Y + (point.Z * depthFactor * MathF.Sin(alpha));
            var projectionScale = Math.Min(bounds.Width, bounds.Height) * ParallelProjectionScaleFactor;
            projectedPoint = ProjectToViewportAxes(bounds, horizontal, vertical, projectionScale);

            return true;
        }

        // Converte coordenadas matematicas para a tela: X cresce para direita e Y para cima.
        private static PointF ProjectToViewportAxes(Rectangle bounds, float horizontal, float vertical, float scale)
        {
            var centerX = bounds.Left + bounds.Width / 2f;
            var centerY = bounds.Top + bounds.Height / 2f;

            return new PointF(
                centerX + horizontal * scale,
                centerY - vertical * scale);
        }

        // Verifica se a face esta virada para a camera.
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

            if (IsOrthographicProjection(projection))
            {
                return Dot(GetOrthographicProjectionDirection(projection), geometricNormal) < 0f;
            }

            var viewVector = GetObliqueProjectionDirection(projection, parameters);

            return Dot(viewVector, geometricNormal) < 0f;
        }

        // Calcula a profundidade de um ponto para ordenacao e Z-buffer.
        private static float GetDepthValue(Vector3D point, ProjectionView projection, ProjectionParameters parameters)
        {
            if (IsOrthographicProjection(projection))
            {
                return Dot(point, GetOrthographicProjectionDirection(projection));
            }

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

        // Desenha uma bolinha amarela indicando a posicao da luz.
        private static void DrawLightMarker(
            Graphics graphics,
            Vector3D[] projectedVertices,
            Rectangle bounds,
            ProjectionView projection,
            ProjectionParameters parameters,
            LightingOptions lighting)
        {
            if (projectedVertices.Length == 0)
            {
                return;
            }

            var modelCenter = ComputeCenter(projectedVertices);
            var lightPoint = new Vector3D(lighting.LightX, lighting.LightY, lighting.LightZ);
            if (projection == ProjectionView.Cavalier || projection == ProjectionView.Cabinet)
            {
                lightPoint = RotateY(lightPoint, DegreesToRadians(parameters.ObliqueRotationYDegrees));
            }
            else if (projection == ProjectionView.OnePointPerspective)
            {
                lightPoint = RotateX(lightPoint, DegreesToRadians(parameters.PerspectiveRotationXDegrees));
                lightPoint = RotateY(lightPoint, DegreesToRadians(parameters.PerspectiveRotationYDegrees));
                lightPoint.Z += MapPerspectiveZOffsetToDepth(parameters.PerspectiveZOffset);
            }

            if (!TryProject(lightPoint, bounds, projection, parameters, out var lightScreenPoint) ||
                !TryProject(modelCenter, bounds, projection, parameters, out var centerScreenPoint))
            {
                return;
            }

            const float radius = 7f;
            using var rayPen = new Pen(Color.FromArgb(170, 192, 138, 0), 1.4f)
            {
                DashStyle = DashStyle.Dash
            };
            using var fillBrush = new SolidBrush(Color.FromArgb(255, 232, 48));
            using var borderPen = new Pen(Color.FromArgb(116, 89, 0), 1.2f);

            graphics.DrawLine(rayPen, centerScreenPoint, lightScreenPoint);
            graphics.FillEllipse(
                fillBrush,
                lightScreenPoint.X - radius,
                lightScreenPoint.Y - radius,
                radius * 2f,
                radius * 2f);
            graphics.DrawEllipse(
                borderPen,
                lightScreenPoint.X - radius,
                lightScreenPoint.Y - radius,
                radius * 2f,
                radius * 2f);
        }

        // Decide se o modelo sera desenhado preenchido, com arestas ou apenas em wireframe.
        private void DrawProjectedMesh(
            Graphics graphics,
            IReadOnlyList<RenderableFace> renderableFaces,
            Rectangle bounds,
            bool fillFaces,
            bool showEdges,
            bool showBackFaces,
            Color edgeColor,
            Color? uniformFillColor,
            ShadingMode shadingMode,
            LightingOptions lighting)
        {
            if (renderableFaces.Count == 0 || bounds.Width <= 0 || bounds.Height <= 0)
            {
                return;
            }

            if (fillFaces || !showBackFaces)
            {
                DrawZBufferedMesh(
                    graphics,
                    renderableFaces,
                    bounds,
                    fillFaces,
                    !showBackFaces,
                    showEdges,
                    edgeColor,
                    uniformFillColor,
                    shadingMode,
                    lighting);
                return;
            }

            if (showEdges)
            {
                DrawWireframe(graphics, renderableFaces, bounds, edgeColor);
            }
        }

        // Desenha faces usando Z-buffer para esconder o que fica atras.
        private void DrawZBufferedMesh(
            Graphics graphics,
            IReadOnlyList<RenderableFace> renderableFaces,
            Rectangle bounds,
            bool fillFaces,
            bool depthTestEdges,
            bool showEdges,
            Color edgeColor,
            Color? uniformFillColor,
            ShadingMode shadingMode,
            LightingOptions lighting)
        {
            var width = bounds.Width;
            var height = bounds.Height;
            var pixelCount = width * height;

            EnsureRasterResources(width, height);
            Array.Fill(_zBuffer, float.PositiveInfinity, 0, pixelCount);
            Array.Fill(_colorBuffer, TransparentPixel, 0, pixelCount);

            foreach (var face in renderableFaces)
            {
                var faceColor = uniformFillColor ?? face.FillColor;
                ScanlineFill(face, bounds, width, height, fillFaces, faceColor, uniformFillColor.HasValue, shadingMode, lighting);
            }

            CopyColorBufferToBitmap(width, height);
            if (showEdges)
            {
                DrawEdgeOverlay(renderableFaces, bounds, width, height, depthTestEdges, edgeColor);
            }

            graphics.DrawImage(
                _rasterBitmap!,
                bounds,
                new Rectangle(0, 0, width, height),
                GraphicsUnit.Pixel);
        }

        // Desenha somente as arestas das faces.
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

        // Garante que os buffers e o bitmap tenham tamanho suficiente.
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

        // Preenche uma face linha por linha, interpolando profundidade, cor e normais.
        private void ScanlineFill(
            RenderableFace face,
            Rectangle bounds,
            int width,
            int height,
            bool writeColor,
            Color materialColor,
            bool useUniformColor,
            ShadingMode shadingMode,
            LightingOptions lighting)
        {
            if (face.ScreenVertices.Length < 3)
            {
                return;
            }

            var points = new Point[face.ScreenVertices.Length];
            var depthSamples = new double[face.ScreenVertices.Length];
            var vertexColors = new ColorVector[face.ScreenVertices.Length];
            var effectiveShadingMode = useUniformColor ? ShadingMode.Flat : shadingMode;
            var flatColorArgb = 0;
            var hasFlatColorArgb = false;
            var needsVertexColors = writeColor && effectiveShadingMode == ShadingMode.Gouraud;

            for (var i = 0; i < face.ScreenVertices.Length; i++)
            {
                points[i] = ToBitmapPoint(face.ScreenVertices[i].Point, bounds);
                depthSamples[i] = ToDepthSample(face.ScreenVertices[i].Depth, face.UseReciprocalDepthInterpolation);

                if (needsVertexColors)
                {
                    vertexColors[i] = CalculateIlluminatedColor(
                        face.ScreenVertices[i].Normal,
                        face.ScreenVertices[i].Position,
                        materialColor,
                        lighting);
                }
            }

            var yMinPolygon = points.Min(point => point.Y);
            var yMaxPolygon = points.Max(point => point.Y);
            var firstVisibleY = Math.Max(yMinPolygon, 0);
            var lastVisibleY = Math.Min(yMaxPolygon, height - 1);

            if (firstVisibleY > lastVisibleY)
            {
                return;
            }

            var edgeTable = new Dictionary<int, List<EdgeEntry>>();

            for (var i = 0; i < points.Length; i++)
            {
                var nextIndex = (i + 1) % points.Length;
                var p1 = points[i];
                var p2 = points[nextIndex];
                var depth1 = depthSamples[i];
                var depth2 = depthSamples[nextIndex];
                var color1 = vertexColors[i];
                var color2 = vertexColors[nextIndex];
                var normal1 = face.ScreenVertices[i].Normal;
                var normal2 = face.ScreenVertices[nextIndex].Normal;
                var position1 = face.ScreenVertices[i].Position;
                var position2 = face.ScreenVertices[nextIndex].Position;

                if (p1.Y == p2.Y)
                {
                    continue;
                }

                if (p1.Y > p2.Y)
                {
                    (p1, p2) = (p2, p1);
                    (depth1, depth2) = (depth2, depth1);
                    (color1, color2) = (color2, color1);
                    (normal1, normal2) = (normal2, normal1);
                    (position1, position2) = (position2, position1);
                }

                if (p2.Y <= 0 || p1.Y >= height)
                {
                    continue;
                }

                var deltaY = p2.Y - p1.Y;
                var incrementX = (double)(p2.X - p1.X) / deltaY;
                var incrementDepth = (depth2 - depth1) / deltaY;
                var startY = Math.Max(p1.Y, 0);
                var yOffset = startY - p1.Y;
                var incrementRed = (color2.R - color1.R) / deltaY;
                var incrementGreen = (color2.G - color1.G) / deltaY;
                var incrementBlue = (color2.B - color1.B) / deltaY;
                var incrementNormalX = (normal2.X - normal1.X) / deltaY;
                var incrementNormalY = (normal2.Y - normal1.Y) / deltaY;
                var incrementNormalZ = (normal2.Z - normal1.Z) / deltaY;
                var incrementPositionX = (position2.X - position1.X) / deltaY;
                var incrementPositionY = (position2.Y - position1.Y) / deltaY;
                var incrementPositionZ = (position2.Z - position1.Z) / deltaY;

                var entry = new EdgeEntry
                {
                    YMax = p2.Y,
                    X = p1.X + (incrementX * yOffset),
                    IncrementX = incrementX,
                    Depth = depth1 + (incrementDepth * yOffset),
                    IncrementDepth = incrementDepth,
                    Red = color1.R + (incrementRed * yOffset),
                    Green = color1.G + (incrementGreen * yOffset),
                    Blue = color1.B + (incrementBlue * yOffset),
                    IncrementRed = incrementRed,
                    IncrementGreen = incrementGreen,
                    IncrementBlue = incrementBlue,
                    NormalX = normal1.X + (incrementNormalX * yOffset),
                    NormalY = normal1.Y + (incrementNormalY * yOffset),
                    NormalZ = normal1.Z + (incrementNormalZ * yOffset),
                    IncrementNormalX = incrementNormalX,
                    IncrementNormalY = incrementNormalY,
                    IncrementNormalZ = incrementNormalZ,
                    PositionX = position1.X + (incrementPositionX * yOffset),
                    PositionY = position1.Y + (incrementPositionY * yOffset),
                    PositionZ = position1.Z + (incrementPositionZ * yOffset),
                    IncrementPositionX = incrementPositionX,
                    IncrementPositionY = incrementPositionY,
                    IncrementPositionZ = incrementPositionZ
                };

                if (!edgeTable.ContainsKey(startY))
                {
                    edgeTable[startY] = new List<EdgeEntry>();
                }

                edgeTable[startY].Add(entry);
            }

            var activeEdgeTable = new List<EdgeEntry>();
            for (var y = firstVisibleY; y <= lastVisibleY; y++)
            {
                if (edgeTable.TryGetValue(y, out var startingEdges))
                {
                    activeEdgeTable.AddRange(startingEdges);
                }

                activeEdgeTable.RemoveAll(edge => edge.YMax == y);
                activeEdgeTable.Sort((left, right) => left.X.CompareTo(right.X));

                for (var i = 0; i + 1 < activeEdgeTable.Count; i += 2)
                {
                    var leftEdge = activeEdgeTable[i];
                    var rightEdge = activeEdgeTable[i + 1];
                    var xStart = (int)Math.Round(leftEdge.X);
                    var xEnd = (int)Math.Round(rightEdge.X);

                    if (xStart == xEnd)
                    {
                        continue;
                    }

                    if (xStart > xEnd)
                    {
                        (xStart, xEnd) = (xEnd, xStart);
                        (leftEdge, rightEdge) = (rightEdge, leftEdge);
                    }

                    var clippedXStart = Math.Max(xStart, 0);
                    var clippedXEnd = Math.Min(xEnd, width);
                    if (clippedXStart >= clippedXEnd)
                    {
                        continue;
                    }

                    var spanWidth = xEnd - xStart;
                    var depthIncrement = spanWidth == 0
                        ? 0d
                        : (rightEdge.Depth - leftEdge.Depth) / spanWidth;
                    var depthSample = leftEdge.Depth + (depthIncrement * (clippedXStart - xStart));
                    var redIncrement = spanWidth == 0 ? 0d : (rightEdge.Red - leftEdge.Red) / spanWidth;
                    var greenIncrement = spanWidth == 0 ? 0d : (rightEdge.Green - leftEdge.Green) / spanWidth;
                    var blueIncrement = spanWidth == 0 ? 0d : (rightEdge.Blue - leftEdge.Blue) / spanWidth;
                    var normalXIncrement = spanWidth == 0 ? 0d : (rightEdge.NormalX - leftEdge.NormalX) / spanWidth;
                    var normalYIncrement = spanWidth == 0 ? 0d : (rightEdge.NormalY - leftEdge.NormalY) / spanWidth;
                    var normalZIncrement = spanWidth == 0 ? 0d : (rightEdge.NormalZ - leftEdge.NormalZ) / spanWidth;
                    var positionXIncrement = spanWidth == 0 ? 0d : (rightEdge.PositionX - leftEdge.PositionX) / spanWidth;
                    var positionYIncrement = spanWidth == 0 ? 0d : (rightEdge.PositionY - leftEdge.PositionY) / spanWidth;
                    var positionZIncrement = spanWidth == 0 ? 0d : (rightEdge.PositionZ - leftEdge.PositionZ) / spanWidth;
                    var red = leftEdge.Red + (redIncrement * (clippedXStart - xStart));
                    var green = leftEdge.Green + (greenIncrement * (clippedXStart - xStart));
                    var blue = leftEdge.Blue + (blueIncrement * (clippedXStart - xStart));
                    var normalX = leftEdge.NormalX + (normalXIncrement * (clippedXStart - xStart));
                    var normalY = leftEdge.NormalY + (normalYIncrement * (clippedXStart - xStart));
                    var normalZ = leftEdge.NormalZ + (normalZIncrement * (clippedXStart - xStart));
                    var positionX = leftEdge.PositionX + (positionXIncrement * (clippedXStart - xStart));
                    var positionY = leftEdge.PositionY + (positionYIncrement * (clippedXStart - xStart));
                    var positionZ = leftEdge.PositionZ + (positionZIncrement * (clippedXStart - xStart));

                    for (var x = clippedXStart; x < clippedXEnd; x++)
                    {
                        var depth = FromDepthSample(depthSample, face.UseReciprocalDepthInterpolation);
                        if (TryUpdateVisibleDepth(x, y, width, height, depth, DepthEpsilon, out var pixelIndex) &&
                            writeColor)
                        {
                            var pixelColorArgb = flatColorArgb;
                            if (effectiveShadingMode == ShadingMode.Flat)
                            {
                                if (!hasFlatColorArgb)
                                {
                                    flatColorArgb = useUniformColor
                                        ? materialColor.ToArgb()
                                        : ColorVectorToArgb(CalculateIlluminatedColor(
                                            face.FaceNormal,
                                            face.Center,
                                            materialColor,
                                            lighting));
                                    hasFlatColorArgb = true;
                                }

                                pixelColorArgb = flatColorArgb;
                            }
                            else if (effectiveShadingMode == ShadingMode.Gouraud)
                            {
                                pixelColorArgb = ColorVectorToArgb(new ColorVector((float)red, (float)green, (float)blue));
                            }
                            else if (effectiveShadingMode == ShadingMode.Phong)
                            {
                                pixelColorArgb = ColorVectorToArgb(CalculateIlluminatedColor(
                                    new Vector3D((float)normalX, (float)normalY, (float)normalZ),
                                    new Vector3D((float)positionX, (float)positionY, (float)positionZ),
                                    materialColor,
                                    lighting));
                            }

                            _colorBuffer[pixelIndex] = pixelColorArgb;
                        }

                        depthSample += depthIncrement;
                        red += redIncrement;
                        green += greenIncrement;
                        blue += blueIncrement;
                        normalX += normalXIncrement;
                        normalY += normalYIncrement;
                        normalZ += normalZIncrement;
                        positionX += positionXIncrement;
                        positionY += positionYIncrement;
                        positionZ += positionZIncrement;
                    }
                }

                foreach (var edge in activeEdgeTable)
                {
                    edge.X += edge.IncrementX;
                    edge.Depth += edge.IncrementDepth;
                    edge.Red += edge.IncrementRed;
                    edge.Green += edge.IncrementGreen;
                    edge.Blue += edge.IncrementBlue;
                    edge.NormalX += edge.IncrementNormalX;
                    edge.NormalY += edge.IncrementNormalY;
                    edge.NormalZ += edge.IncrementNormalZ;
                    edge.PositionX += edge.IncrementPositionX;
                    edge.PositionY += edge.IncrementPositionY;
                    edge.PositionZ += edge.IncrementPositionZ;
                }
            }
        }

        // Calcula a cor final de um ponto usando luz ambiente, difusa e especular.
        private static ColorVector CalculateIlluminatedColor(
            Vector3D normal,
            Vector3D position,
            Color materialColor,
            LightingOptions lighting)
        {
            var material = ColorToVector(materialColor);
            var lightColor = ColorToVector(lighting.LightColor);

            var n = Normalize(normal);
            var lightPosition = new Vector3D(lighting.LightX, lighting.LightY, lighting.LightZ);
            var lightVector = Normalize(Subtract(lightPosition, position));
            var viewVector = Normalize(Subtract(new Vector3D(0f, 0f, -CameraDistance), position));
            var halfVector = Normalize(Add(lightVector, viewVector));

            var diffuseFactor = MathF.Max(0f, Dot(n, lightVector));
            var specularFactor = diffuseFactor <= 0f
                ? 0f
                : MathF.Pow(MathF.Max(0f, Dot(n, halfVector)), MathF.Max(1f, lighting.Shininess));

            var ambient = new ColorVector(
                material.R * lightColor.R * lighting.AmbientIntensity,
                material.G * lightColor.G * lighting.AmbientIntensity,
                material.B * lightColor.B * lighting.AmbientIntensity);

            var diffuse = new ColorVector(
                material.R * lightColor.R * lighting.DiffuseIntensity * diffuseFactor,
                material.G * lightColor.G * lighting.DiffuseIntensity * diffuseFactor,
                material.B * lightColor.B * lighting.DiffuseIntensity * diffuseFactor);

            var specular = new ColorVector(
                lightColor.R * lighting.SpecularIntensity * specularFactor,
                lightColor.G * lighting.SpecularIntensity * specularFactor,
                lightColor.B * lighting.SpecularIntensity * specularFactor);

            return lighting.Component switch
            {
                LightingComponent.Ambient => ClampColor(ambient),
                LightingComponent.Diffuse => ClampColor(diffuse),
                LightingComponent.Specular => ClampColor(specular),
                _ => ClampColor(new ColorVector(
                    ambient.R + diffuse.R + specular.R,
                    ambient.G + diffuse.G + specular.G,
                    ambient.B + diffuse.B + specular.B))
            };
        }

        // Converte uma cor de 0..255 para componentes de 0..1.
        private static ColorVector ColorToVector(Color color)
        {
            return new ColorVector(color.R / 255f, color.G / 255f, color.B / 255f);
        }

        // Limita os componentes da cor para ficarem entre 0 e 1.
        private static ColorVector ClampColor(ColorVector color)
        {
            return new ColorVector(
                Clamp01(color.R),
                Clamp01(color.G),
                Clamp01(color.B));
        }

        // Converte a cor calculada para o formato ARGB usado no bitmap.
        private static int ColorVectorToArgb(ColorVector color)
        {
            var red = ToByte(color.R);
            var green = ToByte(color.G);
            var blue = ToByte(color.B);

            return (255 << 24) | (red << 16) | (green << 8) | blue;
        }

        // Converte um componente de cor de 0..1 para 0..255.
        private static int ToByte(float value)
        {
            return Math.Clamp((int)MathF.Round(Clamp01(value) * 255f), 0, 255);
        }

        // Limita um valor para o intervalo entre 0 e 1.
        private static float Clamp01(float value)
        {
            return Math.Clamp(value, 0f, 1f);
        }

        // Desenha as arestas por cima das faces ja preenchidas.
        private unsafe void DrawEdgeOverlay(
            IReadOnlyList<RenderableFace> renderableFaces,
            Rectangle bounds,
            int width,
            int height,
            bool depthTestEdges,
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
                        var start = face.ScreenVertices[i];
                        var end = face.ScreenVertices[nextIndex];
                        var edge = new Edge2D(start.Point, end.Point);

                        if (!drawnEdges.Add(edge))
                        {
                            continue;
                        }

                        if (depthTestEdges)
                        {
                            DrawDepthTestedLineMidpoint(
                                scan0,
                                stride,
                                width,
                                height,
                                ToBitmapPoint(start.Point, bounds),
                                ToBitmapPoint(end.Point, bounds),
                                start.Depth,
                                end.Depth,
                                face.UseReciprocalDepthInterpolation,
                                edgeColor);
                        }
                        else
                        {
                            DrawLineMidpoint(
                                scan0,
                                stride,
                                width,
                                height,
                                ToBitmapPoint(start.Point, bounds),
                                ToBitmapPoint(end.Point, bounds),
                                edgeColor);
                        }
                    }
                }
            }
            finally
            {
                _rasterBitmap.UnlockBits(bitmapData);
            }
        }

        // Desenha uma linha com teste de profundidade em cada pixel.
        private unsafe void DrawDepthTestedLineMidpoint(
            byte* scan0,
            int stride,
            int width,
            int height,
            Point p1,
            Point p2,
            float startDepth,
            float endDepth,
            bool useReciprocalDepthInterpolation,
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

            var startDepthSample = ToDepthSample(startDepth, useReciprocalDepthInterpolation);
            var endDepthSample = ToDepthSample(endDepth, useReciprocalDepthInterpolation);
            var totalSteps = Math.Max(dx, dy);
            var depthIncrement = totalSteps == 0
                ? 0d
                : (endDepthSample - startDepthSample) / totalSteps;
            var depthSample = startDepthSample;

            var x = x1;
            var y = y1;

            if (dx >= dy)
            {
                var d = (2 * dy) - dx;
                var incE = 2 * dy;
                var incNE = 2 * (dy - dx);

                for (var i = 0; i <= dx; i++)
                {
                    var depth = FromDepthSample(depthSample, useReciprocalDepthInterpolation);
                    SetPixelSafeWhenVisible(scan0, stride, width, height, x, y, depth, color);

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
                    depthSample += depthIncrement;
                }
            }
            else
            {
                var d = (2 * dx) - dy;
                var incE = 2 * dx;
                var incNE = 2 * (dx - dy);

                for (var i = 0; i <= dy; i++)
                {
                    var depth = FromDepthSample(depthSample, useReciprocalDepthInterpolation);
                    SetPixelSafeWhenVisible(scan0, stride, width, height, x, y, depth, color);

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
                    depthSample += depthIncrement;
                }
            }
        }

        // Pinta um pixel somente se ele esta dentro da tela e na frente do atual.
        private unsafe void SetPixelSafeWhenVisible(
            byte* scan0,
            int stride,
            int width,
            int height,
            int x,
            int y,
            float depth,
            Color color)
        {
            if (x < 0 || x >= width || y < 0 || y >= height || float.IsNaN(depth))
            {
                return;
            }

            var index = (y * width) + x;
            if (depth > _zBuffer[index] + EdgeDepthEpsilon)
            {
                return;
            }

            SetPixelSafe(scan0, stride, width, height, x, y, color);
        }

        // Atualiza o Z-buffer quando a nova profundidade esta visivel.
        private bool TryUpdateVisibleDepth(
            int x,
            int y,
            int width,
            int height,
            float depth,
            float depthEpsilon,
            out int index)
        {
            index = -1;
            if (x < 0 || x >= width || y < 0 || y >= height || float.IsNaN(depth))
            {
                return false;
            }

            index = (y * width) + x;
            if (depth > _zBuffer[index] + depthEpsilon)
            {
                return false;
            }

            _zBuffer[index] = depth;
            return true;
        }

        // Copia o buffer de cores para o bitmap que sera mostrado no Graphics.
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

        // Prepara a profundidade para interpolacao durante o preenchimento.
        private static double ToDepthSample(float depth, bool useReciprocalDepthInterpolation)
        {
            return useReciprocalDepthInterpolation
                ? 1d / depth
                : depth;
        }

        // Converte a profundidade interpolada de volta para o valor real.
        private static float FromDepthSample(double depthSample, bool useReciprocalDepthInterpolation)
        {
            if (!useReciprocalDepthInterpolation)
            {
                return (float)depthSample;
            }

            return depthSample <= 0.0000001d
                ? float.PositiveInfinity
                : (float)(1d / depthSample);
        }

        // Implementa o algoritmo do ponto medio para desenhar uma linha pixel a pixel.
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

        // Pinta um pixel se ele estiver dentro dos limites do bitmap.
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

        // Converte um ponto absoluto da tela para coordenada local do bitmap.
        private static Point ToBitmapPoint(PointF point, Rectangle bounds)
        {
            return new Point(
                (int)MathF.Round(point.X - bounds.Left),
                (int)MathF.Round(point.Y - bounds.Top));
        }

        // Desenha a etiqueta com o nome e parametros da projecao atual.
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

        // Cria os dados basicos de cada miniatura de projecao.
        private static ProjectionThumbnailDefinition[] CreateProjectionThumbnailDefinitions()
        {
            return
            [
                new ProjectionThumbnailDefinition(ProjectionView.Frontal, "Frontal", FrontalProjectionColor),
                new ProjectionThumbnailDefinition(ProjectionView.Superior, "Superior", SuperiorProjectionColor),
                new ProjectionThumbnailDefinition(ProjectionView.Lateral, "Lateral", LateralProjectionColor),
                new ProjectionThumbnailDefinition(ProjectionView.Cavalier, "Cavaleira", CavalierProjectionColor),
                new ProjectionThumbnailDefinition(ProjectionView.Cabinet, "Gabinete", CabinetProjectionColor),
                new ProjectionThumbnailDefinition(ProjectionView.OnePointPerspective, "1 PF", OnePointPerspectiveColor)
            ];
        }

        // Desenha uma miniatura individual de uma projecao.
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
                options.ShowEdges,
                options.ShowBackFaces,
                preview.AccentColor,
                overlayFillColor,
                ShadingMode.Flat,
                LightingOptions.Default);

            if (preview.View == ProjectionView.OnePointPerspective)
            {
                DrawOnePointPerspectiveGuides(graphics, previewCanvas, preview.AccentColor);
            }
        }

        // Monta o texto curto exibido na etiqueta da projecao.
        private static string BuildProjectionBadgeText(ProjectionView projection, ProjectionParameters parameters)
        {
            return projection switch
            {
                ProjectionView.Frontal =>
                    "Frontal | plano XY | profundidade Z",
                ProjectionView.Superior =>
                    "Superior | plano XZ | profundidade Y",
                ProjectionView.Lateral =>
                    "Lateral | plano YZ | profundidade X",
                ProjectionView.Cavalier =>
                    $"Cavaleira | L=1.0 | alfa={parameters.ObliqueAlphaDegrees:F0} graus",
                ProjectionView.Cabinet =>
                    $"Gabinete | L=0.5 | alfa={parameters.ObliqueAlphaDegrees:F0} graus",
                ProjectionView.OnePointPerspective =>
                    $"Perspectiva 1 PF | d={FixedOnePointFocalDistance:F0} | z={parameters.PerspectiveZOffset:F0}",
                _ => "Projeção: Normal (XYZ)"
            };
        }

        // Converte o valor do slider de Z em deslocamento de profundidade.
        private static float MapPerspectiveZOffsetToDepth(float zOffset)
        {
            // O slider vai de 100 a 400. Mantemos uma profundidade-base fixa
            // para que a 1 PF nunca encoste demais no plano de projeção, mas
            // ainda preserve o efeito visual do deslocamento Z.
            return PerspectiveBaseDepthOffset + (zOffset / 100f);
        }

        // Diz se a projecao atual e ortografica.
        private static bool IsOrthographicProjection(ProjectionView projection)
        {
            return projection is ProjectionView.Frontal or ProjectionView.Superior or ProjectionView.Lateral;
        }

        // Retorna a direcao de observacao das projecoes ortograficas.
        private static Vector3D GetOrthographicProjectionDirection(ProjectionView projection)
        {
            return projection switch
            {
                ProjectionView.Superior => new Vector3D(0f, -1f, 0f),
                ProjectionView.Lateral => new Vector3D(-1f, 0f, 0f),
                _ => new Vector3D(0f, 0f, 1f)
            };
        }

        // Retorna a direcao de observacao das projecoes obliquas.
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

        // Desenha a linha do horizonte e o ponto de fuga da perspectiva 1 PF.
        private static void DrawOnePointPerspectiveGuides(Graphics graphics, Rectangle bounds, Color accentColor)
        {
            using var horizonPen = new Pen(Color.FromArgb(108, accentColor), 1f);
            using var vanishingPointBrush = new SolidBrush(accentColor);

            var centerX = bounds.Left + (bounds.Width / 2f);
            var centerY = bounds.Top + (bounds.Height / 2f);
            graphics.DrawLine(horizonPen, bounds.Left, centerY, bounds.Right, centerY);
            graphics.FillEllipse(vanishingPointBrush, centerX - 4f, centerY - 4f, 8f, 8f);
        }

        // Converte graus para radianos.
        private static float DegreesToRadians(float degrees)
        {
            return degrees * (MathF.PI / 180f);
        }

        // Rotaciona um ponto ao redor do eixo X.
        private static Vector3D RotateX(Vector3D point, float radians)
        {
            var cosine = MathF.Cos(radians);
            var sine = MathF.Sin(radians);

            return new Vector3D(
                point.X,
                (point.Y * cosine) - (point.Z * sine),
                (point.Y * sine) + (point.Z * cosine));
        }

        // Rotaciona um ponto ao redor do eixo Y.
        private static Vector3D RotateY(Vector3D point, float radians)
        {
            var cosine = MathF.Cos(radians);
            var sine = MathF.Sin(radians);

            return new Vector3D(
                (point.X * cosine) + (point.Z * sine),
                point.Y,
                (-point.X * sine) + (point.Z * cosine));
        }

        // Calcula a normal geometrica de uma face usando tres vertices.
        private static Vector3D ComputeFaceNormal(Vector3D first, Vector3D second, Vector3D third)
        {
            var edgeA = Subtract(second, first);
            var edgeB = Subtract(third, first);
            return Cross(edgeA, edgeB);
        }

        // Subtrai um vetor do outro.
        private static Vector3D Subtract(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        // Soma dois vetores.
        private static Vector3D Add(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        // Divide um vetor por um numero, evitando divisao por zero.
        private static Vector3D Divide(Vector3D vector, float divisor)
        {
            if (MathF.Abs(divisor) <= 0.000001f)
            {
                return vector;
            }

            return new Vector3D(vector.X / divisor, vector.Y / divisor, vector.Z / divisor);
        }

        // Ajusta o vetor para ter tamanho 1.
        private static Vector3D Normalize(Vector3D vector)
        {
            var lengthSquared = LengthSquared(vector);
            if (lengthSquared <= 0.0000001f)
            {
                return new Vector3D(0f, 0f, -1f);
            }

            var inverseLength = 1f / MathF.Sqrt(lengthSquared);
            return new Vector3D(
                vector.X * inverseLength,
                vector.Y * inverseLength,
                vector.Z * inverseLength);
        }

        // Calcula o produto vetorial entre dois vetores.
        private static Vector3D Cross(Vector3D left, Vector3D right)
        {
            return new Vector3D(
                (left.Y * right.Z) - (left.Z * right.Y),
                (left.Z * right.X) - (left.X * right.Z),
                (left.X * right.Y) - (left.Y * right.X));
        }

        // Calcula o produto escalar entre dois vetores.
        private static float Dot(Vector3D left, Vector3D right)
        {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);
        }

        // Calcula o tamanho ao quadrado de um vetor.
        private static float LengthSquared(Vector3D vector)
        {
            return Dot(vector, vector);
        }

        // Verifica se um indice esta dentro dos limites de uma lista.
        private static bool IsValidIndex(int index, int count)
        {
            return index >= 0 && index < count;
        }

        private readonly record struct RenderableFace(
            ScreenVertex[] ScreenVertices,
            Color FillColor,
            Vector3D FaceNormal,
            Vector3D Center,
            bool IsFrontFace,
            bool UseReciprocalDepthInterpolation);

        private readonly record struct ScreenVertex(
            PointF Point,
            float Depth,
            Vector3D Position,
            Vector3D Normal);

        private readonly record struct ColorVector(float R, float G, float B);

        private readonly record struct ProjectionThumbnailDefinition(
            ProjectionView View,
            string Label,
            Color AccentColor);

        private sealed class EdgeEntry
        {
            public int YMax { get; set; }
            public double X { get; set; }
            public double IncrementX { get; set; }
            public double Depth { get; set; }
            public double IncrementDepth { get; set; }
            public double Red { get; set; }
            public double Green { get; set; }
            public double Blue { get; set; }
            public double IncrementRed { get; set; }
            public double IncrementGreen { get; set; }
            public double IncrementBlue { get; set; }
            public double NormalX { get; set; }
            public double NormalY { get; set; }
            public double NormalZ { get; set; }
            public double IncrementNormalX { get; set; }
            public double IncrementNormalY { get; set; }
            public double IncrementNormalZ { get; set; }
            public double PositionX { get; set; }
            public double PositionY { get; set; }
            public double PositionZ { get; set; }
            public double IncrementPositionX { get; set; }
            public double IncrementPositionY { get; set; }
            public double IncrementPositionZ { get; set; }
        }

        private readonly record struct Edge2D(PointF First, PointF Second)
        {
            // Compara arestas considerando que a ordem dos pontos nao importa.
            public bool Equals(Edge2D other)
            {
                return (PointsEqual(First, other.First) && PointsEqual(Second, other.Second)) ||
                       (PointsEqual(First, other.Second) && PointsEqual(Second, other.First));
            }

            // Gera um hash igual para a mesma aresta, mesmo invertendo inicio e fim.
            public override int GetHashCode()
            {
                var firstHash = HashPoint(First);
                var secondHash = HashPoint(Second);
                return firstHash <= secondHash
                    ? HashCode.Combine(firstHash, secondHash)
                    : HashCode.Combine(secondHash, firstHash);
            }

            // Verifica se dois pontos 2D sao exatamente iguais.
            private static bool PointsEqual(PointF first, PointF second)
            {
                return first.X.Equals(second.X) && first.Y.Equals(second.Y);
            }

            // Gera o hash de um ponto 2D.
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

            // Cria um vetor 3D simples com X, Y e Z.
            public Vector3D(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }
    }
}
