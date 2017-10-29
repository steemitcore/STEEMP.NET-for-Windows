using System;
using System.Collections.Generic;
using System.Windows.Forms;
using STEEM.Operations.Enums;
using STEEM.Operations.Post;
using NUnit.Framework;
using STEEM.Helpers;
using System.Net;
using STEEM.Operations.Get;
using Newtonsoft.Json;
using STEEM;
using System.Diagnostics;
using System.IO;

namespace STEEM.Poster
{
    public partial class PostForm : Form
    {
        public Dictionary<string, string> Login;
        public Dictionary<string, ChainInfo> Chain;
        public OperationManager Steem;
        public Dictionary<string, List<byte[]>> UserPrivateKeys;
        public string postedToSteem = null;

        public PostForm()
        {
            InitializeComponent();
        }

      
        public OperationManager Manager(string name)
        {
            switch (name)
            {
                case "Steem":
                    return Steem;

                default:
                    return null;
            }
        }

        public void PostDataToSteem()
        {
            Login = new Dictionary<string, string>()
            {
                {"Steem", txtUsername.Text}
            };

            UserPrivateKeys = new Dictionary<string, List<byte[]>>()
            {
                {"Steem", new List<byte[]> {Base58.GetBytes(txtPassword.Text) }}
            };

            Chain = new Dictionary<string, ChainInfo>();

            var steemChainInfo = ChainManager.GetChainInfo(KnownChains.Steem);
            Chain.Add("Steem", steemChainInfo);
            Steem = new OperationManager(steemChainInfo.Url, steemChainInfo.ChainId);

            var op = new PostOperation(txtTag1.Text, txtUsername.Text, txtTitle.Text, txtResponse.Text, "{\"app\": \"steemit/0.1\", \"tags\": [\"" + txtTag2.Text + "\",\"" + txtTag3.Text + "\",\"" + txtTag4.Text + "\",\"" + txtTag5.Text + "\"]}");
            var prop = Manager("Steem").VerifyAuthority(UserPrivateKeys["Steem"], op);
            var resp = Manager("Steem").GetAccountBandwidth(txtUsername.Text, BandwidthType.Post);
            var Text = JsonConvert.SerializeObject(resp.Result);

            var propa = Manager("Steem").BroadcastOperations(UserPrivateKeys["Steem"], op);

            if (propa.Error == null)
            {
                lblStatus.Text = "Posted to STEEM blockchain, finished";
            }
            else
            {
                lblStatus.Text = propa.Error.Message.ToString();
            }
        }

        public void FollowOperation()
        {
            Login = new Dictionary<string, string>()
            {
                {"Steem", txtUsername.Text}
            };

            UserPrivateKeys = new Dictionary<string, List<byte[]>>()
            {
                {"Steem", new List<byte[]> {Base58.GetBytes(txtPassword.Text) }}
            };

            Chain = new Dictionary<string, ChainInfo>();

            var steemChainInfo = ChainManager.GetChainInfo(KnownChains.Steem);
            Chain.Add("Steem", steemChainInfo);
            Steem = new OperationManager(steemChainInfo.Url, steemChainInfo.ChainId);


            var op = new UnFollowOperation(txtUsername.Text, txtFollower.Text, FollowType.blog, txtUsername.Text);
            var prop = Manager("Steem").VerifyAuthority(UserPrivateKeys["Steem"], op);
            var propa = Manager("Steem").BroadcastOperations(UserPrivateKeys["Steem"], op);

            if (propa.Error == null)
            {
                lblStatus.Text = "Follow user '" + txtFollower.Text + "' transaction successfully completed!";
            }
            else
            {
                lblStatus.Text = "Error follow user: " + propa.Error.Message.ToString();
            }
        }

        public void UnFollowOperation()
        {
            Login = new Dictionary<string, string>()
            {
                {"Steem", txtUsername.Text}
            };

            UserPrivateKeys = new Dictionary<string, List<byte[]>>()
            {
                {"Steem", new List<byte[]> {Base58.GetBytes(txtPassword.Text) }}
            };

            Chain = new Dictionary<string, ChainInfo>();

            var steemChainInfo = ChainManager.GetChainInfo(KnownChains.Steem);
            Chain.Add("Steem", steemChainInfo);
            Steem = new OperationManager(steemChainInfo.Url, steemChainInfo.ChainId);


            var op = new UnFollowOperation(txtUsername.Text, txtFollower.Text, FollowType.ignore, txtUsername.Text);
            var prop = Manager("Steem").VerifyAuthority(UserPrivateKeys["Steem"], op);
            var propa = Manager("Steem").BroadcastOperations(UserPrivateKeys["Steem"], op);

            if (propa.Error == null)
            {
                lblStatus.Text = "Un-follow user '" + txtFollower.Text + "' transaction successfully completed!";
            }
            else
            {
                lblStatus.Text = "Error unfollow user: " + propa.Error.Message.ToString();
            }
        }

