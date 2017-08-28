using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//Стоит хранить тип отношения числом и сравнить числа, а не строки
//Что с сортировкой по алфавиту?
namespace Caldara_Visualisation
{
    public class ListRelation
    {
        private string[] RelationTypes = new string[10] { "CALLS", "READS", "WRITES", "INCLUDES", "CONTAINS", "INHERITS", "OVERRIDES", "IMPORTS", "IMPLICITLY_CALLS", "ASSOCIATES" };
        private List<Relation> _relations = new List<Relation>();
        private static bool empty = false;

        public List<Relation> Relations
        {
            get { return _relations; }
        }

        public static string ReadPart(string file, ref int i)
        {
            if (i != 0 || empty) i++;
            empty = true;

            string s = "";

            for (; (i < file.Length) && (file[i] != ';'); i++)
            {
                s += file[i];
                empty = false;
            }

            return s;
        } //Чтение одного поля до ";"

        public string GetType(string file, ref int i) //Определение типа отношения
        {
            empty = false;
            i++;
            string a = file.Substring(i, 5); //это кусок названия тип файла. именно 5 символов т.к. это длинна самого короткого типа отношения
            int j;

            for (j = 0; j <= 9; j++)
            {
                if (RelationTypes[j].Contains(a))
                {
                    i += RelationTypes[j].Length; //?
                    return RelationTypes[j];
                }
            }

            return RelationTypes[j]; //в любом случае выйдёт раньше
        }

        public void ReadFromFile(string FileName)
        {
            //entityAname = entityAnnameWithParams - класс
            //entityAnnameWithParams = #NP - файл(.h) или класс (просто имя)
            //entityAnnameWithParams contains entityAname - часть класса

            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);
            string file = null;
            int i = 0;
            file = sr.ReadLine();
            int len = file.Length;

            while ((file = sr.ReadLine()) != null)
            {
                i = 0;
                string entityAname = ReadPart(file, ref i);

                string entityAnnameWithParams = ReadPart(file, ref i);

                string entityAfile = ReadPart(file, ref i);
                string line = ReadPart(file, ref i);

                string entityBname = ReadPart(file, ref i);
                string entityBnameWithParams = ReadPart(file, ref i);
                string entityBfile = ReadPart(file, ref i);

                string Type = GetType(file, ref i);
                string entityaddressA = entityAfile + " " + entityAname;
                string entityaddressB = entityBfile + " " + entityBname;

                Entity a = null;
                Entity b = null;

                CreateEntity(ref a, entityAname, entityAnnameWithParams, entityAfile);
                CreateEntity(ref b, entityBname, entityBnameWithParams, entityBfile);


                if (a != b && !a.Path.Contains("/usr") && !b.Path.Contains("/usr") && entityAnnameWithParams != "#NP" && entityBnameWithParams != "#NP" && a.Path.Contains("/mnt") && b.Path.Contains("/mnt")) Relations.Add(new Relation(Type, a, b)); //Если метод вызывает сам себя (почему), то я его не беру!
            }

            fs.Close();
        }

        public void CreateEntity(ref Entity a, string entityName, string entitynnameWithParams, string entityFile) //Определение типа сущности (класс, метод, файл) по полному имени
        {
            if (entityName == entitynnameWithParams)
                a = new Entity(EntityTypes.Class, entityFile, entityName);

            else if (entitynnameWithParams.Contains(entityName))
                a = new Entity(EntityTypes.ClassMember, entityFile, entityName, entitynnameWithParams);

            else if (entityName.Contains(".h") || entityName.Contains(".cpp"))  // && entitynnameWithParams == "#NP" 
                a = new Entity(EntityTypes.File, entityFile);

            else if ((entityFile.Contains(".h") || entityFile.Contains(".cpp")) && entitynnameWithParams == "#NP") //Наверное, так логичней. Лежит в файле и нет параметров.
                a = new Entity(EntityTypes.ClassMember, entityFile, entityName, true);

            else
                a = new Entity(EntityTypes.System, entityFile); //Ошибка! Что такое System?      
        }

        public Entity CreateEntity(string a) //Создаём сущность по строке
        {
            int i = 0;
            Entity ent;
            string entityName = ReadPart(a, ref i);
            string entitynnameWithParams = ReadPart(a, ref i);
            string entityFile = ReadPart(a, ref i);

            if (entityName == entitynnameWithParams)
                ent = new Entity(EntityTypes.Class, entityFile, entityName);

            else if (entitynnameWithParams.Contains(entityName))
                ent = new Entity(EntityTypes.ClassMember, entityFile, entityName, entitynnameWithParams);

            else if (entityName.Contains(".h"))  // && entitynnameWithParams == "#NP" 
                ent = new Entity(EntityTypes.File, entityFile);

            else
                ent = new Entity(EntityTypes.System, entityFile); //Ошибка! Что такое System?   

            return ent;

        }

        public EntityTypes GetEntityType(string type)
        {
            if (EntityTypes.Class.ToString() == type) return EntityTypes.Class;
            else if (EntityTypes.ClassMember.ToString() == type) return EntityTypes.ClassMember;
            else if (EntityTypes.File.ToString() == type) return EntityTypes.File;
            else if (EntityTypes.System.ToString() == type) return EntityTypes.System;
            else return EntityTypes.Directory;
        }

        public List<Relation> GetRelationsFor(Entity a)
        {
            List<Relation> Results = new List<Relation>();

            foreach (Relation r in Relations)
            {
                if (a == r.getEntityA || a == r.getEntityB)
                    Results.Add(r);
            }

            return Results;
        }

        public List<Relation> GetRelationsBetween(Entity a, Entity b)
        {
            List<Relation> Results = new List<Relation>();

            foreach (Relation r in Relations)
            {
                if (((a == r.getEntityA) || (a == r.getEntityB)) && ((b == r.getEntityA) || (b == r.getEntityB))) Results.Add(r);
            }

            return Results;
        }

        public List<Relation> GetRelationsOfType(string Type)
        {
            List<Relation> Results = new List<Relation>();

            foreach (Relation r in Relations)
            {
                if (r.getType == Type) Results.Add(r);
            }

            return Results;
        }

        public List<Relation> GetRelationsOfType(string Type, bool usr)
        {
            List<Relation> Results = new List<Relation>();

            foreach (Relation r in Relations)
            {
                if ((r.getType == Type) && (!r.getEntityA.Path.Contains("/usr") || !r.getEntityB.Path.Contains("/usr"))) Results.Add(r);
            }

            return Results;
        }

        public List<string> GetAllPaths() //Получаем все адреса
        {
            List<string> Results = new List<string>();
            bool A = true;
            bool B = true;

            foreach (Relation r in Relations)
            {
                A = B = true;
                string first = r.getEntityA.Path;
                string second = r.getEntityB.Path;

                foreach (string s in Results) //Отсеиваем адреса, которые уже есть в списке и /usr
                {
                    if (s == first) A = false;
                    if (s == second) B = false;
                }

                if (A && !first.Contains("usr") && first != "" && first.Contains("/"))
                {
                    Results.Add(first);
                }

                if (B && !second.Contains("usr") && second != "" && second.Contains("/"))
                {
                    Results.Add(second);
                }
            }

            return Results;
        }

        public static string toRussianLocale(string number)
        {
            string str = number.Replace(".", ",");
            return str;
        }

        public static void toEnglishLocale(string number)
        {
            number.Replace(',', '.');
        }
    }
}