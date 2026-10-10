using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace TaqiBrowser
{
    public class MainForm : Form
    {
        private TextBox urlBar;
        private Button btnGo, btnBack, btnForward, btnRefresh;
        private WebView2 webView;
        private Label headerLabel;

        public MainForm()
        {
            // Window Configuration
            this.Text = "TAQI Browser - Created by Muhammad Taqi";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);

            // Header Branding Banner
            headerLabel = new Label() 
            { 
                Text = "TAQI BROWSER  |  Created by Muhammad Taqi", 
                Dock = DockStyle.Top, 
                Height = 28, 
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(20, 20, 20),
                ForeColor = Color.Cyan,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            // Navigation Top Bar
            Panel navPanel = new Panel() 
            { 
                Dock = DockStyle.Top, 
                Height = 42, 
                BackColor = Color.FromArgb(45, 45, 48),
                Padding = new Padding(5)
            };

            btnBack = new Button() { Text = "◄", Width = 35, Left = 5, Top = 6, Height = 30, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            btnForward = new Button() { Text = "►", Width = 35, Left = 45, Top = 6, Height = 30, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            btnRefresh = new Button() { Text = "↻", Width = 35, Left = 85, Top = 6, Height = 30, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            
            urlBar = new TextBox() 
            { 
                Left = 128, 
                Top = 8, 
                Width = 900, 
                Height = 28,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top, 
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(25, 25, 25),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            btnGo = new Button() 
            { 
                Text = "Go / Search", 
                Left = 1035, 
                Top = 6, 
                Width = 120, 
                Height = 30, 
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            navPanel.Controls.AddRange(new Control[] { btnBack, btnForward, btnRefresh, urlBar, btnGo });

            // Chromium WebView2 Engine
            webView = new WebView2() { Dock = DockStyle.Fill };

            this.Controls.Add(webView);
            this.Controls.Add(navPanel);
            this.Controls.Add(headerLabel);

            // Button Click & Keyboard Events
            btnGo.Click += (s, e) => NavigateToUrl();
            urlBar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) NavigateToUrl(); };
            btnBack.Click += (s, e) => { if (webView.CoreWebView2 != null && webView.CoreWebView2.CanGoBack) webView.CoreWebView2.GoBack(); };
            btnForward.Click += (s, e) => { if (webView.CoreWebView2 != null && webView.CoreWebView2.CanGoForward) webView.CoreWebView2.GoForward(); };
            btnRefresh.Click += (s, e) => { if (webView.CoreWebView2 != null) webView.CoreWebView2.Reload(); };

            InitializeBrowser();
        }

        private async void InitializeBrowser()
        {
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.Navigate("https://www.google.com");

            // Sync URL bar on navigation
            webView.CoreWebView2.SourceChanged += (s, e) => {
                urlBar.Text = webView.CoreWebView2.Source;
            };
        }

        private void NavigateToUrl()
        {
            if (webView.CoreWebView2 == null) return;

            string query = urlBar.Text.Trim();
            if (string.IsNullOrEmpty(query)) return;

            if (!query.StartsWith("http://") && !query.StartsWith("https://"))
            {
                if (query.Contains(".") && !query.Contains(" "))
                    query = "https://" + query;
                else
                    query = "https://www.google.com/search?q=" + Uri.EscapeDataString(query);
            }

            webView.CoreWebView2.Navigate(query);
        }
    }

    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
