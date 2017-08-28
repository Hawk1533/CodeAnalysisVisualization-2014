using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Caldara_Visualisation
{
    //Стоит хранить тип отношения числом и сравнить числа, а не строки
    //Что с сортировкой по алфавиту?

    public class ListMetric
    {
        private string[] MetricTypes = new string[59] { "LOC_METHOD", "CYCLOMATIC", "NOPARMS", "MAXLEVEL", "NCNBLOC_METHOD", "NOFANIN", "NOFANOUT", "NOLOOPS", "NOIF", "NOBRANCH", "NOGLOVARUSED", "NOSTATICVARUSED", "NORET", "LEVINHER", "LOC_CLASS", "NOMETH", "NOPRIVDATADEC", "NOPROTDATADECL", "NOPUBDATADECL", "NOSTDATADECL", "NOPRIVMETH", "NOPROTMETH", "NOPUBMETH", "NOPTRDECL", "NOVIRTMETH", "NCNBLOC_CLASS", "AVGCOMPMETH", "MAXCOMPMETH", "TOTCOMPMETH", "NOMEMBERDATA", "NONESTEDCLASS", "NOINLINEMETH", "NOCONSTMETH", "NOCLASSDECL", "CLOC_FILE", "RNOCONTROLSTAT", "RNOACCTOGLOB", "DIRECTIVES", "NCNBLOC_FILE", "RACOMMFILE", "NOFUNCDEFFILE", "LOC_FILE", "AVGCOMPFILE", "MAXCOMPFILE", "TOTCOMPFILE", "NOCONSTDECL", "NODATADECL", "NOCFILE", "NOCPPFILE", "RACOMMDIR", "NOFUNCDEFDIR", "NOHEADERFILE", "LOC_DIR", "NCNBLOC_DIR", "CLOC_DIR", "AVGCOMPDIR", "MAXCOMPDIR", "TOTCOMPDIR", "TOTFILE" };
        private List<Metric> _list;
        private static bool empty = false;

        public ListMetric ()
        {
            this._list = new List<Metric>();
        }

        public List<Metric> List
        {
            get { return _list; }
            set { _list = value; }
        }

        public static string ReadPart (string file, ref int i)
        {
            if (i != 0 || empty) i++;
            empty = true;

            string s = "";

            for ( ; ( file[i] != ';' ) && ( i < file.Length ) ; i++)
            {
                s += file[i];
                empty = false;
            }

            return s;
        } //Чтение одного поля до ";"

        public static string ReadLastPart (string file, ref int i) //Читаем последнюючасть до появления большой буквы - начала следующей метрики
        {
            if (i != 0 || empty) i++;
            empty = true;

            string s = "";

            if (file[i] == ';') i++;

            for ( ; ( i < file.Length ) ; i++) //(Convert.ToChar(file[i]) >= 90 || Convert.ToChar(file[i])<=65)
            {
                s += file[i];
                empty = false;
            }

            return s;
        } //Чтение одного поля до ";"

        public static string toRussianLocale (string number)
        {
            string str = number.Replace(".", ",");
            return str;
        }

        public static void toEnglishLocale (string number)
        {
            number.Replace(',', '.');
        }

        public void ReadFromFile (string FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);
            string file = null;
            int i = 0;

            while (( file = sr.ReadLine() ) != null)
            {
                i = 0;
                string Type = ReadPart(file, ref i);
                string entityName = ReadPart(file, ref i);
                string entityNameWithParams = ReadPart(file, ref i);
                string stringValue = ReadPart(file, ref i);

                double Value;

                try { Value = Convert.ToDouble(stringValue); }

                catch (FormatException error)
                {
                    Value = Convert.ToDouble(toRussianLocale(stringValue));
                }

                string entityFile = ReadLastPart(file, ref i);
                string entityAddress = entityFile + " " + entityName;

                Entity e = null;
                CreateEntity(ref e, entityName, entityNameWithParams, entityFile);

                List.Add(new Metric(Type, e, Value));
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

        public Entity CreateEntity (string a)
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

        } //Передаём строчку

        public EntityTypes GetEntityType (string type)
        {
            if (EntityTypes.Class.ToString() == type) return EntityTypes.Class;
            else if (EntityTypes.ClassMember.ToString() == type) return EntityTypes.ClassMember;
            else if (EntityTypes.File.ToString() == type) return EntityTypes.File;
            else if (EntityTypes.System.ToString() == type) return EntityTypes.System;
            else return EntityTypes.Directory;
        }

        public List<Metric> GetMetricsFor (Entity e)
        {
            List<Metric> Results = new List<Metric>();

            foreach (Metric m in List)
            {
                if (e == m.getEntity) Results.Add(m);
            }
            return Results;
        }

        public List<Metric> GetMetricsOfType (string Type)
        {
            List<Metric> Results = new List<Metric>();

            foreach (Metric m in List)
            {
                if (( Type == m.getType ) && ( !m.getEntity.Path.Contains("/usr") )) Results.Add(m);
            }

            return Results;
        }
    }
}
