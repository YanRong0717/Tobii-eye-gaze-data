using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Tobii.Interaction;
using Tobii.Research;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;

namespace DrawText_v10._0 {
    public partial class Form1 : Form {
        
        Font font = new Font("標楷體", 48); // 定義字體大小，預設48
        Font font2 = new Font("標楷體", 12); // 定義字體大小，預設12
        Bitmap b;
        Bitmap BackgroundImage; // 定義背景圖片
        Graphics g; // 定義畫布變數
        

        string direction; //水平或垂直方向
        int rr, gg, bb; //文字的RGBA
        TextFormatFlags flags = TextFormatFlags.NoPadding; // 文字格式:NoPadding
        int screenWidth = Screen.PrimaryScreen.Bounds.Width; // 螢幕寬度
        int screenHeight = Screen.PrimaryScreen.Bounds.Height; //螢幕高度
        string InputString;
        string OpenPath = System.Environment.CurrentDirectory + "\\txt";   // 開啟文字檔案的路徑
        string FilePath_eyeMovment = System.Environment.CurrentDirectory + "\\csv";        // 存檔時的路徑
        string FilePath_picture = System.Environment.CurrentDirectory + "\\csv";    // 存檔時的路徑
        string pic_path = System.Environment.CurrentDirectory + "\\img";    // 要載入圖片時的路徑
        string ImagePath = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);    // 開啟背景圖檔的路徑 (桌面路徑)
        string FileName;        // 輸入的檔案名稱
        int NowPage = 1;        // 當前頁數 預設為起始頁面1
        int SumPage = 0;        // 總頁數
        int SelectPage = 1;        // 使用者當前所選的頁數
        int Row;                // 總行數
        double width;      // 字寬
        double height;     // 字高
        int LetterSpacing = 20; // 字距 預設起始值為20
        int LineSpacing = 20;   // 行距 預設起始值為20
        List<string> List_String_Single = new List<string>();               // 每個字的List
        List<double> List_x = new List<double>();                           // x座標的List
        List<double> List_y = new List<double>();                           // y座標的List
        //**************************** EyeTracker **********************************
        public delegate void MyDelegate(string Left_X, string Left_Y, string Left_Z, string Left_Diameter, string Right_X, string Right_Y, string Right_Z, string Right_Diameter, double ts); // 設定委派所接收的變數
        public string list_current = "A"; //預設當前儲存的buffer為A
        public int Change_List = 300;  // 預設為每300筆資料就改變buffet
        Stopwatch stopwatch = new Stopwatch();
        // Tobii Pro SDK 參考文件 http://devtobiipro.azurewebsites.net/tobii.research/matlab/reference/1.1.0.23-beta-g9262468f/EyeTrackingOperations.html 
        GazePointDataStream GazePointDataStream; // 定義套用的SDK 眼動儀資料流(DataStream)之變數
        EyeTrackerCollection eyeTrackers = EyeTrackingOperations.FindAllEyeTrackers(); // 利用套用的Tobii Pro SDK來找眼動儀
        List<string> myLists_A = new List<string>(); // 定義資料暫存區(Buffer A)
        List<string> myLists_B = new List<string>(); // 定義資料暫存區(Buffer A)
        //**************************** EyeTracker **********************************
        PictureBox pic;
        int pic_num = 1;
        public Form1() {
            InitializeComponent(); // 初始化整個程式
        }

