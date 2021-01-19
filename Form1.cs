using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using MongoDB.Driver;
using MongoDB.Bson;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private string strIP = "Your IP";
        private string fileContent = "start Bin\\launcher.exe -masterip Your IP -lang schinese -enablecustom";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //PingIp(strIP);
            //label1.Visible = false;
            //System.Threading.Thread.Sleep(10000);
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            label1.Text = "正在连接服务器，请稍等";
            
            PingIp(strIP);
        }
        /// <summary>
          /// ping ip,测试能否ping通
          /// </summary>
          /// <param name="strIP">IP地址</param>
          /// <returns></returns>
          public void PingIp(string strIP)
          {
              //bool bRet = false;
              try
             {
                 Ping pingSend = new Ping();
                 PingReply reply = pingSend.Send(strIP, 1000);
                if (reply.Status == IPStatus.Success)
                {//bRet = true;
                    System.Diagnostics.Debug.WriteLine("Success");
                    MessageBox.Show("服务器连接成功", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;
                    label1.Text = "服务器连接正常";
                    //label2.Text = reply.ToString();
                    label2.Visible = true;
                    label2.Text = "Ping:" + reply.RoundtripTime.ToString() + "ms";
                    label3.Visible = true;
                    button2.Visible = true;
                    button4.Visible = true;
                    db();
                }
                else 
                {
                    System.Diagnostics.Debug.WriteLine("Failed");
                    MessageBox.Show("服务器连接失败", "坏起来了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label1.Text = "连接超时 服务器可能\n\n离线或者本机网络连接\n\n出现故障";
                    button5.Visible = true;
                }
                    
            }
             catch (Exception e)
             {
                //bRet = false;
                //MessageBox.Show("ip格式不正确\n连ip格式都能写错\nnt一定是个啥b", "坏起来了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(e.Message.ToString(), "坏起来了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine("Failed");
            }
             //return bRet;
         }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "正在检测服务器连接";
            PingIp(strIP);
        }
        public void writeBATFile(string fileContent)
        {
            string filePath = @"start.bat";
            if (!File.Exists(filePath))
            {
                FileStream fs1 = new FileStream(filePath, FileMode.Create, FileAccess.Write);//创建写入文件
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine(fileContent);//开始写入值
                sw.Close();
                fs1.Close();
                System.Diagnostics.Process.Start("start.bat");
                Application.Exit();
            }
            else
            {
                /*
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(fileContent);//开始写入值
                sr.Close();
                fs.Close();*/
                System.Diagnostics.Process.Start("start.bat");
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            writeBATFile(fileContent);
        }
        /// <summary>
        /// 数据库连接
        /// </summary>
        private const string conn = "mongodb://Your IP:27017";
        /// <summary>
        /// 指定数据库
        /// </summary>
        private const string dbName = "cso2";
        /// <summary>
        /// 指定集合(使用BsonDocument直接定义 不需要了)
        /// </summary>
        //private const string collectionName = "usersessions";
        private void db()
        {
            var client = new MongoDB.Driver.MongoClient(conn);
            IMongoDatabase db = client.GetDatabase(dbName);
            var collection = db.GetCollection<BsonDocument>("usersessions");
            //label3.Text = collection.ToString();
            FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
            //总条目数
            var result = collection.Find<BsonDocument>(builderFilter.Empty).Count();
            
            Console.WriteLine(result);
            if (result == 0)
            {
                label3.Text = "当前没有玩家在线";
            }
            else 
            {
                label3.Text = "当前有" + result.ToString() + "名玩家在线";
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            db();
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("因为nt是条懒狗 所以这里什么也没有", "坏起来了", MessageBoxButtons.OK);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("by:misiu29\nVersion:1.0", "女子", MessageBoxButtons.OK);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
