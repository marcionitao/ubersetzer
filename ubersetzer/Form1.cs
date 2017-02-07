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
        //  string wavPath;
        string teks;
        WebClient tts;
        //   Mp3FileReader reader;
       
       

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

            // Clean all TxtBox when begin
            this.TxtClear();
            // Define the style of DataGridView
            this.DataGridStyle();
            // Define autocomplete in textBox
            this.autoComplete();
            // Add image in all row of column
            this.AddImageColumn();
           
        }

        // Add image in all row of column
        void AddImageColumn()
        {
            // Add image in all row of column
            DataGridViewImageColumn img = new DataGridViewImageColumn();
            Image image = Image.FromFile(@"C:\Users\Márcio\Desktop\david\icons\sound_16.bmp");
            img.Image = image;

            übersetzerDataGridView.Columns.Add(img);

            img.HeaderText = "";
            img.Name = "img";

            // define width size
            DataGridViewColumn column = übersetzerDataGridView.Columns[3];
            column.Width = 30;
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
                    string valueCell = übersetzerDataGridView.CurrentCell.Value.ToString();
                    // clear content speak, then add a new sound  
                    mediaPlayer.currentPlaylist.clear();

                    try
                    {
                        teks = valueCell;
                        mp3Path = Environment.CurrentDirectory + @"\ubersetzer.mp3";
                        urltts = new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=en&q=" + teks);
                        // make download file
                        using (tts = new WebClient())
                        {
                            tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows;)");
                            tts.DownloadFile(urltts, mp3Path);
                        }
                        // play file speak
                        mediaPlayer.URL = mp3Path;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Keine Internetverbindung !");
                    }
                    teks = null;

                }

            }

        }

        // To get value from Cell in position 2
        private void übersetzerDataGridView_2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (übersetzerDataGridView_2.CurrentCell.ColumnIndex.Equals(2) && e.RowIndex != -1)
            {
                if (übersetzerDataGridView_2.CurrentCell != null && übersetzerDataGridView_2.CurrentCell.Value != null)
                {
                    string valueCell = übersetzerDataGridView_2.CurrentCell.Value.ToString();
                    // clear content speak, then add a new sound  
                    mediaPlayer.currentPlaylist.clear();

                    try
                    {
                        teks = valueCell;
                        mp3Path = Environment.CurrentDirectory + @"\ubersetzer.mp3";
                        urltts = new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=es&q=" + teks);
                        // make download file
                        using (tts = new WebClient())
                        {
                            tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows;)");
                            tts.DownloadFile(urltts, mp3Path);
                        }
                        // play file speak
                        mediaPlayer.URL = mp3Path;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Keine Internetverbindung !");
                    }
                    teks = null;

                }

            }

        }

        // To get value from Cell in position 2
        private void übersetzerDataGridView_3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (übersetzerDataGridView_3.CurrentCell.ColumnIndex.Equals(2) && e.RowIndex != -1)
            {
                if (übersetzerDataGridView_3.CurrentCell != null && übersetzerDataGridView_3.CurrentCell.Value != null)
                {
                    string valueCell = übersetzerDataGridView_3.CurrentCell.Value.ToString();
                    // clear content speak, then add a new sound  
                    mediaPlayer.currentPlaylist.clear();

                    try
                    {
                        teks = valueCell;
                        mp3Path = Environment.CurrentDirectory + @"\ubersetzer.mp3";
                        urltts = new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=pt-PT&q=" + teks);
                        // make download file
                        using (tts = new WebClient())
                        {
                            tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows;)");
                            tts.DownloadFile(urltts, mp3Path);
                        }
                        // play file speak
                        mediaPlayer.URL = mp3Path;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Keine Internetverbindung !");
                    }
                    teks = null;

                }

            }

        }

        // Define the style of DataGridView
        void DataGridStyle()
        {
            // format font in DataGridView
            this.übersetzerDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border

            this.übersetzerDataGridView_2.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_2.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_2.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border

            this.übersetzerDataGridView_3.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_3.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_3.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border

            this.übersetzerDataGridView_4.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_4.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_4.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border

            this.übersetzerDataGridView_5.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_5.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_5.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border

            this.übersetzerDataGridView_6.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_6.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_6.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border

            this.übersetzerDataGridView_7.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_7.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_7.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border

            this.übersetzerDataGridView_8.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_8.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_8.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border

            this.übersetzerDataGridView_9.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_9.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_9.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border

            this.übersetzerDataGridView_10.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_10.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_10.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border

            this.übersetzerDataGridView_11.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_11.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_11.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border

            this.übersetzerDataGridView_data.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_data.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 7.00F, FontStyle.Bold);

            // order per Ordnung field
            this.übersetzerDataGridView.Sort(this.dataGridViewTextBoxColumn2, ListSortDirection.Ascending);

        }

    }

}
