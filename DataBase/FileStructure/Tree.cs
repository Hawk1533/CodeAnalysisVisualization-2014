using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Caldara_Visualisation
{
    public class Tree
    {
        private TreeNode _root; //Корень дерева. У нас это сущность, которая ни к кому не Contain в классах и папка проги в структуре.
        private int maxLevel; //Высота дерева.

        public Tree (Tree tree)
        {
            this._root = tree.Root;
        }

        public Tree (TreeNode root)
        {
            this._root = root;
        }

        public TreeNode Root
        {
            get
            {
                return _root;
            }

            set
            {
                _root = value;
            }
        }

        //Общие функции

        public bool Contains (Metric M, TreeNode root, bool result)
        {
            if (M.getEntity == root.E)
            {
                result = true;
                return true;
            }

            foreach (TreeNode Child in root.Children)
            {
                if (result) return true;
                else return Contains(M, Child, result);
            }

            return result;
        }

        public void GetPaintNodes (TreeNode root, List<TreeNode> Nodes)
        {
            bool fin = false;

            if (root.IsOpened && root.S != 0)
            {
                Nodes.Add(root);
                fin = true;
            }

            else Nodes.Add(root);

            foreach (TreeNode Child in root.Children)
            {
                if (fin)
                {
                    fin = false;
                    break;
                }

                GetPaintNodes(Child, Nodes);
            }
        }

        public void GetParentsOfOpenedNodes (TreeNode root, List<TreeNode> Nodes)
        {
            if (root.IsOpened && root.S != 0 && !Nodes.Contains(root.Parent))
            {
                Nodes.Add(root.Parent); //хочу по ссылке
            }

            foreach (TreeNode Child in root.Children)
            {
                GetParentsOfOpenedNodes(Child, Nodes);
            }
        }

        public void OpenNode (TreeNode root, Entity e)
        {
            if (root.E == e)
            {
                foreach (TreeNode Child in root.Children) Child.IsOpened = true;

                if (root.Children.Count != 0) root.IsOpened = false;

                return;
            }

            foreach (TreeNode Child in root.Children)
            {
                OpenNode(Child, e);
            }
        }

        public void CloseNode (TreeNode root, Entity e)
        {
            if (root.E == e) root.IsOpened = true;

            foreach (TreeNode Child in root.Children)
            {
                if (isChildOf(Child, e)) Child.IsOpened = false;
            }


            foreach (TreeNode Child in root.Children)
            {
                CloseNode(Child, e);
            }
        }

        public bool isChildOf (TreeNode Node, Entity Parent)
        {
            TreeNode currentNode = Node;

            while (currentNode.Parent != null)
            {
                if (currentNode.E == Parent) return true;

                currentNode = currentNode.Parent;
            }

            return false;
        }

        public bool HaveOpenedChildren (TreeNode root, bool result)
        {
            foreach (TreeNode Child in root.Children)
            {
                if (Child.IsOpened)
                {
                    result = true;
                    return true;
                }
            }

            foreach (TreeNode Child in root.Children)
            {
                if (result) return true;
                else return HaveOpenedChildren(Child, result);
            }

            return result;
        }

        public void SortTree (TreeNode root)
        {
            if (root.Children.Count > 1) Sort(root.Children);

            foreach (TreeNode Child in root.Children)
            {
                SortTree(Child);
            }
        }

        public void Sort (List<TreeNode> CurrentNodes)
        {
            int l = 0, r = CurrentNodes.Count - 1;
            QuickSort(CurrentNodes, l, r);

            int len = CurrentNodes.Count;
            TreeNode temp;

            //Поменяем с зада наперёд
            for (int i = 0 ; i < len / 2 ; i++)
            {
                temp = new TreeNode(CurrentNodes[i], true);
                CurrentNodes[i] = new TreeNode(CurrentNodes[len - i - 1], true);
                CurrentNodes[len - i - 1] = new TreeNode(temp, true);
            }
        }

        public void QuickSort (List<TreeNode> CurrentNodes, int l, int r)
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
                QuickSort(CurrentNodes, i, r);

            if (l < j)
                QuickSort(CurrentNodes, l, j);
        }

        //Функции для метрик

        public void FillTree (TreeNode root, List<Metric> RecMetrics, List<Metric> ColMetrics) //Метрики
        {
            TourDeRectangles(root, RecMetrics);
            CleanMetricsTree(Root);
            TourDeColors(root, ColMetrics);
            SortTree(root);

            TreeNode currentNode = Root;

            while (currentNode.Children.Count < 2 && currentNode.Children.Count > 0)
            {
                currentNode = new TreeNode(currentNode.Children[0], true);
            }

            Root = new TreeNode(currentNode, true);
            currentNode.Parent = null;
        }

        public void TourDeRectangles (TreeNode root, List<Metric> RecMetrics) //Заполним значение площадей прямоугольников в дереве
        {
            foreach (Metric M in RecMetrics) //Считаем значение площади для каждой вершины на этом уровне
            {
                if (M.getEntity.Path.Contains(root.E.Path))
                {
                    if (root.E.EntityType != EntityTypes.Class && root.E.EntityType != EntityTypes.ClassMember) //Директория или файл
                    {
                        root.S += M.getValue;
                    }

                    else if (root.E.EntityType == EntityTypes.Class) //Класс
                    {
                        if (M.getEntity.Class == root.E.Class) root.S += M.getValue;
                    }

                    else if (root.E.EntityType == EntityTypes.ClassMember) //Часть класса
                    {
                        if (M.getEntity.ClassMember == root.E.ClassMember) root.S += M.getValue;
                    }
                }

                root.Name = root.E.Name();
            }

            foreach (TreeNode Child in root.Children) //Переходим на уровень ниже
            {
                TourDeRectangles(Child, RecMetrics);
            }
        }

        public void TourDeColors (TreeNode root, List<Metric> ColMetrics) //Заполним значение цвета прямоугольников в дереве
        {
            foreach (Metric M in ColMetrics) //Считаем значение цвета для каждой вершины на этом уровне
            {
                if (M.getEntity.Path.Contains(root.E.Path))
                {
                    if (root.E.EntityType != EntityTypes.Class && root.E.EntityType != EntityTypes.ClassMember) //Директория или файл
                    {
                        root.ColorValue += M.getValue;
                    }

                    else if (root.E.EntityType == EntityTypes.Class) //Класс
                    {
                        if (M.getEntity.Class == root.E.Class) root.ColorValue += M.getValue;
                    }

                    else if (root.E.EntityType == EntityTypes.ClassMember) //Часть класса
                    {
                        if (M.getEntity.ClassMember == root.E.ClassMember) root.ColorValue += M.getValue;
                    }
                }
            }


            foreach (TreeNode Child in root.Children) //Переходим на уровень ниже
            {
                TourDeColors(Child, ColMetrics);
            }
        }

        public void CleanMetricsTree (TreeNode root)
        {
            for (int i = 0 ; i < root.Children.Count ; i++)
            {
                if (root.Children[i].S == 0)
                {
                    root.Children.RemoveAt(i); //Записываем номера, которые хотим удалить    
                    i--;
                }
            }

            foreach (TreeNode Child in root.Children)
            {
                CleanMetricsTree(Child);
            }
        } //Чистим пустые ветви


        //Функции для отношений

        public void FillTree (TreeNode root, List<Relation> CurrentRelations) //Отношений
        {
            TourDeRelations(root, CurrentRelations);

            //CleanRelationsTree(Root);

            SortTree(root);

            TreeNode currentNode = Root;

            while (currentNode.Children.Count < 2 && currentNode.Children.Count > 0)
            {
                currentNode = new TreeNode(currentNode.Children[0], true);
            }

            Root = new TreeNode(currentNode, true);
            currentNode.Parent = null;
        }

        public void TourDeRelations (TreeNode root, List<Relation> Relations) //Заполним связанные отношения
        {
            foreach (Relation R in Relations) //Считаем значение цвета для каждой вершины на этом уровне
            {
                if (R.getEntityA.Path.Contains(root.E.Path) || R.getEntityB.Path.Contains(root.E.Path))
                {
                    if (root.E.EntityType != EntityTypes.Class && root.E.EntityType != EntityTypes.ClassMember) //Директория или файл
                    {
                        root.Connected.Add(R);
                    }

                    else if (root.E.EntityType == EntityTypes.Class) //Класс
                    {
                        if (R.getEntityA.Class == root.E.Class || R.getEntityB.Class == root.E.Class) root.Connected.Add(R);
                    }

                    else if (root.E.EntityType == EntityTypes.ClassMember) //Часть класса
                    {
                        if (R.getEntityA.ClassMember == root.E.ClassMember || R.getEntityB.ClassMember == root.E.ClassMember) root.Connected.Add(R);
                    }
                }
            }


            foreach (TreeNode Child in root.Children) //Переходим на уровень ниже
            {
                TourDeRelations(Child, Relations);
            }
        }

        public void CleanRelationsTree(TreeNode root) //Чистим пустые ветви
        {
            for (int i = 0 ; i < root.Children.Count ; i++)
            {
                if (root.Children[i].Connected.Count == 0)
                {
                    root.Children.RemoveAt(i); //Записываем номера, которые хотим удалить    
                    i--;
                }
            }

            foreach (TreeNode Child in root.Children)
            {
                CleanRelationsTree(Child);
            }
        }

        public List<TreeNode> GetPath (TreeNode A, TreeNode B)
        {
            List<TreeNode> Path = new List<TreeNode>();
            Path.Add(A);
            TreeNode currentNode = A;

            while (!B.E.Path.Contains(currentNode.E.Path) && currentNode.E != B.E)
            {
                currentNode = currentNode.Parent;
                Path.Add(currentNode);
            }

            while (currentNode.E != B.E)
            {
                foreach (TreeNode Child in currentNode.Children)
                {
                    if (B.E.Path.Contains(Child.E.Path)) //Идём в правильном направлении
                    {
                        currentNode = Child;
                        Path.Add(currentNode);
                        break;
                    }
                }
            }

            return Path;
        }

        public void FindNode(Entity A, TreeNode root, ref TreeNode Node) //Найдём вершину дерева по сущности
        {
            /*if (root.E == A)
            {
                Node = new TreeNode(root, true);
                return;
            }

            foreach (TreeNode Child in root.Children)
            {
                FindNode(A, Child, ref Node);
            }*/

            if (root.E.Path == A.Path && root.E.Class == A.Class && root.E.ClassMember == A.ClassMember)
            {
                Node = new TreeNode(root, true);
                return;
            }

            foreach (TreeNode Child in root.Children)
            {
                FindNode(A, Child, ref Node);
            }
        }

        public List<TreeNode> GetPath(Entity Start, Entity Finish) //Быдлокод ждёт!
        {
            List<TreeNode> Path = new List<TreeNode>();

            TreeNode A = new TreeNode();
            TreeNode B = new TreeNode();

            //Найдём эти вершины
            FindNode(Start, Root, ref A);
            FindNode(Finish, Root, ref B);

            //Дальше повторяется алгоритм для вершин дерева
            Path.Add(A.E.TreeNode);
            TreeNode currentNode = A;

            if (currentNode.E.EntityType == EntityTypes.ClassMember) //В любом случае
            {
                currentNode = currentNode.Parent;
                Path.Add(currentNode.E.TreeNode);
            }

            if (currentNode.E.EntityType == EntityTypes.Class) //Здесь уже проверим на одинаковость классов
            {
                if (currentNode.E.Class != B.E.Class)
                {
                    currentNode = currentNode.Parent;
                    Path.Add(currentNode.E.TreeNode);
                }
            }

            while (!B.E.Path.Contains(currentNode.E.Path) && currentNode.E != B.E)
            {
                currentNode = currentNode.Parent;
                Path.Add(currentNode.E.TreeNode);
            }

            while (currentNode.E != B.E)
            {
                foreach (TreeNode Child in currentNode.Children)
                {
                    if (B.E.Path.Contains(Child.E.Path)) //Идём в правильном направлении
                    {
                        if (Child.E.EntityType != EntityTypes.Class && Child.E.EntityType != EntityTypes.ClassMember)
                        {
                            currentNode = Child;
                            Path.Add(currentNode.E.TreeNode);
                            break;
                        }

                        else if (Child.E.EntityType == EntityTypes.Class)
                        {
                            if (Child.E.Class == B.E.Class || B.E.Class == null) //У B не должен быть класс!
                            {
                                currentNode = Child;
                                Path.Add(currentNode.E.TreeNode);
                                break;
                            }
                        }

                        else if (Child.E.EntityType == EntityTypes.ClassMember)
                        {
                            if ((Child.E.Class == B.E.Class || B.E.Class == null) && Child.E.ClassMember == B.E.ClassMember)
                            {
                                currentNode = Child;
                                Path.Add(currentNode.E.TreeNode);
                                break;
                            }
                        }
                    }
                }
            }

            return Path;
        }

        public int MaxLevel
        {
            get
            {
                if (( maxLevel == 0 ) && ( _root != null ))
                    CountMaxLevel(_root, 1);
                return maxLevel;
            }
        }

        void CountMaxLevel (TreeNode treeNode, int depth)
        {
            if (treeNode.Children.Count == 0)
            {
                if (depth > maxLevel)
                {
                    maxLevel = depth;
                    return;
                }
                return;
            }

            foreach (TreeNode children in treeNode.Children)
                CountMaxLevel(children, depth + 1);
        }
    }
}
