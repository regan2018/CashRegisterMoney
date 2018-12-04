namespace SuperCollectingSilver
{
    partial class SystemSetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemSetForm));
            this.cmb_port = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_test = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.rb_2_line = new System.Windows.Forms.RadioButton();
            this.rb_5_line = new System.Windows.Forms.RadioButton();
            this.gb_money_type = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gb_ledCustomer = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmb_ledCustomerTestType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_LedDataBits = new System.Windows.Forms.TextBox();
            this.btn_LedCustomerDisplay = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.com_ledCustomerPort = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rb_LCDCustomer = new System.Windows.Forms.RadioButton();
            this.rb_LEDCustomer = new System.Windows.Forms.RadioButton();
            this.com_LedBaudRate = new System.Windows.Forms.ComboBox();
            this.cmb_LedCustomerDisplaySpecification = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.gb_money_type.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gb_ledCustomer.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmb_port
            // 
            this.cmb_port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_port.FormattingEnabled = true;
            this.cmb_port.Location = new System.Drawing.Point(99, 24);
            this.cmb_port.Name = "cmb_port";
            this.cmb_port.Size = new System.Drawing.Size(248, 23);
            this.cmb_port.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "端　　口：";
            // 
            // btn_test
            // 
            this.btn_test.Location = new System.Drawing.Point(226, 120);
            this.btn_test.Name = "btn_test";
            this.btn_test.Size = new System.Drawing.Size(121, 30);
            this.btn_test.TabIndex = 2;
            this.btn_test.Text = "测试";
            this.btn_test.UseVisualStyleBackColor = true;
            this.btn_test.Click += new System.EventHandler(this.btn_test_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(339, 489);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(89, 30);
            this.btn_save.TabIndex = 3;
            this.btn_save.Text = "保存设置";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(29, 504);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "注：请先保存后再测试";
            // 
            // rb_2_line
            // 
            this.rb_2_line.AutoSize = true;
            this.rb_2_line.Location = new System.Drawing.Point(18, 24);
            this.rb_2_line.Name = "rb_2_line";
            this.rb_2_line.Size = new System.Drawing.Size(81, 19);
            this.rb_2_line.TabIndex = 7;
            this.rb_2_line.TabStop = true;
            this.rb_2_line.Text = "2线引脚";
            this.rb_2_line.UseVisualStyleBackColor = true;
            // 
            // rb_5_line
            // 
            this.rb_5_line.AutoSize = true;
            this.rb_5_line.Location = new System.Drawing.Point(105, 24);
            this.rb_5_line.Name = "rb_5_line";
            this.rb_5_line.Size = new System.Drawing.Size(81, 19);
            this.rb_5_line.TabIndex = 8;
            this.rb_5_line.TabStop = true;
            this.rb_5_line.Text = "5线引脚";
            this.rb_5_line.UseVisualStyleBackColor = true;
            // 
            // gb_money_type
            // 
            this.gb_money_type.Controls.Add(this.rb_5_line);
            this.gb_money_type.Controls.Add(this.rb_2_line);
            this.gb_money_type.Location = new System.Drawing.Point(17, 58);
            this.gb_money_type.Name = "gb_money_type";
            this.gb_money_type.Size = new System.Drawing.Size(330, 57);
            this.gb_money_type.TabIndex = 8;
            this.gb_money_type.TabStop = false;
            this.gb_money_type.Text = "钱箱接口类型";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmb_port);
            this.groupBox1.Controls.Add(this.gb_money_type);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_test);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(415, 159);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "钱箱设置";
            // 
            // gb_ledCustomer
            // 
            this.gb_ledCustomer.Controls.Add(this.cmb_LedCustomerDisplaySpecification);
            this.gb_ledCustomer.Controls.Add(this.label7);
            this.gb_ledCustomer.Controls.Add(this.com_LedBaudRate);
            this.gb_ledCustomer.Controls.Add(this.label6);
            this.gb_ledCustomer.Controls.Add(this.cmb_ledCustomerTestType);
            this.gb_ledCustomer.Controls.Add(this.label5);
            this.gb_ledCustomer.Controls.Add(this.tb_LedDataBits);
            this.gb_ledCustomer.Controls.Add(this.btn_LedCustomerDisplay);
            this.gb_ledCustomer.Controls.Add(this.label4);
            this.gb_ledCustomer.Controls.Add(this.com_ledCustomerPort);
            this.gb_ledCustomer.Controls.Add(this.label3);
            this.gb_ledCustomer.Location = new System.Drawing.Point(6, 97);
            this.gb_ledCustomer.Name = "gb_ledCustomer";
            this.gb_ledCustomer.Size = new System.Drawing.Size(403, 174);
            this.gb_ledCustomer.TabIndex = 10;
            this.gb_ledCustomer.TabStop = false;
            this.gb_ledCustomer.Text = "LED客显设置";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 15);
            this.label6.TabIndex = 16;
            this.label6.Text = "测试类型：";
            // 
            // cmb_ledCustomerTestType
            // 
            this.cmb_ledCustomerTestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_ledCustomerTestType.FormattingEnabled = true;
            this.cmb_ledCustomerTestType.Items.AddRange(new object[] {
            "单价",
            "总计",
            "收款",
            "找零",
            "清屏"});
            this.cmb_ledCustomerTestType.Location = new System.Drawing.Point(99, 123);
            this.cmb_ledCustomerTestType.Name = "cmb_ledCustomerTestType";
            this.cmb_ledCustomerTestType.Size = new System.Drawing.Size(121, 23);
            this.cmb_ledCustomerTestType.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(198, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "数 据 位：";
            // 
            // tb_LedDataBits
            // 
            this.tb_LedDataBits.Location = new System.Drawing.Point(287, 86);
            this.tb_LedDataBits.Name = "tb_LedDataBits";
            this.tb_LedDataBits.Size = new System.Drawing.Size(60, 25);
            this.tb_LedDataBits.TabIndex = 13;
            // 
            // btn_LedCustomerDisplay
            // 
            this.btn_LedCustomerDisplay.Location = new System.Drawing.Point(226, 121);
            this.btn_LedCustomerDisplay.Name = "btn_LedCustomerDisplay";
            this.btn_LedCustomerDisplay.Size = new System.Drawing.Size(121, 30);
            this.btn_LedCustomerDisplay.TabIndex = 9;
            this.btn_LedCustomerDisplay.Text = "测试";
            this.btn_LedCustomerDisplay.UseVisualStyleBackColor = true;
            this.btn_LedCustomerDisplay.Click += new System.EventHandler(this.btn_LedCustomerDisplay_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "波 特 率：";
            // 
            // com_ledCustomerPort
            // 
            this.com_ledCustomerPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_ledCustomerPort.FormattingEnabled = true;
            this.com_ledCustomerPort.Location = new System.Drawing.Point(99, 24);
            this.com_ledCustomerPort.Name = "com_ledCustomerPort";
            this.com_ledCustomerPort.Size = new System.Drawing.Size(248, 23);
            this.com_ledCustomerPort.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "端　　口：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rb_LCDCustomer);
            this.groupBox3.Controls.Add(this.rb_LEDCustomer);
            this.groupBox3.Controls.Add(this.gb_ledCustomer);
            this.groupBox3.Location = new System.Drawing.Point(13, 178);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(415, 277);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "客显设置";
            // 
            // rb_LCDCustomer
            // 
            this.rb_LCDCustomer.AutoSize = true;
            this.rb_LCDCustomer.Location = new System.Drawing.Point(271, 55);
            this.rb_LCDCustomer.Name = "rb_LCDCustomer";
            this.rb_LCDCustomer.Size = new System.Drawing.Size(88, 19);
            this.rb_LCDCustomer.TabIndex = 12;
            this.rb_LCDCustomer.Text = "液晶客显";
            this.rb_LCDCustomer.UseVisualStyleBackColor = true;
            // 
            // rb_LEDCustomer
            // 
            this.rb_LEDCustomer.AutoSize = true;
            this.rb_LEDCustomer.Checked = true;
            this.rb_LEDCustomer.Location = new System.Drawing.Point(25, 55);
            this.rb_LEDCustomer.Name = "rb_LEDCustomer";
            this.rb_LEDCustomer.Size = new System.Drawing.Size(82, 19);
            this.rb_LEDCustomer.TabIndex = 11;
            this.rb_LEDCustomer.TabStop = true;
            this.rb_LEDCustomer.Text = "LED客显";
            this.rb_LEDCustomer.UseVisualStyleBackColor = true;
            // 
            // com_LedBaudRate
            // 
            this.com_LedBaudRate.FormattingEnabled = true;
            this.com_LedBaudRate.Items.AddRange(new object[] {
            "9600",
            "4800",
            "2400",
            "1200",
            "600",
            "300"});
            this.com_LedBaudRate.Location = new System.Drawing.Point(99, 87);
            this.com_LedBaudRate.Name = "com_LedBaudRate";
            this.com_LedBaudRate.Size = new System.Drawing.Size(98, 23);
            this.com_LedBaudRate.TabIndex = 17;
            // 
            // cmb_LedCustomerDisplaySpecification
            // 
            this.cmb_LedCustomerDisplaySpecification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_LedCustomerDisplaySpecification.FormattingEnabled = true;
            this.cmb_LedCustomerDisplaySpecification.Location = new System.Drawing.Point(99, 55);
            this.cmb_LedCustomerDisplaySpecification.Name = "cmb_LedCustomerDisplaySpecification";
            this.cmb_LedCustomerDisplaySpecification.Size = new System.Drawing.Size(248, 23);
            this.cmb_LedCustomerDisplaySpecification.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 19;
            this.label7.Text = "客显型号：";
            // 
            // SystemSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 531);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_save);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemSetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.gb_money_type.ResumeLayout(false);
            this.gb_money_type.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gb_ledCustomer.ResumeLayout(false);
            this.gb_ledCustomer.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmb_port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_test;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rb_2_line;
        private System.Windows.Forms.RadioButton rb_5_line;
        private System.Windows.Forms.GroupBox gb_money_type;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gb_ledCustomer;
        private System.Windows.Forms.ComboBox com_ledCustomerPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_LedCustomerDisplay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_LedDataBits;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmb_ledCustomerTestType;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rb_LEDCustomer;
        private System.Windows.Forms.RadioButton rb_LCDCustomer;
        private System.Windows.Forms.ComboBox com_LedBaudRate;
        private System.Windows.Forms.ComboBox cmb_LedCustomerDisplaySpecification;
        private System.Windows.Forms.Label label7;
    }
}