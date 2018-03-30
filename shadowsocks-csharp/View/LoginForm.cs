using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shadowsocks.Controller;
using Shadowsocks.Model;
using Shadowsocks.Properties;
using System.Net;

using Newtonsoft.Json;
using System.IO;

namespace Shadowsocks.View
{
    public partial class LoginForm : Form
    {
        private ShadowsocksController controller;

        private const string REQUEST_URL = "http://192.168.0.123:4000/login";

        private string account;
        private string password;

        public LoginForm(ShadowsocksController controller)
        {
            InitializeComponent();

            this.controller = controller;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            this.account = AccountText.Text;
            this.password = PasswordText.Text;

            string result = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(REQUEST_URL);

                req.Method = "POST";
                req.ContentType = "application/json";

                string postData = "{\"account\":\"" + this.account + "\", \"password\": \"" + this.password + "\"}";

                byte[] data = Encoding.UTF8.GetBytes(postData);

                req.ContentLength = data.Length;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);

                    reqStream.Close();
                }

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                Stream stream = resp.GetResponseStream();

                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }

                Logging.Debug(result);
            }
            catch (Exception error)
            {
                Logging.Error(error);
            }
            

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

    }
}
