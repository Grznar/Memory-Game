﻿namespace MainMenu
{
    partial class StartingMenu
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartingMenu));
            this.newGame = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.settings = new System.Windows.Forms.Button();
            this.quitGame = new System.Windows.Forms.Button();
            this.loadSave = new System.Windows.Forms.Button();
            this.scorebtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // newGame
            // 
            this.newGame.BackColor = System.Drawing.Color.PowderBlue;
            this.newGame.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.newGame.Location = new System.Drawing.Point(465, 148);
            this.newGame.Name = "newGame";
            this.newGame.Size = new System.Drawing.Size(155, 80);
            this.newGame.TabIndex = 2;
            this.newGame.Text = "Nová hra";
            this.newGame.UseVisualStyleBackColor = false;
            this.newGame.Click += new System.EventHandler(this.LoadGame);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(445, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 58);
            this.label1.TabIndex = 3;
            this.label1.Text = "PEXESO";
            // 
            // settings
            // 
            this.settings.BackColor = System.Drawing.Color.PowderBlue;
            this.settings.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.settings.Location = new System.Drawing.Point(465, 253);
            this.settings.Name = "settings";
            this.settings.Size = new System.Drawing.Size(155, 80);
            this.settings.TabIndex = 4;
            this.settings.Text = "Nastavení";
            this.settings.UseVisualStyleBackColor = false;
            this.settings.Click += new System.EventHandler(this.LoadSettings);
            // 
            // quitGame
            // 
            this.quitGame.BackColor = System.Drawing.Color.PowderBlue;
            this.quitGame.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.quitGame.Location = new System.Drawing.Point(465, 351);
            this.quitGame.Name = "quitGame";
            this.quitGame.Size = new System.Drawing.Size(155, 80);
            this.quitGame.TabIndex = 5;
            this.quitGame.Text = "Odejít";
            this.quitGame.UseVisualStyleBackColor = false;
            this.quitGame.Click += new System.EventHandler(this.QuitApp);
            // 
            // loadSave
            // 
            this.loadSave.BackColor = System.Drawing.Color.PowderBlue;
            this.loadSave.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.loadSave.Location = new System.Drawing.Point(690, 148);
            this.loadSave.Name = "loadSave";
            this.loadSave.Size = new System.Drawing.Size(175, 80);
            this.loadSave.TabIndex = 6;
            this.loadSave.Text = "Otevření hry";
            this.loadSave.UseVisualStyleBackColor = false;
            this.loadSave.Click += new System.EventHandler(this.LoadGameDetails);
            // 
            // scorebtn
            // 
            this.scorebtn.BackColor = System.Drawing.Color.PowderBlue;
            this.scorebtn.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.scorebtn.Location = new System.Drawing.Point(691, 351);
            this.scorebtn.Name = "scorebtn";
            this.scorebtn.Size = new System.Drawing.Size(174, 80);
            this.scorebtn.TabIndex = 7;
            this.scorebtn.Text = "Tabulka";
            this.scorebtn.UseVisualStyleBackColor = false;
            this.scorebtn.Click += new System.EventHandler(this.LoadScore);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(70, 125);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(355, 306);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // StartingMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 507);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.scorebtn);
            this.Controls.Add(this.loadSave);
            this.Controls.Add(this.quitGame);
            this.Controls.Add(this.settings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.newGame);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartingMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button newGame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button settings;
        private System.Windows.Forms.Button quitGame;
        private System.Windows.Forms.Button loadSave;
        private System.Windows.Forms.Button scorebtn;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

