using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Caldara_Visualisation
{
    //Домножить координаты на коэфициент
    public partial class Metrics : Form
    {
        string fileName;
        public ListMetric listM;
        public ListRelation listR;
        public List<Metric> RecMetrics, ColMetrics;
        public InternalRectangle Screen, CurrentScreen;
        public FileStructure Struct;
        public List<TreeNode> Nodes;
        public List<TreeNode> CurrentNodes;
        public List<TreeNode> OpenedNodes;
        public Rectangle Rec;
        public static Color[] Colors = new Color[6] { Color.FromArgb(207, 207, 207), Color.FromArgb(156, 156, 156), Color.FromArgb(255, 160, 122), Color.FromArgb(250, 128, 114), Color.FromArgb(255, 99, 71), Color.FromArgb(255, 0, 0) };

        public string currentText, currentRecMetric, currentColMetric, currentEntityType;
        public float MouseX, MouseY;
        public bool ready, text;
        public double basicX0, basicY0, basicX1, basicY1, kX, kY;
        public List<List<string>> ColorCollections, RectangleCollections;
        public List<List<double>> ValueCollections;

        Pen pen;
        Font font, font2;
        Brush brush, brush2;
        public double avgLetterWidth, avgLetterHeight;
        public double criticalColorValue;

        public Metrics (string fileName)
        {
            InitializeComponent();
            this.fileName = fileName;
            DoubleBuffered = true; //Чтобы не моргало
            listM = new ListMetric();
            listR = new ListRelation();
            RecMetrics = new List<Metric>();
            ColMetrics = new List<Metric>();
            Nodes = new List<TreeNode>();
            CurrentNodes = new List<TreeNode>();
            OpenedNodes = new List<TreeNode>();
            Rec = new Rectangle();

            RectangleCollections = new List<List<string>>();
            RectangleCollections.Add(new List<string>() { "LOC_METHOD", "NCNBLOC_METHOD" });
            RectangleCollections.Add(new List<string>() { "LOC_CLASS", "NOPRIVDATADEC", "NOPROTDATADECL", "NOPUBDATADECL", "NOSTDATADECL", "NOPRIVMETH", "NOPROTMETH", "NOPTRDECL", "NOPUBMETH", "NOVIRTMETH", "NCNBLOC_CLASS" });
            RectangleCollections.Add(new List<string>() { "NOCLASSDECL", "CLOC_FILE", "NCNBLOC_FILE", "LOC_FILE", "RACOMMFILE" });
            RectangleCollections.Add(new List<string>() { "NOFUNCDEFDIR", "NOCPPFILE", "LOC_DIR", "NCNBLOC_DIR", "CLOC_DIR", "TOTFILE", "NOHEADERFILE " });


            ColorCollections = new List<List<string>>();
            ColorCollections.Add(new List<string>() { "CYCLOMATIC", "NOPARMS", "MAXLEVEL", "NOLOOPS", "NOIF", "NOBRANCH", "NORET", "LEVINHER", "NOFANIN", "NOFANOUT", "NOGLOVARUSED", "NOSTATICVARUSED" });
            ColorCollections.Add(new List<string>() { "AVGCOMPMETH", "MAXCOMPMETH", "TOTCOMPMETH", "NOINLINEMETH", "NOCONSTMETH", "NONESTEDCLASS", "NOMEMBERDATA" });
            ColorCollections.Add(new List<string>() { "RNOCONTROLSTAT", "RNOACCTOGLOB", "DIRECTIVES", "NOFUNCDEFFILE", "NOPUBMETH", "AVGCOMPFILE", "MAXCOMPFILE", "TOTCOMPFILE", "NOCONSTDECL", "NODATADECL" });
            ColorCollections.Add(new List<string>() { "RACOMMDIR", "NOFUNCDEFDIR", "AVGCOMPDIR", "MAXCOMPDIR", "TOTCOMPDIR" });


            ValueCollections = new List<List<double>>();
            ValueCollections.Add(new List<double>() { 12, 5, 7, 3, 15, 12, 7, 2, 5, 24, 5, 5 });
            ValueCollections.Add(new List<double>() { 5, 8, 40, 4, 2, 5, 5 });
            ValueCollections.Add(new List<double>() { 30, 8, 7, 17, 13, 8, 13, 20, 5, 7 });
            ValueCollections.Add(new List<double>() { 5, 60, 10, 90 });


            ready = false;
            text = false;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 2;
            comboBox3.SelectedIndex = 0;
            label1.Visible = false;
            currentText = "";

            pen = new Pen(Color.Black, 10);
            font = new Font(FontFamily.GenericSansSerif, 14);
            brush = new SolidBrush(Color.Black);
            font2 = new Font(FontFamily.GenericSansSerif, 12);
            brush2 = new SolidBrush(Color.Green);

            label6.Visible = false;
            label6.Text = "abcdefghigklmnopqrstuvwxyz.:_";
            criticalColorValue = 0;

        }

        /// <summary>
        /// Функции
        /// </summary>
        ///
        private void CreateStructure () //Заполняем дерево значениями
        {
            Struct.BuildMetricsFileStructure();
            Struct.Struct.FillTree(Struct.Struct.Root, RecMetrics, ColMetrics);
        }

        public void OpenNodeWithChildren () //Чтобы не было одного прямоугольника на экране
        {
            TreeNode currentRoot = Struct.Struct.Root;

            while (currentRoot.Children.Count < 2 && currentRoot.Children.Count > 0)
            {
                Struct.Struct.OpenNode(currentRoot, currentRoot.E);
                currentRoot = currentRoot.Children[0];
            }

            Struct.Struct.OpenNode(currentRoot, currentRoot.E);
        }

        /// <summary>
        /// События
        /// </summary>

        private void Metrics_Paint (object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            if (ready)
            {
                label6.Font = new Font(FontFamily.GenericSerif, 14);
                double nameWidth = label6.Width;
                double nameHeight = label6.Height;
                float a, b; //Для координат

                for (int i = Nodes.Count - 1 ; i >= 0 ; i--)
                {
                    int X0 = Convert.ToInt32(Nodes[i].Rec.X0);
                    int Y0 = Convert.ToInt32(Nodes[i].Rec.Y0);
                    int X1 = Convert.ToInt32(Nodes[i].Rec.X1);
                    int Y1 = Convert.ToInt32(Nodes[i].Rec.Y1);

                    Point[] Mas = new Point[4] { new Point(X0, Y0), new Point(X0, Y1), new Point(X1, Y1), new Point(X1, Y0) }; //Точки для границ
                    Point[] Mas2 = new Point[4] { new Point(X0 + 1, Y0 + 1), new Point(X0 + 1, Y1), new Point(X1, Y1), new Point(X1, Y0 + 1) }; //Точки для заливки

                    label6.Text = Nodes[i].E.Name(true);
                    label6.Font = new Font(FontFamily.GenericSerif, 14);
                    nameWidth = label6.Width;
                    nameHeight = label6.Height;
                    float prevSize = 14;

                    if (Nodes[i].IsOpened) //Открытая вершина
                    {
                        e.Graphics.FillPolygon(new SolidBrush(Nodes[i].Col), Mas2); //Раскрашиваем только открытые вершины

                        while (( nameWidth > ( X1 - X0 ) || nameHeight > ( Y1 - Y0 ) ) && prevSize > 2) //надпись не влезает - уменьшим шрифт
                        {
                            label6.Font = new Font(FontFamily.GenericSerif, prevSize -= 2);

                            nameWidth = label6.Width;
                            nameHeight = label6.Height;
                        }

                        if (prevSize >= 10)
                        {
                            a = Convert.ToInt32(( X0 + ( ( X1 - X0 ) - nameWidth ) / 2 ));
                            b = Convert.ToInt32(( Y0 + ( ( Y1 - Y0 ) - nameHeight ) / 2 ));
                            e.Graphics.DrawString(Nodes[i].E.Name(true), label6.Font, brush, a, b);
                        }
                    }

                    else //подпись сущности, стоящей выше по иерархии
                    {
                        label6.Font = new Font(FontFamily.GenericSerif, 12);

                        while (( nameWidth > ( X1 - X0 ) || nameHeight > 22 ) && prevSize > 2) //надпись не влезает - уменьшим шрифт
                        {
                            label6.Font = new Font(FontFamily.GenericSerif, prevSize -= 2);

                            nameWidth = label6.Width;
                            nameHeight = label6.Height;
                        }

                        if (prevSize >= 10)
                        {
                            a = Convert.ToInt32(( X0 + ( ( X1 - X0 ) - nameWidth ) / 2 ));
                            b = Convert.ToInt32(( Y0 + ( ( 18 - nameHeight ) / 2 ) ));
                            label6.Font = new Font(FontFamily.GenericSerif, prevSize -= 2, System.Drawing.FontStyle.Bold);
                            e.Graphics.DrawString(Nodes[i].E.Name(true), label6.Font, brush, a, b);
                        }
                    }

                    e.Graphics.DrawPolygon(new Pen(brush, 2), Mas); //А рамку для всех        
                }

                label6.Text = currentText;
                label6.Font = font2;
                a = Convert.ToInt32(this.Size.Width - label6.Width - 35);
                if (currentText != "" && currentText != null) e.Graphics.DrawString(currentText, font2, brush2, a, 5); //Подпись в верхнем правом углу
            }
        }

        private void Metrics_Resize (object sender, EventArgs e)
        {
            Screen = new InternalRectangle(230, 30, this.Size.Width - 36, this.Size.Height - 56);
            CurrentScreen = new InternalRectangle(230, 30, this.Size.Width - 36, this.Size.Height - 56);

            if (ready)
            {
                Rec = new Rectangle(Screen, Nodes, Struct, criticalColorValue);
                Rec.SquarifyAll();
                Nodes.Clear();
                Struct.Struct.GetPaintNodes(Struct.Struct.Root, Nodes);
            }

            Invalidate();
        }

        private void Metrics_MouseMove (object sender, MouseEventArgs e) //Наводим на один из прямоугольников - показываем его имя
        {
            MouseX = e.X;
            MouseY = e.Y;
            text = false;
            currentText = "";

            if (Screen.X0 < MouseX && MouseX < Screen.X1 && Screen.Y0 < MouseY && MouseY < Screen.Y1)
            {
                for (int i = 0 ; i < Nodes.Count ; i++)
                {
                    double X0 = Math.Min(Nodes[i].Rec.X0, Nodes[i].Rec.X1);
                    double X1 = Math.Max(Nodes[i].Rec.X0, Nodes[i].Rec.X1);
                    double Y0 = Math.Min(Nodes[i].Rec.Y0, Nodes[i].Rec.Y1);
                    double Y1 = Math.Max(Nodes[i].Rec.Y0, Nodes[i].Rec.Y1);

                    if (Nodes[i].IsOpened && ready && Nodes[0].Rec != null && X0 < MouseX && MouseX < X1 && Y0 < MouseY && MouseY < Y1)
                    {
                        currentText = Nodes[i].E.Name(true);
                        text = true;
                        Invalidate();
                        break;
                    }
                }
            }
        }

        private void Metrics_MouseClick (object sender, MouseEventArgs e) //Кликаем на один из прямоугольников - открываем его 
        {
            MouseX = e.X;
            MouseY = e.Y;

            for (int i = 0 ; i < Nodes.Count ; i++)
            {
                double X0 = Math.Min(Nodes[i].Rec.X0, Nodes[i].Rec.X1);
                double X1 = Math.Max(Nodes[i].Rec.X0, Nodes[i].Rec.X1);
                double Y0 = Math.Min(Nodes[i].Rec.Y0, Nodes[i].Rec.Y1);
                double Y1 = Math.Max(Nodes[i].Rec.Y0, Nodes[i].Rec.Y1);

                if (Nodes[i].IsOpened && ready && Nodes[0].Rec != null && X0 < MouseX && MouseX < X1 && Y0 < MouseY && MouseY < Y1) //Проверка попадания
                {
                    if (e.Button == MouseButtons.Left) //Левая кнопка мыши
                    {
                        if (Nodes[i].Children.Count != 0) //Если есть дети
                        {
                            Struct.Struct.OpenNode(Struct.Struct.Root, Nodes[i].E);

                            Nodes.Clear();
                            Struct.Struct.GetPaintNodes(Struct.Struct.Root, Nodes);

                            Invalidate();
                            break;
                        }
                    }

                    else  //Правая кнопка мыши
                    {
                        if (Nodes[i].Parent != null) //Зачем закрывать, если нет родителя
                        {
                            Struct.Struct.CloseNode(Struct.Struct.Root, Nodes[i].Parent.E); //Ошибка!!!

                            Nodes.Clear();
                            Struct.Struct.GetPaintNodes(Struct.Struct.Root, Nodes);

                            Invalidate();
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Панель управления
        /// </summary>

        private void button4_Click (object sender, EventArgs e) //Выбираем файл-источник всех метрик
        {
            button1.Enabled = false;
            ready = false;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                listM = new ListMetric();
                listM.ReadFromFile(fileName);
                label1.Visible = false;

                currentRecMetric = comboBox1.SelectedItem.ToString();
                currentColMetric = comboBox2.SelectedItem.ToString();

                RecMetrics = listM.GetMetricsOfType(currentRecMetric);
                ColMetrics = listM.GetMetricsOfType(currentColMetric);

                Struct = new FileStructure(listM.List);
                CreateStructure();
                Nodes.Clear();
                OpenNodeWithChildren();

                button1.Enabled = true;

                try
                {
                    if (textBox1.Text != "") criticalColorValue = Convert.ToDouble(textBox1.Text);
                    else criticalColorValue = ValueCollections[comboBox3.SelectedIndex][comboBox2.SelectedIndex];
                }

                catch { criticalColorValue = ValueCollections[comboBox3.SelectedIndex][comboBox2.SelectedIndex]; } //На случай неверного формата

                Screen = new InternalRectangle(230, 30, this.Size.Width - 36, this.Size.Height - 56);
                CurrentScreen = new InternalRectangle(230, 30, this.Size.Width - 36, this.Size.Height - 56);

                Nodes = new List<TreeNode>();
                Rec = new Rectangle(Screen, Nodes, Struct, criticalColorValue);
                Rec.SquarifyAll();
                Struct.Struct.GetPaintNodes(Struct.Struct.Root, Nodes);
                textBox1.Text = criticalColorValue.ToString();

                ready = true;
            }

            catch
            {
                label1.Visible = true;
            }

            Cursor.Current = Cursors.Default;
            Invalidate();
        }

        private void comboBox1_SelectedIndexChanged (object sender, EventArgs e) //Выбираем метрику прямоугольников
        {
            currentRecMetric = comboBox1.SelectedItem.ToString();
        }

        private void comboBox2_SelectedIndexChanged (object sender, EventArgs e) //Выбираем метрику цвета
        {
            currentColMetric = comboBox2.SelectedItem.ToString();
            if (comboBox3.SelectedIndex >= 0) criticalColorValue = ValueCollections[comboBox3.SelectedIndex][comboBox2.SelectedIndex];
            textBox1.Text = "";
        }

        private void comboBox3_SelectedIndexChanged (object sender, EventArgs e) //Выбираем тип сущности, с которым будем работать
        {
            comboBox1.DataSource = new List<string>(RectangleCollections[comboBox3.SelectedIndex]);
            comboBox2.DataSource = new List<string>(ColorCollections[comboBox3.SelectedIndex]);
            textBox1.Text = "";
        }

        private void button1_Click (object sender, EventArgs e) //Отрисовываем выбранную метрику
        {
            //Для цветов не менять прямоугольник
            RecMetrics = listM.GetMetricsOfType(currentRecMetric);
            ColMetrics = listM.GetMetricsOfType(currentColMetric);

            CreateStructure();
            Nodes.Clear();

            OpenNodeWithChildren();
            CurrentScreen = new InternalRectangle(Screen);

            try
            {
                if (textBox1.Text != "") criticalColorValue = Convert.ToDouble(textBox1.Text);
                else criticalColorValue = ValueCollections[comboBox3.SelectedIndex][comboBox2.SelectedIndex];
            }

            catch { criticalColorValue = ValueCollections[comboBox3.SelectedIndex][comboBox2.SelectedIndex]; } //На случай неверного формата

            Nodes = new List<TreeNode>();
            Rec = new Rectangle(Screen, Nodes, Struct, criticalColorValue);
            Rec.SquarifyAll();
            Struct.Struct.GetPaintNodes(Struct.Struct.Root, Nodes);

            textBox1.Text = criticalColorValue.ToString();
            

            Invalidate();
        }
    }
}
