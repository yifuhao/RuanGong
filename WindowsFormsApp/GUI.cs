using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class GUI : Form
    {
        private bool b_w; //选中 单词最多 标志
        private bool b_c; //选中 字母最多 标志
        private bool b_h; //选中 限定首字母 标志
        private char char_h;
        private bool b_t; //选中 。。尾字母 标志
        private char char_t;
        private bool b_r; //选中 允许环 标志

        private string textOutput; //输出内容

        public GUI()
        {
            InitializeComponent();
        }

        public static void StartGUI()
        {
            Program.showGUI();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) { b_w = true; }
            else { b_w = false; }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked) { b_c = true; }
            else { b_c = false; }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { b_h = true; }
            else { b_h = false; }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string str = textBox3.Text;
            if (str.Length > 0)
            {
                str = str.ToLower();
                char_h = str.ElementAt(0);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) { b_t = true; }
            else { b_t = false; }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string str = textBox4.Text;
            if (str.Length > 0)
            {
                str = str.ToLower();
                char_t = str.ElementAt(0);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked) { b_r = true; }
            else { b_r = false; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog(this);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            StreamWriter myStream;
            //saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            //saveFileDialog1.RestoreDirectory = true;
            string str; str = saveFileDialog1.FileName;
            myStream = new StreamWriter(saveFileDialog1.FileName);
            myStream.Write(textOutput);
            myStream.Flush();
            myStream.Close();
        }
        //“生成”按钮 单击事件
        private void button1_Click(object sender, EventArgs e)
        {
            string textInput;
            if (tabPage.SelectedIndex == 0)
            {
                textInput = "";
                string filename = Directory.GetCurrentDirectory() + "\\" + textBox2.Text;
                if (File.Exists(filename))
                {
                    textInput = File.ReadAllText(filename);
                }
            }
            else
            {
                textInput = textBox1.Text;
            }
            if (textInput.Length == 0)
            {
                textInput = "ERROR : NO INPUT !";
            }
            //调用ConsoleApp1项目内对应接口实现

            //在此简单用输入直接当成输出
            textOutput = textInput;
            //测试输出
            textBox5.Text = textOutput + " " + b_w.ToString() + " " + b_c.ToString() + " " + b_h.ToString()
                + " " + b_t.ToString() + " " + b_r.ToString() + " " + char_h + " " + char_t;
        }


    }
}