        private void Form1_Load(object sender, EventArgs e) { // Form 載入時所做的事情
            // **************** EyeTracker *********************************
            //Debug.Print(ImagePath);
            Host host = new Host(); // 建立眼動儀物件
            GazePointDataStream = host.Streams.CreateGazePointDataStream(); // 將眼動儀的資料流數據取回，放置到前面定義的變數中
            myLists_A.Clear(); // 程式開始前，先清除所有陣列內容
            myLists_B.Clear(); // 程式開始前，先清除所有陣列內容
            foreach (IEyeTracker eyeTracker in eyeTrackers) { // 利用 foreach 迴圈來抓取眼動儀
                if (eyeTracker != null) { // 若眼動儀存在，則開始接收
                    eyeTracker.GazeDataReceived += EyeTracker_GazeDataReceived;
                }
                //ApplyLicense(eyeTracker, licensePath);
            }
            // Form.CheckForIllegalCrossThreadCalls = false; //不須透過委派達到更新UI 但不安全
            // **************** EyeTracker *********************************
            SelectPage = (int)numericUpDown4.Value; // 將當前選取頁面的value，放入SelectPage變數中
            toolTip1.SetToolTip(label5, "Ctrl+Del  : Hide \nCtrl+Ins   : Show \nCtrl+F11 : Min \nCtrl+F12 : Close \nCtrl+F1 : Start \nCtrl+F2 : Stop \nCtrl+F3 : Open \nCtrl+F4 : Close"); // Hover訊息視窗
                                                                                                                                                                             //----------------------------
            int rr = (int)numericUpDown5.Value;
            int gg = (int)numericUpDown6.Value;
            int bb = (int)numericUpDown7.Value;
            //----------------------------
            panel5.Hide();
            
        }

        private void button1_Click(object sender, EventArgs e) // 點選匯入文字檔時所做的事情
        {
            OpenFile();
        }
        private void OpenFile() {
            OpenFileDialog dialog = new OpenFileDialog(); // 新增一個dialog物件
            dialog.Title = "Select txt File"; // 標頭
            dialog.InitialDirectory = OpenPath; // 初始路徑
            dialog.Filter = "txt files (*.*)|*.txt"; // 可選的檔案類型為txt
            if (dialog.ShowDialog() == DialogResult.OK) { // 若在檔案視窗按下確定
                //MessageBox.Show(dialog.FileName);
                StreamReader str = new StreamReader(dialog.FileName); // 新增一個物件，為剛剛選中的檔案
                InputString = str.ReadToEnd(); // 讀取檔案中所有的文字
                FileName = dialog.FileName; // 檔案名稱
                //Debug.Print(FileName);
                //textBox1.Text = InputString;
                str.Close(); // 關閉檔案
                button2.Enabled = true; // 啟用"儲存檔案"按鈕
                button3.Enabled = true; // 啟用"提交"按鈕
                checkBox1.Enabled = true; // 啟用"開始"按鈕
                this.WindowState = FormWindowState.Maximized; // 將整個程式調整為全螢幕

                StreamReader file = new StreamReader(FileName); // 
                while ((InputString = file.ReadLine()) != null) { //每次只讀一行，若該行第一個字為"#"
                    if (InputString.Substring(0, 1) == "#") { //若為 # 則更新當前頁面
                        SumPage++; //總頁面數++
                    }
                }
                numericUpDown4.Maximum = SumPage; //上面迴圈讀完整個檔案後
                button3_Click(null,null); // 在這邊先"提交"一次，使第一頁畫面印出來
            }
            
        }

