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
            this.label1 = new System.Windows.Forms.Label();
            this.ConnectionTextBox = new System.Windows.Forms.MaskedTextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.OnlineGamePictureBox = new System.Windows.Forms.PictureBox();
            this.OnlineScoreLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MainGamePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextBlockPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OnlineGamePictureBox)).BeginInit();
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
            this.StartButton.Location = new System.Drawing.Point(686, 12);
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
            this.StopButton.Location = new System.Drawing.Point(686, 50);
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
            this.PauseButton.Location = new System.Drawing.Point(686, 90);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(75, 23);
            this.PauseButton.TabIndex = 5;
            this.PauseButton.TabStop = false;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(786, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "IP-adres en poort";
            // 
            // ConnectionTextBox
            // 
            this.ConnectionTextBox.Location = new System.Drawing.Point(789, 28);
            this.ConnectionTextBox.Mask = "999.999.999.999:9999";
            this.ConnectionTextBox.Name = "ConnectionTextBox";
            this.ConnectionTextBox.Size = new System.Drawing.Size(100, 20);
            this.ConnectionTextBox.TabIndex = 7;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(789, 54);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 8;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // OnlineGamePictureBox
            // 
            this.OnlineGamePictureBox.Location = new System.Drawing.Point(912, 12);
            this.OnlineGamePictureBox.Name = "OnlineGamePictureBox";
            this.OnlineGamePictureBox.Size = new System.Drawing.Size(400, 720);
            this.OnlineGamePictureBox.TabIndex = 9;
            this.OnlineGamePictureBox.TabStop = false;
            // 
            // OnlineScoreLabel
            // 
            this.OnlineScoreLabel.AutoSize = true;
            this.OnlineScoreLabel.Location = new System.Drawing.Point(1328, 12);
            this.OnlineScoreLabel.Name = "OnlineScoreLabel";
            this.OnlineScoreLabel.Size = new System.Drawing.Size(0, 13);
            this.OnlineScoreLabel.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1633, 754);
            this.Controls.Add(this.OnlineScoreLabel);
            this.Controls.Add(this.OnlineGamePictureBox);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.ConnectionTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.NextBlockPictureBox);
            this.Controls.Add(this.MainGamePictureBox);
            this.Name = "Form1";
            this.Text = "TetrisGame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MainGamePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextBlockPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OnlineGamePictureBox)).EndInit();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox ConnectionTextBox;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.PictureBox OnlineGamePictureBox;
        private System.Windows.Forms.Label OnlineScoreLabel;
    }
}

