using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Caldara_Visualisation
{
    public enum EntityTypes { Directory, File, Class, ClassMember, System }; //Виды сущностей

    public class Entity
    {
        private EntityTypes _entityType;
        private string _path; //Адрес
        private string _class; //Если это класс
        private string _classMember; //Если это часть класса, то конкретизируем
        private GraphTreeNode graphNode;
        private TreeNode treeNode;

        public Entity (string path) //Только адрес. КОРРЕКТИРОВАТЬ
        {
            this._path = path;
        }

        public Entity (EntityTypes A, string path) //Директория или файл
        {
            this._entityType = A;
            this._path = path;
        }

        public Entity (EntityTypes A, string path, string className) //Класс
        {
            this._entityType = A;
            this._path = path;
            this._class = className;
        }

        public Entity (EntityTypes A, string path, string className, string classMember) //Часть класса
        {
            this._entityType = A;
            this._path = path;
            this._class = className;
            this._classMember = classMember;
        }

        public Entity (EntityTypes A, string path, string classMember, bool yea) //Часть класса
        {
            this._entityType = A;
            this._path = path;
            this._classMember = classMember;
        }

        public Entity (Entity e) //Часть класса
        {
            this._entityType = e.EntityType;
            this._path = e.Path;
            this._class = e.Class;
            this._classMember = e.ClassMember;
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string Class
        {
            get { return _class; }
            set { _class = value; }
        }

        public string ClassMember
        {
            get { return _classMember; }
            set { _classMember = value; }
        }

        public EntityTypes EntityType
        {
            get { return _entityType; }
            set { _entityType = value; }
        }

        public GraphTreeNode GraphNode
        {
            get
            {
                return graphNode;
            }

            set
            {
                graphNode = value;
            }
        }

        public TreeNode TreeNode
        {
            get
            {
                return treeNode;
            }

            set
            {
                treeNode = value;
            }
        }

        public string Output ()
        {
            return Class + " " + ClassMember + " " + Path + " " + EntityType;
        }

        public static bool operator == (Entity a, Entity b)
        {
            if (b.Path == null) return false;

            if (a.Class == b.Class && a.ClassMember == b.ClassMember && a.Path == b.Path)
                return true;

            return false;
        }

        public static bool operator != (Entity a, Entity b)
        {
            if (b.Path == null) return true;

            if (a.Class == b.Class && a.ClassMember == b.ClassMember && a.Path == b.Path)
                return false;

            return true;
        }

        public string Name ()
        {
            if (this.EntityType == EntityTypes.Class) return this.Class;
            else if (this.EntityType == EntityTypes.ClassMember) return this.ClassMember;
            else return this.Path;
        }

        public string Name (bool eah)
        {
            string a = "", b = "";
            int i;

            if (this.EntityType == EntityTypes.Class) return this.Class;
            else if (this.EntityType == EntityTypes.ClassMember)
            {
                a = ClassMember.ToString();
                i = a.IndexOf('(');

                if (i == -1) return this.ClassMember;

                while (a[i] != ' ' && a[i] != ':' && a[i] != '*')
                {
                    i--;
                }

                return a.Substring(i, a.IndexOf('(') - i);
            }

            else
            {
                if (Path == "") return ""; //Если пустой путь

                i = Path.Length - 1;

                while (i!=-1 && Path[i] != '/')
                {
                    a += Path[i];
                    i--;
                }

                for (i = a.Length - 1; i >= 0; i--)
                {
                    b += a[i];
                }


                return b;
            }
        }
    }
}

