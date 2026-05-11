using _3d_graphics_editor.Geometry;
using _3d_graphics_editor.IO;
using _3d_graphics_editor.Rendering;

namespace _3d_graphics_editor
{
    public partial class MainForm : Form
    {
        private const float MouseRotationFactor = 0.01f;
        private const float MouseTranslationFactor = 0.0055f;
        private const float MouseScaleFactor = 0.12f;
        private const float MinScale = 0.18f;
        private const float MaxScale = 8f;

        private readonly MeshViewportRenderer _renderer = new();

        private Mesh? _currentMesh;
        private TransformState _transform = TransformState.Default;
        private DragMode _dragMode;
        private Point _lastMousePosition;
        private bool _isUpdatingAxisSelection;
        private bool _isUpdatingProjectionSelection;
        private SidebarView _sidebarView = SidebarView.Transform;

        public MainForm()
        {
            InitializeComponent();

            AttachEvents();
            ResetViewState();
            UpdateProjectionControlTexts();
            UpdateUiState();
        }

        private void AttachEvents()
        {
            openButton.Click += OpenButton_Click;
            clearButton.Click += ClearButton_Click;
            resetViewButton.Click += ResetViewButton_Click;
            transformViewButton.Click += (_, _) => SwitchSidebarView(SidebarView.Transform);
            projectionViewButton.Click += (_, _) => SwitchSidebarView(SidebarView.Projection);

            rotationXCheckBox.CheckedChanged += AxisSelector_CheckedChanged;
            rotationYCheckBox.CheckedChanged += AxisSelector_CheckedChanged;
            rotationZCheckBox.CheckedChanged += AxisSelector_CheckedChanged;
            rotationAllCheckBox.CheckedChanged += AxisSelector_CheckedChanged;
            translationXCheckBox.CheckedChanged += AxisSelector_CheckedChanged;
            translationYCheckBox.CheckedChanged += AxisSelector_CheckedChanged;
            translationZCheckBox.CheckedChanged += AxisSelector_CheckedChanged;
            translationAllCheckBox.CheckedChanged += AxisSelector_CheckedChanged;
            scaleXCheckBox.CheckedChanged += AxisSelector_CheckedChanged;
            scaleYCheckBox.CheckedChanged += AxisSelector_CheckedChanged;
            scaleZCheckBox.CheckedChanged += AxisSelector_CheckedChanged;
            scaleAllCheckBox.CheckedChanged += AxisSelector_CheckedChanged;
            fillFacesCheckBox.CheckedChanged += RenderOptionCheckBox_CheckedChanged;
            showBackFacesCheckBox.CheckedChanged += RenderOptionCheckBox_CheckedChanged;
            normalProjectionRadioButton.CheckedChanged += ProjectionSelector_CheckedChanged;
            cavalierProjectionRadioButton.CheckedChanged += ProjectionSelector_CheckedChanged;
            cabinetProjectionRadioButton.CheckedChanged += ProjectionSelector_CheckedChanged;
            onePointPerspectiveRadioButton.CheckedChanged += ProjectionSelector_CheckedChanged;
            obliqueAlphaTrackBar.ValueChanged += ProjectionParameterControl_ValueChanged;
            obliqueRotationYTrackBar.ValueChanged += ProjectionParameterControl_ValueChanged;
            perspectiveRotationXTrackBar.ValueChanged += ProjectionParameterControl_ValueChanged;
            perspectiveRotationYTrackBar.ValueChanged += ProjectionParameterControl_ValueChanged;
            perspectiveZOffsetTrackBar.ValueChanged += ProjectionParameterControl_ValueChanged;

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
                ResetViewState();
                UpdateUiState();
                viewportPanel.Focus();
                viewportPanel.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    $"Nao foi possivel carregar o arquivo selecionado.\n\nDetalhes: {ex.Message}",
                    "Erro ao abrir OBJ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            _currentMesh = null;
            ResetViewState();
            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void ResetViewButton_Click(object? sender, EventArgs e)
        {
            ResetViewState();
            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void AxisSelector_CheckedChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingAxisSelection || sender is not CheckBox changedCheckBox)
            {
                return;
            }

            if (IsRotationSelector(changedCheckBox))
            {
                SyncAxisGroup(changedCheckBox, rotationAllCheckBox, rotationXCheckBox, rotationYCheckBox, rotationZCheckBox);
            }
            else if (IsTranslationSelector(changedCheckBox))
            {
                SyncAxisGroup(changedCheckBox, translationAllCheckBox, translationXCheckBox, translationYCheckBox, translationZCheckBox);
            }
            else if (IsScaleSelector(changedCheckBox))
            {
                SyncAxisGroup(changedCheckBox, scaleAllCheckBox, scaleXCheckBox, scaleYCheckBox, scaleZCheckBox);
            }

            UpdateUiState();
        }

