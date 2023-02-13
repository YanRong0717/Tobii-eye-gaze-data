using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Match
{
    public partial class Form1 : Form
    {
        bool CheckFile1 = false;
        bool CheckFile2 = false;
        static string SavePath = System.Environment.SpecialFolder.DesktopDirectory.ToString();
        List<string> match_text_str_list = new List<string>();
        List<string> match_text_x_list = new List<string>();
        List<string> match_text_y_list = new List<string>();
        List<string> match_eye_x_list = new List<string>();
        List<string> match_eye_y_list = new List<string>();
        List<string> match_eye_z_list = new List<string>();
        List<string> match_eye_d_list = new List<string>();
        List<double> match_timestamp_list = new List<double>();
        // ************************* EyeData ****************************

        string[] firstLine_eye;
        string[] ContentLine_eye;
        bool IsFirst_eye = true;
        int Row_eye = 0;
        int column_eye = 0;
        List<string[]> mylist_eye = new List<string[]>();
        // ************************* EyeData ****************************
        // ************************* TextData ****************************
        
        string[] firstLine_text;
        string[] ContentLine_text;
        bool IsFirst_text = true;
        int Row_text = 0;
        int column_text = 0;
        int fontsize = 0;
        int letterspacing = 0;
        int linespacing = 0;
        List<string[]> mylist_text = new List<string[]>();
        // ************************* TextData ****************************
        List<double[]> data_eye = new List<double[]>();
        List<double[]> data_text = new List<double[]>();
        List<string> data_text_str = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e){
            
            OpenFileDialog dialog1 = new OpenFileDialog(); //建立物件
            dialog1.Title = "Please select eyeData file."; // 開啟視窗的標題
            dialog1.InitialDirectory = System.Environment.SpecialFolder.DesktopDirectory.ToString(); // 該視窗的預設路徑 desktopDirectory 為桌面
            dialog1.Filter = "csv files (*.*)|*.csv"; // 篩選要選的文件類型格式
            if (dialog1.ShowDialog() == DialogResult.OK) { // 如果按下確定
                
                FileStream fs_eye = new FileStream(dialog1.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                StreamReader sr_eye = new StreamReader(fs_eye, Encoding.UTF8);


                mylist_eye.Clear();
                button1.Enabled = false;
                string fileContent;
                while ((fileContent = sr_eye.ReadLine()) != null) {
                    if (IsFirst_eye == true) {
                        firstLine_eye = fileContent.Split(',');
                        IsFirst_eye = false;
                        column_eye = firstLine_eye.Length;
                        //Debug.Print(fileContent);
                    } else {
                        ContentLine_eye = fileContent.Split(',');
                        //mylist_eye.Add(new string[] {
                        //    ContentLine_eye[0], ContentLine_eye[1], ContentLine_eye[2],
                        //    ContentLine_eye[3], ContentLine_eye[4], ContentLine_eye[5],
                        //    ContentLine_eye[6], ContentLine_eye[7], ContentLine_eye[8]
                        //});

                        if (ContentLine_eye[0] != "NaN" && ContentLine_eye[4] != "NaN") { //兩眼都睜著
                            data_eye.Add(new double[] {
                            Convert.ToDouble(ContentLine_eye[0]) , Convert.ToDouble(ContentLine_eye[1]), // L_X,L_Y(百分比)
                            Convert.ToDouble(ContentLine_eye[0])*1920 , Convert.ToDouble(ContentLine_eye[1])*1080, // L_X,L_Y(像素)
                            Convert.ToDouble(ContentLine_eye[2]) , Convert.ToDouble(ContentLine_eye[3]), // L_Z,L_D(百分比)4 , 5

                            Convert.ToDouble(ContentLine_eye[4]) , Convert.ToDouble(ContentLine_eye[5]), // R_X,R_Y(百分比)
                            Convert.ToDouble(ContentLine_eye[4])*1920 , Convert.ToDouble(ContentLine_eye[5])*1080, // R_X,R_Y(像素)
                            Convert.ToDouble(ContentLine_eye[6]) , Convert.ToDouble(ContentLine_eye[7]), // R_Z,R_D(百分比)

                            (Convert.ToDouble(ContentLine_eye[4]) + Convert.ToDouble(ContentLine_eye[0])) / 2 *1920, // avg_X(像素) 12
                            (Convert.ToDouble(ContentLine_eye[5]) + Convert.ToDouble(ContentLine_eye[1])) / 2 *1080, // avg_Y(像素) 13
                            Convert.ToDouble(ContentLine_eye[8]) ,                                       // timestamp 14
                        });
                        //} else if (ContentLine_eye[4] != "NaN" && ContentLine_eye[0] == "NaN" ) { // 當右眼閉著，左眼睜著
                        //    data_eye.Add(new double[] {
                        //    Convert.ToDouble(ContentLine_eye[0]) , Convert.ToDouble(ContentLine_eye[1]),// L_X,L_Y(百分比)
                        //    Convert.ToDouble(ContentLine_eye[0])*1920 , Convert.ToDouble(ContentLine_eye[1])*1080,// L_X,L_Y(像素)
                        //    Convert.ToDouble(ContentLine_eye[2]) , Convert.ToDouble(ContentLine_eye[3]),// L_Z,L_D(百分比)

                        //    //Convert.ToDouble(ContentLine_eye[4]) , Convert.ToDouble(ContentLine_eye[5]),// R_X,R_Y(百分比)
                        //    //Convert.ToDouble(ContentLine_eye[4])*1920 , Convert.ToDouble(ContentLine_eye[5])*1080,// R_X,R_Y(像素)
                        //    //Convert.ToDouble(ContentLine_eye[6]) , Convert.ToDouble(ContentLine_eye[7]),// R_Z,R_D(百分比)

                        //    Convert.ToDouble(ContentLine_eye[0])*1920 , Convert.ToDouble(ContentLine_eye[1])*1080,// avg_X,avg_Y(像素)
                        //    Convert.ToDouble(ContentLine_eye[8]) ,                                      // timestamp
                            
                            
                        //});
                        //} else if (ContentLine_eye[0] == "NaN" && ContentLine_eye[4] != "NaN") { // 當左眼閉著，右眼睜著
                        //    data_eye.Add(new double[] {
                        //    //Convert.ToDouble(ContentLine_eye[0]) , Convert.ToDouble(ContentLine_eye[1]), // L_X,L_Y(百分比)
                        //    //Convert.ToDouble(ContentLine_eye[0])*1920 , Convert.ToDouble(ContentLine_eye[1])*1080, // L_X,L_Y(像素)
                        //    //Convert.ToDouble(ContentLine_eye[2]) , Convert.ToDouble(ContentLine_eye[3]), // L_Z,L_D(百分比)

                        //    Convert.ToDouble(ContentLine_eye[4]) , Convert.ToDouble(ContentLine_eye[5]), // R_X,R_Y(百分比)
                        //    Convert.ToDouble(ContentLine_eye[4])*1920 , Convert.ToDouble(ContentLine_eye[5])*1080, // R_X,R_Y(像素)
                        //    Convert.ToDouble(ContentLine_eye[6]) , Convert.ToDouble(ContentLine_eye[7]), // R_Z,R_D(百分比)

                        //    Convert.ToDouble(ContentLine_eye[4])*1920 , Convert.ToDouble(ContentLine_eye[5])*1080, // avg_X,avg_Y(像素)
                        //    Convert.ToDouble(ContentLine_eye[8]) ,                                       // timestamp
                            
                        //});
                        }
                        mylist_eye.Clear();
                    }
                    Row_eye++;
                }
                //foreach (string th in firstLine_eye) {
                //    Debug.Print(th);
                //}

                //Debug.Print(Row_eye.ToString());
                //Debug.Print(column_eye.ToString());
                //Debug.Print(mylist_eye[0][8].ToString());

                //Debug.Print(average_eye[0][1].ToString());
                //Debug.Print(average_eye[200][9].ToString());
                fs_eye.Close();
                sr_eye.Close();
                CheckFile1 = true;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog2 = new OpenFileDialog();
            dialog2.Title = "Please select eyeData file.";
            dialog2.InitialDirectory = System.Environment.SpecialFolder.DesktopDirectory.ToString();
            dialog2.Filter = "csv files (*.*)|*.csv";
            if (dialog2.ShowDialog() == DialogResult.OK) {

                FileStream fs_text = new FileStream(dialog2.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                StreamReader sr_text = new StreamReader(fs_text, Encoding.UTF8);

                mylist_text.Clear();
                button2.Enabled = false;
                string fileContent;
                while ((fileContent = sr_text.ReadLine()) != null) {
                    if (IsFirst_text == true) {
                        firstLine_text = fileContent.Split(',');
                        IsFirst_text = false;
                        column_text = firstLine_text.Length;
                        //Debug.Print(fileContent);
                    } else {
                        ContentLine_text = fileContent.Split(',');
                        mylist_text.Add(new string[] {
                        ContentLine_text[0], ContentLine_text[1], ContentLine_text[2],
                        ContentLine_text[3], ContentLine_text[4], ContentLine_text[5],
                        ContentLine_text[6], ContentLine_text[7], ContentLine_text[8]
                    });
                        data_text.Add(new double[] {
                        Convert.ToDouble(ContentLine_text[1]),             //page
                        Convert.ToDouble(ContentLine_text[2]) , Convert.ToDouble(ContentLine_text[3]), // 左上X,左上Y(像素)
                        Convert.ToDouble(ContentLine_text[4]) , Convert.ToDouble(ContentLine_text[5]), // 右下X,右下Y(像素)
                        (Convert.ToDouble(ContentLine_text[4]) + Convert.ToDouble(ContentLine_text[2])) / 2 , // avg_X(像素) 5
                        (Convert.ToDouble(ContentLine_text[5]) + Convert.ToDouble(ContentLine_text[3])) / 2 , // avg_Y(像素) 6
                    });
                        data_text_str.Add(ContentLine_text[0]);
                        fontsize = Convert.ToInt16(ContentLine_text[6]);
                        letterspacing = Convert.ToInt16(ContentLine_text[7]);
                        linespacing = Convert.ToInt16(ContentLine_text[8]);
                    }
                    mylist_text.Clear();
                    Row_text++;
                }
                


                //Debug.Print(Row_text.ToString());
                //Debug.Print(column_text.ToString());
                //Debug.Print(mylist_text[0][5].ToString());

                //Debug.Print(average_text_str[5]);
                fs_text.Close();
                sr_text.Close();
                CheckFile2 = true;
            }
        }

        private void match()
        {
            
            double time_end = 0;
            double time_start = data_eye[0][14];
            int num = 0;

            double x = 0; //%
            double y = 0; //%  
            double z = 0;
            double d = 0;
            string Now_text_str = "";
            string bb = "";
            for (int i = 0; i <= data_eye.Count() -1; i++) {
                //time_start = data_eye[i][14];
                for (int j = 0; j <= data_text.Count() -1 ; j++) {
                    //Debug.Print(data_eye[j][12].ToString()); // avg_eye_x
                    //Debug.Print(data_eye[j][13].ToString()); // avg_eye_y
                    if(data_eye[i][12] > (data_text[j][1] - letterspacing / 2) && data_eye[i][12] < (data_text[j][3] - letterspacing / 2) &&
                       data_eye[i][13] > (data_text[j][2] -linespacing / 2) && data_eye[i][13] < (data_text[j][4] - linespacing / 2 ) ) {

                        //Debug.Print(data_text_str[j]);
                        num = j;
                        Now_text_str = data_text_str[num];
                        x = data_eye[i][12]/1920;
                        y = data_eye[i][13]/1080;
                        z = data_eye[i][4];
                        d = data_eye[i][5];
                        //time_end = data_eye[i][14];

                    }
                }
                if(Now_text_str != bb) {
                    match_text_str_list.Add(Now_text_str);
                    match_text_x_list.Add(data_text[num][5].ToString());
                    match_text_y_list.Add(data_text[num][6].ToString());
                    match_eye_x_list.Add(x.ToString());
                    match_eye_y_list.Add(y.ToString());
                    match_eye_z_list.Add(z.ToString());
                    match_eye_d_list.Add(d.ToString());
                    time_end = data_eye[i][14];
                    //Debug.Print("start:{0} end:{1}" ,time_start ,time_end.ToString());
                    match_timestamp_list.Add((time_end - time_start));
                    time_start = time_end;
                    bb = Now_text_str;
                }
            }

            //Debug.Print(data_text_str.Count().ToString());
            //Debug.Print(data_text.Count().ToString());
            //Debug.Print(data_eye.Count().ToString());
            //Debug.Print(match_text_str_list.Count().ToString());

            int r = 0;
            while(r <= match_text_str_list.Count() -1 ) {
                //Debug.Print(match_text_str_list[r]);
                File.AppendAllText(SavePath, match_text_str_list[r] + "," + match_text_x_list[r] + "," + 
                     match_text_y_list[r] +  "," + match_eye_x_list[r] + "," + match_eye_y_list[r] + "," + 
                     match_eye_z_list[r] + "," +  match_eye_d_list[r] + "," +  match_timestamp_list[r]/1000000 + "," + "\n", Encoding.UTF8);
                r++;
            }
            match_text_str_list.Clear();
            match_text_x_list.Clear();
            match_text_y_list.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (CheckFile1 == true && CheckFile2 == true) {
                FolderBrowserDialog path = new FolderBrowserDialog();
                path.ShowDialog();
                String now = DateTime.Now.ToString("MM-dd HH.mm.ss");
                SavePath = path.SelectedPath + "\\" + "output" + now +  ".csv";
                string[] createText3 = { " String , String_X(px) , String_Y(px) , Eye_X(%) , Eye_Y(%) , Eye_Z , Eye_D , TimeStanp" };
                File.WriteAllLines(SavePath, createText3, Encoding.UTF8);
                match();
                SavePath = System.Environment.SpecialFolder.DesktopDirectory.ToString();
            } else {
                MessageBox.Show("Please select file.");
            }
            
        }
    }
}
