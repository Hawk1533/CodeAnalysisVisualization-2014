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
    public class InternalRectangle
    {
        private double _x0;
        private double _y0;
        private double _x1;
        private double _y1;

        public InternalRectangle (double x0, double y0, double x1, double y1)
        {
            this._x0 = x0;
            this._y0 = y0;
            this._x1 = x1;
            this._y1 = y1;
        }

        public InternalRectangle (InternalRectangle Rect)
        {
            if (Rect != null)
            {
                this._x0 = Rect.X0;
                this._y0 = Rect.Y0;
                this._x1 = Rect.X1;
                this._y1 = Rect.Y1;
            }
        }

        public InternalRectangle (InternalRectangle Rec, bool eah) //Здесь создаём для формы, поэтому x1>x0, y1>y0
        {
            if (Rec != null)
            {
                this._x0 = Math.Min(Rec.X0, Rec.X1);
                this._y0 = Math.Min(Rec.Y0, Rec.Y1);
                this._x1 = Math.Max(Rec.X0, Rec.X1);
                this._y1 = Math.Max(Rec.Y0, Rec.Y1);
            }
        }

        public double GetWidth ()
        {
            return Math.Min(Math.Abs(X1 - X0), Math.Abs(Y1 - Y0));
        }

        public double GetWidth (ref bool vertical)
        {
            double X = Math.Abs(X1 - X0);
            double Y = Math.Abs(Y1 - Y0);

            if (X > Y)
            {
                vertical = true;
                return Y;
            }

            else
            {
                vertical = false;
                return X;
            }
        }

        public double GetHeight ()
        {
            return Math.Max(Math.Abs(X1 - X0), Math.Abs(Y1 - Y0));
        }

        public double GetS ()
        {
            return Math.Abs(( X1 - X0 ) * ( Y1 - Y0 ));
        }

        public double GetS (double k)
        {
            return Math.Abs(( X1 - X0 ) * ( Y1 - Y0 )) * k;
        }

        public double X0
        {
            get { return _x0; }
            set { _x0 = value; }
        }

        public double Y0
        {
            get { return _y0; }
            set { _y0 = value; }
        }

        public double X1
        {
            get { return _x1; }
            set { _x1 = value; }
        }

        public double Y1
        {
            get { return _y1; }
            set { _y1 = value; }
        }
    }
}
