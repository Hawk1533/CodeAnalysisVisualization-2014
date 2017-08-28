using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Caldara_Visualisation
{
    static class BSpline
    {
        static void DrawCubicCurve (Graphics graphics, Pen pen, GraphTreeNode start, GraphTreeNode end, double beta, double step, double a0, double a1, double a2, double a3, double b0, double b1, double b2, double b3)
        {
            double xNext, yNext;
            bool flag = false;

            double xPrev = beta * a0 + (1 - beta) * start.X;
            double yPrev = beta * b0 + (1 - beta) * start.Y;

            for (double t = step; ; t += step)
            {
                if (flag)
                    break;

                if (t >= 1)
                {
                    flag = true;
                    t = 1;
                }

                xNext = beta * (a3 * t * t * t + a2 * t * t + a1 * t + a0) + (1 - beta) * (start.X + (end.X - start.X) * t);
                yNext = beta * (b3 * t * t * t + b2 * t * t + b1 * t + b0) + (1 - beta) * (start.Y + (end.Y - start.Y) * t);

                graphics.DrawLine(pen, (float)xPrev, (float)yPrev, (float)xNext, (float)yNext);

                xPrev = xNext;
                yPrev = yNext;
            }
        }

        public static void DrawBSpline (Graphics graphics, Pen pen, GraphTreeNode[] graphNodes, double beta, double step)
        {
            double a0, a1, a2, a3, b0, b1, b2, b3;
            double deltaX = (graphNodes[graphNodes.Length - 1].X - graphNodes[0].X) / (graphNodes.Length - 1);
            double deltaY = (graphNodes[graphNodes.Length - 1].Y - graphNodes[0].Y) / (graphNodes.Length - 1);

            a0 = graphNodes[0].X;
            b0 = graphNodes[0].Y;

            a1 = graphNodes[1].X - graphNodes[0].X;
            b1 = graphNodes[1].Y - graphNodes[0].Y;

            a2 = 0;
            b2 = 0;

            a3 = (graphNodes[0].X - 2 * graphNodes[1].X + graphNodes[2].X) / 6;
            b3 = (graphNodes[0].Y - 2 * graphNodes[1].Y + graphNodes[2].Y) / 6;

            GraphTreeNode start = graphNodes[0];
            GraphTreeNode end = new GraphTreeNode(graphNodes[0].E, graphNodes[0].X + deltaX, graphNodes[0].Y + deltaY, graphNodes[0].getColor);

            DrawCubicCurve(graphics, pen, start, end, beta, step, a0, a1, a2, a3, b0, b1, b2, b3);

            for (int i = 1; i < graphNodes.Length - 2; i++)
            {
                a0 = (graphNodes[i - 1].X + 4 * graphNodes[i].X + graphNodes[i + 1].X) / 6;
                b0 = (graphNodes[i - 1].Y + 4 * graphNodes[i].Y + graphNodes[i + 1].Y) / 6;

                a1 = (graphNodes[i + 1].X - graphNodes[i - 1].X) / 2;
                b1 = (graphNodes[i + 1].Y - graphNodes[i - 1].Y) / 2;

                a2 = (graphNodes[i - 1].X - 2 * graphNodes[i].X + graphNodes[i + 1].X) / 2;
                b2 = (graphNodes[i - 1].Y - 2 * graphNodes[i].Y + graphNodes[i + 1].Y) / 2;

                a3 = (-graphNodes[i - 1].X + 3 * graphNodes[i].X - 3 * graphNodes[i + 1].X + graphNodes[i + 2].X) / 6;
                b3 = (-graphNodes[i - 1].Y + 3 * graphNodes[i].Y - 3 * graphNodes[i + 1].Y + graphNodes[i + 2].Y) / 6;

                start = new GraphTreeNode(graphNodes[0].E, graphNodes[0].X + deltaX * i, graphNodes[0].Y + deltaY * i, graphNodes[0].getColor);
                end = new GraphTreeNode(graphNodes[0].E, graphNodes[0].X + deltaX * (i + 1), graphNodes[0].Y + deltaY * (i + 1), graphNodes[0].getColor);

                DrawCubicCurve(graphics, pen, start, end, beta, step, a0, a1, a2, a3, b0, b1, b2, b3);
            }

            a0 = graphNodes[graphNodes.Length - 1].X;
            b0 = graphNodes[graphNodes.Length - 1].Y;

            a1 = graphNodes[graphNodes.Length - 2].X - graphNodes[graphNodes.Length - 1].X;
            b1 = graphNodes[graphNodes.Length - 2].Y - graphNodes[graphNodes.Length - 1].Y;

            a2 = 0;
            b2 = 0;

            a3 = (graphNodes[graphNodes.Length - 1].X - 2 * graphNodes[graphNodes.Length - 2].X + graphNodes[graphNodes.Length - 3].X) / 6;
            b3 = (graphNodes[graphNodes.Length - 1].Y - 2 * graphNodes[graphNodes.Length - 2].Y + graphNodes[graphNodes.Length - 3].Y) / 6;

            start = graphNodes[graphNodes.Length - 1];

            end = new GraphTreeNode(graphNodes[0].E, graphNodes[0].X + deltaX * (graphNodes.Length - 2), graphNodes[0].Y + deltaY * (graphNodes.Length - 2), graphNodes[0].getColor);

            DrawCubicCurve(graphics, pen, start, end, beta, step, a0, a1, a2, a3, b0, b1, b2, b3);
        }
    }
}
