namespace SunacCADApp.Test
{
    partial class MainForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button_audit = new System.Windows.Forms.Button();
            this.btn_Rework = new System.Windows.Forms.Button();
            this.btnApproveClose = new System.Windows.Forms.Button();
            this.btn_login = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button01_CreateResult = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(71, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "通过";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(71, 64);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(81, 23);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "接口测试";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(275, 24);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "用户信息";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_audit
            // 
            this.button_audit.Location = new System.Drawing.Point(71, 146);
            this.button_audit.Name = "button_audit";
            this.button_audit.Size = new System.Drawing.Size(279, 23);
            this.button_audit.TabIndex = 3;
            this.button_audit.Text = "2)流程审批(通过)，审批记录知会到业务系统";
            this.button_audit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_audit.UseVisualStyleBackColor = true;
            this.button_audit.Click += new System.EventHandler(this.button_audit_Click);
            // 
            // btn_Rework
            // 
            this.btn_Rework.Location = new System.Drawing.Point(71, 184);
            this.btn_Rework.Name = "btn_Rework";
            this.btn_Rework.Size = new System.Drawing.Size(279, 23);
            this.btn_Rework.TabIndex = 4;
            this.btn_Rework.Text = "3)\t流程审批(退回、发起人取消）";
            this.btn_Rework.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Rework.UseVisualStyleBackColor = true;
            this.btn_Rework.Click += new System.EventHandler(this.btn_Rework_Click);
            // 
            // btnApproveClose
            // 
            this.btnApproveClose.Location = new System.Drawing.Point(71, 224);
            this.btnApproveClose.Name = "btnApproveClose";
            this.btnApproveClose.Size = new System.Drawing.Size(279, 23);
            this.btnApproveClose.TabIndex = 5;
            this.btnApproveClose.Text = "4)流程审批结束(通过、拒绝、作废)";
            this.btnApproveClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApproveClose.UseVisualStyleBackColor = true;
            this.btnApproveClose.Click += new System.EventHandler(this.btnApproveClose_Click);
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(177, 24);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(75, 23);
            this.btn_login.TabIndex = 6;
            this.btn_login.Text = "用户登陆测试";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(177, 64);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(275, 64);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 8;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button01_CreateResult
            // 
            this.button01_CreateResult.Location = new System.Drawing.Point(73, 112);
            this.button01_CreateResult.Name = "button01_CreateResult";
            this.button01_CreateResult.Size = new System.Drawing.Size(277, 23);
            this.button01_CreateResult.TabIndex = 9;
            this.button01_CreateResult.Text = "1)流程发起成功，向业务系统提交创建结果";
            this.button01_CreateResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button01_CreateResult.UseVisualStyleBackColor = true;
            this.button01_CreateResult.Click += new System.EventHandler(this.button01_CreateResult_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(71, 273);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(279, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "6IDM 用户信息同步";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 398);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button01_CreateResult);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.btnApproveClose);
            this.Controls.Add(this.btn_Rework);
            this.Controls.Add(this.button_audit);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "Form测试主界面";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button_audit;
        private System.Windows.Forms.Button btn_Rework;
        private System.Windows.Forms.Button btnApproveClose;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button01_CreateResult;
        private System.Windows.Forms.Button button3;
    }
}

