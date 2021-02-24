using System.Drawing;

namespace Kiwoom
{
    partial class Kiwoom
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name='disposing'>관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Kiwoom));
            this.axKHOpenAPI1 = new AxKHOpenAPILib.AxKHOpenAPI();
            this.프로그램타이틀 = new System.Windows.Forms.Label();
            this.로그인버튼 = new MetroFramework.Controls.MetroButton();
            this.log = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // axKHOpenAPI1
            // 
            this.axKHOpenAPI1.Enabled = true;
            this.axKHOpenAPI1.Location = new System.Drawing.Point(887, 701);
            this.axKHOpenAPI1.Name = "axKHOpenAPI1";
            this.axKHOpenAPI1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKHOpenAPI1.OcxState")));
            this.axKHOpenAPI1.Size = new System.Drawing.Size(75, 44);
            this.axKHOpenAPI1.TabIndex = 0;
            // 
            // 프로그램타이틀
            // 
            this.프로그램타이틀.AutoSize = true;
            this.프로그램타이틀.Font = new System.Drawing.Font("굴림", 15F);
            this.프로그램타이틀.Location = new System.Drawing.Point(733, 18);
            this.프로그램타이틀.Name = "프로그램타이틀";
            this.프로그램타이틀.Size = new System.Drawing.Size(229, 40);
            this.프로그램타이틀.TabIndex = 3;
            this.프로그램타이틀.Text = "주파고 v2.0";
            // 
            // 로그인버튼
            // 
            this.로그인버튼.Location = new System.Drawing.Point(726, 701);
            this.로그인버튼.Name = "로그인버튼";
            this.로그인버튼.Size = new System.Drawing.Size(146, 44);
            this.로그인버튼.TabIndex = 4;
            this.로그인버튼.Text = "로그인버튼";
            // 
            // log
            // 
            this.log.FormattingEnabled = true;
            this.log.ItemHeight = 24;
            this.log.Location = new System.Drawing.Point(726, 72);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(236, 604);
            this.log.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("굴림", 9F);
            this.button1.Location = new System.Drawing.Point(582, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 43);
            this.button1.TabIndex = 6;
            this.button1.Text = "추가";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("굴림", 11F);
            this.textBox1.Location = new System.Drawing.Point(140, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(421, 41);
            this.textBox1.TabIndex = 7;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(27, 72);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 82;
            this.dataGridView1.RowTemplate.Height = 37;
            this.dataGridView1.Size = new System.Drawing.Size(672, 672);
            this.dataGridView1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 11F);
            this.label1.Location = new System.Drawing.Point(31, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 30);
            this.label1.TabIndex = 9;
            this.label1.Text = "종목명";
            // 
            // Kiwoom
            // 
            this.ClientSize = new System.Drawing.Size(974, 782);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.로그인버튼);
            this.Controls.Add(this.프로그램타이틀);
            this.Controls.Add(this.log);
            this.Controls.Add(this.axKHOpenAPI1);
            this.Name = "Kiwoom";
            this.Load += new System.EventHandler(this.Kiwoom_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void 로그인버튼_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private System.Windows.Forms.Label 프로그램타이틀;
        private MetroFramework.Controls.MetroButton 로그인버튼;
        public System.Windows.Forms.ListBox log;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
    }
}