        public void PostDataToSteemWithUpvote()
        {
            Login = new Dictionary<string, string>()
            {
                {"Steem", txtUsername.Text}
            };

            UserPrivateKeys = new Dictionary<string, List<byte[]>>()
            {
                {"Steem", new List<byte[]> {Base58.GetBytes(txtPassword.Text) }}
            };

            Chain = new Dictionary<string, ChainInfo>();

            var steemChainInfo = ChainManager.GetChainInfo(KnownChains.Steem);
            Chain.Add("Steem", steemChainInfo);
            Steem = new OperationManager(steemChainInfo.Url, steemChainInfo.ChainId);

            var op = new PostOperation(txtTag1.Text, txtUsername.Text, txtTitle.Text, txtResponse.Text, "{\"app\": \"steemit/0.1\", \"tags\": [\"" + txtTag2.Text + "\",\"" + txtTag3.Text + "\",\"" + txtTag4.Text + "\",\"" + txtTag5.Text + "\"]}");
            var prop = Manager("Steem").VerifyAuthority(UserPrivateKeys["Steem"], op);
            var propa = Manager("Steem").BroadcastOperations(UserPrivateKeys["Steem"], op);

            if (propa.Error == null)
            {
                lblStatus.Text = "'Post to STEEM' transaction successfully completed!";

                var up = new UpVoteOperation(txtUsername.Text, txtUsername.Text, op.Permlink);
                var uprop = Manager("Steem").VerifyAuthority(UserPrivateKeys["Steem"], up);
                var upropa = Manager("Steem").BroadcastOperations(UserPrivateKeys["Steem"], up);
            }
            else
            {
                lblStatus.Text = "Error posting on STEEM: " + propa.Error.Message.ToString();
            }
        }

        public static bool CheckSteemit()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("https://www.steemit.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void BtnPost_Click(object sender, EventArgs e)
        {
           
        }

        private void PostForm_Load(object sender, EventArgs e)
        {
            if (this.txtPassword.Control is TextBox)
            {
                TextBox tb = this.txtPassword.Control as TextBox;
                tb.PasswordChar = '*';
            }

            txtUsername.Focus();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == String.Empty)
            {
                lblStatus.Text = "Please, enter your STEEM user";
                txtUsername.Focus();
                return;
            }

            if (txtPassword.Text.Trim() == String.Empty)
            {
                lblStatus.Text = "Please, enter your STEEM private posting key";
                txtPassword.Focus();
                return;
            }

            if (txtFollower.Text.Trim() != String.Empty)
            {
                FollowOperation();
            }

            else
            {
                lblStatus.Text = "Please, enter STEEM user you wish to follow first";
                txtFollower.Focus();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == String.Empty)
            {
                lblStatus.Text = "Please, enter your STEEM user";
                txtUsername.Focus();
                return;
            }

            if (txtPassword.Text.Trim() == String.Empty)
            {
                lblStatus.Text = "Please, enter your STEEM private posting key";
                txtPassword.Focus();
                return;
            }

            if (txtFollower.Text.Trim() != String.Empty)
            {
                UnFollowOperation();
            }

            else
            {
                lblStatus.Text = "Please, enter STEEM user you wish to unfollow first";
                txtFollower.Focus();
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/steemitcore");
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            Process.Start("http://steemit.com/@steemitcore");
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {
            Process.Start("http://utopian.io");
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (txtResponse.Text.Trim() != String.Empty)
            {
                txtResponse.SelectAll();
                txtResponse.Copy();
                lblStatus.Text = "Body text copyed in clipboard";
            }
            
            else
            {
                lblStatus.Text = "Please, enter text into STEEM POST body first";
            }

        }

        private void btnH1_Click(object sender, EventArgs e)
        {
            txtResponse.Text = txtResponse.Text + "<h1> </h1>";
        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            txtResponse.Text = txtResponse.Text + "<b> </b>";
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            txtResponse.Text = txtResponse.Text + "<i> </i>";
        }

        private void btnCenter_Click(object sender, EventArgs e)
        {
            txtResponse.Text = txtResponse.Text + "<center> </center>";
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            txtTitle.Clear();
            txtResponse.Clear();
            txtTag1.Clear();
            txtTag2.Clear();
            txtTag3.Clear();
            txtTag4.Clear();
            txtTag5.Clear();
            txtTitle.Focus();
            lblStatus.Text = "Post title, body and tags cleared";
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == String.Empty)
            {
                lblStatus.Text = "Please, enter your STEEM user";
                txtUsername.Focus();
                return;
            }

            if (txtPassword.Text.Trim() == String.Empty)
            {
                lblStatus.Text = "Please, enter your STEEM private posting key";
                txtPassword.Focus();
                return;
            }

            if (txtTitle.Text.Trim() == String.Empty)
            {
                lblStatus.Text = "Please, create your STEEM POST title";
                txtTitle.Focus();
                return;
            }

            if (txtResponse.Text.Trim() == String.Empty)
            {
                lblStatus.Text = "Please, create your STEEM POST body";
                txtResponse.Focus();
                return;
            }

            if (txtTag1.Text.Trim() == String.Empty)
            {
                lblStatus.Text = "Please, create your STEEM POST tags";
                txtTag1.Focus();
                return;
            }


            if (chkUpvote.Checked)
            {
                PostDataToSteemWithUpvote();
            }
            else
            {
                PostDataToSteem();
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + "\\about.txt");
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Process.Start("https://steemit.com/steemit/@steemitcore/steem-desktop-app-steemitcore-info-net-v1-05-now-on-github-check-it-out-free-download");
        }
    }
}
