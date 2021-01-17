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
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).BeginInit();
            this.SuspendLayout();
            // 
            // axKHOpenAPI1
            // 
            this.axKHOpenAPI1.Enabled = true;
            this.axKHOpenAPI1.Location = new System.Drawing.Point(12, 21);
            this.axKHOpenAPI1.Name = "axKHOpenAPI1";
            this.axKHOpenAPI1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKHOpenAPI1.OcxState")));
            this.axKHOpenAPI1.Size = new System.Drawing.Size(80, 22);
            this.axKHOpenAPI1.TabIndex = 0;
            // 
            // 프로그램타이틀
            // 
            this.프로그램타이틀.AutoSize = true;
            this.프로그램타이틀.Location = new System.Drawing.Point(269, 21);
            this.프로그램타이틀.Name = "프로그램타이틀";
            this.프로그램타이틀.Size = new System.Drawing.Size(65, 12);
            this.프로그램타이틀.TabIndex = 3;
            this.프로그램타이틀.Text = "생활비벌자";
            // 
            // 로그인버튼
            // 
            this.로그인버튼.Location = new System.Drawing.Point(510, 12);
            this.로그인버튼.Name = "로그인버튼";
            this.로그인버튼.Size = new System.Drawing.Size(96, 31);
            this.로그인버튼.TabIndex = 4;
            this.로그인버튼.Text = "로그인버튼";
            this.로그인버튼.Click += onClick로그인버튼;
            // 
            // log
            // 
            this.log.FormattingEnabled = true;
            this.log.ItemHeight = 12;
            this.log.Location = new System.Drawing.Point(12, 61);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(594, 628);
            this.log.TabIndex = 2;
            // 
            // Kiwoom
            // 
            this.ClientSize = new System.Drawing.Size(609, 706);
            this.Controls.Add(this.로그인버튼);
            this.Controls.Add(this.프로그램타이틀);
            this.Controls.Add(this.log);
            this.Controls.Add(this.axKHOpenAPI1);
            this.Name = "Kiwoom";
            this.Load += new System.EventHandler(this.Kiwoom_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).EndInit();
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
    }
}

