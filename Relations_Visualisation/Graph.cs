using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Caldara_Visualisation
{
    public class GraphTreeNode
    {
        Entity e;
        double x, y;
        List<GraphTreeEdge> edges = new List<GraphTreeEdge>();
        bool isRed;
        Color color;
        
        public Entity E
        {
            get
            {
                return e;
            }

            set
            {
                e = value;
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

        public List<GraphTreeEdge> Edges
        {
            get
            {
                return edges;
            }

            set
            {
                edges = value;
            }
        }

        public bool IsRed
        {
            get
            {
                return isRed;
            }

            set
            {
                isRed = value;
            }
        }

        public Color getColor
        {
            get
            {
                return color;
            }
        }

        public GraphTreeNode (Entity e)
        {
            this.e = e;
        }

        public GraphTreeNode(Entity e, double x, double y, Color color)
        {
            this.e = e;
            this.x = x;
            this.y = y;
            this.color = color;
        }
    }

    public class GraphTreeEdge
    {
        GraphTreeNode parent, child;

        public GraphTreeNode Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
                parent.Edges.Add(this);
            }
        }

        public GraphTreeNode Child
        {
            get
            {
                return child;
            }

            set
            {
                child = value;
                child.Edges.Add(this);
            }
        }

        public GraphTreeEdge (GraphTreeNode parent, GraphTreeNode child)
        {
            this.parent = parent;
            parent.Edges.Add(this);
            this.child = child;
            child.Edges.Add(this);
        }
    }

    public class GraphTree<T>
    {
        List<GraphTreeNode> nodes = new List<GraphTreeNode>();
        List<GraphTreeEdge> edges = new List<GraphTreeEdge>();

        public List<GraphTreeNode> Nodes
        {
            get
            {
                return nodes;
            }

            set
            {
                nodes = value;
            }
        }

        public List<GraphTreeEdge> Edges
        {
            get
            {
                return edges;
            }

            set
            {
                edges = value;
            }
        }

        public GraphTree() { }

        public GraphTree(List<GraphTreeNode> nodes, List<GraphTreeEdge> edges)
        {
            this.nodes = nodes;
            this.edges = edges;
        }

        public List<GraphTreeNode> GetPath(GraphTreeNode A, GraphTreeNode B)
        {
            List<GraphTreeNode> Path = new List<GraphTreeNode>();
            Path.Add(A);
            GraphTreeNode currentNode = A;

            while (!B.E.Path.Contains(currentNode.E.Path) && currentNode.E != B.E)
            {
                //currentNode.
            }

            return Path;
        }
    }
}
