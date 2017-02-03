using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Net;
using System.IO;
using NAudio;
using NAudio.Wave;

namespace ubersetzer
{
    public partial class Form1 : Form
    {

        Uri urltts;
        string mp3Path;
        string wavPath;
        string teks;
        WebClient tts;
        Mp3FileReader reader;

        public Form1()
        {
            InitializeComponent();
            mediaPlayer.settings.volume = 100;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'übersetzerDataSet.übersetzer' table. You can move, or remove it, as needed.
            this.übersetzerTableAdapter.Fill(this.übersetzerDataSet.übersetzer);
            // TODO: This line of code loads data into the 'übersetzerDataSet.übersetzer_Consulta' table. You can move, or remove it, as needed.
            this.übersetzer_ConsultaTableAdapter.Fill(this.übersetzerDataSet.übersetzer_Consulta);

            this.TxtClear();

            // format font in DataGridView
            this.übersetzerDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_2.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_3.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_4.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_5.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_6.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_7.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_8.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_9.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_10.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_11.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);

            this.übersetzerDataGridView_data.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 7.00F, FontStyle.Bold);

            // order per Ordnung field
            this.übersetzerDataGridView.Sort(this.dataGridViewTextBoxColumn2, ListSortDirection.Ascending);

            autoComplete();

        }

        // autocomplete suggestion must appear based on the column selected
        void autoComplete()
        {
            AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();

            string strProvider = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=C:\Users\Márcio\documents\visual studio 2015\Projects\ubersetzer\ubersetzer\übersetzer.accdb";
            String strText = txtSuche.Text;

            string strSql = "SELECT * from übersetzer where (deutsch like '" + strText + "%') OR (englisch like '" + strText + "%') OR (spanisch like '" + strText + "%')  OR (portugiesisch like '" + strText + "%') OR (französisch like '" + strText + "%') OR (italienisch like '" + strText + "%') OR (türkisch like '" + strText + "%') OR (rumänisch like '" + strText + "%') OR (bulgarisch like '" + strText + "%') OR (russisch like '" + strText + "%') OR (polnisch like '" + strText + "%') OR (arabisch like '" + strText + "%')";

            OleDbConnection con = new OleDbConnection(strProvider);
            OleDbCommand cmd = new OleDbCommand(strSql, con);
            con.Open();

            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                namesCollection.Add(reader["Deutsch"].ToString());
                namesCollection.Add(reader["Englisch"].ToString());
                namesCollection.Add(reader["Spanisch"].ToString());
                namesCollection.Add(reader["Russisch"].ToString());
                namesCollection.Add(reader["Polnisch"].ToString());
                namesCollection.Add(reader["Arabisch"].ToString());
                namesCollection.Add(reader["Portugiesisch"].ToString());
                namesCollection.Add(reader["Französisch"].ToString());
                namesCollection.Add(reader["Italienisch"].ToString());
                namesCollection.Add(reader["Türkisch"].ToString());
                namesCollection.Add(reader["Rumänisch"].ToString());
                namesCollection.Add(reader["Bulgarisch"].ToString());

            }

            con.Close();

            txtSuche.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtSuche.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtSuche.AutoCompleteCustomSource = namesCollection;
        }

        private void btnSuche_Click(object sender, EventArgs e)
        {

            //Console.WriteLine("Valor n existe ");

            string strProvider = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=C:\Users\Márcio\documents\visual studio 2015\Projects\ubersetzer\ubersetzer\übersetzer.accdb";
            String strText = txtSuche.Text;

            string strSql = "SELECT * from übersetzer where (deutsch like '" + strText + "%') OR (englisch like '" + strText + "%') OR (spanisch like '" + strText + "%')  OR (portugiesisch like '" + strText + "%') OR (französisch like '" + strText + "%') OR (italienisch like '" + strText + "%') OR (türkisch like '" + strText + "%') OR (rumänisch like '" + strText + "%') OR (bulgarisch like '" + strText + "%') OR (russisch like '" + strText + "%') OR (polnisch like '" + strText + "%') OR (arabisch like '" + strText + "%')";

            OleDbConnection con = new OleDbConnection(strProvider);
            OleDbCommand cmd = new OleDbCommand(strSql, con);
            con.Open();

            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            // Condition - if field exist or no
            if (reader.HasRows == true)
            {
                while (reader.Read())
                {

                    deutschTextBox.Text = reader["Deutsch"].ToString();
                    englischTextBox.Text = reader["Englisch"].ToString();
                    spanischTextBox.Text = reader["Spanisch"].ToString();
                    russischTextBox.Text = reader["Russisch"].ToString();
                    polnischTextBox.Text = reader["Polnisch"].ToString();
                    arabischTextBox.Text = reader["Arabisch"].ToString();
                    portugiesischTextBox.Text = reader["Portugiesisch"].ToString();
                    französischTextBox.Text = reader["Französisch"].ToString();
                    italienischTextBox.Text = reader["Italienisch"].ToString();
                    türkischTextBox.Text = reader["Türkisch"].ToString();
                    rumänischTextBox.Text = reader["Rumänisch"].ToString();
                    bulgarischTextBox.Text = reader["Bulgarisch"].ToString();

                }
            }
            else
            {
                MessageBox.Show("Daten nicht gefunden!");
            }

            reader.Close();
            con.Close();

        }

        // Method clear all fields and txtSuche
        private void txtSuche_TextChanged(object sender, EventArgs e)
        {

            this.TxtClear();

        }

        // Press ENTER is executable btnSuche_Click to search
        private void txtSuche_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSuche_Click(this, new EventArgs());

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // The fields are clear when click in txtSuche to search word
        public void TxtClear()
        {
            deutschTextBox.Clear();
            englischTextBox.Clear();
            spanischTextBox.Clear();
            russischTextBox.Clear();
            polnischTextBox.Clear();
            arabischTextBox.Clear();
            portugiesischTextBox.Clear();
            französischTextBox.Clear();
            italienischTextBox.Clear();
            türkischTextBox.Clear();
            rumänischTextBox.Clear();
            bulgarischTextBox.Clear();
        }

        private void txtSuche_MouseClick(object sender, EventArgs e)
        {
            txtSuche.Text = "";
        }

        private void SaveButton_Click(System.Object sender, System.EventArgs e)
        {
            // this.übersetzerTableAdapter.Update(this.übersetzerDataSet.übersetzer);

            this.Validate();
            this.übersetzerBindingSource.EndEdit();
            // this.tableAdapterManager.UpdateAll(this.übersetzerDataSet);
            this.übersetzerTableAdapter.Update(this.übersetzerDataSet.übersetzer);
            MessageBox.Show("Die Daten wurden mit Erfolg aufgenommen!");
        }

        // btn to Delete records
        private void DeleteItem_Click(System.Object sender, System.EventArgs e)
        {

            if (MessageBox.Show("Sind Sie wirklich löschen das Element?", "Bestätigung löschen", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                for (int i = 0; i < übersetzerDataGridView_data.SelectedRows.Count; i++)
                {
                    this.übersetzerBindingSource.RemoveAt(übersetzerDataGridView_data.SelectedRows[i].Index);
                }
                this.Validate();
                this.übersetzerBindingSource.EndEdit();
                this.übersetzerTableAdapter.Update(this.übersetzerDataSet.übersetzer);

            }
            else
            {
                this.übersetzerTableAdapter.Fill(this.übersetzerDataSet.übersetzer);
                MessageBox.Show("Zeile nicht entfernt", "Zeile entfernen", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }

        // To get value from Cell in position 2
        private void übersetzerDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (übersetzerDataGridView.CurrentCell.ColumnIndex.Equals(2) && e.RowIndex != -1)
            {
                if (übersetzerDataGridView.CurrentCell != null && übersetzerDataGridView.CurrentCell.Value != null)
                {
                    // MessageBox.Show(übersetzerDataGridView.CurrentCell.Value.ToString());
                    string valueCell = übersetzerDataGridView.CurrentCell.Value.ToString();
                    //MessageBox.Show(valueCell);

                    try
                    {
                        teks = valueCell;
                        mp3Path = Environment.CurrentDirectory + @"\tmp.mp3";
                        wavPath = Environment.CurrentDirectory + @"\ubersetzer.mp3";
                        urltts = new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=en&q=" + teks);

                        /*using (tts = new WebClient())
                        {
                            tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 9.0; Windows;)");
                            tts.DownloadFile(urltts, mp3Path);
                            tts.Dispose();

                        }

                        using (reader = new Mp3FileReader(new FileStream(mp3Path, FileMode.OpenOrCreate)))
                        {
                            WaveFileWriter.CreateWaveFile(wavPath, reader);
                        }
                        */
                        // MessageBox.Show(mp3Path);

                        if (File.Exists(wavPath))
                        {

                            MessageBox.Show("existe");
                            File.Delete(mp3Path);
                            File.Delete(wavPath);

                            using (tts = new WebClient())
                            {
                                tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 9.0; Windows;)");
                                tts.DownloadFile(urltts, mp3Path);
                                // tts.DownloadFileAsync(new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=en&q=" + teks), @"\tmp.mp3");
                            }
                            using (reader = new Mp3FileReader(new FileStream(mp3Path, FileMode.OpenOrCreate)))
                            {
                                WaveFileWriter.CreateWaveFile(wavPath, reader);
                            }

                        }

                        else
                        {
                            MessageBox.Show("não existe");
                            using (tts = new WebClient())
                            {
                                tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 9.0; Windows;)");
                                tts.DownloadFile(urltts, mp3Path);
                            }
                            using (reader = new Mp3FileReader(new FileStream(mp3Path, FileMode.OpenOrCreate)))
                            {
                                WaveFileWriter.CreateWaveFile(wavPath, reader);
                            }
                        }

                        mediaPlayer.URL = wavPath;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex);
                    }

                    teks = null;

                }

            }
        }

    }
}
