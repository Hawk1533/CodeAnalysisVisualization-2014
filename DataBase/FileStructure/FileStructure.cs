using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Caldara_Visualisation
{
    public class FileStructure
    {
        private Tree _struct;
        private List<Relation> _relations;
        private List<Metric> _metrics;

        public FileStructure (FileStructure Struct)
        {
            this._struct = new Tree(Struct.Struct); //ссылка
        }

        public FileStructure (List<Relation> Relations)
        {
            this._relations = Relations;
        }

        public FileStructure (List<Metric> Metrics)
        {
            this._metrics = Metrics;
        }

        public Tree Struct
        {
            get { return _struct; }
            set { _struct = value; }
        }

        public List<Relation> Relations
        {
            get { return _relations; }
        }

        public List<Metric> Metrics
        {
            get { return _metrics; }
        }

        public void BuildMetricsFileStructure () //Для метрик
        {
            Struct = new Tree(new TreeNode(new Entity(EntityTypes.Directory, "/mnt"), new List<TreeNode>())); //просто передадим самое первое. это должно быть mnt и вроде верно
            Struct.Root.IsOpened = true;
            TreeNode currentParent = Struct.Root;

            foreach (Metric M in Metrics)
            {
                AddToTree(M.getEntity);
            }
        }

        public void BuildRelationsFileStructure () //Для отношений
        {
            Struct = new Tree(new TreeNode(new Entity(EntityTypes.Directory, "/mnt"), new List<TreeNode>())); //просто передадим самое первое. это должно быть mnt и вроде верно
            Struct.Root.IsOpened = true;
            Struct.Root.Connected = new List<Relation>();
            TreeNode currentParent = Struct.Root;

            foreach (Relation R in Relations)
            {
                AddToTree(R.getEntityA);
                AddToTree(R.getEntityB);
            }
        }


        public void AddToTree (Entity Entity)
        {
            string path = Entity.Path;
            int i = 4; //Начинаем с 5ого, чтобы не захватывать корень 
            TreeNode currentParent = Struct.Root;
            string currentPath = "/mnt";

            while (i < path.Length)
            {
                currentPath += ReadPart(path, ref i);

                //Если адрес содержит расширения файла, то задаём тип сузности - файл
                if (currentPath.Contains(".h") || currentPath.Contains(".cpp")) AddNode(ref currentParent, new Entity(EntityTypes.File, currentPath));
                else AddNode(ref currentParent, new Entity(EntityTypes.Directory, currentPath));
            }

            if (Entity.EntityType == EntityTypes.Class) //Если это класс, то добавим его вершину
            {
                AddNode(ref currentParent, new Entity(EntityTypes.Class, currentPath, Entity.Class));
            }

            else if (Entity.EntityType == EntityTypes.ClassMember)
            {
                AddNode(ref currentParent, new Entity(EntityTypes.Class, currentPath, Entity.Class));
                AddNode(ref currentParent, new Entity(EntityTypes.ClassMember, currentPath, Entity.Class, Entity.ClassMember));
            }
        }

        public void AddNode (ref TreeNode currentParent, Entity E)
        {
            int check = AlreadyExists(currentParent, E);

            if (check == -1) //такой сущности нет - создаём новую
            {
                TreeNode a = new TreeNode(currentParent, E, new List<TreeNode>());
                currentParent.Children.Add(a);
                currentParent = a;
            }

            else
            {
                currentParent = currentParent.Children[check];
            }
        }

        public int AlreadyExists (TreeNode Parent, Entity Entity) //Проверяем, есть ли вершина с таким именем на этом уровне. вернет номер сущности в списке, если он есть и -1, если нет
        {
            for (int i = 0 ; i < Parent.Children.Count ; i++)
            {
                if (Parent.Children[i].E == Entity) return i;
            }

            return -1;
        }

        public static string ReadPart (string path, ref int i) //Чтение одной вершины до "/" или до конца строки
        {
            string s = "/";
            i++;

            for ( ; ( i < path.Length ) ; i++)
            {
                if (path[i] == '/') break;

                s += path[i];
            }

            return s;
        }
    }
}