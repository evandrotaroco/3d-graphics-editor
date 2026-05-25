using _3d_graphics_editor.Geometry;
using _3d_graphics_editor.IO;
using _3d_graphics_editor.Rendering;
using System.Globalization;
using System.Numerics;

namespace _3d_graphics_editor
{
    public partial class MainForm : Form
    {
        private const float MouseRotationFactor = 0.01f;
        private const float MouseTranslationFactor = 0.0055f;
        private const float MouseScaleFactor = 0.12f;
        private const float MinScale = 0.18f;
        private const float MaxScale = 8f;
        private const int DefaultObliqueAlphaDegrees = 45;
        private const int DefaultObliqueRotationYDegrees = 0;
        private const int DefaultPerspectiveRotationXDegrees = 0;
        private const int DefaultPerspectiveRotationYDegrees = 0;
        private const int DefaultPerspectiveZOffset = 200;
        private const int ContributionProgressMaximum = 1000;
        private const int ProjectionThumbnailWidth = 86;
        private const int ProjectionThumbnailHeight = 78;

        private readonly MeshViewportRenderer _renderer = new();
        private static readonly ProjectionView[] ProjectionThumbnailViews =
        [
            ProjectionView.Frontal,
            ProjectionView.Superior,
            ProjectionView.Lateral,
            ProjectionView.Cavalier,
            ProjectionView.Cabinet,
            ProjectionView.OnePointPerspective
        ];

        private GroupBox? _lightingContributionGroupBox;
        private ProgressBar? _ambientContributionProgressBar;
        private ProgressBar? _diffuseContributionProgressBar;
        private ProgressBar? _specularContributionProgressBar;
        private ProgressBar? _totalContributionProgressBar;
        private Label? _ambientContributionValueLabel;
        private Label? _diffuseContributionValueLabel;
        private Label? _specularContributionValueLabel;
        private Label? _totalContributionValueLabel;
        private Mesh? _currentMesh;
        private TransformState _transform = TransformState.Default;
        private DragMode _dragMode;
        private Point _lastMousePosition;
        private bool _isUpdatingAxisSelection;
        private bool _isUpdatingProjectionSelection;
        private bool _projectionThumbnailsDirty;
        private SidebarView _sidebarView = SidebarView.Transform;
        private readonly System.Windows.Forms.Timer _projectionThumbnailUpdateTimer = new();
        private readonly Dictionary<ProjectionView, PictureBox> _projectionThumbnailPictureBoxes = [];

        public MainForm()
        {
            InitializeComponent();

            ApplyColorButtonTextColor(lightColorButton);
            ConfigureProjectionThumbnailUpdateTimer();
            ConfigureProjectionThumbnailControls();
            ConfigureLightingContributionPanel();
            AttachEvents();
            ResetViewState();
            UpdateProjectionControlTexts();
            UpdateLightingControlTexts();
            UpdateUiState();
        }

