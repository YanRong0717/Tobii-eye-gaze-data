using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SettingText {
    public partial class Form2 : Form {
        public string all = "";
        public Form2() {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            string direction = comboBox1.Text;
            if (direction == "水平") direction = "h";
            if (direction == "垂直") direction = "v";
            int x = Convert.ToInt32(textBox1.Text);
            int y = Convert.ToInt32(textBox2.Text);
            string content = textBox3.Text;
            all = direction + "," + x + "," + y + "," + content;
            //Debug.Print(all);
            int qq = Convert.ToInt32(label5.Text);

            Form1 Form2 = (Form1)this.Owner;//把Form2的父窗口指針賦給lForm1  
            Form2.StrValue = all;//使用父窗口指針賦值  
            this.DialogResult = DialogResult.OK;
            this.Close();
            //http://altoncsharp.blogspot.com/2016/12/cform.html
        }



        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {
            // e.KeyChar == (Char)48 ~ 57 -----> 0~9
            // e.KeyChar == (Char)8 -----------> Backpace
            // e.KeyChar == (Char)13-----------> Enter
            if (e.KeyChar == (Char)48 || e.KeyChar == (Char)49 ||
               e.KeyChar == (Char)50 || e.KeyChar == (Char)51 ||
               e.KeyChar == (Char)52 || e.KeyChar == (Char)53 ||
               e.KeyChar == (Char)54 || e.KeyChar == (Char)55 ||
               e.KeyChar == (Char)56 || e.KeyChar == (Char)57 ||
               e.KeyChar == (Char)13 || e.KeyChar == (Char)8) {
                e.Handled = false;
            } else {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e) {
            // e.KeyChar == (Char)48 ~ 57 -----> 0~9
            // e.KeyChar == (Char)8 -----------> Backpace
            // e.KeyChar == (Char)13-----------> Enter
            if (e.KeyChar == (Char)48 || e.KeyChar == (Char)49 ||
               e.KeyChar == (Char)50 || e.KeyChar == (Char)51 ||
               e.KeyChar == (Char)52 || e.KeyChar == (Char)53 ||
               e.KeyChar == (Char)54 || e.KeyChar == (Char)55 ||
               e.KeyChar == (Char)56 || e.KeyChar == (Char)57 ||
               e.KeyChar == (Char)13 || e.KeyChar == (Char)8) {
                e.Handled = false;
            } else {
                e.Handled = true;
            }
        }
        
        private void textBox1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                button2_Click(this, null);
            }
            if (e.KeyCode == Keys.Escape) {
                button1_Click(this, null);
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                button2_Click(this, null);
            }
            if (e.KeyCode == Keys.Escape) {
                button1_Click(this, null);
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                button2_Click(this, null);
            }
            if (e.KeyCode == Keys.Escape) {
                button1_Click(this, null);
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                button2_Click(this, null);
            }
            if (e.KeyCode == Keys.Escape) {
                button1_Click(this, null);
            }
        }
    }
}
