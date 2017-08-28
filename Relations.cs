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
    public partial class Relations : Form
    {
        Graphics graphics;
        Pen pen = new Pen(Color.Black);
        SolidBrush brush = new SolidBrush(Color.Black);
        GraphTree<Entity> graph = new GraphTree<Entity>();
        string fileName;
        public ListMetric listM;
        public ListRelation listR;
        public List<Relation> RelationsOfType;
        public FileStructure Struct;
        List<TreeNode> OpenedNodes;
        public string currentText;
        public float MouseX;
        public float MouseY;
        public bool ready;
        public bool text;
        public string currentRelation;
        public Tree structure;
        public bool isGraphReady;
        float Diametr;
        double beta;
        
        public Relations (string fileName)
        {
            InitializeComponent();
            this.fileName = fileName;
            listM = new ListMetric();
            listR = new ListRelation();
            RelationsOfType = new List<Relation>();
            OpenedNodes = new List<TreeNode>();
            ready = false;
            text = false;
            relationsTypeComboBox.SelectedIndex = 0;
        }

        public void GraphVisualisation()
        {
            int structureMaxLevel = structure.MaxLevel;
            GraphTreeNode[] nodesForBSpline = new GraphTreeNode[structureMaxLevel];

            CoordinatesForTree(structure.Root, nodesForBSpline, 1, 1, structureMaxLevel, (this.Size.Width - panel1.Width) / 2 + panel1.Width, this.Size.Height / (structureMaxLevel + 1), this.Size.Width - panel1.Width, null);

            isGraphReady = true;

            this.Invalidate();
        }

        public void RelationsVisualisation(TreeNode treeNode)
        {
            brush = new SolidBrush(treeNode.Col);
            graphics.FillEllipse(brush, (float)treeNode.X - Diametr / 2, (float)treeNode.Y - Diametr / 2, Diametr, Diametr);

            foreach(Relation r in treeNode.Connected)
                if (!r.IsVisualised)
                {
                    List<TreeNode> nodesPathList = structure.GetPath(r.getEntityA, r.getEntityB);
                    TreeNode[] nodesPath = nodesPathList.ToArray();
                    GraphTreeNode[] graphNodesPath = new GraphTreeNode[nodesPath.Length];
                    for (int i = 0; i < nodesPath.Length; i++)
                        graphNodesPath[i] = new GraphTreeNode(nodesPath[i].E, nodesPath[i].X, nodesPath[i].Y, nodesPath[i].Col);
                    if (r.IsRed)
                        pen = new Pen(Color.Red);
                    else
                    {
                        Random rand = new Random();
                        pen = new Pen(Color.FromArgb(32, graphNodesPath[rand.Next(0, graphNodesPath.Length - 1)].getColor));
                    }
                    if (graphNodesPath.Length > 2)
                        BSpline.DrawBSpline(graphics, pen, graphNodesPath, beta, 0.01);
                    else
                        if (graphNodesPath.Length == 2)
                            graphics.DrawLine(pen, (float)graphNodesPath[0].X, (float)graphNodesPath[0].Y, (float)graphNodesPath[1].X, (float)graphNodesPath[1].Y);
                    r.IsVisualised = true;
                }

            foreach (TreeNode child in treeNode.Children)
                RelationsVisualisation(child);
        }
        
        public void CoordinatesForTree(TreeNode treeNode, GraphTreeNode[] nodesForBSpline, int numberOfElementsAtCurrentLevel, int currentLevel, int maxLevel, double xOfParent, double yOfRoot, double lengthOfParentSpace, GraphTreeNode lastNode)
        {
            double x = 0, stepX = 0, lengthOfCurrentSpace = 0;
            int currentNodeNumber = 0;
            if (treeNode.Parent != null)
            {
                int currentLevelCount = treeNode.Parent.Children.Count();
                int parentLevelCount = 0;
                if (treeNode.Parent.Parent != null)
                    parentLevelCount = treeNode.Parent.Parent.Children.Count();
                else
                    parentLevelCount = 1;
                for (int i = 0; i < treeNode.Parent.Children.Count(); i++)
                    if (treeNode.Parent.Children[i].E == treeNode.E)
                    {
                        currentNodeNumber = i + 1;
                        break;
                    }
                double xmin = xOfParent - lengthOfParentSpace / 2;
                double xmax = xOfParent + lengthOfParentSpace / 2;
                stepX = currentNodeNumber * lengthOfParentSpace / (currentLevelCount + 1);
                x = xmin + stepX;
                if (currentLevel == 1)
                    lengthOfCurrentSpace = xmax - xmin;
                else
                    lengthOfCurrentSpace = lengthOfParentSpace / (currentLevelCount + 1);
            }
            else
            {
                stepX = lengthOfParentSpace / 2;
                x = stepX;
            }

            double y = 0;
            if (lastNode != null)
                y = yOfRoot + (currentLevel - 1) * this.Size.Height / (maxLevel + 1);
            else
                y = this.Size.Height / (maxLevel + 1);
            Color color = Color.Black;
            if ((currentNodeNumber % 4) == 0)
            {
                y -= Diametr;
                if ((currentNodeNumber % 8) == 0)
                    color = Color.Green;
                else
                    color = Color.Blue;
            }
            else
            {
                if (((currentNodeNumber - 1) % 2) == 0)
                {
                    if (((currentNodeNumber - 1) % 4) == 0)
                        color = Color.Blue;
                    else
                        color = Color.Green;
                }
                else
                    if ((currentNodeNumber % 2) == 0)
                    {
                        y += Diametr;
                        if ((currentNodeNumber % 3) == 0)
                            color = Color.Blue;
                        else
                            color = Color.Green;
                    }
            }

            treeNode.X = x;
            treeNode.Y = y;
            treeNode.Col = color;
            GraphTreeNode node = new GraphTreeNode(treeNode.E, x, y, color);
            graph.Nodes.Add(node);
            treeNode.E.GraphNode = node;
            nodesForBSpline[currentLevel - 1] = node;
            if (lastNode != null)
                graph.Edges.Add(new GraphTreeEdge(lastNode, node));

            foreach (TreeNode children in treeNode.Children)
                CoordinatesForTree(children, nodesForBSpline, treeNode.Children.Count, currentLevel + 1, maxLevel, x, yOfRoot, lengthOfCurrentSpace, node);
        }


        void RelationsIsNotVisualised(TreeNode treeNode)
        {
            foreach (Relation r in treeNode.Connected)
                r.IsVisualised = false;

            foreach (TreeNode child in treeNode.Children)
                RelationsIsNotVisualised(child);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            graphics = e.Graphics;
            graphics.Clear(this.BackColor);
            if (isGraphReady)
                RelationsVisualisation(structure.Root);
        }

        private void button1_Click (object sender, EventArgs e)
        {
            incorrectDataLabel.Visible = false;
            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                try { beta = Convert.ToDouble(textBoxBeta.Text); }

                catch (FormatException error)
                {
                    beta = Convert.ToDouble(ListRelation.toRussianLocale(textBoxBeta.Text));
                }

                Diametr = Convert.ToSingle(textBoxDiametr.Text);
                if (Diametr < 8) Diametr = 8f;
                if (Diametr > 32) Diametr = 32f;
                GraphVisualisation();
            }

            catch
            {
                incorrectDataLabel.Visible = true;
            }

            Cursor.Current = Cursors.Default;
            buttonClear.Enabled = true;
        }

        private void Relations_MouseClick(object sender, MouseEventArgs e)
        {
            if (isGraphReady)
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (GraphTreeNode graphNode in graph.Nodes)
                    if (((graphNode.X - e.X) * (graphNode.X - e.X) + (graphNode.Y - e.Y) * (graphNode.Y - e.Y)) < Diametr * Diametr)
                    {
                        graphNode.IsRed = !(graphNode.IsRed);
                        if (graphNode.IsRed) brush = new SolidBrush(Color.Red);
                        else brush = new SolidBrush(graphNode.getColor);
                        graphics = this.CreateGraphics();
                        graphics.FillEllipse(brush, (float)graphNode.X - Diametr / 2, (float)graphNode.Y - Diametr / 2, Diametr, Diametr);
                        string text = "Entity:\n" + graphNode.E.Path + " " + graphNode.E.Class + " " + graphNode.E.ClassMember + "\n This entity have relations of type \"" + currentRelation + "\" with next entities:\n";
                        TreeNode treeNode = new TreeNode();
                        structure.FindNode(graphNode.E, structure.Root, ref treeNode);
                        foreach (Relation r in treeNode.Connected)
                        {

                            if (!(r.getEntityA.Path == treeNode.E.Path && r.getEntityA.Class == r.getEntityA.Class && r.getEntityA.ClassMember == r.getEntityA.ClassMember))
                                text += "\n" + r.getEntityA.Path + " " + r.getEntityA.Class + " " + r.getEntityA.ClassMember + "\n";
                            else
                                text += "\n" + r.getEntityB.Path + " " + r.getEntityB.Class + " " + r.getEntityB.ClassMember + "\n";
                            r.IsRed = !(r.IsRed);
                            List<TreeNode> nodesPathList = structure.GetPath(r.getEntityA, r.getEntityB);
                            TreeNode[] nodesPath = nodesPathList.ToArray();
                            GraphTreeNode[] graphNodesPath = new GraphTreeNode[nodesPath.Length];
                            for (int i = 0; i < nodesPath.Length; i++)
                                graphNodesPath[i] = new GraphTreeNode(nodesPath[i].E, nodesPath[i].X, nodesPath[i].Y, nodesPath[i].Col);
                            if (r.IsRed)
                                pen = new Pen(Color.Red);
                            else
                            {
                                Random rand = new Random();
                                pen = new Pen(Color.FromArgb(32, graphNodesPath[graphNodesPath.Length - 1].getColor));
                            }
                            if (graphNodesPath.Length > 2)
                                BSpline.DrawBSpline(graphics, pen, graphNodesPath, beta, 0.01);
                            else
                                if (graphNodesPath.Length == 2)
                                    graphics.DrawLine(pen, (float)graphNodesPath[0].X, (float)graphNodesPath[0].Y, (float)graphNodesPath[1].X, (float)graphNodesPath[1].Y);
                            if (!r.IsRed) brush = new SolidBrush(nodesPathList[0].Col);
                            graphics.FillEllipse(brush, (float)nodesPathList[0].X - Diametr / 2, (float)nodesPathList[0].Y - Diametr / 2, Diametr, Diametr);
                            if (!r.IsRed) brush = new SolidBrush(nodesPathList[nodesPathList.Count() - 1].Col);
                            graphics.FillEllipse(brush, (float)nodesPathList[nodesPathList.Count() - 1].X - Diametr / 2, (float)nodesPathList[nodesPathList.Count() - 1].Y - Diametr / 2, Diametr, Diametr);
                        }
                        textBoxInfo.Text = text;
                    }
                Cursor.Current = Cursors.Default;
            }
        }

        private void Relations_MouseMove(object sender, MouseEventArgs e)
        {
            if (isGraphReady)
            {
                    bool flag = false;
                    foreach (GraphTreeNode graphNode in graph.Nodes)
                        if (((graphNode.X - e.X) * (graphNode.X - e.X) + (graphNode.Y - e.Y) * (graphNode.Y - e.Y)) < Diametr * Diametr)
                        {
                            labelEntity.Text = "Entity: " + graphNode.E.Path + " " + graphNode.E.Class + " " + graphNode.E.ClassMember;
                            labelEntity.Visible = true;
                            flag = true;
                            break;
                        }
                    if (!flag)
                    {
                        labelEntity.Visible = false;
                        labelEntity.Text = "Entity: ";
                    }
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            labelEntity.Visible = false;
            labelEntity.Text = "Entity: ";
            textBoxInfo.Text = "";
            graphics = this.CreateGraphics();
            graphics.Clear(this.BackColor);
            RelationsIsNotVisualised(structure.Root);
            isGraphReady = false;
            buttonClear.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            errorLabel.Visible = false;

            try
            {
            Cursor.Current = Cursors.WaitCursor;
            listR = new ListRelation();
            listR.ReadFromFile(fileName);
            currentRelation = relationsTypeComboBox.SelectedItem.ToString();
            RelationsOfType = listR.GetRelationsOfType(currentRelation);

            Struct = new FileStructure(RelationsOfType);
            Struct.BuildRelationsFileStructure();
            Struct.Struct.FillTree(Struct.Struct.Root, RelationsOfType);
            structure = Struct.Struct;

            button1.Enabled = true;

            Cursor.Current = Cursors.Default;
            }

            catch
            {
                errorLabel.Visible = true;
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