        private void button2_Click(object sender, EventArgs e) { // 當按下"儲存檔案"
            String now = DateTime.Now.ToString("MM-dd HH.mm.ss"); // 取得當前時間
            FilePath_picture = FilePath_picture + "\\" + now + " Text by page." + SelectPage + ".csv"; // 定義存檔路徑與檔名(副檔名)
            string[] createText2 = { " String , page , X(LeftTop) , Y(LeftTop) ,  X(RightBottom) , Y(RightBottom) , FontSize , LetterSpacing , LineSpacing" }; // 設定第一列文字
            Debug.Print(FilePath_picture); 
            File.WriteAllLines(FilePath_picture, createText2, Encoding.UTF8); // 將第一列標頭先寫入檔案中
            // 將各個變數存至csv檔案中 ***************************************************
            for (int i = 0; i <= List_String_Single.Count-1; i++) { // 將單個字元與xy座標寫入(append)檔案中，該座標已經加上字距、行距
                File.AppendAllText(FilePath_picture, List_String_Single[i] + "," + SelectPage + ","
                + (List_x[i]+100) + "," + (List_y[i]+100) + "," + (List_x[i] + 100 + width) + "," + (List_y[i] + 100 + height) + ","
                + font.Size + "," + LetterSpacing + "," + LineSpacing + "\n", Encoding.UTF8);
            }
            MessageBox.Show("已儲存 Page:" + SelectPage.ToString()); // Show出Message.box
            FilePath_picture = System.Environment.CurrentDirectory + "\\csv"; //重置
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e) { // 設定快捷鍵
            Keys key = e.KeyCode;
            if (e.Control != true)//如果沒按Ctrl鍵
                return;
            switch (key) {
                case Keys.F12: // 關閉程式
                    this.Close();
                    break;
                case Keys.F11: // 設定程式最小化  
                    this.WindowState = FormWindowState.Minimized;
                    break;
                case Keys.Delete: // 隱藏工具列
                    panel1.Hide();
                    break;
                case Keys.Insert: // 顯示工具列
                    panel1.Show();
                    break;
                case Keys.F1: // 開始測試
                    checkBox1.Checked = true;
                    break;
                case Keys.F2: // 結束測試
                    checkBox1.Checked = false;
                    break;
                case Keys.F3: // 開啟表格
                    panel5.Show();
                    button6_Click(null, null);
                    break;
                case Keys.F4: // 關閉表格
                    panel5.Hide();
                    break;

            }
        }


// numericUpDown 設定 ********************************************************
// 若NumericUpDown 數值發生變化，則設定以下變數為調整後數值
        private void numericUpDown1_ValueChanged(object sender, EventArgs e) { //字型大小
            font = new Font("標楷體", Convert.ToInt32(numericUpDown1.Value)); 
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e) { // 字元間距
            LetterSpacing = Convert.ToInt32(numericUpDown2.Value);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e) { // 行距
            LineSpacing = Convert.ToInt32(numericUpDown3.Value);
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e) { // page
            SelectPage = Convert.ToInt32(numericUpDown4.Value);
        }
        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        { // red
            rr = Convert.ToInt32(numericUpDown5.Value);
        }
        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        { // green
            gg = Convert.ToInt32(numericUpDown6.Value);
        }
        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        { // blue
            bb = Convert.ToInt32(numericUpDown7.Value);
        }

