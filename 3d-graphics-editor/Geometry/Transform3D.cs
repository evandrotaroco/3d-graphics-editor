namespace _3d_graphics_editor.Geometry
{
    public readonly record struct TransformState(
        float RotationX,
        float RotationY,
        float RotationZ,
        float TranslationX,
        float TranslationY,
        float TranslationZ,
        float ScaleX,
        float ScaleY,
        float ScaleZ)
    {
        public static TransformState Default =>
            new(-0.28f, -0.48f, 0f, 0f, 0f, 0f, 1f, 1f, 1f);

        public float UniformScale => (ScaleX + ScaleY + ScaleZ) / 3f;
    }

    public static class Transform3D
    {
        public static Matrix4 Identity()
        {
            return new Matrix4(
                1f, 0f, 0f, 0f,
                0f, 1f, 0f, 0f,
                0f, 0f, 1f, 0f,
                0f, 0f, 0f, 1f);
        }

        public static Matrix4 CreateTranslation(float tx, float ty, float tz)
        {
            return new Matrix4(
                1f, 0f, 0f, tx,
                0f, 1f, 0f, ty,
                0f, 0f, 1f, tz,
                0f, 0f, 0f, 1f);
        }

        public static Matrix4 CreateScale(float sx, float sy, float sz)
        {
            return new Matrix4(
                sx, 0f, 0f, 0f,
                0f, sy, 0f, 0f,
                0f, 0f, sz, 0f,
                0f, 0f, 0f, 1f);
        }

        public static Matrix4 CreateRotationX(float angle)
        {
            var cos = MathF.Cos(angle);
            var sin = MathF.Sin(angle);

            return new Matrix4(
                1f, 0f, 0f, 0f,
                0f, cos, -sin, 0f,
                0f, sin, cos, 0f,
                0f, 0f, 0f, 1f);
        }

        public static Matrix4 CreateRotationY(float angle)
        {
            var cos = MathF.Cos(angle);
            var sin = MathF.Sin(angle);

            return new Matrix4(
                cos, 0f, sin, 0f,
                0f, 1f, 0f, 0f,
                -sin, 0f, cos, 0f,
                0f, 0f, 0f, 1f);
        }

        public static Matrix4 CreateRotationZ(float angle)
        {
            var cos = MathF.Cos(angle);
            var sin = MathF.Sin(angle);

            return new Matrix4(
                cos, -sin, 0f, 0f,
                sin, cos, 0f, 0f,
                0f, 0f, 1f, 0f,
                0f, 0f, 0f, 1f);
        }

        public static Matrix4 Compose(params Matrix4[] matrices)
        {
            var result = Identity();

            foreach (var matrix in matrices)
            {
                result *= matrix;
            }

            return result;
        }

        public static Vertex TransformVertex(Vertex vertex, Matrix4 matrix)
        {
            var transformed = matrix.Transform(new Vector4(vertex.X, vertex.Y, vertex.Z, 1f));
            var w = MathF.Abs(transformed.W) <= 0.0001f ? 1f : transformed.W;

            return new Vertex(
                transformed.X / w,
                transformed.Y / w,
                transformed.Z / w);
        }
    }

    public readonly record struct Matrix4(
        float M11, float M12, float M13, float M14,
        float M21, float M22, float M23, float M24,
        float M31, float M32, float M33, float M34,
        float M41, float M42, float M43, float M44)
    {
        public static Matrix4 operator *(Matrix4 left, Matrix4 right)
        {
            return new Matrix4(
                (left.M11 * right.M11) + (left.M12 * right.M21) + (left.M13 * right.M31) + (left.M14 * right.M41),
                (left.M11 * right.M12) + (left.M12 * right.M22) + (left.M13 * right.M32) + (left.M14 * right.M42),
                (left.M11 * right.M13) + (left.M12 * right.M23) + (left.M13 * right.M33) + (left.M14 * right.M43),
                (left.M11 * right.M14) + (left.M12 * right.M24) + (left.M13 * right.M34) + (left.M14 * right.M44),
                (left.M21 * right.M11) + (left.M22 * right.M21) + (left.M23 * right.M31) + (left.M24 * right.M41),
                (left.M21 * right.M12) + (left.M22 * right.M22) + (left.M23 * right.M32) + (left.M24 * right.M42),
                (left.M21 * right.M13) + (left.M22 * right.M23) + (left.M23 * right.M33) + (left.M24 * right.M43),
                (left.M21 * right.M14) + (left.M22 * right.M24) + (left.M23 * right.M34) + (left.M24 * right.M44),
                (left.M31 * right.M11) + (left.M32 * right.M21) + (left.M33 * right.M31) + (left.M34 * right.M41),
                (left.M31 * right.M12) + (left.M32 * right.M22) + (left.M33 * right.M32) + (left.M34 * right.M42),
                (left.M31 * right.M13) + (left.M32 * right.M23) + (left.M33 * right.M33) + (left.M34 * right.M43),
                (left.M31 * right.M14) + (left.M32 * right.M24) + (left.M33 * right.M34) + (left.M34 * right.M44),
                (left.M41 * right.M11) + (left.M42 * right.M21) + (left.M43 * right.M31) + (left.M44 * right.M41),
                (left.M41 * right.M12) + (left.M42 * right.M22) + (left.M43 * right.M32) + (left.M44 * right.M42),
                (left.M41 * right.M13) + (left.M42 * right.M23) + (left.M43 * right.M33) + (left.M44 * right.M43),
                (left.M41 * right.M14) + (left.M42 * right.M24) + (left.M43 * right.M34) + (left.M44 * right.M44));
        }

        public Vector4 Transform(Vector4 vector)
        {
            return new Vector4(
                (M11 * vector.X) + (M12 * vector.Y) + (M13 * vector.Z) + (M14 * vector.W),
                (M21 * vector.X) + (M22 * vector.Y) + (M23 * vector.Z) + (M24 * vector.W),
                (M31 * vector.X) + (M32 * vector.Y) + (M33 * vector.Z) + (M34 * vector.W),
                (M41 * vector.X) + (M42 * vector.Y) + (M43 * vector.Z) + (M44 * vector.W));
        }
    }

    public readonly record struct Vector4(float X, float Y, float Z, float W);
}
