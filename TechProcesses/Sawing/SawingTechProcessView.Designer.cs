﻿namespace CAM.Sawing
{
    partial class SawingTechProcessView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbFrequency = new System.Windows.Forms.TextBox();
            this.sawingTechProcessBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lbFrequency = new System.Windows.Forms.Label();
            this.bObjects = new System.Windows.Forms.Button();
            this.tbObjects = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbTool = new System.Windows.Forms.TextBox();
            this.bTool = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tbThickness = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbMachine = new System.Windows.Forms.ComboBox();
            this.lbMachine = new System.Windows.Forms.Label();
            this.cbMaterial = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbSawingModes = new System.Windows.Forms.GroupBox();
            this.sawingModesView = new CAM.Sawing.SawingModesView();
            this.cbObjectType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPenetrationFeed = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sawingTechProcessBindingSource)).BeginInit();
            this.gbSawingModes.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbFrequency
            // 
            this.tbFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFrequency.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.sawingTechProcessBindingSource, "Frequency", true));
            this.tbFrequency.Location = new System.Drawing.Point(102, 106);
            this.tbFrequency.Name = "tbFrequency";
            this.tbFrequency.Size = new System.Drawing.Size(152, 20);
            this.tbFrequency.TabIndex = 50;
            // 
            // sawingTechProcessBindingSource
            // 
            this.sawingTechProcessBindingSource.DataSource = typeof(CAM.Sawing.SawingTechProcess);
            // 
            // lbFrequency
            // 
            this.lbFrequency.AutoSize = true;
            this.lbFrequency.Location = new System.Drawing.Point(3, 109);
            this.lbFrequency.Name = "lbFrequency";
            this.lbFrequency.Size = new System.Drawing.Size(58, 13);
            this.lbFrequency.TabIndex = 70;
            this.lbFrequency.Text = "Шпиндель";
            // 
            // bObjects
            // 
            this.bObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bObjects.Location = new System.Drawing.Point(234, 162);
            this.bObjects.Name = "bObjects";
            this.bObjects.Size = new System.Drawing.Size(21, 21);
            this.bObjects.TabIndex = 68;
            this.bObjects.TabStop = false;
            this.bObjects.Text = "۞";
            this.bObjects.UseVisualStyleBackColor = true;
            this.bObjects.Click += new System.EventHandler(this.bObjects_Click);
            // 
            // tbObjects
            // 
            this.tbObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbObjects.Location = new System.Drawing.Point(102, 162);
            this.tbObjects.Name = "tbObjects";
            this.tbObjects.ReadOnly = true;
            this.tbObjects.Size = new System.Drawing.Size(130, 20);
            this.tbObjects.TabIndex = 60;
            this.tbObjects.Enter += new System.EventHandler(this.tbObjects_Enter);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 166);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 66;
            this.label8.Text = "Объекты";
            // 
            // tbTool
            // 
            this.tbTool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTool.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbTool.Location = new System.Drawing.Point(102, 83);
            this.tbTool.Name = "tbTool";
            this.tbTool.ReadOnly = true;
            this.tbTool.Size = new System.Drawing.Size(130, 20);
            this.tbTool.TabIndex = 40;
            // 
            // bTool
            // 
            this.bTool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bTool.Location = new System.Drawing.Point(234, 83);
            this.bTool.Name = "bTool";
            this.bTool.Size = new System.Drawing.Size(20, 20);
            this.bTool.TabIndex = 64;
            this.bTool.TabStop = false;
            this.bTool.Text = "Ξ";
            this.bTool.UseVisualStyleBackColor = true;
            this.bTool.Click += new System.EventHandler(this.bTool_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 63;
            this.label6.Text = "Инструмент";
            // 
            // tbThickness
            // 
            this.tbThickness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbThickness.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.sawingTechProcessBindingSource, "Thickness", true));
            this.tbThickness.Location = new System.Drawing.Point(102, 51);
            this.tbThickness.Name = "tbThickness";
            this.tbThickness.Size = new System.Drawing.Size(152, 20);
            this.tbThickness.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 60;
            this.label4.Text = "Толщина";
            // 
            // cbMachine
            // 
            this.cbMachine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMachine.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.sawingTechProcessBindingSource, "MachineType", true));
            this.cbMachine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMachine.FormattingEnabled = true;
            this.cbMachine.Location = new System.Drawing.Point(102, 3);
            this.cbMachine.Name = "cbMachine";
            this.cbMachine.Size = new System.Drawing.Size(152, 21);
            this.cbMachine.TabIndex = 10;
            // 
            // lbMachine
            // 
            this.lbMachine.AutoSize = true;
            this.lbMachine.Location = new System.Drawing.Point(3, 6);
            this.lbMachine.Name = "lbMachine";
            this.lbMachine.Size = new System.Drawing.Size(43, 13);
            this.lbMachine.TabIndex = 50;
            this.lbMachine.Text = "Станок";
            // 
            // cbMaterial
            // 
            this.cbMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMaterial.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.sawingTechProcessBindingSource, "Material", true));
            this.cbMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMaterial.FormattingEnabled = true;
            this.cbMaterial.Location = new System.Drawing.Point(102, 27);
            this.cbMaterial.Name = "cbMaterial";
            this.cbMaterial.Size = new System.Drawing.Size(152, 21);
            this.cbMaterial.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 74;
            this.label1.Text = "Материал";
            // 
            // gbSawingModes
            // 
            this.gbSawingModes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSawingModes.Controls.Add(this.sawingModesView);
            this.gbSawingModes.Controls.Add(this.cbObjectType);
            this.gbSawingModes.Controls.Add(this.label2);
            this.gbSawingModes.Location = new System.Drawing.Point(3, 202);
            this.gbSawingModes.Name = "gbSawingModes";
            this.gbSawingModes.Size = new System.Drawing.Size(252, 335);
            this.gbSawingModes.TabIndex = 70;
            this.gbSawingModes.TabStop = false;
            this.gbSawingModes.Text = "Режимы обработки объектов";
            // 
            // sawingModesView
            // 
            this.sawingModesView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sawingModesView.Location = new System.Drawing.Point(6, 46);
            this.sawingModesView.Name = "sawingModesView";
            this.sawingModesView.Size = new System.Drawing.Size(240, 283);
            this.sawingModesView.TabIndex = 77;
            // 
            // cbObjectType
            // 
            this.cbObjectType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjectType.FormattingEnabled = true;
            this.cbObjectType.Items.AddRange(new object[] {
            "Отрезок",
            "Криволинейный"});
            this.cbObjectType.Location = new System.Drawing.Point(94, 19);
            this.cbObjectType.Name = "cbObjectType";
            this.cbObjectType.Size = new System.Drawing.Size(152, 21);
            this.cbObjectType.TabIndex = 75;
            this.cbObjectType.SelectedIndexChanged += new System.EventHandler(this.cbObjectType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 76;
            this.label2.Text = "Тип объекта";
            // 
            // tbPenetrationFeed
            // 
            this.tbPenetrationFeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPenetrationFeed.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.sawingTechProcessBindingSource, "PenetrationFeed", true));
            this.tbPenetrationFeed.Location = new System.Drawing.Point(102, 129);
            this.tbPenetrationFeed.Name = "tbPenetrationFeed";
            this.tbPenetrationFeed.Size = new System.Drawing.Size(152, 20);
            this.tbPenetrationFeed.TabIndex = 55;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 132);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 76;
            this.label9.Text = "Скор. малая";
            // 
            // SawingTechProcessView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbPenetrationFeed);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.gbSawingModes);
            this.Controls.Add(this.cbMaterial);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFrequency);
            this.Controls.Add(this.lbFrequency);
            this.Controls.Add(this.bObjects);
            this.Controls.Add(this.tbObjects);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbTool);
            this.Controls.Add(this.bTool);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbThickness);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbMachine);
            this.Controls.Add(this.lbMachine);
            this.Name = "SawingTechProcessView";
            this.Size = new System.Drawing.Size(257, 540);
            ((System.ComponentModel.ISupportInitialize)(this.sawingTechProcessBindingSource)).EndInit();
            this.gbSawingModes.ResumeLayout(false);
            this.gbSawingModes.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbFrequency;
        private System.Windows.Forms.Label lbFrequency;
        private System.Windows.Forms.Button bObjects;
        private System.Windows.Forms.TextBox tbObjects;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbTool;
        private System.Windows.Forms.Button bTool;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbThickness;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbMachine;
        private System.Windows.Forms.Label lbMachine;
        private System.Windows.Forms.ComboBox cbMaterial;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbSawingModes;
        private System.Windows.Forms.ComboBox cbObjectType;
        private System.Windows.Forms.Label label2;
        private SawingModesView sawingModesView;
        private System.Windows.Forms.BindingSource sawingTechProcessBindingSource;
        private System.Windows.Forms.TextBox tbPenetrationFeed;
        private System.Windows.Forms.Label label9;
    }
}
