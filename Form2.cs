using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taihu
{
    public partial class Form2 : Form
    {
        ComboBox combo1;
        ComboBox combo2;
        DateTimePicker date;
        TextBox text1;
        private Point mouse_offset;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "2020" && textBox2.Text == "2020")
            {
                SQLModify();
            }
            else
            {
                MessageBox.Show("对不起，您的密码错误，请重新输入");
            }
        }

        private void SQLModify()
        {
            this.Controls.Clear();
            this.TopMost = true;
            this.Text = "数据库维护系统";
            this.Height = 210;
            this.Width = 260;
            Panel pPanel = new Panel();
            pPanel.Height = 150;
            pPanel.Width = 220;
            pPanel.Top = 8;
            pPanel.Left = 12;
            pPanel.BackColor = Color.LightBlue;
            pPanel.Visible = true;
            Label lab1 = new Label();
            lab1.Text = "日期:";
            lab1.Top = 10;
            lab1.Left = 10;
            lab1.Width = 50;
            date = new DateTimePicker();
            date.Top = 6;
            date.Left = 70;
            date.Width = 125;
            Label lab2 = new Label();
            lab2.Text = "表名：";
            lab2.Top = 39;
            lab2.Left = 10;
            lab2.Width = 75;
            combo1 = new ComboBox();
            text1 = new TextBox();
            text1.Top = 93;
            text1.Left = 70;
            text1.Width = 125;
            Label lab3 = new Label();
            Label lab4 = new Label();
            lab4.Text = "站点：";
            lab4.Top = 64;
            lab4.Left = 10;
            lab4.Width = 60;
            lab3.Text = "数值：";
            lab3.Top = 97;
            lab3.Left = 10;
            lab3.Width = 58;
            combo2 = new ComboBox();
            combo2.Top = 64;
            combo2.Left = 70;
            combo2.Width = 125;
            combo1.Items.Add("太湖五站");
            combo1.Items.Add("重要站点水位");
            combo1.Items.Add("重要站点降雨量");
            combo1.Items.Add("引排水量");
            combo1.Top = 35;
            combo1.Left = 70;
            combo1.Width = 125;
            pPanel.Controls.Add(combo1);
            pPanel.Controls.Add(lab4);
            pPanel.Controls.Add(combo2);
            pPanel.Controls.Add(lab3);
            pPanel.Controls.Add(text1);
            pPanel.Controls.Add(date);
            Button b2 = new Button();
            b2.Top = 119;
            b2.Left = 65;
            b2.Text = "导入";
            b2.Width = 50;
            Button b1 = new Button();
            b1.Top = 119;
            b1.Width = 100;
            b1.Text = "上传数据";
            b1.Left = 120;
            Button b3 = new Button();
            b3.Top = 119;
            b3.Left = 10;
            b3.Width = 50;
            b3.Text = "删除";
            pPanel.Controls.Add(b3);
            text1.TabIndex = 4;
            combo2.TabIndex = 3;
            combo1.TabIndex = 2;
            pPanel.Controls.Add(b1);
            pPanel.Controls.Add(lab2);
            pPanel.Controls.Add(lab1);
            pPanel.Controls.Add(b2);
            this.Controls.Add(pPanel);
            combo1.SelectedIndexChanged += new EventHandler(combo1_SelectedIndexChanged);
            b3.Click += new EventHandler(b3_Click);
            b2.Click += new EventHandler(b2_Click);
            pPanel.MouseDown += new MouseEventHandler(pPanel_MouseDown);
            pPanel.MouseUp += new MouseEventHandler(pPanel_MouseUp);
            b1.Click += new EventHandler(b1_Click);
        }

        private void b3_Click(object sender, EventArgs e)
        {
            string connString = "server=47.102.138.168;database=GIS;uid=GISJQQ;pwd=Jqq963741ArcObjects";
            SqlConnection SC = new SqlConnection(connString);
            SC.Open();//数据库建立连接
            SqlCommand SCmd = new SqlCommand();
            SCmd.Connection = SC;
            SCmd.CommandType = CommandType.Text;
            if (combo1.SelectedIndex == 0)//基于4个不同类型进行上传操作
            {
                SCmd.CommandText = "delete from TH5 where WaterH='"+combo2.SelectedItem+"'and Date='"+date.Value.ToString("yyyy-MM-dd")+"'";
            }
            else if (combo1.SelectedIndex == 1)
            {
                SCmd.CommandText = "delete from ZDSW where site='" + combo2.SelectedItem +"'and date='" + date.Value.ToString("yyyy-MM-dd")+"'";
            }
            else if (combo1.SelectedIndex == 2)
            {
                SCmd.CommandText = "delete from ZDSW where site='" + combo2.SelectedItem + "'and date='" + date.Value.ToString("yyyy-MM-dd")+"'";
            }
            else
            {
                SCmd.CommandText = "delete from YPSL_3 where YPSL='" + combo2.SelectedItem + "'and Date='" + date.Value.ToString("yyyy-MM-dd")+ "'";
            }
            SCmd.ExecuteNonQuery();
            SC.Close();//关闭数据库
            MessageBox.Show("删除记录成功！");
        }

        private void combo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combo1.SelectedIndex == 0)
            {
                combo2.Items.Clear();
                combo2.Items.Add("望亭太");
                combo2.Items.Add("太浦口");
                combo2.Items.Add("西山");
                combo2.Items.Add("夹浦");
                combo2.Items.Add("小梅口");
                combo2.Items.Add("太湖平均");
            }
            else if (combo1.SelectedIndex == 1|| combo2.SelectedIndex==2)
            {
                combo2.Items.Clear();
                combo2.Items.Add("常州");
                combo2.Items.Add("无锡南门");
                combo2.Items.Add("杭长桥");
                combo2.Items.Add("甘露");
                combo2.Items.Add("嘉兴");
                combo2.Items.Add("平望");
                combo2.Items.Add("琳桥");
                combo2.Items.Add("苏州");
            }
            else if(combo1.SelectedIndex == 3)
            {
                combo2.Items.Clear();
                combo2.Items.Add("常熟枢纽");
                combo2.Items.Add("望亭枢纽");
                combo2.Items.Add("太浦闸");
            }
        }

        private void b2_Click(object sender, EventArgs e)
        {
            String sheetName = Interaction.InputBox("请输入你想要创建的表名：", "创建数据库$表", "", -1,-1);
            //将excel中的数据导入到sqlserver的数据库中，如果sql中的数据表不存在则创建        
            string connString = "server=47.102.138.168;database=GIS;uid=GISJQQ;pwd=Jqq963741ArcObjects";
            System.Windows.Forms.OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "请选择您所需要的Excel表格";
            fd.Filter = "表格类型(*.xls)|*.xls";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                DataSet ds = new DataSet();
                try
                {
                    string strConn = "Provider = Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fd.FileName 
                        + ";" + "Extended Properties = Excel 8.0;";//连接所需要被导入的Excel表格
                    OleDbConnection conn = new OleDbConnection(strConn);
                    conn.Open();//建立连接
                 string strExcel = "";
                    OleDbDataAdapter myCommand = null;//初始化查询语句
                    strExcel = string.Format("select * from [{0}$]", sheetName);
                    myCommand = new OleDbDataAdapter(strExcel, strConn);//执行查询
                    myCommand.Fill(ds, sheetName);//将表格中的数据传递给ds数据集
                    string strSql = string.Format
                        ("if not exists(select * from sysobjects where name = '{0}') create table {0}(", sheetName); 
                    foreach (System.Data.DataColumn c in ds.Tables[0].Columns)//遍历ds数据集
                    {
                        strSql += string.Format("[{0}] varchar(255),", c.ColumnName);
                    }
                    strSql = strSql.Trim(',') + ")";
                    using (System.Data.SqlClient.SqlConnection sqlconn = 
                        new System.Data.SqlClient.SqlConnection(connString))
                    {
                        sqlconn.Open();//建立与数据库的连接
                        System.Data.SqlClient.SqlCommand command = sqlconn.CreateCommand();
                        command.CommandText = strSql;
                        command.ExecuteNonQuery();
                        sqlconn.Close();
                    }
                    using (System.Data.SqlClient.SqlBulkCopy bcp = new System.Data.SqlClient.SqlBulkCopy(connString))
                    {
                        bcp.SqlRowsCopied += new System.Data.SqlClient.SqlRowsCopiedEventHandler(bcp_SqlRowsCopied);
                        bcp.BatchSize = 100;//每次传输的行数        
                        bcp.NotifyAfter = 100;//进度提示的行数        
                        bcp.DestinationTableName = sheetName;//目标表        
                        bcp.WriteToServer(ds.Tables[0]);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void bcp_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            this.Text = e.RowsCopied.ToString();
            this.Update();
        }

        private void b1_Click(object sender, EventArgs e)
        {
            string connString = "server=47.102.138.168;database=GIS;uid=GISJQQ;pwd=Jqq963741ArcObjects";
            SqlConnection SC = new SqlConnection(connString);
            SC.Open();//数据库建立连接
            SqlCommand SCmd = new SqlCommand();
            SCmd.Connection = SC;
            SCmd.CommandType = CommandType.Text;
            if (combo1.SelectedIndex == 0)//基于4个不同类型进行上传操作
            {
                SCmd.CommandText = "insert into TH5(Date,Station,WaterH) values('" + date.Value.ToString("yyyy-MM-dd") 
                    + "','" + text1.Text.Trim().ToString() + "','" + combo2.SelectedItem + "')";
            }
            else if (combo1.SelectedIndex == 1)
            {
                SCmd.CommandText = "insert into ZDSW(date,site,waterlevel) values('" + date.Value.ToString("yyyy-MM-dd") 
                    + "','" + text1.Text.Trim().ToString() + "','" + combo2.SelectedItem + "')";
            }
            else if (combo1.SelectedIndex == 2)
            {
                SCmd.CommandText = "insert into ZDSW(date,site,rainfall) values('" + date.Value.ToString("yyyy-MM-dd") 
                    + "','" + text1.Text.Trim().ToString() + "','" + combo2.SelectedItem + "')";
            }
            else
            {
                SCmd.CommandText = "insert into YPSL_3(Date,Station,YPSL) values('" + date.Value.ToString("yyyy-MM-dd") 
                    + "','" + text1.Text.Trim().ToString() + "','" + combo2.SelectedItem + "')";
            }
            SCmd.ExecuteNonQuery();
            SC.Close();//关闭数据库
            MessageBox.Show("上传数据成功！");
        }

        private void pPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                System.Drawing.Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                ((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos);
            }
        }

        private void pPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.TopMost = true;
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                if (textBox1.Text == "2020" && textBox2.Text == "2020")
                {
                    SQLModify();
                }
                else
                {
                    MessageBox.Show("对不起，您的密码错误，请重新输入");
                }
            }
        }
    }
}
