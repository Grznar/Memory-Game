namespace MainMenu
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.PowderBlue;
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.Location = new System.Drawing.Point(465, 148);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 80);
            this.button1.TabIndex = 2;
            this.button1.Text = "Nová hra";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.LoadGame);
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
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.PowderBlue;
            this.button2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button2.Location = new System.Drawing.Point(465, 249);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 80);
            this.button2.TabIndex = 4;
            this.button2.Text = "Nastavení";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.LoadSettings);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.PowderBlue;
            this.button3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button3.Location = new System.Drawing.Point(465, 351);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(155, 80);
            this.button3.TabIndex = 5;
            this.button3.Text = "Odejít";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.QuitApp);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.PowderBlue;
            this.button4.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button4.Location = new System.Drawing.Point(690, 148);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(175, 80);
            this.button4.TabIndex = 6;
            this.button4.Text = "Otevření hry";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.PowderBlue;
            this.button5.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button5.Location = new System.Drawing.Point(895, 417);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(174, 80);
            this.button5.TabIndex = 7;
            this.button5.Text = "Tabulka";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.LoadScore);
            // 
            // StartingMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 519);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "StartingMenu";
            this.ShowIcon = false;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}

