using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortfolioQ6CSVReader
{
    public partial class Form1 : Form
    {
        private Label[] labels;
        private TextBox[] textBoxs;
        public Form1()
        {
            InitializeComponent();
            
            labels = new Label[] { label1, label2, label3, label4, label5, label6, label7, label8, label9, label10, label11 };
            textBoxs = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11 };
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {

        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            string path = getPathfromDialog();

            if (path != null)
            {
                var streamReader = new StreamReader(path);
                var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

                Console.WriteLine(csvReader.Read());
                Console.WriteLine(csvReader.ReadHeader());
                string[] headers = csvReader.HeaderRecord;
                int colCount = headers.Length;
                //Console.WriteLine("[{0}]", string.Join(", ", headers) + colCount);
                dataGridView.ColumnCount = colCount;
                for (int i = 0; i < colCount; i++)
                {
                    dataGridView.Columns[i].Name = headers[i];
                    labels[i].Visible = true;
                    labels[i].Text = headers[i];
                    textBoxs[i].Visible = true;
                }
                string value;
                string[] row = new string[colCount];

                while (csvReader.Read())
                {
                    for (int i = 0; csvReader.TryGetField<string>(i, out value); i++)
                    {
                        row[i] = value;
                    }

                    dataGridView.Rows.Add(row);
                }
            }
        }

        private string getPathfromDialog()
        {
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Title = "Browse Text Files";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DefaultExt = "csv";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ReadOnlyChecked = true;
            openFileDialog.ShowReadOnly = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            DataSet customersDataSet = new DataSet();

        }
    }
}
