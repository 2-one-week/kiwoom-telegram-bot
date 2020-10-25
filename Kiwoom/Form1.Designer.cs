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
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
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
            this.로그인버튼 = new System.Windows.Forms.Button();
            this.axKHOpenAPI1 = new AxKHOpenAPILib.AxKHOpenAPI();
            this.bannerAds1 = new AdsJumboWinForm.BannerAds();
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).BeginInit();
            this.SuspendLayout();
            // 
            // 로그인버튼
            // 
            this.로그인버튼.Location = new System.Drawing.Point(192, 10);
            this.로그인버튼.Name = "로그인버튼";
            this.로그인버튼.Size = new System.Drawing.Size(93, 31);
            this.로그인버튼.TabIndex = 1;
            this.로그인버튼.Text = "로그인";
            this.로그인버튼.UseVisualStyleBackColor = true;
            this.로그인버튼.Click += new System.EventHandler(this.onClick로그인버튼);
            // 
            // axKHOpenAPI1
            // 
            this.axKHOpenAPI1.Enabled = true;
            this.axKHOpenAPI1.Location = new System.Drawing.Point(13, 12);
            this.axKHOpenAPI1.Name = "axKHOpenAPI1";
            this.axKHOpenAPI1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKHOpenAPI1.OcxState")));
            this.axKHOpenAPI1.Size = new System.Drawing.Size(136, 42);
            this.axKHOpenAPI1.TabIndex = 0;
            // 
            // bannerAds1
            // 
            this.bannerAds1.ApplicationId = null;
            this.bannerAds1.BackColor = System.Drawing.Color.White;
            this.bannerAds1.HeightAd = 0;
            this.bannerAds1.Location = new System.Drawing.Point(13, 103);
            this.bannerAds1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.bannerAds1.Name = "bannerAds1";
            this.bannerAds1.Size = new System.Drawing.Size(728, 90);
            this.bannerAds1.TabIndex = 2;
            this.bannerAds1.WidthAd = 0;
            // 
            // Kiwoom
            // 
            this.ClientSize = new System.Drawing.Size(763, 197);
            this.Controls.Add(this.bannerAds1);
            this.Controls.Add(this.로그인버튼);
            this.Controls.Add(this.axKHOpenAPI1);
            this.Name = "Kiwoom";
            this.Load += new System.EventHandler(this.Kiwoom_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axKHOpenAPI1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private System.Windows.Forms.Button 로그인버튼;
        private AdsJumboWinForm.BannerAds bannerAds1;
    }
}

