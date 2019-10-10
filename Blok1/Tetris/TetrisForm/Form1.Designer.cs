namespace TetrisForm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainGamePictureBox = new System.Windows.Forms.PictureBox();
            this.NextBlockPictureBox = new System.Windows.Forms.PictureBox();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MainGamePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextBlockPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MainGamePictureBox
            // 
            this.MainGamePictureBox.Location = new System.Drawing.Point(12, 12);
            this.MainGamePictureBox.Name = "MainGamePictureBox";
            this.MainGamePictureBox.Size = new System.Drawing.Size(400, 720);
            this.MainGamePictureBox.TabIndex = 0;
            this.MainGamePictureBox.TabStop = false;
            // 
            // NextBlockPictureBox
            // 
            this.NextBlockPictureBox.Location = new System.Drawing.Point(410, 12);
            this.NextBlockPictureBox.Name = "NextBlockPictureBox";
            this.NextBlockPictureBox.Size = new System.Drawing.Size(240, 180);
            this.NextBlockPictureBox.TabIndex = 1;
            this.NextBlockPictureBox.TabStop = false;
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Location = new System.Drawing.Point(418, 226);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(0, 13);
            this.InfoLabel.TabIndex = 2;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(954, 12);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 3;
            this.StartButton.TabStop = false;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Enabled = false;
            this.StopButton.Location = new System.Drawing.Point(954, 50);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(75, 23);
            this.StopButton.TabIndex = 4;
            this.StopButton.TabStop = false;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Enabled = false;
            this.PauseButton.Location = new System.Drawing.Point(954, 90);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(75, 23);
            this.PauseButton.TabIndex = 5;
            this.PauseButton.TabStop = false;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 754);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.NextBlockPictureBox);
            this.Controls.Add(this.MainGamePictureBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MainGamePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextBlockPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox MainGamePictureBox;
        private System.Windows.Forms.PictureBox NextBlockPictureBox;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button PauseButton;
    }
}