        private void AttachEvents()
        {
            openButton.Click += OpenButton_Click;
            clearButton.Click += ClearButton_Click;
            resetViewButton.Click += ResetViewButton_Click;
            transformViewButton.Click += (_, _) => SwitchSidebarView(SidebarView.Transform);
            projectionViewButton.Click += (_, _) => SwitchSidebarView(SidebarView.Projection);
            lightingViewButton.Click += (_, _) => SwitchSidebarView(SidebarView.Lighting);

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
            showEdgesCheckBox.CheckedChanged += RenderOptionCheckBox_CheckedChanged;
            showBackFacesCheckBox.CheckedChanged += RenderOptionCheckBox_CheckedChanged;
            showProjectionThumbnailsCheckBox.CheckedChanged += RenderOptionCheckBox_CheckedChanged;
            normalProjectionRadioButton.CheckedChanged += ProjectionSelector_CheckedChanged;
            frontalProjectionRadioButton.CheckedChanged += ProjectionSelector_CheckedChanged;
            superiorProjectionRadioButton.CheckedChanged += ProjectionSelector_CheckedChanged;
            lateralProjectionRadioButton.CheckedChanged += ProjectionSelector_CheckedChanged;
            cavalierProjectionRadioButton.CheckedChanged += ProjectionSelector_CheckedChanged;
            cabinetProjectionRadioButton.CheckedChanged += ProjectionSelector_CheckedChanged;
            onePointPerspectiveRadioButton.CheckedChanged += ProjectionSelector_CheckedChanged;
            obliqueAlphaTrackBar.ValueChanged += ProjectionParameterControl_ValueChanged;
            obliqueRotationYTrackBar.ValueChanged += ProjectionParameterControl_ValueChanged;
            perspectiveRotationXTrackBar.ValueChanged += ProjectionParameterControl_ValueChanged;
            perspectiveRotationYTrackBar.ValueChanged += ProjectionParameterControl_ValueChanged;
            perspectiveZOffsetTrackBar.ValueChanged += ProjectionParameterControl_ValueChanged;
            flatShadingRadioButton.CheckedChanged += LightingControl_ValueChanged;
            gouraudShadingRadioButton.CheckedChanged += LightingControl_ValueChanged;
            phongShadingRadioButton.CheckedChanged += LightingControl_ValueChanged;
            ambientIntensityTrackBar.ValueChanged += LightingControl_ValueChanged;
            diffuseIntensityTrackBar.ValueChanged += LightingControl_ValueChanged;
            specularIntensityTrackBar.ValueChanged += LightingControl_ValueChanged;
            shininessTrackBar.ValueChanged += LightingControl_ValueChanged;
            lightXTrackBar.ValueChanged += LightingControl_ValueChanged;
            lightYTrackBar.ValueChanged += LightingControl_ValueChanged;
            lightZTrackBar.ValueChanged += LightingControl_ValueChanged;
            totalLightingRadioButton.CheckedChanged += LightingControl_ValueChanged;
            ambientComponentRadioButton.CheckedChanged += LightingControl_ValueChanged;
            diffuseComponentRadioButton.CheckedChanged += LightingControl_ValueChanged;
            specularComponentRadioButton.CheckedChanged += LightingControl_ValueChanged;
            lightColorButton.Click += (_, _) => ChooseLightingColor(lightColorButton);

            viewportPanel.Paint += ViewportPanel_Paint;
            viewportPanel.Resize += (_, _) =>
            {
                LayoutProjectionThumbnailsPanel();
                viewportPanel.Invalidate();
            };
            viewportPanel.MouseDown += ViewportPanel_MouseDown;
            viewportPanel.MouseMove += ViewportPanel_MouseMove;
            viewportPanel.MouseUp += ViewportPanel_MouseUp;
            viewportPanel.MouseLeave += ViewportPanel_MouseLeave;
            viewportPanel.MouseWheel += ViewportPanel_MouseWheel;
            viewportPanel.MouseEnter += (_, _) => viewportPanel.Focus();
            FormClosed += (_, _) =>
            {
                _projectionThumbnailUpdateTimer.Dispose();
                DisposeProjectionThumbnailImages();
            };
        }

        private void ConfigureProjectionThumbnailUpdateTimer()
        {
            _projectionThumbnailUpdateTimer.Interval = 180;
            _projectionThumbnailUpdateTimer.Tick += ProjectionThumbnailUpdateTimer_Tick;
        }

        private void ConfigureProjectionThumbnailControls()
        {
            projectionThumbnailsPanel.SuspendLayout();

            foreach (var view in ProjectionThumbnailViews)
            {
                var projection = view;
                var pictureBox = new PictureBox
                {
                    Cursor = Cursors.Hand,
                    Margin = new Padding(0, 0, 4, 0),
                    Name = $"{projection}ThumbnailPictureBox",
                    Size = new Size(ProjectionThumbnailWidth, ProjectionThumbnailHeight),
                    SizeMode = PictureBoxSizeMode.Normal,
                    TabStop = false
                };

                pictureBox.Click += (_, _) => SelectProjectionThumbnail(projection);
                projectionThumbnailsPanel.Controls.Add(pictureBox);
                _projectionThumbnailPictureBoxes[projection] = pictureBox;
            }

            projectionThumbnailsPanel.ResumeLayout();
            projectionThumbnailsPanel.BringToFront();
            LayoutProjectionThumbnailsPanel();
        }

        private void ConfigureLightingContributionPanel()
        {
            _lightingContributionGroupBox = new GroupBox
            {
                Location = new Point(0, lightingComponentGroupBox.Bottom + 8),
                Name = "lightingContributionGroupBox",
                Size = new Size(346, 132),
                TabIndex = 6,
                TabStop = false,
                Text = "Contribuição dos componentes"
            };

            _ambientContributionProgressBar = AddContributionRow(
                _lightingContributionGroupBox,
                24,
                "Ambiente",
                out _ambientContributionValueLabel);
            _diffuseContributionProgressBar = AddContributionRow(
                _lightingContributionGroupBox,
                50,
                "Difusa",
                out _diffuseContributionValueLabel);
            _specularContributionProgressBar = AddContributionRow(
                _lightingContributionGroupBox,
                76,
                "Especular",
                out _specularContributionValueLabel);
            _totalContributionProgressBar = AddContributionRow(
                _lightingContributionGroupBox,
                102,
                "Total",
                out _totalContributionValueLabel);

            lightingPagePanel.Controls.Add(_lightingContributionGroupBox);
        }

