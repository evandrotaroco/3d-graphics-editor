namespace _3d_graphics_editor.Rendering
{
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            TabStop = true;
            SetStyle(ControlStyles.Selectable, true);
        }
    }
}
