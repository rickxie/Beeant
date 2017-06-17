namespace Beeant.Distributed.Service.Task
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSwich = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.btnClearCache = new System.Windows.Forms.Button();
            this.ckTask = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnReLoad = new System.Windows.Forms.Button();
            this.btnUnSelectAll = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExcute = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSwich
            // 
            this.btnSwich.Location = new System.Drawing.Point(299, 25);
            this.btnSwich.Name = "btnSwich";
            this.btnSwich.Size = new System.Drawing.Size(75, 23);
            this.btnSwich.TabIndex = 0;
            this.btnSwich.Text = "开启";
            this.btnSwich.UseVisualStyleBackColor = true;
            this.btnSwich.Click += new System.EventHandler(this.btnSwich_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(170, 25);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(108, 21);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "调度时间（单位毫秒）";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "加载配置缓存时间（单位秒）";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(170, 67);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(108, 21);
            this.numericUpDown2.TabIndex = 4;
            this.numericUpDown2.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // btnClearCache
            // 
            this.btnClearCache.Location = new System.Drawing.Point(299, 64);
            this.btnClearCache.Name = "btnClearCache";
            this.btnClearCache.Size = new System.Drawing.Size(75, 23);
            this.btnClearCache.TabIndex = 5;
            this.btnClearCache.Text = "清除缓存";
            this.btnClearCache.UseVisualStyleBackColor = true;
            this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
            // 
            // ckTask
            // 
            this.ckTask.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ckTask.FormattingEnabled = true;
            this.ckTask.Location = new System.Drawing.Point(0, 132);
            this.ckTask.Name = "ckTask";
            this.ckTask.Size = new System.Drawing.Size(464, 212);
            this.ckTask.TabIndex = 6;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(0, 99);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 7;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnReLoad
            // 
            this.btnReLoad.Location = new System.Drawing.Point(261, 99);
            this.btnReLoad.Name = "btnReLoad";
            this.btnReLoad.Size = new System.Drawing.Size(75, 23);
            this.btnReLoad.TabIndex = 8;
            this.btnReLoad.Text = "重新加载";
            this.btnReLoad.UseVisualStyleBackColor = true;
            this.btnReLoad.Click += new System.EventHandler(this.btnReLoad_Click);
            // 
            // btnUnSelectAll
            // 
            this.btnUnSelectAll.Location = new System.Drawing.Point(81, 99);
            this.btnUnSelectAll.Name = "btnUnSelectAll";
            this.btnUnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnUnSelectAll.TabIndex = 9;
            this.btnUnSelectAll.Text = "反选";
            this.btnUnSelectAll.UseVisualStyleBackColor = true;
            this.btnUnSelectAll.Click += new System.EventHandler(this.btnUnSelectAll_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(170, 99);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExcute
            // 
            this.btnExcute.Location = new System.Drawing.Point(343, 98);
            this.btnExcute.Name = "btnExcute";
            this.btnExcute.Size = new System.Drawing.Size(75, 23);
            this.btnExcute.TabIndex = 11;
            this.btnExcute.Text = "立即执行";
            this.btnExcute.UseVisualStyleBackColor = true;
            this.btnExcute.Click += new System.EventHandler(this.btnExcute_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 344);
            this.Controls.Add(this.btnExcute);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUnSelectAll);
            this.Controls.Add(this.btnReLoad);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.ckTask);
            this.Controls.Add(this.btnClearCache);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.btnSwich);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "服务调度";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSwich;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Button btnClearCache;
        private System.Windows.Forms.CheckedListBox ckTask;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnReLoad;
        private System.Windows.Forms.Button btnUnSelectAll;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExcute;
    }
}

