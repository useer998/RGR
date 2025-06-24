namespace BattleshipGame
{
    partial class Form1
    {
        /// <summary>
        /// Обов’язкова змінна конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Оголошення контролів
        private System.Windows.Forms.Panel panelPlayer;
        private System.Windows.Forms.Panel panelComputer;
        private System.Windows.Forms.Button btnRotate;
        private System.Windows.Forms.Button btnRandomPlace;
        private System.Windows.Forms.Button btnStartGame;

        /// <summary>
        /// Звільнити всі використовувані ресурси.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматично створений конструктором форм Windows

        private void InitializeComponent()
        {
            this.panelPlayer = new System.Windows.Forms.Panel();
            this.panelComputer = new System.Windows.Forms.Panel();
            this.btnRotate = new System.Windows.Forms.Button();
            this.btnRandomPlace = new System.Windows.Forms.Button();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelPlayer
            // 
            this.panelPlayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPlayer.Location = new System.Drawing.Point(50, 150);
            this.panelPlayer.Name = "panelPlayer";
            this.panelPlayer.Size = new System.Drawing.Size(300, 300);
            this.panelPlayer.TabIndex = 0;
            // 
            // panelComputer
            // 
            this.panelComputer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelComputer.Location = new System.Drawing.Point(450, 150);
            this.panelComputer.Name = "panelComputer";
            this.panelComputer.Size = new System.Drawing.Size(300, 300);
            this.panelComputer.TabIndex = 1;
            // 
            // btnRotate
            // 
            this.btnRotate.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnRotate.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnRotate.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
            this.btnRotate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btnRotate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.btnRotate.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.btnRotate.Location = new System.Drawing.Point(410, 39);
            this.btnRotate.Margin = new System.Windows.Forms.Padding(0);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(340, 40);
            this.btnRotate.TabIndex = 2;
            this.btnRotate.Text = "Горизонтально";
            this.btnRotate.UseVisualStyleBackColor = false;
            // 
            // btnRandomPlace
            // 
            this.btnRandomPlace.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnRandomPlace.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnRandomPlace.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
            this.btnRandomPlace.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btnRandomPlace.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.btnRandomPlace.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.btnRandomPlace.Location = new System.Drawing.Point(50, 39);
            this.btnRandomPlace.Margin = new System.Windows.Forms.Padding(0);
            this.btnRandomPlace.Name = "btnRandomPlace";
            this.btnRandomPlace.Size = new System.Drawing.Size(340, 40);
            this.btnRandomPlace.TabIndex = 3;
            this.btnRandomPlace.Text = "Випадкове розміщення";
            this.btnRandomPlace.UseVisualStyleBackColor = false;
            // 
            // btnStartGame
            // 
            this.btnStartGame.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnStartGame.Enabled = false;
            this.btnStartGame.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStartGame.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black;
            this.btnStartGame.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.btnStartGame.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.btnStartGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(204)));
            this.btnStartGame.Location = new System.Drawing.Point(50, 88);
            this.btnStartGame.Margin = new System.Windows.Forms.Padding(0);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(700, 40);
            this.btnStartGame.TabIndex = 4;
            this.btnStartGame.Text = "Почати гру";
            this.btnStartGame.UseVisualStyleBackColor = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatus.Location = new System.Drawing.Point(50, 0);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 70);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Статус...";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.panelPlayer);
            this.Controls.Add(this.panelComputer);
            this.Controls.Add(this.btnRotate);
            this.Controls.Add(this.btnRandomPlace);
            this.Controls.Add(this.btnStartGame);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Морський бій";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
    }
}
