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
        private Button openButton;
        private Button clearButton;
        private Button resetViewButton;
        private Label infoLabel;
        private Label zoomLabel;

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
            zoomLabel = new Label();
            viewportPanel = new _3d_graphics_editor.Rendering.DoubleBufferedPanel();
            sidebarPanel = new Panel();
            infoLabel = new Label();
            resetViewButton = new Button();
            clearButton = new Button();
            openButton = new Button();
            viewportGroupBox.SuspendLayout();
            sidebarPanel.SuspendLayout();
            SuspendLayout();
            // 
            // viewportGroupBox
            // 
            viewportGroupBox.Controls.Add(zoomLabel);
            viewportGroupBox.Controls.Add(viewportPanel);
            viewportGroupBox.Dock = DockStyle.Fill;
            viewportGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            viewportGroupBox.Location = new Point(0, 0);
            viewportGroupBox.Name = "viewportGroupBox";
            viewportGroupBox.Padding = new Padding(14, 14, 14, 28);
            viewportGroupBox.Size = new Size(1038, 729);
            viewportGroupBox.TabIndex = 0;
            viewportGroupBox.TabStop = false;
            // 
            // zoomLabel
            // 
            zoomLabel.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            zoomLabel.ForeColor = Color.FromArgb(61, 73, 89);
            zoomLabel.Location = new Point(12, 704);
            zoomLabel.Name = "zoomLabel";
            zoomLabel.Size = new Size(88, 24);
            zoomLabel.TabIndex = 6;
            zoomLabel.Text = "Zoom: 100%";
            // 
            // viewportPanel
            // 
            viewportPanel.BackColor = Color.White;
            viewportPanel.Dock = DockStyle.Fill;
            viewportPanel.Location = new Point(14, 32);
            viewportPanel.Name = "viewportPanel";
            viewportPanel.Size = new Size(1010, 669);
            viewportPanel.TabIndex = 0;
            viewportPanel.TabStop = true;
            // 
            // sidebarPanel
            // 
            sidebarPanel.BackColor = Color.FromArgb(242, 245, 248);
            sidebarPanel.Controls.Add(infoLabel);
            sidebarPanel.Controls.Add(resetViewButton);
            sidebarPanel.Controls.Add(clearButton);
            sidebarPanel.Controls.Add(openButton);
            sidebarPanel.Dock = DockStyle.Right;
            sidebarPanel.Location = new Point(1038, 0);
            sidebarPanel.Name = "sidebarPanel";
            sidebarPanel.Padding = new Padding(18);
            sidebarPanel.Size = new Size(312, 729);
            sidebarPanel.TabIndex = 1;
            // 
            // infoLabel
            // 
            infoLabel.ForeColor = Color.FromArgb(61, 73, 89);
            infoLabel.Location = new Point(18, 150);
            infoLabel.Name = "infoLabel";
            infoLabel.Size = new Size(276, 86);
            infoLabel.TabIndex = 4;
            infoLabel.Text = "Nenhum modelo carregado.";
            // 
            // resetViewButton
            // 
            resetViewButton.Location = new Point(18, 106);
            resetViewButton.Name = "resetViewButton";
            resetViewButton.Size = new Size(276, 38);
            resetViewButton.TabIndex = 3;
            resetViewButton.Text = "Resetar visão";
            resetViewButton.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            clearButton.Location = new Point(18, 62);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(276, 38);
            clearButton.TabIndex = 2;
            clearButton.Text = "Limpar";
            clearButton.UseVisualStyleBackColor = true;
            // 
            // openButton
            // 
            openButton.Location = new Point(18, 18);
            openButton.Name = "openButton";
            openButton.Size = new Size(276, 38);
            openButton.TabIndex = 1;
            openButton.Text = "Abrir .obj";
            openButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1350, 729);
            Controls.Add(viewportGroupBox);
            Controls.Add(sidebarPanel);
            MinimumSize = new Size(980, 640);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "3D Graphics Editor";
            viewportGroupBox.ResumeLayout(false);
            sidebarPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
    }
}
