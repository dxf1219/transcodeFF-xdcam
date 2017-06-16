namespace transcodeFF
{
    partial class Form_Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label_time = new System.Windows.Forms.Label();
            this.label_process = new System.Windows.Forms.Label();
            this.progressBar_value = new System.Windows.Forms.ProgressBar();
            this.textBox_nowtask = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_dest = new System.Windows.Forms.TextBox();
            this.textBox_src = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_timer = new System.Windows.Forms.ToolStripStatusLabel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "源路径";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label_time);
            this.splitContainer1.Panel1.Controls.Add(this.label_process);
            this.splitContainer1.Panel1.Controls.Add(this.progressBar_value);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_nowtask);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_dest);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_src);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(860, 593);
            this.splitContainer1.SplitterDistance = 147;
            this.splitContainer1.TabIndex = 1;
            // 
            // label_time
            // 
            this.label_time.AutoSize = true;
            this.label_time.Location = new System.Drawing.Point(553, 110);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size(52, 13);
            this.label_time.TabIndex = 8;
            this.label_time.Text = "总耗时:0";
            // 
            // label_process
            // 
            this.label_process.AutoSize = true;
            this.label_process.Location = new System.Drawing.Point(23, 107);
            this.label_process.Name = "label_process";
            this.label_process.Size = new System.Drawing.Size(34, 13);
            this.label_process.TabIndex = 7;
            this.label_process.Text = "进度:";
            // 
            // progressBar_value
            // 
            this.progressBar_value.Location = new System.Drawing.Point(84, 101);
            this.progressBar_value.Name = "progressBar_value";
            this.progressBar_value.Size = new System.Drawing.Size(452, 23);
            this.progressBar_value.TabIndex = 6;
            // 
            // textBox_nowtask
            // 
            this.textBox_nowtask.Location = new System.Drawing.Point(84, 71);
            this.textBox_nowtask.Name = "textBox_nowtask";
            this.textBox_nowtask.Size = new System.Drawing.Size(449, 20);
            this.textBox_nowtask.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "当前任务:";
            // 
            // textBox_dest
            // 
            this.textBox_dest.Location = new System.Drawing.Point(87, 40);
            this.textBox_dest.Name = "textBox_dest";
            this.textBox_dest.Size = new System.Drawing.Size(449, 20);
            this.textBox_dest.TabIndex = 3;
            // 
            // textBox_src
            // 
            this.textBox_src.Location = new System.Drawing.Point(87, 14);
            this.textBox_src.Name = "textBox_src";
            this.textBox_src.Size = new System.Drawing.Size(449, 20);
            this.textBox_src.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "目标路径";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel_timer});
            this.statusStrip1.Location = new System.Drawing.Point(0, 420);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(860, 22);
            this.statusStrip1.TabIndex = 42;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(58, 17);
            this.toolStripStatusLabel1.Text = "当前时间:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(13, 17);
            this.toolStripStatusLabel2.Text = "||";
            // 
            // toolStripStatusLabel_timer
            // 
            this.toolStripStatusLabel_timer.Name = "toolStripStatusLabel_timer";
            this.toolStripStatusLabel_timer.Size = new System.Drawing.Size(144, 17);
            this.toolStripStatusLabel_timer.Text = "toolStripStatusLabel_TIME";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(860, 442);
            this.richTextBox1.TabIndex = 41;
            this.richTextBox1.Text = "";
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 593);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Main";
            this.Text = "Transcoder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_nowtask;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_dest;
        private System.Windows.Forms.TextBox textBox_src;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_timer;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label_process;
        private System.Windows.Forms.ProgressBar progressBar_value;
        private System.Windows.Forms.Label label_time;
    }
}

