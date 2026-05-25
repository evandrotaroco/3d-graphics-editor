using System.Drawing;

namespace _3d_graphics_editor.Rendering
{
    public enum ShadingMode
    {
        Flat,
        Gouraud,
        Phong
    }

    public enum ViewportMode
    {
        Transform,
        Projection
    }

    public enum ProjectionView
    {
        Normal,
        Frontal,
        Superior,
        Lateral,
        Cavalier,
        Cabinet,
        OnePointPerspective
    }

    public enum LightingComponent
    {
        Total,
        Ambient,
        Diffuse,
        Specular
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

    public readonly record struct LightingOptions(
        Color LightColor,
        float AmbientIntensity,
        float DiffuseIntensity,
        float SpecularIntensity,
        float Shininess,
        float LightX,
        float LightY,
        float LightZ,
        LightingComponent Component)
    {
        public static LightingOptions Default =>
            new(
                Color.FromArgb(255, 255, 255),
                0.18f,
                0.82f,
                0.72f,
                32f,
                -1.3f,
                0.8f,
                -2.3f,
                LightingComponent.Total);
    }

    public readonly record struct ViewportRenderOptions(
        bool FillFaces,
        bool ShowEdges,
        bool ShowBackFaces,
        ViewportMode Mode,
        ProjectionView Projection,
        ProjectionParameters Parameters,
        bool ShowLightMarker,
        ShadingMode ShadingMode,
        LightingOptions Lighting)
    {
        public static ViewportRenderOptions Default =>
            new(
                true,
                true,
                false,
                ViewportMode.Transform,
                ProjectionView.Normal,
                ProjectionParameters.Default,
                false,
                ShadingMode.Phong,
                LightingOptions.Default);
    }
}
