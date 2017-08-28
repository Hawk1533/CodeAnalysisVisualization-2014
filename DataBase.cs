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
    public partial class DataBase : Form
    {
        ListRelation listR = new ListRelation();
        ListMetric listM = new ListMetric();

        public DataBase ()
        {
            InitializeComponent();
        }

        private void button1_Click (object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button2_Click (object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void button3_Click (object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            listR.ReadFromFile(openFileDialog1.FileName);
            listM.ReadFromFile(openFileDialog2.FileName);
            Cursor.Current = Cursors.Default;
        }

        private void button4_Click (object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dataGridView1.Rows.Clear();
            string T = textBox2.Text;
            List<Relation> Result = new List<Relation>();
            Result = listR.GetRelationsOfType(textBox2.Text);

            for (int i = 0 ; i < Result.Count ; i++)
            {
                string[] row = { Result[i].getEntityA.Output(), Result[i].getEntityB.Output(), Result[i].getType };
                dataGridView1.Rows.Add(row);
            }


            Cursor.Current = Cursors.Default;
        }

        private void button5_Click (object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            List<Relation> ResultR = listR.GetRelationsFor(listR.CreateEntity(textBox3.Text));
            List<Metric> ResultM = listM.GetMetricsFor(listM.CreateEntity(textBox3.Text));

            for (int i = 0 ; i < ResultR.Count ; i++)
            {
                string[] row = { ResultR[i].getEntityA.Output(), ResultR[i].getEntityB.Output(), ResultR[i].getType };
                dataGridView1.Rows.Add(row);
            }

            for (int i = 0 ; i < ResultM.Count ; i++)
            {
                string[] row = { ResultM[i].getEntity.Output(), ResultM[i].getType, ResultM[i].getValue.ToString() };
                dataGridView2.Rows.Add(row);
            }


            Cursor.Current = Cursors.Default;

        }

        private void button6_Click (object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dataGridView2.Rows.Clear();
            List<Metric> Result = listM.GetMetricsOfType(textBox4.Text);

            for (int i = 0 ; i < Result.Count ; i++)
            {
                string[] row = { Result[i].getEntity.Output(), Result[i].getType, Result[i].getValue.ToString() };
                dataGridView2.Rows.Add(row);
            }


            Cursor.Current = Cursors.Default;
        }

        private void button7_Click (object sender, EventArgs e)
        {
            FileStructure Struct = new FileStructure(listR.Relations);
            Struct.BuildMetricsFileStructure();
        }

        private void buttonVisMet_Click(object sender, EventArgs e)
        {
            Metrics metricsForm = new Metrics(openFileDialog2.FileName);
            metricsForm.Show();
        }

        private void buttonVisRel_Click(object sender, EventArgs e)
        {
            Relations relationsForm = new Relations(openFileDialog1.FileName);
            relationsForm.Show();
        }
    }
}
