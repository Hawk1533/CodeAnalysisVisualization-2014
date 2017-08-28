using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Caldara_Visualisation
{
    public class Rectangle
    {
        private InternalRectangle _currentRectangle;
        private List<InternalRectangle> _row;
        private FileStructure _tree;
        private List<TreeNode> _nodes;
        private List<TreeNode> _currentNodes;
        private static double k;
        private bool _vertical;
        public int currentPlaced;
        public static Color[] Colors = new Color[6] { Color.FromArgb(207, 207, 207), Color.FromArgb(156, 156, 156), Color.FromArgb(255, 160, 122), Color.FromArgb(250, 128, 114), Color.FromArgb(255, 99, 71), Color.FromArgb(255, 0, 0) };
        private double _criticalColorValue;



        public Rectangle ()
        {
        }

        public Rectangle (InternalRectangle HoleRectangle, List<TreeNode> CurrentNodes, FileStructure Tree, double criticalColorValue)
        {
            this._currentRectangle = HoleRectangle;
            this._row = new List<InternalRectangle>();
            this._nodes = new List<TreeNode>(CurrentNodes);
            this._currentNodes = new List<TreeNode>(CurrentNodes);
            k = GetK();
            if (Tree != null) this._tree = new FileStructure(Tree);
            double a = HoleRectangle.GetWidth(ref _vertical); //Пока так!
            this._criticalColorValue = criticalColorValue;
        }

        public InternalRectangle CurrentRectangle
        {
            get { return _currentRectangle; }
            set { _currentRectangle = value; }
        }

        public List<TreeNode> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        public List<TreeNode> CurrentNodes
        {
            get { return _currentNodes; }
            set { _currentNodes = value; }
        }

        public List<InternalRectangle> Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public FileStructure Tree
        {
            get { return _tree; }
            set { _tree = value; }
        }

        public bool Vertical
        {
            get { return _vertical; }
            set { _vertical = value; }
        }

        public double CriticalColorValue
        {
            get { return _criticalColorValue; }
            set { _criticalColorValue = value; }
        }

        public void SquarifyAll ()
        {
            currentPlaced = 0;
            Nodes.Clear();
            CurrentNodes.Clear();
            Nodes.Add(Tree.Struct.Root);
            CurrentNodes.Add(Tree.Struct.Root);
            k = GetK();
            Squarify(0, 0, CurrentRectangle.GetWidth(ref _vertical));

            for (int i = 0 ; i < Nodes.Count ; i++)
            {
                currentPlaced = 0;
                CurrentNodes.Clear();
                CurrentNodes.AddRange(Nodes[i].Children);

                if (Nodes[i].E.Name(true) == "inet.h")
                {
                    k = 3;
                }

                if (Nodes[i].Children.Count > 0) //>1
                {
                    CurrentRectangle = new InternalRectangle(Nodes[i].Rec.X0 + 5, Nodes[i].Rec.Y0 + 17, Nodes[i].Rec.X1 - 5, Nodes[i].Rec.Y1 - 5);
                    k = GetK(); //Меняется площадь квадрата
                }

                else CurrentRectangle = Nodes[i].Rec;

                Squarify(0, 0, CurrentRectangle.GetWidth(ref _vertical));
                Nodes.AddRange(CurrentNodes);

                Nodes.RemoveAt(i);
                i--;
            }
        }

        public InternalRectangle MergeRectangles (List<InternalRectangle> R) //Соединение в один прямоугольник
        {
            int a = R.Count - 1;
            InternalRectangle Result = new InternalRectangle(R[0].X0, R[0].Y0, R[a].X1, R[a].Y1); //Что для горизонтального

            return Result;
        }

        public void SeparateRectangles (List<InternalRectangle> R) //Отделяем уже разделённые прямоугольники
        {
            InternalRectangle A = MergeRectangles(R);

            if (_vertical) CurrentRectangle = new InternalRectangle(A.X1, A.Y0, CurrentRectangle.X1, CurrentRectangle.Y1);
            else CurrentRectangle = new InternalRectangle(CurrentRectangle.X0, CurrentRectangle.Y0, A.X1, A.Y1);
        }

        public double Width (List<InternalRectangle> R)
        {
            SeparateRectangles(R);

            return CurrentRectangle.GetWidth(ref _vertical);
        }

        public void LayOutRow (int N0, int N1, double width, bool vertical)
        {
            double x0 = CurrentRectangle.X0;
            double y0;

            //Просчитываем координату 'y' для горизонтального и вертикального расположения
            if (_vertical) y0 = CurrentRectangle.Y0;
            else y0 = CurrentRectangle.Y1;

            Row.Clear();
            double S = 0;
            double currentWidth = 0, currentHeight = 0;

            //Добавляем прямоугольники только по площади
            for (int i = N0 ; i <= N1 ; i++)
            {
                Row.Add(new InternalRectangle(0, 0, 1, CurrentNodes[i].S * k));
                S += CurrentNodes[i].S * k;
            }

            //Для вертикальной и горизонтальной позиций координаты на форме будут отличаться
            if (vertical)
            {
                for (int i = 0 ; i < Row.Count ; i++)
                {
                    currentWidth = width * Row[i].GetS() / S;
                    currentHeight = Row[i].GetS() / currentWidth;
                    Row[i] = new InternalRectangle(x0, y0, x0 + currentHeight, y0 + currentWidth);

                    //Задаём стартовые координаты для следующего прямоугольника
                    x0 = Row[i].X0;
                    y0 = Row[i].Y1;
                }
            }

            else
            {
                for (int i = 0 ; i < Row.Count ; i++)
                {
                    currentWidth = width * Row[i].GetS() / S;
                    currentHeight = Row[i].GetS() / currentWidth;
                    Row[i] = new InternalRectangle(x0, y0, x0 + currentWidth, y0 - currentHeight);

                    //Задаём стартовые координаты для следующего прямоугольника
                    x0 = Row[i].X1;
                    y0 = Row[i].Y0;
                }
            }

            foreach (InternalRectangle r in Row)
            {
                CurrentNodes[currentPlaced].Rec = new InternalRectangle(r, true); //Создадим для формы
                CurrentNodes[currentPlaced].Col = GetColor(CurrentNodes[currentPlaced]); //Запишем цвет
                currentPlaced++;

            }
        }

        public void Squarify (int N0, int N1, double width)
        {
            if (( N1 < CurrentNodes.Count - 1 ) && ( Worst(N0, N1, width) >= Worst(N0, N1 + 1, width) ))
            {
                Squarify(N0, N1 + 1, width);
            }

            else
            {
                if (N1 >= CurrentNodes.Count - 1) N1 = CurrentNodes.Count - 1;

                LayOutRow(N0, N1, width, _vertical);

                if (N1 + 1 < CurrentNodes.Count)
                {
                    Squarify(N1 + 1, N1 + 2, Width(Row));
                }
            }

            return;
        }

        public double GetK ()
        {
            double Sum = 0;

            foreach (TreeNode Child in CurrentNodes) Sum += Child.S;

            double a = CurrentRectangle.GetS(1);

            return CurrentRectangle.GetS(1) / Sum;
        }

        public double Worst (int N0, int N1, double width) //Вычисляем наихудшее отношение сторон (наиболее далёкое от квадрата)
        {
            double worst = 0;
            double S = 0; //площадь сумарного прямоугольника

            for (int i = N0 ; i <= N1 ; i++)
            {
                S += CurrentNodes[i].S * k;
            }

            for (int i = N0 ; i <= N1 ; i++)
            {
                double t = Math.Max(( S * S ) / ( width * width * CurrentNodes[i].S * k ), ( width * width * CurrentNodes[i].S * k ) / ( S * S ));

                if (worst < t) worst = t;
            }

            return worst;
        }

        private Color GetColor (TreeNode N)
        {
            int n = 0;
            N.CountLeaves(N, ref n);

            double MetricValue = 0;
            if (n != 0) MetricValue = N.ColorValue / n;

            if (MetricValue <= CriticalColorValue / 5) return Colors[0];
            else if (MetricValue <= CriticalColorValue / 5 * 2) return Colors[1];
            else if (MetricValue <= CriticalColorValue / 2) return Colors[2];
            else if (MetricValue <= CriticalColorValue / 5 * 4) return Colors[3];
            else if (MetricValue <= CriticalColorValue) return Colors[4];
            else return Colors[5];
        }

        public void Sort (int l, int r)
        {
            QuickSort(l, r);

            int len = CurrentNodes.Count;
            TreeNode temp;

            //Поменяем с зада на перёд
            for (int i = 0 ; i < len / 2 ; i++)
            {
                //temp = new TreeNode(CurrentNodes[i], true);
                //CurrentNodes[i] = new TreeNode(CurrentNodes[len - i - 1], true);
                //CurrentNodes[len - i - 1] = new TreeNode(temp, true);

                temp = CurrentNodes[i];
                CurrentNodes[i] = CurrentNodes[len - i - 1];
                CurrentNodes[len - i - 1] = temp;
            }

            //foreach (TreeNode Node in CurrentNodes) Node.Rec = null;
        }

        public void QuickSort (int l, int r)
        {
            TreeNode temp;
            double x = CurrentNodes[l + ( r - l ) / 2].S;
            //запись эквивалентна (l+r)/2,
            //но не вызввает переполнения на больших данных
            int i = l;
            int j = r;
            //код в while обычно выносят в процедуру particle
            while (i <= j)
            {
                while (CurrentNodes[i].S < x) i++;
                while (CurrentNodes[j].S > x) j--;
                if (i <= j)
                {
                    temp = new TreeNode(CurrentNodes[i], true);
                    CurrentNodes[i] = new TreeNode(CurrentNodes[j], true);
                    CurrentNodes[j] = new TreeNode(temp, true);
                    i++;
                    j--;
                }
            }
            if (i < r)
                QuickSort(i, r);

            if (l < j)
                QuickSort(l, j);
        }
    }
}
