using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Caldara_Visualisation
{
    public class Section
    {
        private InternalRectangle _rec;
        private double _value;
        private Entity _e;
        private string _name;

        public Section (Metric m)
        {
            this._value = m.getValue;
            this._e = new Entity(m.getEntity);
            this._name = E.Name();
        }

        public Section (Entity e, double value)
        {
            this._value = value;
            this._e = new Entity(e);
            this._name = e.Name();
        }

        public Section (Entity e, double value, InternalRectangle Rect)
        {
            this._value = value;
            this._e = new Entity(e);
            this._name = e.Name();
            this._rec = new InternalRectangle(Rect);
        }

        public Section (Metric m, InternalRectangle Rect)
        {
            this._value = m.getValue;
            this._name = m.getEntity.Path + " " + m.getEntity.Class + " " + m.getEntity.ClassMember;
            this._rec = new InternalRectangle(Rect);
        }

        public Section (Section a)
        {
            this._value = a.Value;
            this._e = a.E;
            this._name = a.Name;

            if (a.Rec != null)
            {
                this._rec = new InternalRectangle(a.Rec);
            }
        }

        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public InternalRectangle Rec
        {
            get { return _rec; }
            set { _rec = value; }
        }

        public Entity E
        {
            get { return _e; }
            set { _e = value; }
        }
    }
}
