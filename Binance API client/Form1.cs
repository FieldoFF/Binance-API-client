using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Binance_API_client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private long GetTimestamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }
        public static string CreateSignature(string prmetrs, string secret)
        {
            byte[] bkey = Encoding.Default.GetBytes(secret);
            using (var hmac = new HMACSHA256(bkey))
            {
                byte[] bstr = Encoding.Default.GetBytes(prmetrs);
                var bhash = hmac.ComputeHash(bstr);
                return BitConverter.ToString(bhash).Replace("-", string.Empty).ToLower();
            }
        }

        public static string QuerySender(string api, string secret, string baseurl, string url, string prmetrs, string queryType)
        {
            string ans;
            string query = baseurl + url + "?" + prmetrs + "&signature=" + CreateSignature(prmetrs, secret);
            WebRequest req = WebRequest.Create(@query);
            req.Method = queryType;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers.Add("X-MBX-APIKEY: " + api);

            WebResponse response = req.GetResponse();
            using (Stream s = response.GetResponseStream()) //Пишем в поток
            {
                using (StreamReader r = new StreamReader(s)) //Читаем из потока
                {
                    ans = r.ReadToEnd();  //Выводим в textbox1
                }
            }
            response.Close(); //Закрываем поток*/
            return ans;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = QuerySender(textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text+ GetTimestamp().ToString(), comboBox1.SelectedItem.ToString());
        }
    }
}
