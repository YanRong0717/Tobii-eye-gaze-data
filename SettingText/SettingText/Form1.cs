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
using System.IO;
using System.Text.RegularExpressions;

namespace SettingText {
    public partial class Form1 : Form {

        MatchCollection match;
        List<int> cc = new List<int>();
        int sel_direction; //選到的index
        string direction = "n";
        int page = 1;
        private string strValue;
        public string StrValue {
            set {
                strValue = value;
            }
        }

        public Form1() {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e) {
            if(comboBox1.Text == "垂直") {
                direction = "v";
            }
            if (comboBox1.Text == "水平") {
                direction = "h";
            }
            if(direction != "n" && textBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "") {

                listBox1.Items.Add(direction + "," + textBox1.Text + "," +
                textBox2.Text + "," + textBox4.Text);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e) {
            listBox1.Items.Add("#1");
        }

        private void button2_Click(object sender, EventArgs e) {
            page++;
            listBox1.Items.Add("#" + page);
            label6.Text = "#" + page;
        }

        private void button3_Click(object sender, EventArgs e) {
            if(listBox1.SelectedItem != null) {
                if (Regex.IsMatch(listBox1.SelectedItem.ToString(), "^#") == false) {
                    //Debug.Print(listBox1.SelectedIndex.ToString());
                    string s = listBox1.SelectedItem.ToString();
                    string content;
                    int x;
                    int y;
                    string[] array = s.Split(",".ToCharArray());
                    if (array[0] == "v") sel_direction = 0;
                    if (array[0] == "h") sel_direction = 1;
                    x = Convert.ToInt32(array[1]);
                    y = Convert.ToInt32(array[2]);
                    content = array[3];

                    Form2 Form2 = new Form2();
                    Form2.Owner = this;//重要的一步，主要是使Form2的Owner指針指向Form1

                    Form2.textBox1.Text = x.ToString();
                    Form2.textBox2.Text = y.ToString();
                    Form2.textBox3.Text = content;
                    Form2.comboBox1.SelectedIndex = sel_direction;
                    Form2.label5.Text = listBox1.SelectedIndex.ToString();
                    Form2.label5.Hide();

                    Form2.ShowDialog();
                    if (Form2.DialogResult == DialogResult.OK) {
                        listBox1.Items[listBox1.SelectedIndex] = strValue;
                    }
                    
                    //listBox1.Items[0] = "123";
                }



            }
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

        private void button5_Click(object sender, EventArgs e) {
            string qq; //檔案路徑
            SaveFileDialog saveFileDialog = new SaveFileDialog(); //建立存檔物件
            saveFileDialog.Filter = "txt|*.txt"; // 設定存檔格式
            saveFileDialog.Title = "Save the Text"; // 設定存檔對話框之標題
            saveFileDialog.ShowDialog(); // 顯示存檔對話框
            
            if (saveFileDialog.FileName != "") { // 如果存檔名稱不等於空值則執行以下內容
                qq = saveFileDialog.FileName.ToString(); // 檔案路經 = 檔案路經+檔案名稱 
                foreach (string item in listBox1.Items) { //迴圈抓取要放入存檔文件的內容
                    File.AppendAllText(qq, item + "\n", Encoding.UTF8); //將內容(item) append到檔案中
                }
                MessageBox.Show("Saved"); // 顯示存檔成功
            }
            
        }

        private void button4_Click(object sender, EventArgs e) {

            if (this.listBox1.SelectedIndex != -1) {
                if (Regex.IsMatch(listBox1.SelectedItem.ToString(), "^#")) {
                    page -= 1;
                    label6.Text = "#" + page.ToString();

                    if (this.listBox1.SelectedIndex != -1) {
                        this.listBox1.Items.RemoveAt(this.listBox1.SelectedIndex);
                    }

                    List<string> aa = new List<string>();
                    foreach (string item in listBox1.Items) {
                        match = Regex.Matches(item, @"^#.+");
                        //Debug.Print(match.ToString());
                        foreach (Match m in match) {
                            aa.Add(m.ToString());
                        }
                    }
                    cc.Clear();
                    for (int k = 0; k < aa.Count; k++) {
                        for (int i = 0; i < listBox1.Items.Count; i++) {
                            if (listBox1.Items[i].ToString().Contains(aa[k]))
                                if (listBox1.Items[i].ToString() != "#1") {
                                    cc.Add(i);
                                }
                        }
                    }
                    for (int g = 0; g < cc.Count; g++) {
                        Debug.Print(cc[g].ToString());
                        int t = Convert.ToInt16(cc[g]);
                        listBox1.Items[t] = "#" + (g + 2);
                    }
                    listBox1.Refresh();
                }
            }
                

            if (this.listBox1.SelectedIndex != -1) {
                this.listBox1.Items.RemoveAt(this.listBox1.SelectedIndex);
            }

            listBox1.Refresh();
        }

        private void button6_Click(object sender, EventArgs e) {
            if (listBox1.Items.Count > 0) {
                listBox1.Items.Clear();
            }
            listBox1.Items.Add("#1");
            label6.Text = "#1";
            page = 1;
            listBox1.Refresh();
        }

        private void button7_Click(object sender, EventArgs e) {
            if (listBox1.SelectedItem != null) {
                if (comboBox1.Text == "垂直") {
                    direction = "v";
                }
                if (comboBox1.Text == "水平") {
                    direction = "h";
                }
                if (direction != "n" && textBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "") {

                    listBox1.Items.Insert(listBox1.SelectedIndex, direction + "," + textBox1.Text + "," +
                    textBox2.Text + "," + textBox4.Text);
                }

            }
                
        }

        private void button8_Click(object sender, EventArgs e) {
            listBox1.ClearSelected();
            if (textBox3.Text != "") {
                for (int i = 0; i < listBox1.Items.Count; i++) {
                    if (listBox1.Items[i].ToString().Contains(textBox3.Text))
                        listBox1.SetSelected(i, true);
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                button8_Click(this, null);
            }
        }
    }
}
