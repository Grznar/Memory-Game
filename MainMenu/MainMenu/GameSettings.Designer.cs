namespace MainMenu
{
    partial class GameSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameSettings));
            this.isPcPlayer = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.menuButton = new System.Windows.Forms.Button();
            this.playerCountTextBox = new System.Windows.Forms.TextBox();
            this.cardCountOne = new System.Windows.Forms.RadioButton();
            this.cardCountTwo = new System.Windows.Forms.RadioButton();
            this.cardCountThree = new System.Windows.Forms.RadioButton();
            this.difficultyOne = new System.Windows.Forms.RadioButton();
            this.difficultyTwo = new System.Windows.Forms.RadioButton();
            this.difficultyThree = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.volumeBool = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // isPcPlayer
            // 
            this.isPcPlayer.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.isPcPlayer.Location = new System.Drawing.Point(397, 208);
            this.isPcPlayer.Name = "isPcPlayer";
            this.isPcPlayer.Size = new System.Drawing.Size(24, 22);
            this.isPcPlayer.TabIndex = 0;
            this.isPcPlayer.UseVisualStyleBackColor = true;
            this.isPcPlayer.CheckedChanged += new System.EventHandler(this.isPcPlayerChecked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(49, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "Počet hráčů (2-6):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(49, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(276, 31);
            this.label2.TabIndex = 4;
            this.label2.Text = "Velikost hracího pole:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(49, 199);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 31);
            this.label3.TabIndex = 6;
            this.label3.Text = "Počítač:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(49, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 31);
            this.label4.TabIndex = 8;
            this.label4.Text = "Zvuk:";
            // 
            // menuButton
            // 
            this.menuButton.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.menuButton.Location = new System.Drawing.Point(708, 26);
            this.menuButton.Name = "menuButton";
            this.menuButton.Size = new System.Drawing.Size(215, 128);
            this.menuButton.TabIndex = 9;
            this.menuButton.Text = "Hlavní nabídka";
            this.menuButton.UseVisualStyleBackColor = true;
            this.menuButton.Click += new System.EventHandler(this.LoadMenu);
            // 
            // playerCountTextBox
            // 
            this.playerCountTextBox.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.playerCountTextBox.Location = new System.Drawing.Point(358, 44);
            this.playerCountTextBox.Name = "playerCountTextBox";
            this.playerCountTextBox.Size = new System.Drawing.Size(100, 35);
            this.playerCountTextBox.TabIndex = 11;
            this.playerCountTextBox.Text = "2";
            this.playerCountTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cardCountOne
            // 
            this.cardCountOne.AutoSize = true;
            this.cardCountOne.Location = new System.Drawing.Point(24, 11);
            this.cardCountOne.Name = "cardCountOne";
            this.cardCountOne.Size = new System.Drawing.Size(59, 24);
            this.cardCountOne.TabIndex = 12;
            this.cardCountOne.TabStop = true;
            this.cardCountOne.Text = "2x2";
            this.cardCountOne.UseVisualStyleBackColor = true;
            this.cardCountOne.CheckedChanged += new System.EventHandler(this.CardCountChanged);
            // 
            // cardCountTwo
            // 
            this.cardCountTwo.AutoSize = true;
            this.cardCountTwo.Location = new System.Drawing.Point(92, 11);
            this.cardCountTwo.Name = "cardCountTwo";
            this.cardCountTwo.Size = new System.Drawing.Size(59, 24);
            this.cardCountTwo.TabIndex = 13;
            this.cardCountTwo.TabStop = true;
            this.cardCountTwo.Text = "4x4";
            this.cardCountTwo.UseVisualStyleBackColor = true;
            this.cardCountTwo.CheckedChanged += new System.EventHandler(this.CardCountChanged);
            // 
            // cardCountThree
            // 
            this.cardCountThree.AutoSize = true;
            this.cardCountThree.Location = new System.Drawing.Point(177, 11);
            this.cardCountThree.Name = "cardCountThree";
            this.cardCountThree.Size = new System.Drawing.Size(59, 24);
            this.cardCountThree.TabIndex = 14;
            this.cardCountThree.TabStop = true;
            this.cardCountThree.Text = "6x6";
            this.cardCountThree.UseVisualStyleBackColor = true;
            this.cardCountThree.CheckedChanged += new System.EventHandler(this.CardCountChanged);
            // 
            // difficultyOne
            // 
            this.difficultyOne.AutoSize = true;
            this.difficultyOne.Location = new System.Drawing.Point(0, 6);
            this.difficultyOne.Name = "difficultyOne";
            this.difficultyOne.Size = new System.Drawing.Size(78, 24);
            this.difficultyOne.TabIndex = 15;
            this.difficultyOne.TabStop = true;
            this.difficultyOne.Text = "Lehká";
            this.difficultyOne.UseVisualStyleBackColor = true;
            this.difficultyOne.CheckedChanged += new System.EventHandler(this.difficultyChanged);
            // 
            // difficultyTwo
            // 
            this.difficultyTwo.AutoSize = true;
            this.difficultyTwo.Location = new System.Drawing.Point(86, 6);
            this.difficultyTwo.Name = "difficultyTwo";
            this.difficultyTwo.Size = new System.Drawing.Size(85, 24);
            this.difficultyTwo.TabIndex = 16;
            this.difficultyTwo.TabStop = true;
            this.difficultyTwo.Text = "Střední";
            this.difficultyTwo.UseVisualStyleBackColor = true;
            this.difficultyTwo.CheckedChanged += new System.EventHandler(this.difficultyChanged);
            // 
            // difficultyThree
            // 
            this.difficultyThree.AutoSize = true;
            this.difficultyThree.Location = new System.Drawing.Point(192, 6);
            this.difficultyThree.Name = "difficultyThree";
            this.difficultyThree.Size = new System.Drawing.Size(116, 24);
            this.difficultyThree.TabIndex = 17;
            this.difficultyThree.TabStop = true;
            this.difficultyThree.Text = "Velmi těžká";
            this.difficultyThree.UseVisualStyleBackColor = true;
            this.difficultyThree.CheckedChanged += new System.EventHandler(this.difficultyChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cardCountOne);
            this.groupBox1.Controls.Add(this.cardCountTwo);
            this.groupBox1.Controls.Add(this.cardCountThree);
            this.groupBox1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.groupBox1.Location = new System.Drawing.Point(331, 97);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 35);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.difficultyOne);
            this.groupBox2.Controls.Add(this.difficultyTwo);
            this.groupBox2.Controls.Add(this.difficultyThree);
            this.groupBox2.Location = new System.Drawing.Point(307, 257);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(321, 30);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            // 
            // volumeBool
            // 
            this.volumeBool.AutoSize = true;
            this.volumeBool.Checked = true;
            this.volumeBool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.volumeBool.Location = new System.Drawing.Point(397, 157);
            this.volumeBool.Name = "volumeBool";
            this.volumeBool.Size = new System.Drawing.Size(22, 21);
            this.volumeBool.TabIndex = 21;
            this.volumeBool.UseVisualStyleBackColor = true;
            this.volumeBool.CheckedChanged += new System.EventHandler(this.VolumeChecked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(49, 256);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 31);
            this.label5.TabIndex = 22;
            this.label5.Text = "Obtížnost:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(621, 199);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(316, 285);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // GameSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 479);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.volumeBool);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.playerCountTextBox);
            this.Controls.Add(this.menuButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.isPcPlayer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nastavení";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox isPcPlayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button menuButton;
        private System.Windows.Forms.TextBox playerCountTextBox;
        private System.Windows.Forms.ComboBox comboKarty;
        private System.Windows.Forms.RadioButton cardCountOne;
        private System.Windows.Forms.RadioButton cardCountTwo;
        private System.Windows.Forms.RadioButton cardCountThree;
        private System.Windows.Forms.RadioButton difficultyOne;
        private System.Windows.Forms.RadioButton difficultyTwo;
        private System.Windows.Forms.RadioButton difficultyThree;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox volumeBool;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}