        private void RenderOptionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender == fillFacesCheckBox && fillFacesCheckBox.Checked)
            {
                showBackFacesCheckBox.Checked = false;
            }

            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void ProjectionSelector_CheckedChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingProjectionSelection || sender is not RadioButton radioButton || !radioButton.Checked)
            {
                return;
            }

            SyncProjectionSelection(radioButton);
            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void ProjectionParameterControl_ValueChanged(object? sender, EventArgs e)
        {
            UpdateProjectionControlTexts();
            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void SyncAxisGroup(CheckBox changedCheckBox, CheckBox allCheckBox, CheckBox xCheckBox, CheckBox yCheckBox, CheckBox zCheckBox)
        {
            _isUpdatingAxisSelection = true;

            try
            {
                if (changedCheckBox == allCheckBox)
                {
                    xCheckBox.Checked = allCheckBox.Checked;
                    yCheckBox.Checked = allCheckBox.Checked;
                    zCheckBox.Checked = allCheckBox.Checked;
                    return;
                }

                allCheckBox.Checked = xCheckBox.Checked && yCheckBox.Checked && zCheckBox.Checked;
            }
            finally
            {
                _isUpdatingAxisSelection = false;
            }
        }

        private void SyncProjectionSelection(RadioButton selectedRadioButton)
        {
            _isUpdatingProjectionSelection = true;

            try
            {
                normalProjectionRadioButton.Checked = selectedRadioButton == normalProjectionRadioButton;
                cavalierProjectionRadioButton.Checked = selectedRadioButton == cavalierProjectionRadioButton;
                cabinetProjectionRadioButton.Checked = selectedRadioButton == cabinetProjectionRadioButton;
                onePointPerspectiveRadioButton.Checked = selectedRadioButton == onePointPerspectiveRadioButton;
            }
            finally
            {
                _isUpdatingProjectionSelection = false;
            }
        }

        private void ViewportPanel_Paint(object? sender, PaintEventArgs e)
        {
            _renderer.Render(
                e.Graphics,
                viewportPanel.ClientRectangle,
                _currentMesh,
                _transform,
                BuildRenderOptions());
        }

        private void ViewportPanel_MouseDown(object? sender, MouseEventArgs e)
        {
            if (_currentMesh is null)
            {
                return;
            }

            if (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right)
            {
                return;
            }

            viewportPanel.Focus();
            _dragMode = e.Button == MouseButtons.Left ? DragMode.Rotate : DragMode.Translate;
            _lastMousePosition = e.Location;
            viewportPanel.Cursor = _dragMode == DragMode.Rotate ? Cursors.SizeAll : Cursors.Hand;
        }

        private void ViewportPanel_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_dragMode == DragMode.None)
            {
                return;
            }

            var deltaX = e.X - _lastMousePosition.X;
            var deltaY = e.Y - _lastMousePosition.Y;
            _lastMousePosition = e.Location;

            if (_dragMode == DragMode.Rotate)
            {
                RotateModel(deltaX, deltaY);
                return;
            }

            TranslateModel(deltaX, deltaY);
        }

        private void ViewportPanel_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (_currentMesh is null)
            {
                return;
            }

            ApplyScale(e.Delta > 0 ? MouseScaleFactor : -MouseScaleFactor);
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
            _dragMode = DragMode.None;
            viewportPanel.Cursor = _currentMesh is null ? Cursors.Default : Cursors.SizeAll;
        }

        private void RotateModel(int deltaX, int deltaY)
        {
            if (_currentMesh is null)
            {
                return;
            }

            var rotateX = rotationXCheckBox.Checked;
            var rotateY = rotationYCheckBox.Checked;
            var rotateZ = rotationZCheckBox.Checked;

            if (!rotateX && !rotateY && !rotateZ)
            {
                return;
            }

            // Quando mais de um eixo esta marcado, o mesmo arrasto aplica as
            // componentes correspondentes em paralelo para permitir combinacoes.
            _transform = _transform with
            {
                RotationX = rotateX ? _transform.RotationX - (deltaY * MouseRotationFactor) : _transform.RotationX,
                RotationY = rotateY ? _transform.RotationY - (deltaX * MouseRotationFactor) : _transform.RotationY,
                RotationZ = rotateZ ? _transform.RotationZ - (deltaX * MouseRotationFactor) : _transform.RotationZ
            };

            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void TranslateModel(int deltaX, int deltaY)
        {
            if (_currentMesh is null)
            {
                return;
            }

            var moveX = translationXCheckBox.Checked;
            var moveY = translationYCheckBox.Checked;
            var moveZ = translationZCheckBox.Checked;

            if (!moveX && !moveY && !moveZ)
            {
                return;
            }

            _transform = _transform with
            {
                TranslationX = moveX ? _transform.TranslationX + (deltaX * MouseTranslationFactor) : _transform.TranslationX,
                TranslationY = moveY ? _transform.TranslationY - (deltaY * MouseTranslationFactor) : _transform.TranslationY,
                TranslationZ = moveZ ? _transform.TranslationZ - (deltaY * MouseTranslationFactor) : _transform.TranslationZ
            };

            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void ApplyScale(float delta)
        {
            if (_currentMesh is null)
            {
                return;
            }

            var scaleX = scaleXCheckBox.Checked;
            var scaleY = scaleYCheckBox.Checked;
            var scaleZ = scaleZCheckBox.Checked;

            if (!scaleX && !scaleY && !scaleZ)
            {
                return;
            }

            var factor = 1f + delta;
            _transform = _transform with
            {
                ScaleX = scaleX ? Math.Clamp(_transform.ScaleX * factor, MinScale, MaxScale) : _transform.ScaleX,
                ScaleY = scaleY ? Math.Clamp(_transform.ScaleY * factor, MinScale, MaxScale) : _transform.ScaleY,
                ScaleZ = scaleZ ? Math.Clamp(_transform.ScaleZ * factor, MinScale, MaxScale) : _transform.ScaleZ
            };

            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void ResetViewState()
        {
            _transform = TransformState.Default;
        }

        private void UpdateUiState()
        {
            var hasMesh = _currentMesh is not null;

            clearButton.Enabled = hasMesh;
            resetViewButton.Enabled = hasMesh;
            renderGroupBox.Enabled = hasMesh;
            rotationGroupBox.Enabled = hasMesh;
            translationGroupBox.Enabled = hasMesh;
            scaleGroupBox.Enabled = hasMesh;
            projectionModeGroupBox.Enabled = hasMesh;
            perspectiveProjectionGroupBox.Enabled = hasMesh;
            fillFacesCheckBox.Enabled = hasMesh;
            showBackFacesCheckBox.Enabled = hasMesh && !fillFacesCheckBox.Checked;
            viewportPanel.Cursor = hasMesh ? Cursors.SizeAll : Cursors.Default;
            UpdateProjectionControlAvailability(hasMesh);
            UpdateSidebarViewState();
        }

        private ViewportRenderOptions BuildRenderOptions()
        {
            var mode = _sidebarView == SidebarView.Projection
                ? ViewportMode.Projection
                : ViewportMode.Transform;

            var projection = mode == ViewportMode.Projection
                ? GetSelectedProjectionView()
                : ProjectionView.Normal;

            return new ViewportRenderOptions(
                fillFacesCheckBox.Checked,
                showBackFacesCheckBox.Checked,
                mode,
                projection,
                BuildProjectionParameters());
        }

        private bool IsRotationSelector(CheckBox checkBox)
        {
            return checkBox == rotationXCheckBox ||
                   checkBox == rotationYCheckBox ||
                   checkBox == rotationZCheckBox ||
                   checkBox == rotationAllCheckBox;
        }

        private bool IsTranslationSelector(CheckBox checkBox)
        {
            return checkBox == translationXCheckBox ||
                   checkBox == translationYCheckBox ||
                   checkBox == translationZCheckBox ||
                   checkBox == translationAllCheckBox;
        }

        private bool IsScaleSelector(CheckBox checkBox)
        {
            return checkBox == scaleXCheckBox ||
                   checkBox == scaleYCheckBox ||
                   checkBox == scaleZCheckBox ||
                   checkBox == scaleAllCheckBox;
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

        private void SwitchSidebarView(SidebarView sidebarView)
        {
            if (_sidebarView == sidebarView)
            {
                return;
            }

            _sidebarView = sidebarView;

            if (sidebarView == SidebarView.Transform && !normalProjectionRadioButton.Checked)
            {
                normalProjectionRadioButton.Checked = true;
            }

            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void UpdateSidebarViewState()
        {
            var isTransformView = _sidebarView == SidebarView.Transform;

            transformPagePanel.Visible = isTransformView;
            projectionPagePanel.Visible = !isTransformView;

            ApplySidebarButtonStyle(transformViewButton, isTransformView);
            ApplySidebarButtonStyle(projectionViewButton, !isTransformView);
        }

        private static void ApplySidebarButtonStyle(Button button, bool isActive)
        {
            button.BackColor = isActive
                ? Color.FromArgb(55, 113, 196)
                : Color.FromArgb(232, 236, 241);
            button.ForeColor = isActive ? Color.White : Color.FromArgb(43, 54, 68);
            button.FlatAppearance.BorderColor = isActive
                ? Color.FromArgb(55, 113, 196)
                : Color.FromArgb(206, 213, 221);
        }

        private ProjectionView GetSelectedProjectionView()
        {
            if (normalProjectionRadioButton.Checked)
            {
                return ProjectionView.Normal;
            }

            if (cabinetProjectionRadioButton.Checked)
            {
                return ProjectionView.Cabinet;
            }

            if (onePointPerspectiveRadioButton.Checked)
            {
                return ProjectionView.OnePointPerspective;
            }

            return ProjectionView.Cavalier;
        }

        private ProjectionParameters BuildProjectionParameters()
        {
            return new ProjectionParameters(
                obliqueAlphaTrackBar.Value,
                obliqueRotationYTrackBar.Value,
                perspectiveRotationXTrackBar.Value,
                perspectiveRotationYTrackBar.Value,
                perspectiveZOffsetTrackBar.Value);
        }

        private void UpdateProjectionControlAvailability(bool hasMesh)
        {
            normalProjectionRadioButton.Enabled = hasMesh;
            cavalierProjectionRadioButton.Enabled = hasMesh;
            cabinetProjectionRadioButton.Enabled = hasMesh;
            onePointPerspectiveRadioButton.Enabled = hasMesh;

            var obliqueSelected = hasMesh && (cavalierProjectionRadioButton.Checked || cabinetProjectionRadioButton.Checked);
            obliqueAlphaLabel.Enabled = obliqueSelected;
            obliqueAlphaTrackBar.Enabled = obliqueSelected;
            obliqueRotationYLabel.Enabled = obliqueSelected;
            obliqueRotationYTrackBar.Enabled = obliqueSelected;

            var perspectiveSelected = hasMesh && onePointPerspectiveRadioButton.Checked;
            perspectiveRotationXLabel.Enabled = perspectiveSelected;
            perspectiveRotationXTrackBar.Enabled = perspectiveSelected;
            perspectiveRotationYLabel.Enabled = perspectiveSelected;
            perspectiveRotationYTrackBar.Enabled = perspectiveSelected;
            perspectiveZOffsetLabel.Enabled = perspectiveSelected;
            perspectiveZOffsetTrackBar.Enabled = perspectiveSelected;
        }

        private void UpdateProjectionControlTexts()
        {
            obliqueAlphaLabel.Text = $"Angulo alfa: {obliqueAlphaTrackBar.Value} deg";
            obliqueRotationYLabel.Text = $"Rotacao em Y: {obliqueRotationYTrackBar.Value} deg";
            perspectiveRotationXLabel.Text = $"Rotacao em X: {perspectiveRotationXTrackBar.Value} deg";
            perspectiveRotationYLabel.Text = $"Rotacao em Y: {perspectiveRotationYTrackBar.Value} deg";
            perspectiveZOffsetLabel.Text = $"Z offset: {perspectiveZOffsetTrackBar.Value}";
        }

        private enum DragMode
        {
            None,
            Rotate,
            Translate
        }

        private enum SidebarView
        {
            Transform,
            Projection
        }
    }
}
