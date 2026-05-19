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
            components = new System.ComponentModel.Container();
            viewportGroupBox = new GroupBox();
            viewportPanel = new Rendering.DoubleBufferedPanel();
            sidebarPanel = new Panel();
            modeContentPanel = new Panel();
            projectionPagePanel = new Panel();
            orthographicProjectionGroupBox = new GroupBox();
            frontalProjectionRadioButton = new RadioButton();
            superiorProjectionRadioButton = new RadioButton();
            lateralProjectionRadioButton = new RadioButton();
            projectionModeGroupBox = new GroupBox();
            normalProjectionRadioButton = new RadioButton();
            showProjectionThumbnailsCheckBox = new CheckBox();
            onePointPerspectiveRadioButton = new RadioButton();
            cabinetProjectionRadioButton = new RadioButton();
            cavalierProjectionRadioButton = new RadioButton();
            obliqueAlphaLabel = new Label();
            obliqueAlphaTrackBar = new TrackBar();
            obliqueRotationYLabel = new Label();
            obliqueRotationYTrackBar = new TrackBar();
            perspectiveProjectionGroupBox = new GroupBox();
            perspectiveZOffsetLabel = new Label();
            perspectiveZOffsetTrackBar = new TrackBar();
            perspectiveRotationYLabel = new Label();
            perspectiveRotationYTrackBar = new TrackBar();
            perspectiveRotationXLabel = new Label();
            perspectiveRotationXTrackBar = new TrackBar();
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
            fillFacesCheckBox = new CheckBox();
            projectionViewButton = new Button();
            transformViewButton = new Button();
            resetViewButton = new Button();
            clearButton = new Button();
            openButton = new Button();
            sidebarTitleLabel = new Label();
            viewportGroupBox.SuspendLayout();
            sidebarPanel.SuspendLayout();
            modeContentPanel.SuspendLayout();
            projectionPagePanel.SuspendLayout();
            orthographicProjectionGroupBox.SuspendLayout();
            projectionModeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)obliqueAlphaTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)obliqueRotationYTrackBar).BeginInit();
            perspectiveProjectionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)perspectiveZOffsetTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)perspectiveRotationYTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)perspectiveRotationXTrackBar).BeginInit();
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
            viewportGroupBox.Size = new Size(1038, 753);
            viewportGroupBox.TabIndex = 0;
            viewportGroupBox.TabStop = false;
            viewportGroupBox.Text = "Visualizacao 3D";
            //
            // viewportPanel
            //
            viewportPanel.BackColor = Color.White;
            viewportPanel.Cursor = Cursors.Default;
            viewportPanel.Dock = DockStyle.Fill;
            viewportPanel.Location = new Point(14, 32);
            viewportPanel.Name = "viewportPanel";
            viewportPanel.Size = new Size(1010, 707);
            viewportPanel.TabIndex = 0;
            viewportPanel.TabStop = true;
            //
            // sidebarPanel
            //
            sidebarPanel.BackColor = Color.FromArgb(242, 245, 248);
            sidebarPanel.Controls.Add(modeContentPanel);
            sidebarPanel.Controls.Add(renderGroupBox);
            sidebarPanel.Controls.Add(projectionViewButton);
            sidebarPanel.Controls.Add(transformViewButton);
            sidebarPanel.Controls.Add(resetViewButton);
            sidebarPanel.Controls.Add(clearButton);
            sidebarPanel.Controls.Add(openButton);
            sidebarPanel.Controls.Add(sidebarTitleLabel);
            sidebarPanel.Dock = DockStyle.Right;
            sidebarPanel.Location = new Point(1038, 0);
            sidebarPanel.Name = "sidebarPanel";
            sidebarPanel.Padding = new Padding(18);
            sidebarPanel.Size = new Size(312, 753);
            sidebarPanel.TabIndex = 1;
            //
            // modeContentPanel
            //
            modeContentPanel.Controls.Add(projectionPagePanel);
            modeContentPanel.Controls.Add(transformPagePanel);
            modeContentPanel.Location = new Point(18, 250);
            modeContentPanel.Name = "modeContentPanel";
            modeContentPanel.Size = new Size(276, 470);
            modeContentPanel.TabIndex = 7;
            //
            // projectionPagePanel
            //
            projectionPagePanel.Controls.Add(perspectiveProjectionGroupBox);
            projectionPagePanel.Controls.Add(projectionModeGroupBox);
            projectionPagePanel.Controls.Add(orthographicProjectionGroupBox);
            projectionPagePanel.Controls.Add(showProjectionThumbnailsCheckBox);
            projectionPagePanel.Controls.Add(normalProjectionRadioButton);
            projectionPagePanel.Location = new Point(0, 0);
            projectionPagePanel.Name = "projectionPagePanel";
            projectionPagePanel.Size = new Size(276, 470);
            projectionPagePanel.TabIndex = 1;
            projectionPagePanel.Visible = false;
            //
            // orthographicProjectionGroupBox
            //
            orthographicProjectionGroupBox.Controls.Add(lateralProjectionRadioButton);
            orthographicProjectionGroupBox.Controls.Add(superiorProjectionRadioButton);
            orthographicProjectionGroupBox.Controls.Add(frontalProjectionRadioButton);
            orthographicProjectionGroupBox.Location = new Point(0, 34);
            orthographicProjectionGroupBox.Name = "orthographicProjectionGroupBox";
            orthographicProjectionGroupBox.Size = new Size(276, 82);
            orthographicProjectionGroupBox.TabIndex = 1;
            orthographicProjectionGroupBox.TabStop = false;
            orthographicProjectionGroupBox.Text = "Projecoes ortograficas";
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
            superiorProjectionRadioButton.Size = new Size(96, 19);
            superiorProjectionRadioButton.TabIndex = 1;
            superiorProjectionRadioButton.Text = "Superior (XZ)";
            superiorProjectionRadioButton.UseVisualStyleBackColor = true;
            //
            // frontalProjectionRadioButton
            //
            frontalProjectionRadioButton.AutoSize = true;
            frontalProjectionRadioButton.Location = new Point(18, 24);
            frontalProjectionRadioButton.Name = "frontalProjectionRadioButton";
            frontalProjectionRadioButton.Size = new Size(90, 19);
            frontalProjectionRadioButton.TabIndex = 0;
            frontalProjectionRadioButton.Text = "Frontal (XY)";
            frontalProjectionRadioButton.UseVisualStyleBackColor = true;
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
            projectionModeGroupBox.Size = new Size(276, 154);
            projectionModeGroupBox.TabIndex = 2;
            projectionModeGroupBox.TabStop = false;
            projectionModeGroupBox.Text = "Projecoes obliquas";
            //
            // normalProjectionRadioButton
            //
            normalProjectionRadioButton.AutoSize = true;
            normalProjectionRadioButton.Checked = true;
            normalProjectionRadioButton.Location = new Point(4, 8);
            normalProjectionRadioButton.Name = "normalProjectionRadioButton";
            normalProjectionRadioButton.Size = new Size(95, 19);
            normalProjectionRadioButton.TabIndex = 0;
            normalProjectionRadioButton.TabStop = true;
            normalProjectionRadioButton.Text = "Normal (XYZ)";
            normalProjectionRadioButton.UseVisualStyleBackColor = true;
            //
            // showProjectionThumbnailsCheckBox
            //
            showProjectionThumbnailsCheckBox.AutoSize = true;
            showProjectionThumbnailsCheckBox.Location = new Point(130, 8);
            showProjectionThumbnailsCheckBox.Name = "showProjectionThumbnailsCheckBox";
            showProjectionThumbnailsCheckBox.Size = new Size(130, 19);
            showProjectionThumbnailsCheckBox.TabIndex = 4;
            showProjectionThumbnailsCheckBox.Text = "Mostrar miniaturas";
            showProjectionThumbnailsCheckBox.UseVisualStyleBackColor = true;
            //
            // obliqueRotationYTrackBar
            //
            obliqueRotationYTrackBar.AutoSize = false;
            obliqueRotationYTrackBar.Location = new Point(18, 120);
            obliqueRotationYTrackBar.Maximum = 180;
            obliqueRotationYTrackBar.Minimum = -180;
            obliqueRotationYTrackBar.Name = "obliqueRotationYTrackBar";
            obliqueRotationYTrackBar.Size = new Size(240, 28);
            obliqueRotationYTrackBar.TabIndex = 5;
            obliqueRotationYTrackBar.TickStyle = TickStyle.None;
            //
            // obliqueRotationYLabel
            //
            obliqueRotationYLabel.ForeColor = Color.FromArgb(61, 73, 89);
            obliqueRotationYLabel.Location = new Point(18, 100);
            obliqueRotationYLabel.Name = "obliqueRotationYLabel";
            obliqueRotationYLabel.Size = new Size(240, 17);
            obliqueRotationYLabel.TabIndex = 4;
            obliqueRotationYLabel.Text = "Rotacao em Y: 0 deg";
            //
            // obliqueAlphaTrackBar
            //
            obliqueAlphaTrackBar.AutoSize = false;
            obliqueAlphaTrackBar.Location = new Point(18, 72);
            obliqueAlphaTrackBar.Maximum = 90;
            obliqueAlphaTrackBar.Minimum = 0;
            obliqueAlphaTrackBar.Name = "obliqueAlphaTrackBar";
            obliqueAlphaTrackBar.Size = new Size(240, 28);
            obliqueAlphaTrackBar.TabIndex = 3;
            obliqueAlphaTrackBar.TickStyle = TickStyle.None;
            obliqueAlphaTrackBar.Value = 45;
            //
            // obliqueAlphaLabel
            //
            obliqueAlphaLabel.ForeColor = Color.FromArgb(61, 73, 89);
            obliqueAlphaLabel.Location = new Point(18, 52);
            obliqueAlphaLabel.Name = "obliqueAlphaLabel";
            obliqueAlphaLabel.Size = new Size(240, 17);
            obliqueAlphaLabel.TabIndex = 2;
            obliqueAlphaLabel.Text = "Angulo alfa: 45 deg";
            //
            // onePointPerspectiveRadioButton
            //
            onePointPerspectiveRadioButton.AutoSize = true;
            onePointPerspectiveRadioButton.Location = new Point(18, 27);
            onePointPerspectiveRadioButton.Name = "onePointPerspectiveRadioButton";
            onePointPerspectiveRadioButton.Size = new Size(114, 19);
            onePointPerspectiveRadioButton.TabIndex = 0;
            onePointPerspectiveRadioButton.Text = "Perspectiva 1 PF";
            onePointPerspectiveRadioButton.UseVisualStyleBackColor = true;
            //
            // cabinetProjectionRadioButton
            //
            cabinetProjectionRadioButton.AutoSize = true;
            cabinetProjectionRadioButton.Location = new Point(120, 27);
            cabinetProjectionRadioButton.Name = "cabinetProjectionRadioButton";
            cabinetProjectionRadioButton.Size = new Size(76, 19);
            cabinetProjectionRadioButton.TabIndex = 1;
            cabinetProjectionRadioButton.Text = "Gabinete";
            cabinetProjectionRadioButton.UseVisualStyleBackColor = true;
            //
            // cavalierProjectionRadioButton
            //
            cavalierProjectionRadioButton.AutoSize = true;
            cavalierProjectionRadioButton.Location = new Point(18, 27);
            cavalierProjectionRadioButton.Name = "cavalierProjectionRadioButton";
            cavalierProjectionRadioButton.Size = new Size(78, 19);
            cavalierProjectionRadioButton.TabIndex = 0;
            cavalierProjectionRadioButton.Text = "Cavaleira";
            cavalierProjectionRadioButton.UseVisualStyleBackColor = true;
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
            perspectiveProjectionGroupBox.Size = new Size(276, 178);
            perspectiveProjectionGroupBox.TabIndex = 3;
            perspectiveProjectionGroupBox.TabStop = false;
            perspectiveProjectionGroupBox.Text = "Projecao de perspectiva 1 ponto de fuga";
            //
            // perspectiveZOffsetTrackBar
            //
            perspectiveZOffsetTrackBar.AutoSize = false;
            perspectiveZOffsetTrackBar.Location = new Point(18, 140);
            perspectiveZOffsetTrackBar.Maximum = 400;
            perspectiveZOffsetTrackBar.Minimum = 100;
            perspectiveZOffsetTrackBar.Name = "perspectiveZOffsetTrackBar";
            perspectiveZOffsetTrackBar.Size = new Size(240, 28);
            perspectiveZOffsetTrackBar.TabIndex = 6;
            perspectiveZOffsetTrackBar.TickStyle = TickStyle.None;
            perspectiveZOffsetTrackBar.Value = 200;
            //
            // perspectiveZOffsetLabel
            //
            perspectiveZOffsetLabel.ForeColor = Color.FromArgb(61, 73, 89);
            perspectiveZOffsetLabel.Location = new Point(18, 120);
            perspectiveZOffsetLabel.Name = "perspectiveZOffsetLabel";
            perspectiveZOffsetLabel.Size = new Size(240, 17);
            perspectiveZOffsetLabel.TabIndex = 5;
            perspectiveZOffsetLabel.Text = "Z offset: 200";
            //
            // perspectiveRotationYTrackBar
            //
            perspectiveRotationYTrackBar.AutoSize = false;
            perspectiveRotationYTrackBar.Location = new Point(18, 92);
            perspectiveRotationYTrackBar.Maximum = 180;
            perspectiveRotationYTrackBar.Minimum = -180;
            perspectiveRotationYTrackBar.Name = "perspectiveRotationYTrackBar";
            perspectiveRotationYTrackBar.Size = new Size(240, 28);
            perspectiveRotationYTrackBar.TabIndex = 4;
            perspectiveRotationYTrackBar.TickStyle = TickStyle.None;
            //
            // perspectiveRotationYLabel
            //
            perspectiveRotationYLabel.ForeColor = Color.FromArgb(61, 73, 89);
            perspectiveRotationYLabel.Location = new Point(18, 72);
            perspectiveRotationYLabel.Name = "perspectiveRotationYLabel";
            perspectiveRotationYLabel.Size = new Size(240, 17);
            perspectiveRotationYLabel.TabIndex = 3;
            perspectiveRotationYLabel.Text = "Rotacao em Y: 0 deg";
            //
            // perspectiveRotationXTrackBar
            //
            perspectiveRotationXTrackBar.AutoSize = false;
            perspectiveRotationXTrackBar.Location = new Point(18, 44);
            perspectiveRotationXTrackBar.Maximum = 180;
            perspectiveRotationXTrackBar.Minimum = -180;
            perspectiveRotationXTrackBar.Name = "perspectiveRotationXTrackBar";
            perspectiveRotationXTrackBar.Size = new Size(240, 28);
            perspectiveRotationXTrackBar.TabIndex = 2;
            perspectiveRotationXTrackBar.TickStyle = TickStyle.None;
            //
            // perspectiveRotationXLabel
            //
            perspectiveRotationXLabel.ForeColor = Color.FromArgb(61, 73, 89);
            perspectiveRotationXLabel.Location = new Point(142, 28);
            perspectiveRotationXLabel.Name = "perspectiveRotationXLabel";
            perspectiveRotationXLabel.Size = new Size(240, 17);
            perspectiveRotationXLabel.TabIndex = 1;
            perspectiveRotationXLabel.Text = "Rotacao em X: 0 deg";
            //
            // transformPagePanel
            //
            transformPagePanel.Controls.Add(scaleGroupBox);
            transformPagePanel.Controls.Add(translationGroupBox);
            transformPagePanel.Controls.Add(rotationGroupBox);
            transformPagePanel.Location = new Point(0, 0);
            transformPagePanel.Name = "transformPagePanel";
            transformPagePanel.Size = new Size(276, 470);
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
            scaleGroupBox.Size = new Size(276, 76);
            scaleGroupBox.TabIndex = 2;
            scaleGroupBox.TabStop = false;
            scaleGroupBox.Text = "Escala no scroll";
            //
            // scaleHintLabel
            //
            scaleHintLabel.ForeColor = Color.FromArgb(96, 106, 118);
            scaleHintLabel.Location = new Point(18, 47);
            scaleHintLabel.Name = "scaleHintLabel";
            scaleHintLabel.Size = new Size(243, 15);
            scaleHintLabel.TabIndex = 4;
            scaleHintLabel.Text = "Marque varios eixos ou XYZ para todos.";
            //
            // scaleAllCheckBox
            //
            scaleAllCheckBox.AutoSize = true;
            scaleAllCheckBox.Checked = true;
            scaleAllCheckBox.CheckState = CheckState.Checked;
            scaleAllCheckBox.Location = new Point(186, 23);
            scaleAllCheckBox.Name = "scaleAllCheckBox";
            scaleAllCheckBox.Size = new Size(46, 19);
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
            translationGroupBox.Size = new Size(276, 80);
            translationGroupBox.TabIndex = 1;
            translationGroupBox.TabStop = false;
            translationGroupBox.Text = "Translacao no botao direito";
            //
            // translationHintLabel
            //
            translationHintLabel.ForeColor = Color.FromArgb(96, 106, 118);
            translationHintLabel.Location = new Point(18, 49);
            translationHintLabel.Name = "translationHintLabel";
            translationHintLabel.Size = new Size(243, 15);
            translationHintLabel.TabIndex = 4;
            translationHintLabel.Text = "Marque varios eixos ou XYZ para todos.";
            //
            // translationAllCheckBox
            //
            translationAllCheckBox.AutoSize = true;
            translationAllCheckBox.Location = new Point(186, 24);
            translationAllCheckBox.Name = "translationAllCheckBox";
            translationAllCheckBox.Size = new Size(46, 19);
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
            rotationGroupBox.Size = new Size(276, 80);
            rotationGroupBox.TabIndex = 0;
            rotationGroupBox.TabStop = false;
            rotationGroupBox.Text = "Rotacao no botao esquerdo";
            //
            // rotationHintLabel
            //
            rotationHintLabel.ForeColor = Color.FromArgb(96, 106, 118);
            rotationHintLabel.Location = new Point(18, 49);
            rotationHintLabel.Name = "rotationHintLabel";
            rotationHintLabel.Size = new Size(243, 15);
            rotationHintLabel.TabIndex = 4;
            rotationHintLabel.Text = "Marque varios eixos ou XYZ para todos.";
            //
            // rotationAllCheckBox
            //
            rotationAllCheckBox.AutoSize = true;
            rotationAllCheckBox.Location = new Point(186, 24);
            rotationAllCheckBox.Name = "rotationAllCheckBox";
            rotationAllCheckBox.Size = new Size(46, 19);
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
            renderGroupBox.Controls.Add(fillFacesCheckBox);
            renderGroupBox.Location = new Point(18, 164);
            renderGroupBox.Name = "renderGroupBox";
            renderGroupBox.Size = new Size(276, 72);
            renderGroupBox.TabIndex = 6;
            renderGroupBox.TabStop = false;
            renderGroupBox.Text = "Visualizacao";
            //
            // showBackFacesCheckBox
            //
            showBackFacesCheckBox.AutoSize = true;
            showBackFacesCheckBox.Location = new Point(18, 44);
            showBackFacesCheckBox.Name = "showBackFacesCheckBox";
            showBackFacesCheckBox.Size = new Size(145, 19);
            showBackFacesCheckBox.TabIndex = 1;
            showBackFacesCheckBox.Text = "Mostrar faces traseiras";
            showBackFacesCheckBox.UseVisualStyleBackColor = true;
            //
            // fillFacesCheckBox
            //
            fillFacesCheckBox.AutoSize = true;
            fillFacesCheckBox.Checked = true;
            fillFacesCheckBox.CheckState = CheckState.Checked;
            fillFacesCheckBox.Location = new Point(18, 22);
            fillFacesCheckBox.Name = "fillFacesCheckBox";
            fillFacesCheckBox.Size = new Size(112, 19);
            fillFacesCheckBox.TabIndex = 0;
            fillFacesCheckBox.Text = "Preencher faces";
            fillFacesCheckBox.UseVisualStyleBackColor = true;
            //
            // projectionViewButton
            //
            projectionViewButton.FlatStyle = FlatStyle.Flat;
            projectionViewButton.Location = new Point(160, 124);
            projectionViewButton.Name = "projectionViewButton";
            projectionViewButton.Size = new Size(134, 30);
            projectionViewButton.TabIndex = 5;
            projectionViewButton.Text = "Projecao";
            projectionViewButton.UseVisualStyleBackColor = true;
            //
            // transformViewButton
            //
            transformViewButton.FlatStyle = FlatStyle.Flat;
            transformViewButton.Location = new Point(18, 124);
            transformViewButton.Name = "transformViewButton";
            transformViewButton.Size = new Size(134, 30);
            transformViewButton.TabIndex = 4;
            transformViewButton.Text = "Transformacao";
            transformViewButton.UseVisualStyleBackColor = true;
            //
            // resetViewButton
            //
            resetViewButton.Location = new Point(18, 84);
            resetViewButton.Name = "resetViewButton";
            resetViewButton.Size = new Size(276, 32);
            resetViewButton.TabIndex = 3;
            resetViewButton.Text = "Resetar transformacoes";
            resetViewButton.UseVisualStyleBackColor = true;
            //
            // clearButton
            //
            clearButton.Location = new Point(160, 46);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(134, 30);
            clearButton.TabIndex = 2;
            clearButton.Text = "Limpar";
            clearButton.UseVisualStyleBackColor = true;
            //
            // openButton
            //
            openButton.Location = new Point(18, 46);
            openButton.Name = "openButton";
            openButton.Size = new Size(134, 30);
            openButton.TabIndex = 1;
            openButton.Text = "Abrir .obj";
            openButton.UseVisualStyleBackColor = true;
            //
            // sidebarTitleLabel
            //
            sidebarTitleLabel.AutoSize = false;
            sidebarTitleLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            sidebarTitleLabel.ForeColor = Color.FromArgb(33, 43, 55);
            sidebarTitleLabel.Location = new Point(18, 18);
            sidebarTitleLabel.Name = "sidebarTitleLabel";
            sidebarTitleLabel.Size = new Size(276, 28);
            sidebarTitleLabel.TabIndex = 0;
            sidebarTitleLabel.Text = "Ferramentas 3D";
            //
            // MainForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1350, 753);
            Controls.Add(viewportGroupBox);
            Controls.Add(sidebarPanel);
            MinimumSize = new Size(980, 792);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "3D Graphics Editor";
            viewportGroupBox.ResumeLayout(false);
            sidebarPanel.ResumeLayout(false);
            modeContentPanel.ResumeLayout(false);
            projectionPagePanel.ResumeLayout(false);
            orthographicProjectionGroupBox.ResumeLayout(false);
            orthographicProjectionGroupBox.PerformLayout();
            projectionModeGroupBox.ResumeLayout(false);
            projectionModeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)obliqueAlphaTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)obliqueRotationYTrackBar).EndInit();
            perspectiveProjectionGroupBox.ResumeLayout(false);
            perspectiveProjectionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)perspectiveZOffsetTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)perspectiveRotationYTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)perspectiveRotationXTrackBar).EndInit();
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
