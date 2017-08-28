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
    public class Relation
    {
        private string _type;
        private Entity _a;
        private Entity _b;
        private bool isVisualised;
        private bool isRed;

        public Relation (string Type, Entity a, Entity b)
        {
            this._type = Type;
            this._a = a;
            //_a.ListOfRelations.Add(this);
            this._b = b;
            //_b.ListOfRelations.Add(this);
        }

        public string getType
        {
            get
            {
                return _type;
            }
        }

        public Entity getEntityA
        {
            get
            {
                return _a;
            }
        }

        public Entity getEntityB
        {
            get
            {
                return _b;
            }
        }

        public bool IsVisualised
        {
            get
            {
                return isVisualised;
            }

            set
            {
                isVisualised = value;
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

        public void AddRelations()
        {
            //_a.ListOfRelations.Add(this);
            //_b.ListOfRelations.Add(this);
        }
    }
}