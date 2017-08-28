using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Caldara_Visualisation
{
    public class TreeNode
    {
        private Entity _e; //Здесь будет лежать Entity
        private TreeNode _parent; //Родитель
        private List<TreeNode> _children; //Список дочерних вершин 
        private bool _isOpened; //Стоит ли визуализатору показывать данную сущность
        private string _name;

        //Для метрик
        private InternalRectangle _rec;
        private double _s;
        private double _colorValue;
        private Color _col;

        //Для отношений
        private List<Relation> _connected; //Список отношений, в который участвует данная сущность 
        private double x, y;

        public TreeNode ()
        {
            this._s = 0;
            this._isOpened = false;
        }

        public TreeNode (Entity E)
        {
            this._e = E;
            _e.TreeNode = this;
            this._s = 0;
            this._isOpened = false;
            this._connected = new List<Relation>();
        }

        public TreeNode (TreeNode parent)
        {
            this._parent = parent;
            this._s = 0;
            this._isOpened = false;
        }

        public TreeNode (TreeNode parent, Entity E)
        {
            this._parent = parent;
            this._e = E;
            _e.TreeNode = this;
            this._s = 0;
            this._isOpened = false;
        }

        public TreeNode (TreeNode parent, Entity E, List<TreeNode> children)
        {
            this._parent = parent;
            this._e = E;
            _e.TreeNode = this;
            this._children = children;
            this._s = 0;
            this._isOpened = false;
            this._connected = new List<Relation>();
        }

        public TreeNode (TreeNode Node, bool yea)
        {
            this._parent = Node.Parent;
            this._e = Node.E;
            _e.TreeNode = this;
            this._children = Node.Children;
            this._s = Node.S;
            this._colorValue = Node.ColorValue;
            this._isOpened = Node.IsOpened;
            this._rec = new InternalRectangle(Node.Rec);
            this._name = Node.Name;
            this._connected = Node.Connected;
            this.x = Node.X;
            this.y = Node.Y;
            this._col = Node.Col;
        }

        public TreeNode (Entity E, List<TreeNode> children)
        {
            this._e = E;
            _e.TreeNode = this;
            this._children = children;
            this._s = 0;
            this._isOpened = false;
        }

        public InternalRectangle Rec
        {
            get { return _rec; }
            set { _rec = value; }
        }

        public bool IsOpened
        {
            get
            {
                return _isOpened;
            }

            set
            {
                _isOpened = value;
            }
        }

        public double S
        {
            get
            {
                return _s;
            }

            set
            {
                _s = value;
            }
        }

        public Color Col
        {
            get
            {
                return _col;
            }

            set
            {
                _col = value;
            }
        }

        public double ColorValue
        {
            get
            {
                return _colorValue;
            }

            set
            {
                _colorValue = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public TreeNode Parent
        {
            get
            {
                return _parent;
            }

            set
            {
                _parent = value;

            }
        }

        public List<TreeNode> Children
        {
            get
            {
                return _children;
            }

            set
            {
                _children = value;

            }
        }

        public Entity E
        {
            get
            {
                return _e;
            }

            set
            {
                _e = value;
                _e.TreeNode = this;
            }
        }

        public List<Relation> Connected
        {
            get
            {
                return _connected;
            }

            set
            {
                _connected = value;
            }
        }

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public void CountLeaves (TreeNode root, ref int leaves)
        {
            if (root.Children.Count == 0 && root.ColorValue != 0 && root.S != 0) //Если у вершины присутствует значение метрики цвета, но нет значения метрики прямоугольника то она нам не подходит
            {
                leaves++;
            }

            foreach (TreeNode Child in root.Children)
            {
                CountLeaves(Child, ref leaves);
            }
        }
    }
}
