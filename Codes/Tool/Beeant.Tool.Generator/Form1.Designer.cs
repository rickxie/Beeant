namespace Beeant.Tool.Generator
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtSqlCon = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTable = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.中文 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.文件属性Byte名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.是否图片 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.是否附件 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.是否枚举 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.cbSite = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbTemplate = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTemplate = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据库";
            // 
            // txtSqlCon
            // 
            this.txtSqlCon.Location = new System.Drawing.Point(102, 28);
            this.txtSqlCon.Name = "txtSqlCon";
            this.txtSqlCon.Size = new System.Drawing.Size(480, 21);
            this.txtSqlCon.TabIndex = 1;
            this.txtSqlCon.Text = "server=.\\SQL2016;uid=sa;pwd=1;database=Beeant;Pooling=true;";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(864, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "加载";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(601, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "表";
            // 
            // txtTable
            // 
            this.txtTable.Location = new System.Drawing.Point(624, 30);
            this.txtTable.Name = "txtTable";
            this.txtTable.Size = new System.Drawing.Size(190, 21);
            this.txtTable.TabIndex = 4;
            this.txtTable.Text = "t_";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.名称,
            this.中文,
            this.文件属性Byte名称,
            this.是否图片,
            this.是否附件,
            this.是否枚举});
            this.dataGridView1.Location = new System.Drawing.Point(30, 76);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(909, 281);
            this.dataGridView1.TabIndex = 5;
            // 
            // 名称
            // 
            this.名称.HeaderText = "名称";
            this.名称.Name = "名称";
            this.名称.ReadOnly = true;
            // 
            // 中文
            // 
            this.中文.HeaderText = "中文";
            this.中文.Name = "中文";
            this.中文.ReadOnly = true;
            // 
            // 文件属性Byte名称
            // 
            this.文件属性Byte名称.HeaderText = "文件属性Byte名称";
            this.文件属性Byte名称.Name = "文件属性Byte名称";
            this.文件属性Byte名称.ReadOnly = true;
            this.文件属性Byte名称.Width = 200;
            // 
            // 是否图片
            // 
            this.是否图片.HeaderText = "是否图片";
            this.是否图片.Name = "是否图片";
            this.是否图片.ReadOnly = true;
            // 
            // 是否附件
            // 
            this.是否附件.HeaderText = "是否附件";
            this.是否附件.Name = "是否附件";
            this.是否附件.ReadOnly = true;
            // 
            // 是否枚举
            // 
            this.是否枚举.HeaderText = "是否枚举";
            this.是否枚举.Name = "是否枚举";
            this.是否枚举.ReadOnly = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(30, 380);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(132, 16);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "是否生成Appliction";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(411, 812);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "生成";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // cbSite
            // 
            this.cbSite.FormattingEnabled = true;
            this.cbSite.Location = new System.Drawing.Point(309, 381);
            this.cbSite.Name = "cbSite";
            this.cbSite.Size = new System.Drawing.Size(121, 20);
            this.cbSite.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(235, 383);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "添加站点";
            // 
            // cbTemplate
            // 
            this.cbTemplate.FormattingEnabled = true;
            this.cbTemplate.Location = new System.Drawing.Point(659, 388);
            this.cbTemplate.Name = "cbTemplate";
            this.cbTemplate.Size = new System.Drawing.Size(121, 20);
            this.cbTemplate.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(601, 391);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "模板";
            // 
            // txtTemplate
            // 
            this.txtTemplate.Location = new System.Drawing.Point(30, 427);
            this.txtTemplate.Multiline = true;
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.Size = new System.Drawing.Size(909, 366);
            this.txtTemplate.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 865);
            this.Controls.Add(this.txtTemplate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbTemplate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbSite);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtTable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSqlCon);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSqlCon;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTable;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 中文;
        private System.Windows.Forms.DataGridViewTextBoxColumn 文件属性Byte名称;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 是否图片;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 是否附件;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 是否枚举;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cbSite;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbTemplate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTemplate;
    }
}

