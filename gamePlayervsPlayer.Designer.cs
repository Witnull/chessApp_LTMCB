namespace ChessApp
{
    partial class gamePlayervsPlayer
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
            this.SuspendLayout();
            // 
            // gamePlayervsPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "gamePlayervsPlayer";
            this.Text = "gamePlayervsPlayer";
            this.ResizeEnd += new System.EventHandler(this.gamePlayervsPlayer_ResizeEnd);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.gamePlayervsPlayer_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gamePlayervsPlayer_MouseClick);
            this.Resize += new System.EventHandler(this.gamePlayervsPlayer_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}