using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Caldara_Visualisation
{
    public class Metric
    {
        private Entity _e; //адрес
        private string _type;
        private double _value; //значение метрики

        public Metric (string Type, Entity e, double Value)
        {
            this._type = Type;
            this._e = e;
            this._value = Value;
        }

        public string getType
        {
            get
            {
                return _type;
            }
        }

        public Entity getEntity
        {
            get
            {
                return _e;
            }
        }

        public double getValue
        {
            get
            {
                return _value;
            }
        }
    }
}