        //EyeTracker_GazeDataReceived---------------------------------
        private void EyeTracker_GazeDataReceived(object sender, GazeDataEventArgs e) { // 這個為Tobii Pro SDK 的函數，負責接收數據
            string L_X = (e.LeftEye.GazePoint.PositionOnDisplayArea.X).ToString("f2");
            string L_Y = (e.LeftEye.GazePoint.PositionOnDisplayArea.Y).ToString("f2");
            string L_D = (e.LeftEye.Pupil.PupilDiameter).ToString("f2");
            string L_Z = (e.LeftEye.GazeOrigin.PositionInTrackBoxCoordinates.Z).ToString("f2");
            string R_X = (e.RightEye.GazePoint.PositionOnDisplayArea.X).ToString("f2");
            string R_Y = (e.RightEye.GazePoint.PositionOnDisplayArea.Y).ToString("f2");
            string R_Z = (e.RightEye.GazeOrigin.PositionInTrackBoxCoordinates.Z).ToString("f2");
            string R_D = (e.RightEye.Pupil.PupilDiameter).ToString("f2");
            var ts = e.DeviceTimeStamp;

            // Check data is or not NaN 
            if (L_X == "非數值") { // 原本是NaN 但因為我上面把他轉成string 所以輸出變成"非數值"
                L_X = "NaN";
            }
            if (L_Y == "非數值") {
                L_Y = "NaN";
            }
            if (L_D == "非數值") {
                L_D = "NaN";
            }
            if (L_Z == "非數值") {
                L_Z = "NaN";
            }
            if (R_X == "非數值") {
                R_X = "NaN";
            }
            if (R_Y == "非數值") {
                R_Y = "NaN";
            }
            if (R_Z == "非數值") {
                R_Z = "NaN";
            }
            if (R_D == "非數值") {
                R_D = "NaN";
            }

            Update(L_X, L_Y, L_Z, L_D, R_X, R_Y, R_Z, R_D, ts); // 更新眼動儀收到的數值
            if (checkBox1.Checked == true) {
                //----------------------加入Lsit------------------------
                if (list_current == "A") {
                    myLists_A.Add(L_X + "," + L_Y + "," + L_Z + "," + L_D + "," +
                        R_X + "," + R_Y + "," + R_Z + "," + R_D + "," + ts.ToString("f2"));
                } else if (list_current == "B") {
                    myLists_B.Add(L_X + "," + L_Y + "," + L_Z + "," + L_D + "," +
                        R_X + "," + R_Y + "," + R_Z + "," + R_D + "," + ts.ToString("f2"));
                }
                //----------------------加入Lsit------------------------
                //Debug.Print((myLists_A[myLists_A.Count() - 1]).ToString());
            }
        }
        //Update--------------------------------------
        private void Update(string Left_X, string Left_Y, string Left_Z, string Left_Diameter, string Right_X, string Right_Y, string Right_Z, string Right_Diameter, double ts) {
            if (myLists_A.Count() >= Change_List || myLists_B.Count() >= Change_List) { // 將收到的數值放入List中儲存
                //Debug.Print((myLists_A[myLists_A.Count() - 1]).ToString());

                if (backgroundWorker1.IsBusy == false) { // 如果背景處理已經不再Busy，則改變當前暫存區(Buffer)
                    if (list_current == "A") {
                        list_current = "B";
                    } else if (list_current == "B") {
                        list_current = "A";
                    }
                    backgroundWorker1.RunWorkerAsync();
                }
            }
        }
//backgroundWorker1_DoWork---------------------
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) { // 背景處理做的事情:將List的數值Append至檔案中
            if (list_current == "A") {
                File.AppendAllLines(FilePath_eyeMovment, myLists_B);
            } else if (list_current == "B") {
                File.AppendAllLines(FilePath_eyeMovment, myLists_A);
            }
            //Debug.Print(myLists_A.Count().ToString());
        }
        //backgroundWorker1_RunWorkerCompleted--------
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) { // 做完背景處理後，改變當前Buffer
            if (list_current == "A") {
                myLists_B.Clear();
            } else if (list_current == "B") {
                myLists_A.Clear();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) { // 若"開始按鈕"選取狀態發生改變
            if (checkBox1.Checked == true) { // 按下開始時(已被Checked)
                checkBox1.Text = "End"; // 改變按鈕文字
                String now = DateTime.Now.ToString("MM-dd HH.mm.ss"); // 當前時間
                FilePath_eyeMovment = FilePath_eyeMovment + "\\" + now + " EyeData" + ".csv"; // 定義路徑
                string[] createText = { "Left X , Left Y , Left Z , Left Diameter ," + // 第一列文字內容
                        " Right X , Right Y , Right Z , Right Diameter ,TimeStamp" };
                File.WriteAllLines(FilePath_eyeMovment, createText, Encoding.UTF8); // 寫入第一列文字至檔案
            } else { // 按下結束後
                //------------將剩餘list.count未滿300的存檔------------
                //Debug.Print(myLists_A.Count().ToString());
                //Debug.Print(myLists_B.Count().ToString());
                if (list_current == "A") { 
                    File.AppendAllLines(FilePath_eyeMovment, myLists_A, Encoding.UTF8);
                    myLists_A.Clear();
                } else if (list_current == "B") {
                    File.AppendAllLines(FilePath_eyeMovment, myLists_B, Encoding.UTF8);
                    myLists_B.Clear();
                }
                //------------將剩餘list.count未滿300的存檔------------
                checkBox1.Text = "Start"; // 改變按鈕文字
                myLists_A.Clear(); // 清空陣列
                myLists_B.Clear(); // 清空陣列
                stopwatch.Reset(); // 重置
                FilePath_eyeMovment = System.Environment.CurrentDirectory + "\\csv";        // 存檔時的路徑
            }
        }
        public void button3_Click(object sender, EventArgs e) { //提交按鈕被點擊時
            List_y.Clear();
            List_x.Clear();
            List_String_Single.Clear();
            g = panel1.CreateGraphics(); // 建立一個圖形
            //g.Clear(Color.Transparent);
            width = TextRenderer.MeasureText(g, "。", font, Size, flags).Width + LetterSpacing;
            height = TextRenderer.MeasureText(g, "。", font, Size, flags).Height + LineSpacing;

            b = new Bitmap(screenWidth, screenHeight); // 建立一個與螢幕相同大小的Bitmap
            
            g = Graphics.FromImage(b);

            StreamReader file = new StreamReader(FileName);


            // https://social.msdn.microsoft.com/Forums/vstudio/en-US/9c85d1f3-66a1-42be-9a6e-175fc3c6e739/remove-picture-box-controls?forum=csharpgeneral
            for (int k = 0; k < pic_num;k++) // 這裡不知為啥foreach 只跑一次，消不掉所有的picturebox，才會多一個pic_num判斷總input圖片數
            {
                foreach (Control control in panel3.Controls)
                {
                    PictureBox picture = control as PictureBox;
                    if (picture != null)
                    {
                        panel3.Controls.Remove(picture);
                    }
                }
            }



            panel3.Refresh();
            pic_num = 1;

            while ((InputString = file.ReadLine()) != null) { // 一列一列讀
                if (InputString.Substring(0, 1) == "#") { //若為 # 則更新當前頁面
                    NowPage = Convert.ToInt32(InputString.Substring(1, 1)); // 將檔案中的"頁數"文字，轉成Int格式
                }
                if (SelectPage == NowPage) {;
                    if (InputString.Substring(0, 1) != "#" && InputString.Substring(0, 1) == "@") { //是圖片且不是文字
                        
                        string[] pic_array;

                        pic_array = InputString.Split(','); // 因為匯入的檔案，設定文字的方式是 : 圖片標頭(0),X座標(1),Y座標(2),檔案名稱(3)，因此用逗號將他們隔開
                        string pic_name = pic_array[3];
                        int x = Convert.ToInt32(pic_array[1]);
                        int y = Convert.ToInt32(pic_array[2]);
                        //Debug.Print(pic_name);

                        

                        if (File.Exists(pic_path + "\\" + pic_name))
                        {
                            pic_num++;
                            pic  = new PictureBox();
                            pic.SizeMode = PictureBoxSizeMode.AutoSize;
                            pic.Image = Image.FromFile(pic_path + "\\" + pic_name);
                            pic.BackColor = Color.Transparent;
                            pic.Name = "pic";
                            pic.Location = new Point(x, y);
                            panel3.Controls.Add(pic);
                            pic.BringToFront();

                            //Debug.Print(pic.Name);
                            //ww++;
                        }
                        else
                        {
                            MessageBox.Show("圖片載入失敗，可能圖片路徑或檔名錯誤 !");
                        }
                        
                    }
                    if (InputString.Substring(0, 1) != "#" && InputString.Substring(0, 1) != "@") { 
                        NowPage = SelectPage;
                        string[] InputString_array; // 定義一個字串陣列，存放該列中的各個字元
                        //Debug.Print(InputString);
                        Row++; // 每讀取一列，就++
                        InputString_array = InputString.Split(','); // 因為匯入的檔案，設定文字的方式是 : 排列方式(0),X座標(1),Y座標(2),文字內容(3)，因此用逗號將他們隔開
                        InputString = InputString_array[3]; // or InputString_array[InputString_array.Length - 1]
                        Point startPoint = new Point(Convert.ToInt32(InputString_array[1]), Convert.ToInt32(InputString_array[2]));
                        direction = InputString_array[0];
                        MatchCollection match = Regex.Matches(InputString, @"."); // 正則表示式，將文字內容以"任何字元(.)"將他們切割成一個一個文字
                        foreach (Match m in match) { // 將每個match到的文字(也就是該列的各個文字)畫圖
                            List_String_Single.Add(m.ToString());
                            if (direction == "v") { // 垂直
                                //TextRenderer.DrawText(g, m.ToString(), font, startPoint, Color.Black, flags); // 設定
                                TextRenderer.DrawText(g, m.ToString(), font, startPoint, Color.FromArgb(0, rr, gg, bb), flags); // 設定
                                List_x.Add(startPoint.X); // 新增當前X座標至List
                                List_y.Add(startPoint.Y); // 新增當前Y座標至List
                                startPoint.Y += (int)height; // 每次執行完一個字元後，就加行距
                            } else if (direction == "h") { // 水平
                                //TextRenderer.DrawText(g, m.ToString(), font, startPoint, Color.Black, flags);
                                TextRenderer.DrawText(g, m.ToString(), font, startPoint, Color.FromArgb(0, rr, gg, bb), flags);
                                List_x.Add(startPoint.X); // 新增當前X座標至List
                                List_y.Add(startPoint.Y); // 新增當前Y座標至List
                                startPoint.X += (int)width; // 每次執行完一個字元後，就加字距
                            }
                        }
                    }
                    
                }

            }
            file.Close(); // 關閉檔案
            //Debug.Print(Row.ToString());
            //foreach (double d in List_y) {
            //    Debug.Print(d.ToString());
            //}
            


            pictureBox1.Image = b; // 印出圖片
            pictureBox1.BringToFront();
        }

        private void button4_Click(object sender, EventArgs e) // 匯入背景
        {
            OpenFileDialog OpenImage = new OpenFileDialog(); // 設定一個Dialog物件 
            OpenImage.InitialDirectory = ImagePath; // 設定初始路徑
            OpenImage.Filter = "圖檔|*.jpg;*.png;*.jpeg"; // 設定選取檔案格式
            //this.SendToBack();
            if (OpenImage.ShowDialog() == DialogResult.OK) { // 若按下確定
                BackgroundImage = new Bitmap(OpenImage.FileName); 
                //pictureBox2.Image = BackgroundImage;
                //pictureBox2.BackgroundImageLayout = ImageLayout.Center;
                //pictureBox2.Refresh();
                //pictureBox2.BringToFront();
                panel3.BackgroundImage = BackgroundImage; // 將此容器(panel)的背景，設為載入的圖片
                panel3.BackgroundImageLayout = ImageLayout.Center; // 將圖片放置正中間(Center)
                //panel3.Location = new Point(15, 15);
                //panel3.Refresh();
                //pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
                //panel2.Parent = this.pictureBox2;//將pane2父控件設為背景圖片控件
                //panel1.Parent = this.pictureBox2;//將panel父控件設為背景圖片控件

                //pictureBox1.Hide();
                //pictureBox2.Hide();
                //panel1.Hide();
                //panel2.Hide();
                //panel3.Hide();

                //https://blog.csdn.net/u013816709/article/details/38299005
            }

        }

        
        

        private void button6_Click(object sender, EventArgs e) // 印出表格
        {
            Graphics table = panel5.CreateGraphics();
            Pen pen = new Pen(Color.Black, 1.0f);
            pen.DashStyle = DashStyle.Dash;
            Pen pen2 = new Pen(Color.Red, 1.0f);
            for (int i = 0; i <= 9; i++)
            {
                if (i % 4 == 0)
                {
                    table.DrawLine(pen2, 0, 100 * i, 1700, 100 * i);
                }
                else
                {
                    table.DrawLine(pen, 0, 100 * i, 1700, 100 * i);
                }
            }
            for (int i = 0; i <= 18; i++)
            {
                if (i % 5 == 0)
                {
                    table.DrawLine(pen2, 100 * i, 0, 100 * i, 900);
                }
                else
                {
                    table.DrawLine(pen, 100 * i, 0, 100 * i, 900);
                }
            }
            TextRenderer.DrawText(table ,"(0,0)", font2, new Point(0, 0), Color.Black); // 設定
            TextRenderer.DrawText(table, "(1700,0)", font2, new Point(1700, 0), Color.Black); // 設定
            TextRenderer.DrawText(table, "(0,900)", font2, new Point(0, 900), Color.Black); // 設定
            TextRenderer.DrawText(table, "(1700,900)", font2, new Point(1700, 900), Color.Black); // 設定
        }
    }
}
