namespace _3d_graphics_editor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private GroupBox viewportGroupBox;
        private Rendering.DoubleBufferedPanel viewportPanel;
        private FlowLayoutPanel projectionThumbnailsPanel;
        private Panel sidebarPanel;
        private Label sidebarTitleLabel;
        private Button openButton;
        private Button clearButton;
        private Button resetViewButton;
        private Button transformViewButton;
        private Button projectionViewButton;
        private GroupBox renderGroupBox;
        private CheckBox fillFacesCheckBox;
        private CheckBox showBackFacesCheckBox;
        private Panel modeContentPanel;
        private Panel transformPagePanel;
        private Panel projectionPagePanel;
        private GroupBox rotationGroupBox;
        private GroupBox translationGroupBox;
        private GroupBox scaleGroupBox;
        private CheckBox rotationXCheckBox;
        private CheckBox rotationYCheckBox;
        private CheckBox rotationZCheckBox;
        private CheckBox rotationAllCheckBox;
        private CheckBox translationXCheckBox;
        private CheckBox translationYCheckBox;
        private CheckBox translationZCheckBox;
        private CheckBox translationAllCheckBox;
        private CheckBox scaleXCheckBox;
        private CheckBox scaleYCheckBox;
        private CheckBox scaleZCheckBox;
        private CheckBox scaleAllCheckBox;
        private Label rotationHintLabel;
        private Label translationHintLabel;
        private Label scaleHintLabel;
        private GroupBox orthographicProjectionGroupBox;
        private GroupBox projectionModeGroupBox;
        private RadioButton normalProjectionRadioButton;
        private CheckBox showProjectionThumbnailsCheckBox;
        private RadioButton frontalProjectionRadioButton;
        private RadioButton superiorProjectionRadioButton;
        private RadioButton lateralProjectionRadioButton;
        private RadioButton cavalierProjectionRadioButton;
        private RadioButton cabinetProjectionRadioButton;
        private RadioButton onePointPerspectiveRadioButton;
        private Label obliqueAlphaLabel;
        private TrackBar obliqueAlphaTrackBar;
        private Label obliqueRotationYLabel;
        private TrackBar obliqueRotationYTrackBar;
        private GroupBox perspectiveProjectionGroupBox;
        private Label perspectiveRotationXLabel;
        private TrackBar perspectiveRotationXTrackBar;
        private Label perspectiveRotationYLabel;
        private TrackBar perspectiveRotationYTrackBar;
        private Label perspectiveZOffsetLabel;
        private TrackBar perspectiveZOffsetTrackBar;
        private Button lightingViewButton;
        private Panel lightingPagePanel;
        private GroupBox shadingGroupBox;
        private CheckBox showEdgesCheckBox;
        private RadioButton flatShadingRadioButton;
        private RadioButton gouraudShadingRadioButton;
        private RadioButton phongShadingRadioButton;
        private Label lightColorLabel;
        private Button lightColorButton;
        private GroupBox ambientLightingGroupBox;
        private Label ambientIntensityLabel;
        private TrackBar ambientIntensityTrackBar;
        private GroupBox diffuseLightingGroupBox;
        private Label diffuseIntensityLabel;
        private TrackBar diffuseIntensityTrackBar;
        private GroupBox specularLightingGroupBox;
        private Label specularIntensityLabel;
        private TrackBar specularIntensityTrackBar;
        private Label shininessLabel;
        private TrackBar shininessTrackBar;
        private GroupBox lightPositionGroupBox;
        private Label lightXLabel;
        private TrackBar lightXTrackBar;
        private Label lightYLabel;
        private TrackBar lightYTrackBar;
        private Label lightZLabel;
        private TrackBar lightZTrackBar;
        private CheckBox showLightMarkerCheckBox;
        private GroupBox lightingComponentGroupBox;
        private RadioButton totalLightingRadioButton;
        private RadioButton ambientComponentRadioButton;
        private RadioButton diffuseComponentRadioButton;
        private RadioButton specularComponentRadioButton;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            viewportGroupBox = new GroupBox();
            viewportPanel = new _3d_graphics_editor.Rendering.DoubleBufferedPanel();
            projectionThumbnailsPanel = new FlowLayoutPanel();
            sidebarPanel = new Panel();
            modeContentPanel = new Panel();
            lightingPagePanel = new Panel();
            lightingComponentGroupBox = new GroupBox();
            specularComponentRadioButton = new RadioButton();
            diffuseComponentRadioButton = new RadioButton();
            ambientComponentRadioButton = new RadioButton();
            totalLightingRadioButton = new RadioButton();
            lightPositionGroupBox = new GroupBox();
            showLightMarkerCheckBox = new CheckBox();
            lightZTrackBar = new TrackBar();
            lightZLabel = new Label();
            lightYTrackBar = new TrackBar();
            lightYLabel = new Label();
            lightXTrackBar = new TrackBar();
            lightXLabel = new Label();
            specularLightingGroupBox = new GroupBox();
            shininessTrackBar = new TrackBar();
            shininessLabel = new Label();
            specularIntensityTrackBar = new TrackBar();
            specularIntensityLabel = new Label();
            diffuseLightingGroupBox = new GroupBox();
            diffuseIntensityTrackBar = new TrackBar();
            diffuseIntensityLabel = new Label();
            ambientLightingGroupBox = new GroupBox();
            ambientIntensityTrackBar = new TrackBar();
            ambientIntensityLabel = new Label();
            shadingGroupBox = new GroupBox();
            lightColorButton = new Button();
            lightColorLabel = new Label();
            phongShadingRadioButton = new RadioButton();
            gouraudShadingRadioButton = new RadioButton();
            flatShadingRadioButton = new RadioButton();
            showEdgesCheckBox = new CheckBox();
            fillFacesCheckBox = new CheckBox();
            projectionPagePanel = new Panel();
            perspectiveProjectionGroupBox = new GroupBox();
            perspectiveZOffsetTrackBar = new TrackBar();
            perspectiveZOffsetLabel = new Label();
            perspectiveRotationYTrackBar = new TrackBar();
            perspectiveRotationYLabel = new Label();
            perspectiveRotationXTrackBar = new TrackBar();
            perspectiveRotationXLabel = new Label();
            onePointPerspectiveRadioButton = new RadioButton();
            projectionModeGroupBox = new GroupBox();
            obliqueRotationYTrackBar = new TrackBar();
            obliqueRotationYLabel = new Label();
            obliqueAlphaTrackBar = new TrackBar();
            obliqueAlphaLabel = new Label();
            cabinetProjectionRadioButton = new RadioButton();
            cavalierProjectionRadioButton = new RadioButton();
            orthographicProjectionGroupBox = new GroupBox();
            lateralProjectionRadioButton = new RadioButton();
            superiorProjectionRadioButton = new RadioButton();
            frontalProjectionRadioButton = new RadioButton();
            showProjectionThumbnailsCheckBox = new CheckBox();
            normalProjectionRadioButton = new RadioButton();
            transformPagePanel = new Panel();
            scaleGroupBox = new GroupBox();
            scaleHintLabel = new Label();
            scaleAllCheckBox = new CheckBox();
            scaleZCheckBox = new CheckBox();
            scaleYCheckBox = new CheckBox();
            scaleXCheckBox = new CheckBox();
            translationGroupBox = new GroupBox();
            translationHintLabel = new Label();
            translationAllCheckBox = new CheckBox();
            translationZCheckBox = new CheckBox();
            translationYCheckBox = new CheckBox();
            translationXCheckBox = new CheckBox();
            rotationGroupBox = new GroupBox();
            rotationHintLabel = new Label();
            rotationAllCheckBox = new CheckBox();
            rotationZCheckBox = new CheckBox();
            rotationYCheckBox = new CheckBox();
            rotationXCheckBox = new CheckBox();
            renderGroupBox = new GroupBox();
            showBackFacesCheckBox = new CheckBox();
            lightingViewButton = new Button();
            projectionViewButton = new Button();
            transformViewButton = new Button();
            resetViewButton = new Button();
            clearButton = new Button();
            openButton = new Button();
            sidebarTitleLabel = new Label();
            viewportGroupBox.SuspendLayout();
            viewportPanel.SuspendLayout();
            sidebarPanel.SuspendLayout();
            modeContentPanel.SuspendLayout();
            lightingPagePanel.SuspendLayout();
            lightingComponentGroupBox.SuspendLayout();
            lightPositionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lightZTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lightYTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lightXTrackBar).BeginInit();
            specularLightingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)shininessTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)specularIntensityTrackBar).BeginInit();
            diffuseLightingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)diffuseIntensityTrackBar).BeginInit();
            ambientLightingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ambientIntensityTrackBar).BeginInit();
            shadingGroupBox.SuspendLayout();
            projectionPagePanel.SuspendLayout();
            perspectiveProjectionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)perspectiveZOffsetTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)perspectiveRotationYTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)perspectiveRotationXTrackBar).BeginInit();
            projectionModeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)obliqueRotationYTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)obliqueAlphaTrackBar).BeginInit();
            orthographicProjectionGroupBox.SuspendLayout();
            transformPagePanel.SuspendLayout();
            scaleGroupBox.SuspendLayout();
            translationGroupBox.SuspendLayout();
            rotationGroupBox.SuspendLayout();
            renderGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // viewportGroupBox
            // 
            viewportGroupBox.Controls.Add(viewportPanel);
            viewportGroupBox.Dock = DockStyle.Fill;
            viewportGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            viewportGroupBox.Location = new Point(0, 0);
            viewportGroupBox.Name = "viewportGroupBox";
            viewportGroupBox.Padding = new Padding(14);
            viewportGroupBox.Size = new Size(950, 749);
            viewportGroupBox.TabIndex = 0;
            viewportGroupBox.TabStop = false;
            viewportGroupBox.Text = "Visualização 3D";
            // 
            // viewportPanel
            // 
            viewportPanel.BackColor = Color.White;
            viewportPanel.Controls.Add(projectionThumbnailsPanel);
            viewportPanel.Dock = DockStyle.Fill;
            viewportPanel.Location = new Point(14, 32);
            viewportPanel.Name = "viewportPanel";
            viewportPanel.Size = new Size(922, 703);
            viewportPanel.TabIndex = 0;
            viewportPanel.TabStop = true;
            // 
            // projectionThumbnailsPanel
            // 
            projectionThumbnailsPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            projectionThumbnailsPanel.AutoSize = true;
            projectionThumbnailsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            projectionThumbnailsPanel.BackColor = Color.FromArgb(245, 248, 252);
            projectionThumbnailsPanel.Location = new Point(360, 606);
            projectionThumbnailsPanel.Name = "projectionThumbnailsPanel";
            projectionThumbnailsPanel.Padding = new Padding(6);
            projectionThumbnailsPanel.Size = new Size(12, 12);
            projectionThumbnailsPanel.TabIndex = 0;
            projectionThumbnailsPanel.Visible = false;
            projectionThumbnailsPanel.WrapContents = false;
            // 
            // sidebarPanel
            // 
            sidebarPanel.BackColor = Color.FromArgb(242, 245, 248);
            sidebarPanel.Controls.Add(modeContentPanel);
            sidebarPanel.Controls.Add(renderGroupBox);
            sidebarPanel.Controls.Add(lightingViewButton);
            sidebarPanel.Controls.Add(projectionViewButton);
            sidebarPanel.Controls.Add(transformViewButton);
            sidebarPanel.Controls.Add(resetViewButton);
            sidebarPanel.Controls.Add(clearButton);
            sidebarPanel.Controls.Add(openButton);
            sidebarPanel.Controls.Add(sidebarTitleLabel);
            sidebarPanel.Dock = DockStyle.Right;
            sidebarPanel.Location = new Point(950, 0);
            sidebarPanel.Name = "sidebarPanel";
            sidebarPanel.Padding = new Padding(18);
            sidebarPanel.Size = new Size(400, 749);
            sidebarPanel.TabIndex = 1;
            // 
            // modeContentPanel
            // 
            modeContentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            modeContentPanel.Controls.Add(lightingPagePanel);
            modeContentPanel.Controls.Add(projectionPagePanel);
            modeContentPanel.Controls.Add(transformPagePanel);
            modeContentPanel.Location = new Point(18, 226);
            modeContentPanel.Name = "modeContentPanel";
            modeContentPanel.Size = new Size(364, 490);
            modeContentPanel.TabIndex = 7;
            // 
            // lightingPagePanel
            // 
            lightingPagePanel.AutoScroll = true;
            lightingPagePanel.Controls.Add(lightingComponentGroupBox);
            lightingPagePanel.Controls.Add(lightPositionGroupBox);
            lightingPagePanel.Controls.Add(specularLightingGroupBox);
            lightingPagePanel.Controls.Add(diffuseLightingGroupBox);
            lightingPagePanel.Controls.Add(ambientLightingGroupBox);
            lightingPagePanel.Controls.Add(shadingGroupBox);
            lightingPagePanel.Dock = DockStyle.Fill;
            lightingPagePanel.Location = new Point(0, 0);
            lightingPagePanel.Name = "lightingPagePanel";
            lightingPagePanel.Size = new Size(364, 490);
            lightingPagePanel.TabIndex = 2;
            lightingPagePanel.Visible = false;
            // 
            // lightingComponentGroupBox
            // 
            lightingComponentGroupBox.Controls.Add(specularComponentRadioButton);
            lightingComponentGroupBox.Controls.Add(diffuseComponentRadioButton);
            lightingComponentGroupBox.Controls.Add(ambientComponentRadioButton);
            lightingComponentGroupBox.Controls.Add(totalLightingRadioButton);
            lightingComponentGroupBox.Location = new Point(0, 568);
            lightingComponentGroupBox.Name = "lightingComponentGroupBox";
            lightingComponentGroupBox.Size = new Size(346, 86);
            lightingComponentGroupBox.TabIndex = 5;
            lightingComponentGroupBox.TabStop = false;
            lightingComponentGroupBox.Text = "Componente exibido";
            // 
            // specularComponentRadioButton
            // 
            specularComponentRadioButton.AutoSize = true;
            specularComponentRadioButton.Location = new Point(120, 52);
            specularComponentRadioButton.Name = "specularComponentRadioButton";
            specularComponentRadioButton.Size = new Size(75, 19);
            specularComponentRadioButton.TabIndex = 3;
            specularComponentRadioButton.Text = "Especular";
            specularComponentRadioButton.UseVisualStyleBackColor = true;
            // 
            // diffuseComponentRadioButton
            // 
            diffuseComponentRadioButton.AutoSize = true;
            diffuseComponentRadioButton.Location = new Point(240, 24);
            diffuseComponentRadioButton.Name = "diffuseComponentRadioButton";
            diffuseComponentRadioButton.Size = new Size(58, 19);
            diffuseComponentRadioButton.TabIndex = 2;
            diffuseComponentRadioButton.Text = "Difusa";
            diffuseComponentRadioButton.UseVisualStyleBackColor = true;
            // 
            // ambientComponentRadioButton
            // 
            ambientComponentRadioButton.AutoSize = true;
            ambientComponentRadioButton.Location = new Point(120, 24);
            ambientComponentRadioButton.Name = "ambientComponentRadioButton";
            ambientComponentRadioButton.Size = new Size(77, 19);
            ambientComponentRadioButton.TabIndex = 1;
            ambientComponentRadioButton.Text = "Ambiente";
            ambientComponentRadioButton.UseVisualStyleBackColor = true;
            // 
            // totalLightingRadioButton
            // 
            totalLightingRadioButton.AutoSize = true;
            totalLightingRadioButton.Checked = true;
            totalLightingRadioButton.Location = new Point(18, 24);
            totalLightingRadioButton.Name = "totalLightingRadioButton";
            totalLightingRadioButton.Size = new Size(51, 19);
            totalLightingRadioButton.TabIndex = 0;
            totalLightingRadioButton.TabStop = true;
            totalLightingRadioButton.Text = "Total";
            totalLightingRadioButton.UseVisualStyleBackColor = true;
            // 
            // lightPositionGroupBox
            // 
            lightPositionGroupBox.Controls.Add(showLightMarkerCheckBox);
            lightPositionGroupBox.Controls.Add(lightZTrackBar);
            lightPositionGroupBox.Controls.Add(lightZLabel);
            lightPositionGroupBox.Controls.Add(lightYTrackBar);
            lightPositionGroupBox.Controls.Add(lightYLabel);
            lightPositionGroupBox.Controls.Add(lightXTrackBar);
            lightPositionGroupBox.Controls.Add(lightXLabel);
            lightPositionGroupBox.Location = new Point(0, 416);
            lightPositionGroupBox.Name = "lightPositionGroupBox";
            lightPositionGroupBox.Size = new Size(346, 144);
            lightPositionGroupBox.TabIndex = 4;
            lightPositionGroupBox.TabStop = false;
            lightPositionGroupBox.Text = "Posição da luz";
            // 
            // showLightMarkerCheckBox
            // 
            showLightMarkerCheckBox.AutoSize = true;
            showLightMarkerCheckBox.Checked = true;
            showLightMarkerCheckBox.CheckState = CheckState.Checked;
            showLightMarkerCheckBox.Location = new Point(18, 116);
            showLightMarkerCheckBox.Name = "showLightMarkerCheckBox";
            showLightMarkerCheckBox.Size = new Size(155, 19);
            showLightMarkerCheckBox.TabIndex = 6;
            showLightMarkerCheckBox.Text = "Mostrar marcador da luz";
            showLightMarkerCheckBox.UseVisualStyleBackColor = true;
            // 
            // lightZTrackBar
            // 
            lightZTrackBar.AutoSize = false;
            lightZTrackBar.Location = new Point(98, 84);
            lightZTrackBar.Maximum = 130;
            lightZTrackBar.Minimum = -130;
            lightZTrackBar.Name = "lightZTrackBar";
            lightZTrackBar.Size = new Size(230, 24);
            lightZTrackBar.TabIndex = 5;
            lightZTrackBar.TickStyle = TickStyle.None;
            lightZTrackBar.Value = -120;
            // 
            // lightZLabel
            // 
            lightZLabel.ForeColor = Color.FromArgb(61, 73, 89);
            lightZLabel.Location = new Point(18, 88);
            lightZLabel.Name = "lightZLabel";
            lightZLabel.Size = new Size(72, 17);
            lightZLabel.TabIndex = 4;
            lightZLabel.Text = "Z: -1.20";
            // 
            // lightYTrackBar
            // 
            lightYTrackBar.AutoSize = false;
            lightYTrackBar.Location = new Point(98, 52);
            lightYTrackBar.Maximum = 80;
            lightYTrackBar.Minimum = -80;
            lightYTrackBar.Name = "lightYTrackBar";
            lightYTrackBar.Size = new Size(230, 24);
            lightYTrackBar.TabIndex = 3;
            lightYTrackBar.TickStyle = TickStyle.None;
            lightYTrackBar.Value = 65;
            // 
            // lightYLabel
            // 
            lightYLabel.ForeColor = Color.FromArgb(61, 73, 89);
            lightYLabel.Location = new Point(18, 56);
            lightYLabel.Name = "lightYLabel";
            lightYLabel.Size = new Size(72, 17);
            lightYLabel.TabIndex = 2;
            lightYLabel.Text = "Y: 0.65";
            // 
            // lightXTrackBar
            // 
            lightXTrackBar.AutoSize = false;
            lightXTrackBar.Location = new Point(98, 20);
            lightXTrackBar.Maximum = 140;
            lightXTrackBar.Minimum = -140;
            lightXTrackBar.Name = "lightXTrackBar";
            lightXTrackBar.Size = new Size(230, 24);
            lightXTrackBar.TabIndex = 1;
            lightXTrackBar.TickStyle = TickStyle.None;
            lightXTrackBar.Value = 75;
            // 
            // lightXLabel
            // 
            lightXLabel.ForeColor = Color.FromArgb(61, 73, 89);
            lightXLabel.Location = new Point(18, 24);
            lightXLabel.Name = "lightXLabel";
            lightXLabel.Size = new Size(72, 17);
            lightXLabel.TabIndex = 0;
            lightXLabel.Text = "X: 0.75";
            // 
            // specularLightingGroupBox
            // 
            specularLightingGroupBox.Controls.Add(shininessTrackBar);
            specularLightingGroupBox.Controls.Add(shininessLabel);
            specularLightingGroupBox.Controls.Add(specularIntensityTrackBar);
            specularLightingGroupBox.Controls.Add(specularIntensityLabel);
            specularLightingGroupBox.Location = new Point(0, 296);
            specularLightingGroupBox.Name = "specularLightingGroupBox";
            specularLightingGroupBox.Size = new Size(346, 112);
            specularLightingGroupBox.TabIndex = 3;
            specularLightingGroupBox.TabStop = false;
            specularLightingGroupBox.Text = "Especular";
            // 
            // shininessTrackBar
            // 
            shininessTrackBar.AutoSize = false;
            shininessTrackBar.Location = new Point(98, 72);
            shininessTrackBar.Maximum = 128;
            shininessTrackBar.Minimum = 1;
            shininessTrackBar.Name = "shininessTrackBar";
            shininessTrackBar.Size = new Size(230, 28);
            shininessTrackBar.TabIndex = 3;
            shininessTrackBar.TickStyle = TickStyle.None;
            shininessTrackBar.Value = 32;
            // 
            // shininessLabel
            // 
            shininessLabel.ForeColor = Color.FromArgb(61, 73, 89);
            shininessLabel.Location = new Point(18, 76);
            shininessLabel.Name = "shininessLabel";
            shininessLabel.Size = new Size(72, 17);
            shininessLabel.TabIndex = 2;
            shininessLabel.Text = "Brilho: 32";
            // 
            // specularIntensityTrackBar
            // 
            specularIntensityTrackBar.AutoSize = false;
            specularIntensityTrackBar.Location = new Point(18, 44);
            specularIntensityTrackBar.Maximum = 100;
            specularIntensityTrackBar.Name = "specularIntensityTrackBar";
            specularIntensityTrackBar.Size = new Size(310, 28);
            specularIntensityTrackBar.TabIndex = 1;
            specularIntensityTrackBar.TickStyle = TickStyle.None;
            specularIntensityTrackBar.Value = 72;
            // 
            // specularIntensityLabel
            // 
            specularIntensityLabel.ForeColor = Color.FromArgb(61, 73, 89);
            specularIntensityLabel.Location = new Point(18, 24);
            specularIntensityLabel.Name = "specularIntensityLabel";
            specularIntensityLabel.Size = new Size(164, 17);
            specularIntensityLabel.TabIndex = 0;
            specularIntensityLabel.Text = "Intensidade: 72%";
            // 
            // diffuseLightingGroupBox
            // 
            diffuseLightingGroupBox.Controls.Add(diffuseIntensityTrackBar);
            diffuseLightingGroupBox.Controls.Add(diffuseIntensityLabel);
            diffuseLightingGroupBox.Location = new Point(0, 212);
            diffuseLightingGroupBox.Name = "diffuseLightingGroupBox";
            diffuseLightingGroupBox.Size = new Size(346, 76);
            diffuseLightingGroupBox.TabIndex = 2;
            diffuseLightingGroupBox.TabStop = false;
            diffuseLightingGroupBox.Text = "Difusa";
            // 
            // diffuseIntensityTrackBar
            // 
            diffuseIntensityTrackBar.AutoSize = false;
            diffuseIntensityTrackBar.Location = new Point(18, 40);
            diffuseIntensityTrackBar.Maximum = 100;
            diffuseIntensityTrackBar.Name = "diffuseIntensityTrackBar";
            diffuseIntensityTrackBar.Size = new Size(310, 28);
            diffuseIntensityTrackBar.TabIndex = 1;
            diffuseIntensityTrackBar.TickStyle = TickStyle.None;
            diffuseIntensityTrackBar.Value = 82;
            // 
            // diffuseIntensityLabel
            // 
            diffuseIntensityLabel.ForeColor = Color.FromArgb(61, 73, 89);
            diffuseIntensityLabel.Location = new Point(18, 24);
            diffuseIntensityLabel.Name = "diffuseIntensityLabel";
            diffuseIntensityLabel.Size = new Size(164, 17);
            diffuseIntensityLabel.TabIndex = 0;
            diffuseIntensityLabel.Text = "Intensidade: 82%";
            // 
            // ambientLightingGroupBox
            // 
            ambientLightingGroupBox.Controls.Add(ambientIntensityTrackBar);
            ambientLightingGroupBox.Controls.Add(ambientIntensityLabel);
            ambientLightingGroupBox.Location = new Point(0, 128);
            ambientLightingGroupBox.Name = "ambientLightingGroupBox";
            ambientLightingGroupBox.Size = new Size(346, 76);
            ambientLightingGroupBox.TabIndex = 1;
            ambientLightingGroupBox.TabStop = false;
            ambientLightingGroupBox.Text = "Ambiente";
            // 
            // ambientIntensityTrackBar
            // 
            ambientIntensityTrackBar.AutoSize = false;
            ambientIntensityTrackBar.Location = new Point(18, 40);
            ambientIntensityTrackBar.Maximum = 100;
            ambientIntensityTrackBar.Name = "ambientIntensityTrackBar";
            ambientIntensityTrackBar.Size = new Size(310, 28);
            ambientIntensityTrackBar.TabIndex = 1;
            ambientIntensityTrackBar.TickStyle = TickStyle.None;
            ambientIntensityTrackBar.Value = 18;
            // 
            // ambientIntensityLabel
            // 
            ambientIntensityLabel.ForeColor = Color.FromArgb(61, 73, 89);
            ambientIntensityLabel.Location = new Point(18, 24);
            ambientIntensityLabel.Name = "ambientIntensityLabel";
            ambientIntensityLabel.Size = new Size(164, 17);
            ambientIntensityLabel.TabIndex = 0;
            ambientIntensityLabel.Text = "Intensidade: 18%";
            // 
            // shadingGroupBox
            // 
            shadingGroupBox.Controls.Add(lightColorButton);
            shadingGroupBox.Controls.Add(lightColorLabel);
            shadingGroupBox.Controls.Add(phongShadingRadioButton);
            shadingGroupBox.Controls.Add(gouraudShadingRadioButton);
            shadingGroupBox.Controls.Add(flatShadingRadioButton);
            shadingGroupBox.Controls.Add(showEdgesCheckBox);
            shadingGroupBox.Controls.Add(fillFacesCheckBox);
            shadingGroupBox.Location = new Point(0, 0);
            shadingGroupBox.Name = "shadingGroupBox";
            shadingGroupBox.Size = new Size(346, 120);
            shadingGroupBox.TabIndex = 0;
            shadingGroupBox.TabStop = false;
            shadingGroupBox.Text = "Tonalização";
            // 
            // lightColorButton
            // 
            lightColorButton.BackColor = Color.White;
            lightColorButton.FlatAppearance.BorderColor = Color.FromArgb(130, 142, 156);
            lightColorButton.FlatStyle = FlatStyle.Flat;
            lightColorButton.Location = new Point(104, 82);
            lightColorButton.Name = "lightColorButton";
            lightColorButton.Size = new Size(70, 24);
            lightColorButton.TabIndex = 6;
            lightColorButton.Text = "Cor";
            lightColorButton.UseVisualStyleBackColor = false;
            // 
            // lightColorLabel
            // 
            lightColorLabel.ForeColor = Color.FromArgb(61, 73, 89);
            lightColorLabel.Location = new Point(18, 86);
            lightColorLabel.Name = "lightColorLabel";
            lightColorLabel.Size = new Size(82, 17);
            lightColorLabel.TabIndex = 5;
            lightColorLabel.Text = "Cor da luz";
            // 
            // phongShadingRadioButton
            // 
            phongShadingRadioButton.AutoSize = true;
            phongShadingRadioButton.Checked = true;
            phongShadingRadioButton.Location = new Point(188, 52);
            phongShadingRadioButton.Name = "phongShadingRadioButton";
            phongShadingRadioButton.Size = new Size(60, 19);
            phongShadingRadioButton.TabIndex = 4;
            phongShadingRadioButton.TabStop = true;
            phongShadingRadioButton.Text = "Phong";
            phongShadingRadioButton.UseVisualStyleBackColor = true;
            // 
            // gouraudShadingRadioButton
            // 
            gouraudShadingRadioButton.AutoSize = true;
            gouraudShadingRadioButton.Location = new Point(92, 52);
            gouraudShadingRadioButton.Name = "gouraudShadingRadioButton";
            gouraudShadingRadioButton.Size = new Size(71, 19);
            gouraudShadingRadioButton.TabIndex = 3;
            gouraudShadingRadioButton.Text = "Gouraud";
            gouraudShadingRadioButton.UseVisualStyleBackColor = true;
            // 
            // flatShadingRadioButton
            // 
            flatShadingRadioButton.AutoSize = true;
            flatShadingRadioButton.Location = new Point(18, 52);
            flatShadingRadioButton.Name = "flatShadingRadioButton";
            flatShadingRadioButton.Size = new Size(44, 19);
            flatShadingRadioButton.TabIndex = 2;
            flatShadingRadioButton.Text = "Flat";
            flatShadingRadioButton.UseVisualStyleBackColor = true;
            // 
            // showEdgesCheckBox
            // 
            showEdgesCheckBox.AutoSize = true;
            showEdgesCheckBox.Checked = true;
            showEdgesCheckBox.CheckState = CheckState.Checked;
            showEdgesCheckBox.Location = new Point(150, 22);
            showEdgesCheckBox.Name = "showEdgesCheckBox";
            showEdgesCheckBox.Size = new Size(106, 19);
            showEdgesCheckBox.TabIndex = 1;
            showEdgesCheckBox.Text = "Mostrar arestas";
            showEdgesCheckBox.UseVisualStyleBackColor = true;
            // 
            // fillFacesCheckBox
            // 
            fillFacesCheckBox.AutoSize = true;
            fillFacesCheckBox.Location = new Point(18, 22);
            fillFacesCheckBox.Name = "fillFacesCheckBox";
            fillFacesCheckBox.Size = new Size(109, 19);
            fillFacesCheckBox.TabIndex = 0;
            fillFacesCheckBox.Text = "Preencher faces";
            fillFacesCheckBox.UseVisualStyleBackColor = true;
            // 
            // projectionPagePanel
            // 
            projectionPagePanel.AutoScroll = true;
            projectionPagePanel.Controls.Add(perspectiveProjectionGroupBox);
            projectionPagePanel.Controls.Add(projectionModeGroupBox);
            projectionPagePanel.Controls.Add(orthographicProjectionGroupBox);
            projectionPagePanel.Controls.Add(showProjectionThumbnailsCheckBox);
            projectionPagePanel.Controls.Add(normalProjectionRadioButton);
            projectionPagePanel.Dock = DockStyle.Fill;
            projectionPagePanel.Location = new Point(0, 0);
            projectionPagePanel.Name = "projectionPagePanel";
            projectionPagePanel.Size = new Size(364, 490);
            projectionPagePanel.TabIndex = 1;
            projectionPagePanel.Visible = false;
            // 
            // perspectiveProjectionGroupBox
            // 
            perspectiveProjectionGroupBox.Controls.Add(perspectiveZOffsetTrackBar);
            perspectiveProjectionGroupBox.Controls.Add(perspectiveZOffsetLabel);
            perspectiveProjectionGroupBox.Controls.Add(perspectiveRotationYTrackBar);
            perspectiveProjectionGroupBox.Controls.Add(perspectiveRotationYLabel);
            perspectiveProjectionGroupBox.Controls.Add(perspectiveRotationXTrackBar);
            perspectiveProjectionGroupBox.Controls.Add(perspectiveRotationXLabel);
            perspectiveProjectionGroupBox.Controls.Add(onePointPerspectiveRadioButton);
            perspectiveProjectionGroupBox.Location = new Point(0, 292);
            perspectiveProjectionGroupBox.Name = "perspectiveProjectionGroupBox";
            perspectiveProjectionGroupBox.Size = new Size(346, 178);
            perspectiveProjectionGroupBox.TabIndex = 3;
            perspectiveProjectionGroupBox.TabStop = false;
            perspectiveProjectionGroupBox.Text = "Projeção de perspectiva 1 ponto de fuga";
            // 
            // perspectiveZOffsetTrackBar
            // 
            perspectiveZOffsetTrackBar.AutoSize = false;
            perspectiveZOffsetTrackBar.Location = new Point(18, 140);
            perspectiveZOffsetTrackBar.Maximum = 400;
            perspectiveZOffsetTrackBar.Minimum = 100;
            perspectiveZOffsetTrackBar.Name = "perspectiveZOffsetTrackBar";
            perspectiveZOffsetTrackBar.Size = new Size(310, 28);
            perspectiveZOffsetTrackBar.TabIndex = 6;
            perspectiveZOffsetTrackBar.TickStyle = TickStyle.None;
            perspectiveZOffsetTrackBar.Value = 200;
            // 
            // perspectiveZOffsetLabel
            // 
            perspectiveZOffsetLabel.ForeColor = Color.FromArgb(61, 73, 89);
            perspectiveZOffsetLabel.Location = new Point(18, 120);
            perspectiveZOffsetLabel.Name = "perspectiveZOffsetLabel";
            perspectiveZOffsetLabel.Size = new Size(310, 17);
            perspectiveZOffsetLabel.TabIndex = 5;
            perspectiveZOffsetLabel.Text = "Deslocamento Z: 200";
            // 
            // perspectiveRotationYTrackBar
            // 
            perspectiveRotationYTrackBar.AutoSize = false;
            perspectiveRotationYTrackBar.Location = new Point(18, 92);
            perspectiveRotationYTrackBar.Maximum = 180;
            perspectiveRotationYTrackBar.Minimum = -180;
            perspectiveRotationYTrackBar.Name = "perspectiveRotationYTrackBar";
            perspectiveRotationYTrackBar.Size = new Size(310, 28);
            perspectiveRotationYTrackBar.TabIndex = 4;
            perspectiveRotationYTrackBar.TickStyle = TickStyle.None;
            // 
            // perspectiveRotationYLabel
            // 
            perspectiveRotationYLabel.ForeColor = Color.FromArgb(61, 73, 89);
            perspectiveRotationYLabel.Location = new Point(18, 72);
            perspectiveRotationYLabel.Name = "perspectiveRotationYLabel";
            perspectiveRotationYLabel.Size = new Size(310, 17);
            perspectiveRotationYLabel.TabIndex = 3;
            perspectiveRotationYLabel.Text = "Rotação em Y: 0 graus";
            // 
            // perspectiveRotationXTrackBar
            // 
            perspectiveRotationXTrackBar.AutoSize = false;
            perspectiveRotationXTrackBar.Location = new Point(18, 44);
            perspectiveRotationXTrackBar.Maximum = 180;
            perspectiveRotationXTrackBar.Minimum = -180;
            perspectiveRotationXTrackBar.Name = "perspectiveRotationXTrackBar";
            perspectiveRotationXTrackBar.Size = new Size(310, 28);
            perspectiveRotationXTrackBar.TabIndex = 2;
            perspectiveRotationXTrackBar.TickStyle = TickStyle.None;
            // 
            // perspectiveRotationXLabel
            // 
            perspectiveRotationXLabel.ForeColor = Color.FromArgb(61, 73, 89);
            perspectiveRotationXLabel.Location = new Point(142, 28);
            perspectiveRotationXLabel.Name = "perspectiveRotationXLabel";
            perspectiveRotationXLabel.Size = new Size(186, 17);
            perspectiveRotationXLabel.TabIndex = 1;
            perspectiveRotationXLabel.Text = "Rotação em X: 0 graus";
            // 
            // onePointPerspectiveRadioButton
            // 
            onePointPerspectiveRadioButton.AutoSize = true;
            onePointPerspectiveRadioButton.Location = new Point(18, 27);
            onePointPerspectiveRadioButton.Name = "onePointPerspectiveRadioButton";
            onePointPerspectiveRadioButton.Size = new Size(110, 19);
            onePointPerspectiveRadioButton.TabIndex = 0;
            onePointPerspectiveRadioButton.Text = "Perspectiva 1 PF";
            onePointPerspectiveRadioButton.UseVisualStyleBackColor = true;
            // 
            // projectionModeGroupBox
            // 
            projectionModeGroupBox.Controls.Add(obliqueRotationYTrackBar);
            projectionModeGroupBox.Controls.Add(obliqueRotationYLabel);
            projectionModeGroupBox.Controls.Add(obliqueAlphaTrackBar);
            projectionModeGroupBox.Controls.Add(obliqueAlphaLabel);
            projectionModeGroupBox.Controls.Add(cabinetProjectionRadioButton);
            projectionModeGroupBox.Controls.Add(cavalierProjectionRadioButton);
            projectionModeGroupBox.Location = new Point(0, 126);
            projectionModeGroupBox.Name = "projectionModeGroupBox";
            projectionModeGroupBox.Size = new Size(346, 154);
            projectionModeGroupBox.TabIndex = 2;
            projectionModeGroupBox.TabStop = false;
            projectionModeGroupBox.Text = "Projeções oblíquas";
            // 
            // obliqueRotationYTrackBar
            // 
            obliqueRotationYTrackBar.AutoSize = false;
            obliqueRotationYTrackBar.Location = new Point(18, 120);
            obliqueRotationYTrackBar.Maximum = 180;
            obliqueRotationYTrackBar.Minimum = -180;
            obliqueRotationYTrackBar.Name = "obliqueRotationYTrackBar";
            obliqueRotationYTrackBar.Size = new Size(310, 28);
            obliqueRotationYTrackBar.TabIndex = 5;
            obliqueRotationYTrackBar.TickStyle = TickStyle.None;
            // 
            // obliqueRotationYLabel
            // 
            obliqueRotationYLabel.ForeColor = Color.FromArgb(61, 73, 89);
            obliqueRotationYLabel.Location = new Point(18, 100);
            obliqueRotationYLabel.Name = "obliqueRotationYLabel";
            obliqueRotationYLabel.Size = new Size(310, 17);
            obliqueRotationYLabel.TabIndex = 4;
            obliqueRotationYLabel.Text = "Rotação em Y: 0 graus";
            // 
            // obliqueAlphaTrackBar
            // 
            obliqueAlphaTrackBar.AutoSize = false;
            obliqueAlphaTrackBar.Location = new Point(18, 72);
            obliqueAlphaTrackBar.Maximum = 90;
            obliqueAlphaTrackBar.Name = "obliqueAlphaTrackBar";
            obliqueAlphaTrackBar.Size = new Size(310, 28);
            obliqueAlphaTrackBar.TabIndex = 3;
            obliqueAlphaTrackBar.TickStyle = TickStyle.None;
            obliqueAlphaTrackBar.Value = 45;
            // 
            // obliqueAlphaLabel
            // 
            obliqueAlphaLabel.ForeColor = Color.FromArgb(61, 73, 89);
            obliqueAlphaLabel.Location = new Point(18, 52);
            obliqueAlphaLabel.Name = "obliqueAlphaLabel";
            obliqueAlphaLabel.Size = new Size(310, 17);
            obliqueAlphaLabel.TabIndex = 2;
            obliqueAlphaLabel.Text = "Ângulo alfa: 45 graus";
            // 
            // cabinetProjectionRadioButton
            // 
            cabinetProjectionRadioButton.AutoSize = true;
            cabinetProjectionRadioButton.Location = new Point(120, 27);
            cabinetProjectionRadioButton.Name = "cabinetProjectionRadioButton";
            cabinetProjectionRadioButton.Size = new Size(72, 19);
            cabinetProjectionRadioButton.TabIndex = 1;
            cabinetProjectionRadioButton.Text = "Gabinete";
            cabinetProjectionRadioButton.UseVisualStyleBackColor = true;
            // 
            // cavalierProjectionRadioButton
            // 
            cavalierProjectionRadioButton.AutoSize = true;
            cavalierProjectionRadioButton.Location = new Point(18, 27);
            cavalierProjectionRadioButton.Name = "cavalierProjectionRadioButton";
            cavalierProjectionRadioButton.Size = new Size(73, 19);
            cavalierProjectionRadioButton.TabIndex = 0;
            cavalierProjectionRadioButton.Text = "Cavaleira";
            cavalierProjectionRadioButton.UseVisualStyleBackColor = true;
            // 
            // orthographicProjectionGroupBox
            // 
            orthographicProjectionGroupBox.Controls.Add(lateralProjectionRadioButton);
            orthographicProjectionGroupBox.Controls.Add(superiorProjectionRadioButton);
            orthographicProjectionGroupBox.Controls.Add(frontalProjectionRadioButton);
            orthographicProjectionGroupBox.Location = new Point(0, 34);
            orthographicProjectionGroupBox.Name = "orthographicProjectionGroupBox";
            orthographicProjectionGroupBox.Size = new Size(346, 82);
            orthographicProjectionGroupBox.TabIndex = 1;
            orthographicProjectionGroupBox.TabStop = false;
            orthographicProjectionGroupBox.Text = "Projeções ortográficas";
            // 
            // lateralProjectionRadioButton
            // 
            lateralProjectionRadioButton.AutoSize = true;
            lateralProjectionRadioButton.Location = new Point(18, 51);
            lateralProjectionRadioButton.Name = "lateralProjectionRadioButton";
            lateralProjectionRadioButton.Size = new Size(85, 19);
            lateralProjectionRadioButton.TabIndex = 2;
            lateralProjectionRadioButton.Text = "Lateral (YZ)";
            lateralProjectionRadioButton.UseVisualStyleBackColor = true;
            // 
            // superiorProjectionRadioButton
            // 
            superiorProjectionRadioButton.AutoSize = true;
            superiorProjectionRadioButton.Location = new Point(140, 24);
            superiorProjectionRadioButton.Name = "superiorProjectionRadioButton";
            superiorProjectionRadioButton.Size = new Size(94, 19);
            superiorProjectionRadioButton.TabIndex = 1;
            superiorProjectionRadioButton.Text = "Superior (XZ)";
            superiorProjectionRadioButton.UseVisualStyleBackColor = true;
            // 
            // frontalProjectionRadioButton
            // 
            frontalProjectionRadioButton.AutoSize = true;
            frontalProjectionRadioButton.Location = new Point(18, 24);
            frontalProjectionRadioButton.Name = "frontalProjectionRadioButton";
            frontalProjectionRadioButton.Size = new Size(87, 19);
            frontalProjectionRadioButton.TabIndex = 0;
            frontalProjectionRadioButton.Text = "Frontal (XY)";
            frontalProjectionRadioButton.UseVisualStyleBackColor = true;
            // 
            // showProjectionThumbnailsCheckBox
            // 
            showProjectionThumbnailsCheckBox.AutoSize = true;
            showProjectionThumbnailsCheckBox.Location = new Point(190, 8);
            showProjectionThumbnailsCheckBox.Name = "showProjectionThumbnailsCheckBox";
            showProjectionThumbnailsCheckBox.Size = new Size(126, 19);
            showProjectionThumbnailsCheckBox.TabIndex = 4;
            showProjectionThumbnailsCheckBox.Text = "Mostrar miniaturas";
            showProjectionThumbnailsCheckBox.UseVisualStyleBackColor = true;
            // 
            // normalProjectionRadioButton
            // 
            normalProjectionRadioButton.AutoSize = true;
            normalProjectionRadioButton.Checked = true;
            normalProjectionRadioButton.Location = new Point(4, 8);
            normalProjectionRadioButton.Name = "normalProjectionRadioButton";
            normalProjectionRadioButton.Size = new Size(97, 19);
            normalProjectionRadioButton.TabIndex = 0;
            normalProjectionRadioButton.TabStop = true;
            normalProjectionRadioButton.Text = "Normal (XYZ)";
            normalProjectionRadioButton.UseVisualStyleBackColor = true;
            // 
            // transformPagePanel
            // 
            transformPagePanel.AutoScroll = true;
            transformPagePanel.Controls.Add(scaleGroupBox);
            transformPagePanel.Controls.Add(translationGroupBox);
            transformPagePanel.Controls.Add(rotationGroupBox);
            transformPagePanel.Dock = DockStyle.Fill;
            transformPagePanel.Location = new Point(0, 0);
            transformPagePanel.Name = "transformPagePanel";
            transformPagePanel.Size = new Size(364, 490);
            transformPagePanel.TabIndex = 0;
            // 
            // scaleGroupBox
            // 
            scaleGroupBox.Controls.Add(scaleHintLabel);
            scaleGroupBox.Controls.Add(scaleAllCheckBox);
            scaleGroupBox.Controls.Add(scaleZCheckBox);
            scaleGroupBox.Controls.Add(scaleYCheckBox);
            scaleGroupBox.Controls.Add(scaleXCheckBox);
            scaleGroupBox.Location = new Point(0, 176);
            scaleGroupBox.Name = "scaleGroupBox";
            scaleGroupBox.Size = new Size(346, 76);
            scaleGroupBox.TabIndex = 2;
            scaleGroupBox.TabStop = false;
            scaleGroupBox.Text = "Escala no scroll";
            // 
            // scaleHintLabel
            // 
            scaleHintLabel.ForeColor = Color.FromArgb(96, 106, 118);
            scaleHintLabel.Location = new Point(18, 47);
            scaleHintLabel.Name = "scaleHintLabel";
            scaleHintLabel.Size = new Size(310, 15);
            scaleHintLabel.TabIndex = 4;
            scaleHintLabel.Text = "Marque vários eixos ou XYZ para todos.";
            // 
            // scaleAllCheckBox
            // 
            scaleAllCheckBox.AutoSize = true;
            scaleAllCheckBox.Checked = true;
            scaleAllCheckBox.CheckState = CheckState.Checked;
            scaleAllCheckBox.Location = new Point(186, 23);
            scaleAllCheckBox.Name = "scaleAllCheckBox";
            scaleAllCheckBox.Size = new Size(47, 19);
            scaleAllCheckBox.TabIndex = 3;
            scaleAllCheckBox.Text = "XYZ";
            scaleAllCheckBox.UseVisualStyleBackColor = true;
            // 
            // scaleZCheckBox
            // 
            scaleZCheckBox.AutoSize = true;
            scaleZCheckBox.Checked = true;
            scaleZCheckBox.CheckState = CheckState.Checked;
            scaleZCheckBox.Location = new Point(130, 23);
            scaleZCheckBox.Name = "scaleZCheckBox";
            scaleZCheckBox.Size = new Size(33, 19);
            scaleZCheckBox.TabIndex = 2;
            scaleZCheckBox.Text = "Z";
            scaleZCheckBox.UseVisualStyleBackColor = true;
            // 
            // scaleYCheckBox
            // 
            scaleYCheckBox.AutoSize = true;
            scaleYCheckBox.Checked = true;
            scaleYCheckBox.CheckState = CheckState.Checked;
            scaleYCheckBox.Location = new Point(74, 23);
            scaleYCheckBox.Name = "scaleYCheckBox";
            scaleYCheckBox.Size = new Size(33, 19);
            scaleYCheckBox.TabIndex = 1;
            scaleYCheckBox.Text = "Y";
            scaleYCheckBox.UseVisualStyleBackColor = true;
            // 
            // scaleXCheckBox
            // 
            scaleXCheckBox.AutoSize = true;
            scaleXCheckBox.Checked = true;
            scaleXCheckBox.CheckState = CheckState.Checked;
            scaleXCheckBox.Location = new Point(18, 23);
            scaleXCheckBox.Name = "scaleXCheckBox";
            scaleXCheckBox.Size = new Size(33, 19);
            scaleXCheckBox.TabIndex = 0;
            scaleXCheckBox.Text = "X";
            scaleXCheckBox.UseVisualStyleBackColor = true;
            // 
            // translationGroupBox
            // 
            translationGroupBox.Controls.Add(translationHintLabel);
            translationGroupBox.Controls.Add(translationAllCheckBox);
            translationGroupBox.Controls.Add(translationZCheckBox);
            translationGroupBox.Controls.Add(translationYCheckBox);
            translationGroupBox.Controls.Add(translationXCheckBox);
            translationGroupBox.Location = new Point(0, 88);
            translationGroupBox.Name = "translationGroupBox";
            translationGroupBox.Size = new Size(346, 80);
            translationGroupBox.TabIndex = 1;
            translationGroupBox.TabStop = false;
            translationGroupBox.Text = "Translação no botão direito";
            // 
            // translationHintLabel
            // 
            translationHintLabel.ForeColor = Color.FromArgb(96, 106, 118);
            translationHintLabel.Location = new Point(18, 49);
            translationHintLabel.Name = "translationHintLabel";
            translationHintLabel.Size = new Size(310, 15);
            translationHintLabel.TabIndex = 4;
            translationHintLabel.Text = "Marque vários eixos ou XYZ para todos.";
            // 
            // translationAllCheckBox
            // 
            translationAllCheckBox.AutoSize = true;
            translationAllCheckBox.Location = new Point(186, 24);
            translationAllCheckBox.Name = "translationAllCheckBox";
            translationAllCheckBox.Size = new Size(47, 19);
            translationAllCheckBox.TabIndex = 3;
            translationAllCheckBox.Text = "XYZ";
            translationAllCheckBox.UseVisualStyleBackColor = true;
            // 
            // translationZCheckBox
            // 
            translationZCheckBox.AutoSize = true;
            translationZCheckBox.Location = new Point(130, 24);
            translationZCheckBox.Name = "translationZCheckBox";
            translationZCheckBox.Size = new Size(33, 19);
            translationZCheckBox.TabIndex = 2;
            translationZCheckBox.Text = "Z";
            translationZCheckBox.UseVisualStyleBackColor = true;
            // 
            // translationYCheckBox
            // 
            translationYCheckBox.AutoSize = true;
            translationYCheckBox.Checked = true;
            translationYCheckBox.CheckState = CheckState.Checked;
            translationYCheckBox.Location = new Point(74, 24);
            translationYCheckBox.Name = "translationYCheckBox";
            translationYCheckBox.Size = new Size(33, 19);
            translationYCheckBox.TabIndex = 1;
            translationYCheckBox.Text = "Y";
            translationYCheckBox.UseVisualStyleBackColor = true;
            // 
            // translationXCheckBox
            // 
            translationXCheckBox.AutoSize = true;
            translationXCheckBox.Checked = true;
            translationXCheckBox.CheckState = CheckState.Checked;
            translationXCheckBox.Location = new Point(18, 24);
            translationXCheckBox.Name = "translationXCheckBox";
            translationXCheckBox.Size = new Size(33, 19);
            translationXCheckBox.TabIndex = 0;
            translationXCheckBox.Text = "X";
            translationXCheckBox.UseVisualStyleBackColor = true;
            // 
            // rotationGroupBox
            // 
            rotationGroupBox.Controls.Add(rotationHintLabel);
            rotationGroupBox.Controls.Add(rotationAllCheckBox);
            rotationGroupBox.Controls.Add(rotationZCheckBox);
            rotationGroupBox.Controls.Add(rotationYCheckBox);
            rotationGroupBox.Controls.Add(rotationXCheckBox);
            rotationGroupBox.Location = new Point(0, 0);
            rotationGroupBox.Name = "rotationGroupBox";
            rotationGroupBox.Size = new Size(346, 80);
            rotationGroupBox.TabIndex = 0;
            rotationGroupBox.TabStop = false;
            rotationGroupBox.Text = "Rotação no botão esquerdo";
            // 
            // rotationHintLabel
            // 
            rotationHintLabel.ForeColor = Color.FromArgb(96, 106, 118);
            rotationHintLabel.Location = new Point(18, 49);
            rotationHintLabel.Name = "rotationHintLabel";
            rotationHintLabel.Size = new Size(310, 15);
            rotationHintLabel.TabIndex = 4;
            rotationHintLabel.Text = "Marque vários eixos ou XYZ para todos.";
            // 
            // rotationAllCheckBox
            // 
            rotationAllCheckBox.AutoSize = true;
            rotationAllCheckBox.Location = new Point(186, 24);
            rotationAllCheckBox.Name = "rotationAllCheckBox";
            rotationAllCheckBox.Size = new Size(47, 19);
            rotationAllCheckBox.TabIndex = 3;
            rotationAllCheckBox.Text = "XYZ";
            rotationAllCheckBox.UseVisualStyleBackColor = true;
            // 
            // rotationZCheckBox
            // 
            rotationZCheckBox.AutoSize = true;
            rotationZCheckBox.Location = new Point(130, 24);
            rotationZCheckBox.Name = "rotationZCheckBox";
            rotationZCheckBox.Size = new Size(33, 19);
            rotationZCheckBox.TabIndex = 2;
            rotationZCheckBox.Text = "Z";
            rotationZCheckBox.UseVisualStyleBackColor = true;
            // 
            // rotationYCheckBox
            // 
            rotationYCheckBox.AutoSize = true;
            rotationYCheckBox.Checked = true;
            rotationYCheckBox.CheckState = CheckState.Checked;
            rotationYCheckBox.Location = new Point(74, 24);
            rotationYCheckBox.Name = "rotationYCheckBox";
            rotationYCheckBox.Size = new Size(33, 19);
            rotationYCheckBox.TabIndex = 1;
            rotationYCheckBox.Text = "Y";
            rotationYCheckBox.UseVisualStyleBackColor = true;
            // 
            // rotationXCheckBox
            // 
            rotationXCheckBox.AutoSize = true;
            rotationXCheckBox.Checked = true;
            rotationXCheckBox.CheckState = CheckState.Checked;
            rotationXCheckBox.Location = new Point(18, 24);
            rotationXCheckBox.Name = "rotationXCheckBox";
            rotationXCheckBox.Size = new Size(33, 19);
            rotationXCheckBox.TabIndex = 0;
            rotationXCheckBox.Text = "X";
            rotationXCheckBox.UseVisualStyleBackColor = true;
            // 
            // renderGroupBox
            // 
            renderGroupBox.Controls.Add(showBackFacesCheckBox);
            renderGroupBox.Location = new Point(18, 164);
            renderGroupBox.Name = "renderGroupBox";
            renderGroupBox.Size = new Size(364, 50);
            renderGroupBox.TabIndex = 6;
            renderGroupBox.TabStop = false;
            renderGroupBox.Text = "Faces";
            // 
            // showBackFacesCheckBox
            // 
            showBackFacesCheckBox.AutoSize = true;
            showBackFacesCheckBox.Location = new Point(18, 22);
            showBackFacesCheckBox.Name = "showBackFacesCheckBox";
            showBackFacesCheckBox.Size = new Size(143, 19);
            showBackFacesCheckBox.TabIndex = 1;
            showBackFacesCheckBox.Text = "Mostrar faces traseiras";
            showBackFacesCheckBox.UseVisualStyleBackColor = true;
            // 
            // lightingViewButton
            // 
            lightingViewButton.FlatStyle = FlatStyle.Flat;
            lightingViewButton.Location = new Point(266, 124);
            lightingViewButton.Name = "lightingViewButton";
            lightingViewButton.Size = new Size(116, 30);
            lightingViewButton.TabIndex = 6;
            lightingViewButton.Text = "Iluminação";
            lightingViewButton.UseVisualStyleBackColor = true;
            // 
            // projectionViewButton
            // 
            projectionViewButton.FlatStyle = FlatStyle.Flat;
            projectionViewButton.Location = new Point(142, 124);
            projectionViewButton.Name = "projectionViewButton";
            projectionViewButton.Size = new Size(116, 30);
            projectionViewButton.TabIndex = 5;
            projectionViewButton.Text = "Projeção";
            projectionViewButton.UseVisualStyleBackColor = true;
            // 
            // transformViewButton
            // 
            transformViewButton.FlatStyle = FlatStyle.Flat;
            transformViewButton.Location = new Point(18, 124);
            transformViewButton.Name = "transformViewButton";
            transformViewButton.Size = new Size(116, 30);
            transformViewButton.TabIndex = 4;
            transformViewButton.Text = "Transformação";
            transformViewButton.UseVisualStyleBackColor = true;
            // 
            // resetViewButton
            // 
            resetViewButton.Location = new Point(18, 84);
            resetViewButton.Name = "resetViewButton";
            resetViewButton.Size = new Size(364, 32);
            resetViewButton.TabIndex = 3;
            resetViewButton.Text = "Resetar";
            resetViewButton.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            clearButton.Location = new Point(206, 46);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(176, 30);
            clearButton.TabIndex = 2;
            clearButton.Text = "Limpar";
            clearButton.UseVisualStyleBackColor = true;
            // 
            // openButton
            // 
            openButton.Location = new Point(18, 46);
            openButton.Name = "openButton";
            openButton.Size = new Size(176, 30);
            openButton.TabIndex = 1;
            openButton.Text = "Abrir .obj";
            openButton.UseVisualStyleBackColor = true;
            // 
            // sidebarTitleLabel
            // 
            sidebarTitleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            sidebarTitleLabel.ForeColor = Color.FromArgb(33, 43, 55);
            sidebarTitleLabel.Location = new Point(18, 18);
            sidebarTitleLabel.Name = "sidebarTitleLabel";
            sidebarTitleLabel.Size = new Size(364, 28);
            sidebarTitleLabel.TabIndex = 0;
            sidebarTitleLabel.Text = "Ferramentas 3D";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1350, 749);
            Controls.Add(viewportGroupBox);
            Controls.Add(sidebarPanel);
            MinimumSize = new Size(980, 718);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "3D Graphics Editor";
            viewportGroupBox.ResumeLayout(false);
            viewportPanel.ResumeLayout(false);
            viewportPanel.PerformLayout();
            sidebarPanel.ResumeLayout(false);
            modeContentPanel.ResumeLayout(false);
            lightingPagePanel.ResumeLayout(false);
            lightingComponentGroupBox.ResumeLayout(false);
            lightingComponentGroupBox.PerformLayout();
            lightPositionGroupBox.ResumeLayout(false);
            lightPositionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)lightZTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)lightYTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)lightXTrackBar).EndInit();
            specularLightingGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)shininessTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)specularIntensityTrackBar).EndInit();
            diffuseLightingGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)diffuseIntensityTrackBar).EndInit();
            ambientLightingGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ambientIntensityTrackBar).EndInit();
            shadingGroupBox.ResumeLayout(false);
            shadingGroupBox.PerformLayout();
            projectionPagePanel.ResumeLayout(false);
            projectionPagePanel.PerformLayout();
            perspectiveProjectionGroupBox.ResumeLayout(false);
            perspectiveProjectionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)perspectiveZOffsetTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)perspectiveRotationYTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)perspectiveRotationXTrackBar).EndInit();
            projectionModeGroupBox.ResumeLayout(false);
            projectionModeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)obliqueRotationYTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)obliqueAlphaTrackBar).EndInit();
            orthographicProjectionGroupBox.ResumeLayout(false);
            orthographicProjectionGroupBox.PerformLayout();
            transformPagePanel.ResumeLayout(false);
            scaleGroupBox.ResumeLayout(false);
            scaleGroupBox.PerformLayout();
            translationGroupBox.ResumeLayout(false);
            translationGroupBox.PerformLayout();
            rotationGroupBox.ResumeLayout(false);
            rotationGroupBox.PerformLayout();
            renderGroupBox.ResumeLayout(false);
            renderGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
