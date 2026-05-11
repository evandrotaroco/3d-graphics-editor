namespace _3d_graphics_editor.Rendering
{
    public enum ViewportMode
    {
        Transform,
        Projection
    }

    public enum ProjectionView
    {
        Normal,
        Cavalier,
        Cabinet,
        OnePointPerspective
    }

    public readonly record struct ProjectionParameters(
        float ObliqueAlphaDegrees,
        float ObliqueRotationYDegrees,
        float PerspectiveRotationXDegrees,
        float PerspectiveRotationYDegrees,
        float PerspectiveZOffset)
    {
        public static ProjectionParameters Default => new(45f, 0f, 0f, 0f, 200f);
    }

    public readonly record struct ViewportRenderOptions(
        bool FillFaces,
        bool ShowBackFaces,
        ViewportMode Mode,
        ProjectionView Projection,
        ProjectionParameters Parameters)
    {
        public static ViewportRenderOptions Default =>
            new(true, false, ViewportMode.Transform, ProjectionView.Normal, ProjectionParameters.Default);
    }
}
