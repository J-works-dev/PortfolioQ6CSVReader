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
        string[] headers;
        public Form1()
        {
            InitializeComponent();
            
            labels = new Label[] { label1, label2, label3, label4, label5, label6, label7, label8, label9, label10 };
            textBoxs = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10 };
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {

        }

        private void buttonDelete_Click(object sender, EventArgs e)
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
                headers = csvReader.HeaderRecord;
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
            if (dataGridView.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Result.csv";
                bool fileError = false;
                dataGridView.AllowUserToAddRows = false;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            statusLabel.Text = "Error: Unable to write = " + ex.Message;
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dataGridView.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dataGridView.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dataGridView.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dataGridView.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dataGridView.Rows[i - 1].Cells[headers[j]].Value.ToString() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            statusLabel.Text = "Data Exported Successfully !";
                        }
                        catch (Exception ex)
                        {
                            statusLabel.Text = "Error :" + ex.Message;
                        }
                    }
                }
            }
            else
            {
                statusLabel.Text = "No Record To Export !";
            }
        }
    }
}