        private static ProgressBar AddContributionRow(
            Control parent,
            int y,
            string labelText,
            out Label valueLabel)
        {
            var namePrefix = labelText.ToLowerInvariant();
            var rowLabel = new Label
            {
                ForeColor = Color.FromArgb(61, 73, 89),
                Location = new Point(18, y),
                Name = $"{namePrefix}ContributionLabel",
                Size = new Size(82, 17),
                Text = labelText
            };

            var progressBar = new ProgressBar
            {
                Location = new Point(104, y + 2),
                Maximum = ContributionProgressMaximum,
                Name = $"{namePrefix}ContributionProgressBar",
                Size = new Size(170, 14),
                Style = ProgressBarStyle.Continuous
            };

            valueLabel = new Label
            {
                ForeColor = Color.FromArgb(61, 73, 89),
                Location = new Point(286, y),
                Name = $"{namePrefix}ContributionValueLabel",
                Size = new Size(44, 17),
                Text = "0.00",
                TextAlign = ContentAlignment.TopRight
            };

            parent.Controls.Add(rowLabel);
            parent.Controls.Add(progressBar);
            parent.Controls.Add(valueLabel);

            return progressBar;
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
                    throw new InvalidOperationException("O arquivo não possui geometria suficiente para renderização.");
                }

