using _3d_graphics_editor.Geometry;
using _3d_graphics_editor.IO;
using _3d_graphics_editor.Rendering;

namespace _3d_graphics_editor
{
    public partial class MainForm : Form
    {
        private const float MouseRotationFactor = 0.01f;
        private const float MinZoom = 0.18f;
        private const float MaxZoom = 8f;

        private readonly MeshViewportRenderer _renderer = new();

        private Mesh? _currentMesh;
        private string? _currentFilePath;
        private float _rotationX;
        private float _rotationY;
        private float _rotationZ;
        private float _zoom = 1f;
        private bool _isDragging;
        private Point _lastMousePosition;

        public MainForm()
        {
            InitializeComponent();

            AttachEvents();
            ResetViewState();
            UpdateUiState();
        }

        private void AttachEvents()
        {
            openButton.Click += OpenButton_Click;
            clearButton.Click += ClearButton_Click;
            resetViewButton.Click += ResetViewButton_Click;

            viewportPanel.Paint += ViewportPanel_Paint;
            viewportPanel.Resize += (_, _) => viewportPanel.Invalidate();
            viewportPanel.MouseDown += ViewportPanel_MouseDown;
            viewportPanel.MouseMove += ViewportPanel_MouseMove;
            viewportPanel.MouseUp += ViewportPanel_MouseUp;
            viewportPanel.MouseLeave += ViewportPanel_MouseLeave;
            viewportPanel.MouseWheel += ViewportPanel_MouseWheel;
            viewportPanel.MouseEnter += (_, _) => viewportPanel.Focus();
        }

        private void OpenButton_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Wavefront OBJ (*.obj)|*.obj|Todos os arquivos (*.*)|*.*",
                Title = "Abrir modelo OBJ",
                InitialDirectory = GetInitialDirectory()
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                var mesh = ObjMeshSerializer.Load(dialog.FileName);
                if (mesh.Vertices.Count == 0 || mesh.Faces.Count == 0)
                {
                    throw new InvalidOperationException("O arquivo nao possui geometria suficiente para renderizacao.");
                }

                _currentMesh = mesh;
                _currentFilePath = dialog.FileName;
                ResetViewState();
                UpdateUiState();
                viewportPanel.Focus();
                viewportPanel.Invalidate();

                statusLabel.Text = $"Modelo carregado: {Path.GetFileName(dialog.FileName)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    $"Nao foi possivel carregar o arquivo selecionado.\n\nDetalhes: {ex.Message}",
                    "Erro ao abrir OBJ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                statusLabel.Text = "Falha ao carregar o arquivo OBJ.";
            }
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            ClearScene();
        }

        private void ResetViewButton_Click(object? sender, EventArgs e)
        {
            ResetViewState();
            viewportPanel.Invalidate();
            UpdateUiState();
            statusLabel.Text = _currentMesh is null
                ? "Visualizacao resetada."
                : "Visualizacao do modelo resetada.";
        }

        private void ViewportPanel_Paint(object? sender, PaintEventArgs e)
        {
            _renderer.Render(
                e.Graphics,
                viewportPanel.ClientRectangle,
                _currentMesh,
                _rotationX,
                _rotationY,
                _rotationZ,
                _zoom);
        }

        private void ViewportPanel_MouseDown(object? sender, MouseEventArgs e)
        {
            if (_currentMesh is null || e.Button != MouseButtons.Left)
            {
                return;
            }

            viewportPanel.Focus();
            _isDragging = true;
            _lastMousePosition = e.Location;
            viewportPanel.Cursor = Cursors.SizeAll;
        }

        private void ViewportPanel_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!_isDragging)
            {
                return;
            }

            var deltaX = e.X - _lastMousePosition.X;
            var deltaY = e.Y - _lastMousePosition.Y;
            _lastMousePosition = e.Location;

            RotateModel(-deltaY * MouseRotationFactor, -deltaX * MouseRotationFactor, 0f);
        }

        private void ViewportPanel_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (_currentMesh is null)
            {
                return;
            }

            var factor = e.Delta > 0 ? 1.12f : 1f / 1.12f;
            _zoom = Math.Clamp(_zoom * factor, MinZoom, MaxZoom);

            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void ViewportPanel_MouseUp(object? sender, MouseEventArgs e)
        {
            EndDragging();
        }

        private void ViewportPanel_MouseLeave(object? sender, EventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.None)
            {
                EndDragging();
            }
        }

        private void EndDragging()
        {
            _isDragging = false;
            viewportPanel.Cursor = _currentMesh is null ? Cursors.Default : Cursors.SizeAll;
        }

        private void RotateModel(float deltaX, float deltaY, float deltaZ)
        {
            if (_currentMesh is null)
            {
                return;
            }

            _rotationX += deltaX;
            _rotationY += deltaY;
            _rotationZ += deltaZ;

            viewportPanel.Invalidate();
        }

        private void ClearScene()
        {
            _currentMesh = null;
            _currentFilePath = null;
            ResetViewState();
            viewportPanel.Invalidate();
            UpdateUiState();
            statusLabel.Text = "Viewport limpa.";
        }

        private void ResetViewState()
        {
            _rotationX = -0.28f;
            _rotationY = -0.48f;
            _rotationZ = 0f;
            _zoom = 1f;
        }

        private void UpdateUiState()
        {
            var hasMesh = _currentMesh is not null;

            clearButton.Enabled = hasMesh;
            resetViewButton.Enabled = hasMesh;
            viewportPanel.Cursor = hasMesh ? Cursors.SizeAll : Cursors.Default;
            zoomLabel.Text = $"Zoom: {_zoom:P0}";

            if (!hasMesh)
            {
                infoLabel.Text = "Nenhum modelo carregado.";
                return;
            }

            var fileName = Path.GetFileName(_currentFilePath);
            infoLabel.Text =
                $"Arquivo: {fileName}\nVertices: {_currentMesh!.Vertices.Count}\nFaces: {_currentMesh.Faces.Count}";
        }

        private static string GetInitialDirectory()
        {
            var currentDirectory = AppContext.BaseDirectory;
            var directory = new DirectoryInfo(currentDirectory);

            while (directory is not null)
            {
                var dataDirectory = Path.Combine(directory.FullName, "Data");
                if (Directory.Exists(dataDirectory))
                {
                    return dataDirectory;
                }

                directory = directory.Parent;
            }

            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}
