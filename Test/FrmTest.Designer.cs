namespace DVLD
{
    partial class FrmTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTest));
            this.button2 = new System.Windows.Forms.Button();
            this.ctrlSechduleTest1 = new DVLD.CtrlSechduleTest();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderSize = 2;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft YaHei UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(273, 1116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(167, 46);
            this.button2.TabIndex = 10;
            this.button2.Text = "close ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ctrlSechduleTest1
            // 
            this.ctrlSechduleTest1.Location = new System.Drawing.Point(4, 12);
            this.ctrlSechduleTest1.Name = "ctrlSechduleTest1";
            this.ctrlSechduleTest1.Size = new System.Drawing.Size(749, 1098);
            this.ctrlSechduleTest1.TabIndex = 0;
            // 
            // FrmVisionTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 1171);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ctrlSechduleTest1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "FrmVisionTest";
            this.Text = "FrmVisionTest";
            this.Load += new System.EventHandler(this.FrmVisionTest_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CtrlSechduleTest ctrlSechduleTest1;
        private System.Windows.Forms.Button button2;
    }
}