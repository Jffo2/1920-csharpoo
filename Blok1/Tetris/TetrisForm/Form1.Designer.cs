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
            this.InfoLabel.Size = new System.Drawing.Size(35, 13);
            this.InfoLabel.TabIndex = 2;
            this.InfoLabel.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 754);
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
    }
}

