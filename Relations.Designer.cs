namespace Caldara_Visualisation
{
    partial class Relations
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && ( components != null ))
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
        private void InitializeComponent ()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorLabel = new System.Windows.Forms.Label();
            this.textBoxDiametr = new System.Windows.Forms.TextBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxBeta = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.relationsTypeComboBox = new System.Windows.Forms.ComboBox();
            this.labelEntity = new System.Windows.Forms.Label();
            this.incorrectDataLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel1.Controls.Add(this.incorrectDataLabel);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.textBoxInfo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.errorLabel);
            this.panel1.Controls.Add(this.textBoxDiametr);
            this.panel1.Controls.Add(this.buttonClear);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxBeta);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.relationsTypeComboBox);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 575);
            this.panel1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(33, 80);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Search";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxInfo.Location = new System.Drawing.Point(3, 392);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ReadOnly = true;
            this.textBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxInfo.Size = new System.Drawing.Size(194, 180);
            this.textBoxInfo.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 197);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Diametr (from 8 to 32)";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.errorLabel.ForeColor = System.Drawing.Color.Red;
            this.errorLabel.Location = new System.Drawing.Point(29, 357);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(153, 20);
            this.errorLabel.TabIndex = 9;
            this.errorLabel.Text = "Error: wrong file type";
            this.errorLabel.Visible = false;
            // 
            // textBoxDiametr
            // 
            this.textBoxDiametr.Location = new System.Drawing.Point(33, 225);
            this.textBoxDiametr.Name = "textBoxDiametr";
            this.textBoxDiametr.Size = new System.Drawing.Size(100, 20);
            this.textBoxDiametr.TabIndex = 11;
            this.textBoxDiametr.Text = "16";
            // 
            // buttonClear
            // 
            this.buttonClear.Enabled = false;
            this.buttonClear.Location = new System.Drawing.Point(33, 322);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(100, 23);
            this.buttonClear.TabIndex = 11;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Beta";
            // 
            // textBoxBeta
            // 
            this.textBoxBeta.Location = new System.Drawing.Point(33, 157);
            this.textBoxBeta.Name = "textBoxBeta";
            this.textBoxBeta.Size = new System.Drawing.Size(100, 20);
            this.textBoxBeta.TabIndex = 11;
            this.textBoxBeta.Text = "0,85";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Relations type";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(33, 284);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Visualize";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // relationsTypeComboBox
            // 
            this.relationsTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.relationsTypeComboBox.FormattingEnabled = true;
            this.relationsTypeComboBox.Items.AddRange(new object[] {
            "CALLS",
            "FP-CALLS",
            "CONTAINS",
            "READSVAR",
            "WRITESVAR",
            "INCLUDES",
            "CONTAINS",
            "INHERITS",
            "OVERRIDES",
            "IMPORTS",
            "IMPLICITLY_CALLS",
            "ASSOCIATES"});
            this.relationsTypeComboBox.Location = new System.Drawing.Point(33, 38);
            this.relationsTypeComboBox.Name = "relationsTypeComboBox";
            this.relationsTypeComboBox.Size = new System.Drawing.Size(100, 21);
            this.relationsTypeComboBox.TabIndex = 1;
            // 
            // labelEntity
            // 
            this.labelEntity.AutoSize = true;
            this.labelEntity.Location = new System.Drawing.Point(206, 9);
            this.labelEntity.Name = "labelEntity";
            this.labelEntity.Size = new System.Drawing.Size(39, 13);
            this.labelEntity.TabIndex = 10;
            this.labelEntity.Text = "Entity: ";
            this.labelEntity.Visible = false;
            // 
            // incorrectDataLabel
            // 
            this.incorrectDataLabel.AutoSize = true;
            this.incorrectDataLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.incorrectDataLabel.ForeColor = System.Drawing.Color.Red;
            this.incorrectDataLabel.Location = new System.Drawing.Point(29, 248);
            this.incorrectDataLabel.Name = "incorrectDataLabel";
            this.incorrectDataLabel.Size = new System.Drawing.Size(149, 20);
            this.incorrectDataLabel.TabIndex = 13;
            this.incorrectDataLabel.Text = "Error: incorrect data";
            this.incorrectDataLabel.Visible = false;
            // 
            // Relations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(919, 575);
            this.Controls.Add(this.labelEntity);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "Relations";
            this.Text = "Relations";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Relations_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Relations_MouseMove);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox relationsTypeComboBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label labelEntity;
        private System.Windows.Forms.TextBox textBoxBeta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDiametr;
        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label incorrectDataLabel;



    }
}