                _currentMesh = mesh;
                ResetViewState();
                ResetProjectionState();
                RefreshProjectionThumbnails();
                UpdateUiState();
                viewportPanel.Focus();
                viewportPanel.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    $"Não foi possível carregar o arquivo selecionado.\n\nDetalhes: {ex.Message}",
                    "Erro ao abrir OBJ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            _currentMesh = null;
            ResetViewState();
            RefreshProjectionThumbnails();
            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void ResetViewButton_Click(object? sender, EventArgs e)
        {
            ResetViewState();
            ResetProjectionState();
            RefreshProjectionThumbnails();
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

            RefreshProjectionThumbnails();
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
            RefreshProjectionThumbnails();
            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void ProjectionParameterControl_ValueChanged(object? sender, EventArgs e)
        {
            UpdateProjectionControlTexts();
            RefreshProjectionThumbnails();
            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void LightingControl_ValueChanged(object? sender, EventArgs e)
        {
            UpdateLightingControlTexts();
            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void ChooseLightingColor(Button colorButton)
        {
            using var dialog = new ColorDialog
            {
                AllowFullOpen = true,
                AnyColor = true,
                Color = colorButton.BackColor,
                FullOpen = true
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            colorButton.BackColor = dialog.Color;
            ApplyColorButtonTextColor(colorButton);
            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private static void ApplyColorButtonTextColor(Button button)
        {
            var luminance = (0.299f * button.BackColor.R) +
                            (0.587f * button.BackColor.G) +
                            (0.114f * button.BackColor.B);
            button.ForeColor = luminance > 150f ? Color.FromArgb(28, 36, 45) : Color.White;
        }

        private void ProjectionThumbnailUpdateTimer_Tick(object? sender, EventArgs e)
        {
            _projectionThumbnailUpdateTimer.Stop();
            RefreshProjectionThumbnailsIfDirty();
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
                frontalProjectionRadioButton.Checked = selectedRadioButton == frontalProjectionRadioButton;
                superiorProjectionRadioButton.Checked = selectedRadioButton == superiorProjectionRadioButton;
                lateralProjectionRadioButton.Checked = selectedRadioButton == lateralProjectionRadioButton;
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
            var wasDragging = _dragMode != DragMode.None;
            _dragMode = DragMode.None;
            viewportPanel.Cursor = _currentMesh is null ? Cursors.Default : Cursors.SizeAll;

            if (wasDragging)
            {
                RefreshProjectionThumbnailsIfDirty();
            }
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

            // Quando mais de um eixo está marcado, o mesmo arrasto aplica as
            // componentes correspondentes em paralelo para permitir combinações.
            _transform = _transform with
            {
                RotationX = rotateX ? _transform.RotationX - (deltaY * MouseRotationFactor) : _transform.RotationX,
                RotationY = rotateY ? _transform.RotationY - (deltaX * MouseRotationFactor) : _transform.RotationY,
                RotationZ = rotateZ ? _transform.RotationZ - (deltaX * MouseRotationFactor) : _transform.RotationZ
            };

            MarkProjectionThumbnailsDirty();
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

            MarkProjectionThumbnailsDirty();
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

            MarkProjectionThumbnailsDirty();
            ScheduleProjectionThumbnailRefresh();
            viewportPanel.Invalidate();
            UpdateUiState();
        }

        private void ResetViewState()
        {
            _transform = TransformState.Default;
        }

        private void ResetProjectionState()
        {
            SyncProjectionSelection(normalProjectionRadioButton);
            showProjectionThumbnailsCheckBox.Checked = false;
            obliqueAlphaTrackBar.Value = DefaultObliqueAlphaDegrees;
            obliqueRotationYTrackBar.Value = DefaultObliqueRotationYDegrees;
            perspectiveRotationXTrackBar.Value = DefaultPerspectiveRotationXDegrees;
            perspectiveRotationYTrackBar.Value = DefaultPerspectiveRotationYDegrees;
            perspectiveZOffsetTrackBar.Value = DefaultPerspectiveZOffset;
            projectionPagePanel.AutoScrollPosition = Point.Empty;
            UpdateProjectionControlTexts();
        }

        private void SelectProjectionThumbnail(ProjectionView projection)
        {
            if (_currentMesh is null)
            {
                return;
            }

            var radioButton = projection switch
            {
                ProjectionView.Frontal => frontalProjectionRadioButton,
                ProjectionView.Superior => superiorProjectionRadioButton,
                ProjectionView.Lateral => lateralProjectionRadioButton,
                ProjectionView.Cabinet => cabinetProjectionRadioButton,
                ProjectionView.OnePointPerspective => onePointPerspectiveRadioButton,
                _ => cavalierProjectionRadioButton
            };

            radioButton.Checked = true;
            viewportPanel.Focus();
        }

        private void MarkProjectionThumbnailsDirty()
        {
            _projectionThumbnailsDirty = true;
        }

        private void ScheduleProjectionThumbnailRefresh()
        {
            if (!showProjectionThumbnailsCheckBox.Checked)
            {
                return;
            }

            _projectionThumbnailUpdateTimer.Stop();
            _projectionThumbnailUpdateTimer.Start();
        }

        private void RefreshProjectionThumbnails()
        {
            _projectionThumbnailUpdateTimer.Stop();
            _projectionThumbnailsDirty = false;
            RenderProjectionThumbnailControls();
        }

        private void RefreshProjectionThumbnailsIfDirty()
        {
            if (!_projectionThumbnailsDirty)
            {
                return;
            }

            RefreshProjectionThumbnails();
            viewportPanel.Invalidate();
        }

        private void RenderProjectionThumbnailControls()
        {
            if (_currentMesh is null || !showProjectionThumbnailsCheckBox.Checked)
            {
                DisposeProjectionThumbnailImages();
                UpdateProjectionThumbnailsPanel();
                return;
            }

            var images = _renderer.RenderProjectionThumbnails(
                _currentMesh,
                _transform,
                BuildProjectionThumbnailRenderOptions(),
                new Size(ProjectionThumbnailWidth, ProjectionThumbnailHeight));

            foreach (var view in ProjectionThumbnailViews)
            {
                if (!_projectionThumbnailPictureBoxes.TryGetValue(view, out var pictureBox))
                {
                    continue;
                }

                var previousImage = pictureBox.Image;
                pictureBox.Image = images.TryGetValue(view, out var image) ? image : null;
                previousImage?.Dispose();
            }

            UpdateProjectionThumbnailsPanel();
        }

        private void DisposeProjectionThumbnailImages()
        {
            foreach (var pictureBox in _projectionThumbnailPictureBoxes.Values)
            {
                pictureBox.Image?.Dispose();
                pictureBox.Image = null;
            }
        }

        private void UpdateProjectionThumbnailsPanel()
        {
            var shouldShow = _currentMesh is not null &&
                             _sidebarView == SidebarView.Projection &&
                             showProjectionThumbnailsCheckBox.Checked;

            projectionThumbnailsPanel.Visible = shouldShow;
            if (!shouldShow)
            {
                return;
            }

            LayoutProjectionThumbnailsPanel();
            projectionThumbnailsPanel.BringToFront();
        }

        private void LayoutProjectionThumbnailsPanel()
        {
            var preferredSize = projectionThumbnailsPanel.PreferredSize;
            var x = Math.Max(8, viewportPanel.ClientSize.Width - preferredSize.Width - 18);
            var y = Math.Max(8, viewportPanel.ClientSize.Height - preferredSize.Height - 18);
            projectionThumbnailsPanel.Location = new Point(x, y);
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
            orthographicProjectionGroupBox.Enabled = hasMesh;
            projectionModeGroupBox.Enabled = hasMesh;
            perspectiveProjectionGroupBox.Enabled = hasMesh;
            shadingGroupBox.Enabled = hasMesh;
            ambientLightingGroupBox.Enabled = hasMesh;
            diffuseLightingGroupBox.Enabled = hasMesh;
            specularLightingGroupBox.Enabled = hasMesh;
            lightPositionGroupBox.Enabled = hasMesh;
            lightingComponentGroupBox.Enabled = hasMesh;
            if (_lightingContributionGroupBox is not null)
            {
                _lightingContributionGroupBox.Enabled = hasMesh;
            }

            fillFacesCheckBox.Enabled = hasMesh;
            showEdgesCheckBox.Enabled = hasMesh;
            showBackFacesCheckBox.Enabled = hasMesh && !fillFacesCheckBox.Checked;
            showProjectionThumbnailsCheckBox.Enabled = hasMesh;
            viewportPanel.Cursor = hasMesh ? Cursors.SizeAll : Cursors.Default;
            UpdateProjectionControlAvailability(hasMesh);
            UpdateSidebarViewState();
            UpdateProjectionThumbnailsPanel();
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
                showEdgesCheckBox.Checked,
                showBackFacesCheckBox.Checked,
                mode,
                projection,
                BuildProjectionParameters(),
                _sidebarView == SidebarView.Lighting && fillFacesCheckBox.Checked,
                GetSelectedShadingMode(),
                BuildLightingOptions());
        }

        private ViewportRenderOptions BuildProjectionThumbnailRenderOptions()
        {
            return new ViewportRenderOptions(
                fillFacesCheckBox.Checked,
                showEdgesCheckBox.Checked,
                showBackFacesCheckBox.Checked,
                ViewportMode.Projection,
                GetSelectedProjectionView(),
                BuildProjectionParameters(),
                false,
                ShadingMode.Flat,
                LightingOptions.Default);
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
            var isProjectionView = _sidebarView == SidebarView.Projection;
            var isLightingView = _sidebarView == SidebarView.Lighting;

            transformPagePanel.Visible = isTransformView;
            projectionPagePanel.Visible = isProjectionView;
            lightingPagePanel.Visible = isLightingView;

            ApplySidebarButtonStyle(transformViewButton, isTransformView);
            ApplySidebarButtonStyle(projectionViewButton, isProjectionView);
            ApplySidebarButtonStyle(lightingViewButton, isLightingView);
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

            if (frontalProjectionRadioButton.Checked)
            {
                return ProjectionView.Frontal;
            }

            if (superiorProjectionRadioButton.Checked)
            {
                return ProjectionView.Superior;
            }

            if (lateralProjectionRadioButton.Checked)
            {
                return ProjectionView.Lateral;
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

        private ShadingMode GetSelectedShadingMode()
        {
            if (flatShadingRadioButton.Checked)
            {
                return ShadingMode.Flat;
            }

            if (gouraudShadingRadioButton.Checked)
            {
                return ShadingMode.Gouraud;
            }

            return ShadingMode.Phong;
        }

        private LightingOptions BuildLightingOptions()
        {
            return new LightingOptions(
                lightColorButton.BackColor,
                ambientIntensityTrackBar.Value / 100f,
                diffuseIntensityTrackBar.Value / 100f,
                specularIntensityTrackBar.Value / 100f,
                shininessTrackBar.Value,
                lightXTrackBar.Value / 100f,
                lightYTrackBar.Value / 100f,
                lightZTrackBar.Value / 100f,
                GetSelectedLightingComponent());
        }

        private LightingComponent GetSelectedLightingComponent()
        {
            if (ambientComponentRadioButton.Checked)
            {
                return LightingComponent.Ambient;
            }

            if (diffuseComponentRadioButton.Checked)
            {
                return LightingComponent.Diffuse;
            }

            if (specularComponentRadioButton.Checked)
            {
                return LightingComponent.Specular;
            }

            return LightingComponent.Total;
        }

        private void UpdateProjectionControlAvailability(bool hasMesh)
        {
            normalProjectionRadioButton.Enabled = hasMesh;
            frontalProjectionRadioButton.Enabled = hasMesh;
            superiorProjectionRadioButton.Enabled = hasMesh;
            lateralProjectionRadioButton.Enabled = hasMesh;
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
            obliqueAlphaLabel.Text = $"Ângulo alfa: {obliqueAlphaTrackBar.Value} graus";
            obliqueRotationYLabel.Text = $"Rotação em Y: {obliqueRotationYTrackBar.Value} graus";
            perspectiveRotationXLabel.Text = $"Rotação em X: {perspectiveRotationXTrackBar.Value} graus";
            perspectiveRotationYLabel.Text = $"Rotação em Y: {perspectiveRotationYTrackBar.Value} graus";
            perspectiveZOffsetLabel.Text = $"Deslocamento Z: {perspectiveZOffsetTrackBar.Value}";
        }

        private void UpdateLightingControlTexts()
        {
            ambientIntensityLabel.Text = $"Intensidade: {ambientIntensityTrackBar.Value}%";
            diffuseIntensityLabel.Text = $"Intensidade: {diffuseIntensityTrackBar.Value}%";
            specularIntensityLabel.Text = $"Intensidade: {specularIntensityTrackBar.Value}%";
            shininessLabel.Text = $"Brilho: {shininessTrackBar.Value}";
            lightXLabel.Text = $"X: {lightXTrackBar.Value / 100f:F2}";
            lightYLabel.Text = $"Y: {lightYTrackBar.Value / 100f:F2}";
            lightZLabel.Text = $"Z: {lightZTrackBar.Value / 100f:F2}";
            UpdateLightingContributionPanel();
        }

        private void UpdateLightingContributionPanel()
        {
            var contribution = CalculateLightingContribution();
            SetContributionValue(_ambientContributionProgressBar, _ambientContributionValueLabel, contribution.Ambient);
            SetContributionValue(_diffuseContributionProgressBar, _diffuseContributionValueLabel, contribution.Diffuse);
            SetContributionValue(_specularContributionProgressBar, _specularContributionValueLabel, contribution.Specular);
            SetContributionValue(_totalContributionProgressBar, _totalContributionValueLabel, contribution.Total);
        }

        private LightingContribution CalculateLightingContribution()
        {
            var normal = new Vector3(0f, 0f, -1f);
            var lightVector = NormalizeOrFallback(
                new Vector3(
                    lightXTrackBar.Value / 100f,
                    lightYTrackBar.Value / 100f,
                    lightZTrackBar.Value / 100f),
                normal);
            var viewVector = normal;
            var halfVector = NormalizeOrFallback(lightVector + viewVector, normal);
            var diffuseFactor = MathF.Max(0f, Vector3.Dot(normal, lightVector));
            var specularFactor = diffuseFactor <= 0f
                ? 0f
                : MathF.Pow(
                    MathF.Max(0f, Vector3.Dot(normal, halfVector)),
                    MathF.Max(1f, shininessTrackBar.Value));

            return new LightingContribution(
                ambientIntensityTrackBar.Value / 100f,
                (diffuseIntensityTrackBar.Value / 100f) * diffuseFactor,
                (specularIntensityTrackBar.Value / 100f) * specularFactor);
        }

        private static void SetContributionValue(ProgressBar? progressBar, Label? valueLabel, float value)
        {
            if (progressBar is null || valueLabel is null)
            {
                return;
            }

            var ratio = Math.Clamp(value, 0f, 1f);
            progressBar.Value = Math.Clamp(
                (int)MathF.Round(progressBar.Maximum * ratio),
                progressBar.Minimum,
                progressBar.Maximum);
            valueLabel.Text = value.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static Vector3 NormalizeOrFallback(Vector3 vector, Vector3 fallback)
        {
            return vector.LengthSquared() <= 0.000001f
                ? fallback
                : Vector3.Normalize(vector);
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
            Projection,
            Lighting
        }

        private readonly record struct LightingContribution(
            float Ambient,
            float Diffuse,
            float Specular)
        {
            public float Total => Ambient + Diffuse + Specular;
        }
    }
}
