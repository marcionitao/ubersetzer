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
//using NAudio;
//using NAudio.Wave;

namespace ubersetzer
{
    public partial class Form1 : Form
    {

        Uri urltts;
        string mp3Path, path;
        string teks;
        WebClient tts;

        public Form1()
        {
            InitializeComponent();
            mediaPlayer.settings.volume = 100;

            MessageBox.Show("Herzlich Willkommen! Dies ist eine Probeversion. Diese Version wird in 30 Tagen verfallen");
            this.DateTimeOut();// Time out date

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // TODO: This line of code loads data into the 'übersetzerDataSet.übersetzer' table. You can move, or remove it, as needed.
            this.übersetzerTableAdapter.Fill(this.übersetzerDataSet.übersetzer);
            // order the fields per Ordnung and Transaction 
            this.übersetzerBindingSource.Sort = "Transaction DESC, Ordnung ASC";
            // TODO: This line of code loads data into the 'übersetzerDataSet.übersetzer_Consulta' table. You can move, or remove it, as needed.
            this.übersetzer_ConsultaTableAdapter.Fill(this.übersetzerDataSet.übersetzer_Consulta);

            // Clean all TxtBox when begin
            this.TxtClear();

            // Define the style of DataGridView
            this.DataGridStyle();
            // Define autocomplete in textBox
            this.autoComplete();
            // Add image of sound in all row of column
            // this.AddImageColumn();

        }

        // Add image of sound in all row of column for parameters
        void AddImageColumn(DataGridView dataGridImage)
        {
            // dataGridImage = new Datagridview();
            // Add image in all row of column
            DataGridViewImageColumn img = new DataGridViewImageColumn();
            Image image = Image.FromFile(@"C:\Users\Márcio\Desktop\david\icons\sound_16.bmp");
            img.Image = image;

            dataGridImage.Columns.Add(img);
            //übersetzerDataGridView.Columns.Add(img);

            img.HeaderText = "";
            img.Name = "img";

            // define width size
            // DataGridViewColumn column = übersetzerDataGridView.Columns[3];
            DataGridViewColumn column = dataGridImage.Columns[3];
            column.Width = 30;
        }

        // Update datas after insert new registe
        void updateDataBase()
        {

            try
            {
                string strProvider = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=C:\VisualStudio_Projects\ubersetzer\ubersetzer\übersetzer.accdb";
                OleDbConnection con = new OleDbConnection(strProvider);
                con.Open();
                //string strSql = "update [übersetzer] set [Ordnung]='" + ordnungTextBox.Text + "' ,[Transaction]='" + transaktionComboBox.Text + "' ,[Deutsch]='" + deutschTextBox1.Text + "' ,[Englisch]='" + englischTextBox1.Text + "' ,[Spanisch]='" + spanischTextBox1.Text + "' ,[Portugiesisch]='" + portugiesischTextBox1.Text + "' ,[Französisch]='" + französischTextBox1.Text + "' ,[Italienisch]='" + italienischTextBox1.Text + "' ,[Türkisch]='" + türkischTextBox1.Text + "' ,[Rumänisch]='" + rumänischTextBox1.Text + "' ,[Bulgarisch]='" + bulgarischTextBox1.Text + "' ,[Russisch]='" + russischTextBox1.Text + "' ,[Polnisch]='" + polnischTextBox1.Text + "' ,[Arabisch]='" + arabischTextBox1.Text + "' where ID=" + id.Text + "";
                string strSql = "UPDATE [übersetzer] SET [Ordnung]=@Ordnung,[Transaction]=@Transaction,[Deutsch]=@Deustch,[Englisch]=@Englisch,[Spanisch]=@Spanisch,[Portugiesisch]=@Portugiesisch,[Französisch]=@Französisch,[Italienisch]=@Italienisch,[Türkisch]=@Türkisch,[Rumänisch]=@Rumänisch,[Bulgarisch]=@Bulgarisch,[Russisch]=@Russisch,[Polnisch]=@Polnisch,[Arabisch]=@Arabisch WHERE ID=@ID";
                
                // MessageBox.Show(strSql);
                OleDbCommand cmd = new OleDbCommand(strSql, con);

                cmd.Parameters.AddWithValue("@ID", id.Text);
                cmd.Parameters.AddWithValue("@Ordnung", ordnungTextBox.Text);
                cmd.Parameters.AddWithValue("@Transaction", transaktionComboBox.Text);
                cmd.Parameters.AddWithValue("@Deutsch", deutschTextBox1.Text);
                cmd.Parameters.AddWithValue("@Englisch", englischTextBox1.Text);
                cmd.Parameters.AddWithValue("@Spanisch", spanischTextBox1.Text);
                cmd.Parameters.AddWithValue("@Portugiesisch", portugiesischTextBox1.Text);
                cmd.Parameters.AddWithValue("@Französisch", französischTextBox1.Text);
                cmd.Parameters.AddWithValue("@Italienisch", italienischTextBox1.Text);
                cmd.Parameters.AddWithValue("@Türkisch", türkischTextBox1.Text);
                cmd.Parameters.AddWithValue("@Rumänisch", rumänischTextBox1.Text);
                cmd.Parameters.AddWithValue("@Bulgarisch", bulgarischTextBox1.Text);
                cmd.Parameters.AddWithValue("@Russisch", russischTextBox1.Text);
                cmd.Parameters.AddWithValue("@Polnisch", polnischTextBox1.Text);
                cmd.Parameters.AddWithValue("@Arabisch", arabischTextBox1.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Die Daten wurden mit Erfolg aufgenommen!");
                con.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Achtung! Setzen Sie nicht Zeichen, wie zB: / + '- \\ ; ´ | \" * ´ ");
                //MessageBox.Show("Error : "+ex);
            }

            this.Validate();
            this.übersetzerBindingSource.EndEdit();
            this.übersetzerTableAdapter.Update(this.übersetzerDataSet.übersetzer);

        }

        // save or update database when click in save button
        void insertDataBase()
        {

            if ((ordnungTextBox.Text == "") || (transaktionComboBox.Text == ""))
            {
                MessageBox.Show("Es gibt wichtige Felder leer. Bitte füllen sie in und speichern!");
            }
            else
            {
                try
                {
                    string strProvider = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=C:\VisualStudio_Projects\ubersetzer\ubersetzer\übersetzer.accdb";
                    OleDbConnection con = new OleDbConnection(strProvider);
                    con.Open();
                    string strSql = "INSERT INTO [übersetzer] ([Ordnung],[Transaction],[Deutsch],[Englisch],[Spanisch],[Portugiesisch],[Französisch],[Italienisch],[Türkisch],[Rumänisch],[Bulgarisch],[Russisch],[Polnisch],[Arabisch])" + " VALUES(@Ordnung, @Transaction, @Deustch, @Englisch,@Spanisch,@Portugiesisch,@Französisch,@Italienisch,@Türkisch,@Rumänisch,@Bulgarisch,@Russisch,@Polnisch,@Arabisch)";

                    OleDbCommand cmd = new OleDbCommand(strSql, con);

                    cmd.Parameters.AddWithValue("@Ordnung", ordnungTextBox.Text);
                    cmd.Parameters.AddWithValue("@Transaction", transaktionComboBox.Text);
                    cmd.Parameters.AddWithValue("@Deutsch", deutschTextBox1.Text);
                    cmd.Parameters.AddWithValue("@Englisch", englischTextBox1.Text);
                    cmd.Parameters.AddWithValue("@Spanisch", spanischTextBox1.Text);
                    cmd.Parameters.AddWithValue("@Portugiesisch", portugiesischTextBox1.Text);
                    cmd.Parameters.AddWithValue("@Französisch", französischTextBox1.Text);
                    cmd.Parameters.AddWithValue("@Italienisch", italienischTextBox1.Text);
                    cmd.Parameters.AddWithValue("@Türkisch", türkischTextBox1.Text);
                    cmd.Parameters.AddWithValue("@Rumänisch", rumänischTextBox1.Text);
                    cmd.Parameters.AddWithValue("@Bulgarisch", bulgarischTextBox1.Text);
                    cmd.Parameters.AddWithValue("@Russisch", russischTextBox1.Text);
                    cmd.Parameters.AddWithValue("@Polnisch", polnischTextBox1.Text);
                    cmd.Parameters.AddWithValue("@Arabisch", arabischTextBox1.Text);

                    int a = cmd.ExecuteNonQuery();
                    if (a > 0)
                    {
                        MessageBox.Show("Die Daten wurden mit Erfolg aufgenommen!");
                    }
                    // this.refreshDataBase();
                    con.Close();
                }
                catch (System.Exception ex)
                {
                    Console.Write("-" + ex);
                }

                this.Validate();
                this.übersetzerBindingSource.EndEdit();
                this.übersetzerTableAdapter.Update(this.übersetzerDataSet.übersetzer);

                this.übersetzerTableAdapter.Fill(this.übersetzerDataSet.übersetzer);
                this.übersetzerBindingSource.ResetBindings(false);

            }

        }

        // autocomplete suggestion must appear based on the column selected
        void autoComplete()
        {
            AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();

            // string strProvider = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=C:\Users\Márcio\documents\visual studio 2015\Projects\ubersetzer\ubersetzer\übersetzer.accdb";
            string strProvider = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=C:\VisualStudio_Projects\ubersetzer\ubersetzer\übersetzer.accdb";
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
            this.DateTimeOut();// Time out date

            // string strProvider = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=C:\Users\Márcio\documents\visual studio 2015\Projects\ubersetzer\ubersetzer\übersetzer.accdb";
            string strProvider = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=C:\VisualStudio_Projects\ubersetzer\ubersetzer\übersetzer.accdb";
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
            this.DateTimeOut();// Time out date
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
            //id.Clear();
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

        // Delete Record
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string strProvider = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=C:\VisualStudio_Projects\ubersetzer\ubersetzer\übersetzer.accdb";
                OleDbConnection con = new OleDbConnection(strProvider);
                con.Open();

                string strSql = "DELETE FROM [übersetzer]  where ID=" + id.Text + "";
                OleDbCommand cmd = new OleDbCommand(strSql, con);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Die Daten wurden erfolgreich gelöscht!");
                con.Close();

                for (int i = 0; i < übersetzerDataGridView_data.SelectedRows.Count; i++)
                {
                    this.übersetzerBindingSource.RemoveAt(übersetzerDataGridView_data.SelectedRows[i].Index);
                }
                this.Validate();
                this.übersetzerBindingSource.EndEdit();
                this.übersetzerTableAdapter.Update(this.übersetzerDataSet.übersetzer);
            }
            catch (System.Exception ex)
            {
                Console.Write("-" + ex);
            }
        }

        private void SaveButton_Click(System.Object sender, System.EventArgs e)
        {
            //First Verify if record exist, then define save or update
            try
            {
                string strProvider = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source=C:\VisualStudio_Projects\ubersetzer\ubersetzer\übersetzer.accdb";
                OleDbConnection con = new OleDbConnection(strProvider);
                con.Open();
                string strSql = "SELECT COUNT(*) FROM [übersetzer] WHERE ([ID] = @id)";

                // MessageBox.Show(strSql);
                OleDbCommand cmd = new OleDbCommand(strSql, con);
                cmd.Parameters.AddWithValue("@id", id.Text);

                int UserExist = (int)cmd.ExecuteScalar();

                if (UserExist > 0)
                {
                    this.updateDataBase();// if registe exist update
                }
                else
                {
                    this.insertDataBase();// if registe not exist add new
                }

                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Update fehlgeschlagen!" + ex);
            }
        }

        /*---------------Begin------------------Define action on click in the word to translator-------------------------------------------------*/

        // To get value from Cell in position 3 - England
        private void übersetzerDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Clear the select Cell
            // this.übersetzerDataGridView.ClearSelection();

            if (übersetzerDataGridView.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
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
                    catch (Exception)
                    {
                        MessageBox.Show("Der Ton ist leider nicht verfügbar!");
                    }
                    teks = null;

                }

            }

        }
       
        // To get value from Cell in position 3- Spain
        private void übersetzerDataGridView_2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (übersetzerDataGridView_2.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
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
                    catch (Exception)
                    {
                        MessageBox.Show("Der Ton ist leider nicht verfügbar!");
                    }
                    teks = null;

                }

            }

        }

        // To get value from Cell in position 3 - Portugal
        private void übersetzerDataGridView_3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (übersetzerDataGridView_3.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
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
                    catch (Exception)
                    {
                        MessageBox.Show("Der Ton ist leider nicht verfügbar!");
                    }
                    teks = null;

                }

            }

        }

        // To get value from Cell in position 3 - France
        private void übersetzerDataGridView_4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (übersetzerDataGridView_4.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
            {
                if (übersetzerDataGridView_4.CurrentCell != null && übersetzerDataGridView_4.CurrentCell.Value != null)
                {
                    string valueCell = übersetzerDataGridView_4.CurrentCell.Value.ToString();
                    // clear content speak, then add a new sound  
                    mediaPlayer.currentPlaylist.clear();

                    try
                    {
                        teks = valueCell;
                        mp3Path = Environment.CurrentDirectory + @"\ubersetzer.mp3";
                        urltts = new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=fr-FR&q=" + teks);
                        // make download file
                        using (tts = new WebClient())
                        {
                            tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows;)");
                            tts.DownloadFile(urltts, mp3Path);
                        }
                        // play file speak
                        mediaPlayer.URL = mp3Path;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der Ton ist leider nicht verfügbar!");
                    }
                    teks = null;

                }

            }

        }

        // To get value from Cell in position 3 - Italy
        private void übersetzerDataGridView_5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (übersetzerDataGridView_5.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
            {
                if (übersetzerDataGridView_5.CurrentCell != null && übersetzerDataGridView_5.CurrentCell.Value != null)
                {
                    string valueCell = übersetzerDataGridView_5.CurrentCell.Value.ToString();
                    // clear content speak, then add a new sound  
                    mediaPlayer.currentPlaylist.clear();

                    try
                    {
                        teks = valueCell;
                        mp3Path = Environment.CurrentDirectory + @"\ubersetzer.mp3";
                        urltts = new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=it&q=" + teks);
                        // make download file
                        using (tts = new WebClient())
                        {
                            tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows;)");
                            tts.DownloadFile(urltts, mp3Path);
                        }
                        // play file speak
                        mediaPlayer.URL = mp3Path;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der Ton ist leider nicht verfügbar!");
                    }
                    teks = null;

                }

            }

        }

        // To get value from Cell in position 3 - Turkei
        private void übersetzerDataGridView_6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (übersetzerDataGridView_6.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
            {
                if (übersetzerDataGridView_6.CurrentCell != null && übersetzerDataGridView_6.CurrentCell.Value != null)
                {
                    string valueCell = übersetzerDataGridView_6.CurrentCell.Value.ToString();
                    // clear content speak, then add a new sound  
                    mediaPlayer.currentPlaylist.clear();

                    try
                    {
                        teks = valueCell;
                        mp3Path = Environment.CurrentDirectory + @"\ubersetzer.mp3";
                        urltts = new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=tr-TR&q=" + teks);
                        // make download file
                        using (tts = new WebClient())
                        {
                            tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows;)");
                            tts.DownloadFile(urltts, mp3Path);
                        }
                        // play file speak
                        mediaPlayer.URL = mp3Path;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der Ton ist leider nicht verfügbar!");
                    }
                    teks = null;

                }

            }

        }

        // To get value from Cell in position 3 - Romenien
        private void übersetzerDataGridView_7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (übersetzerDataGridView_7.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
            {
                if (übersetzerDataGridView_7.CurrentCell != null && übersetzerDataGridView_7.CurrentCell.Value != null)
                {
                    string valueCell = übersetzerDataGridView_7.CurrentCell.Value.ToString();
                    // clear content speak, then add a new sound  
                    mediaPlayer.currentPlaylist.clear();

                    try
                    {
                        teks = valueCell;
                        mp3Path = Environment.CurrentDirectory + @"\ubersetzer.mp3";
                        urltts = new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=ro-RO&q=" + teks);
                        // make download file
                        using (tts = new WebClient())
                        {
                            tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows;)");
                            tts.DownloadFile(urltts, mp3Path);
                        }
                        // play file speak
                        mediaPlayer.URL = mp3Path;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der Ton ist leider nicht verfügbar!");
                    }
                    teks = null;

                }

            }

        }

        // To get value from Cell in position 3 - Bulgarien
        private void übersetzerDataGridView_8_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (übersetzerDataGridView_8.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
            {
                if (übersetzerDataGridView_8.CurrentCell != null && übersetzerDataGridView_8.CurrentCell.Value != null)
                {
                    string valueCell = übersetzerDataGridView_8.CurrentCell.Value.ToString();
                    // clear content speak, then add a new sound  
                    mediaPlayer.currentPlaylist.clear();

                    try
                    {
                        teks = valueCell;
                        mp3Path = Environment.CurrentDirectory + @"\ubersetzer.mp3";
                        urltts = new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=ru&q=" + teks);
                        // make download file
                        using (tts = new WebClient())
                        {
                            tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows;)");
                            tts.DownloadFile(urltts, mp3Path);
                        }
                        // play file speak
                        mediaPlayer.URL = mp3Path;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der Ton ist leider nicht verfügbar!");
                    }
                    teks = null;

                }

            }

        }

        // To get value from Cell in position 3 - Russland
        private void übersetzerDataGridView_9_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (übersetzerDataGridView_9.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
            {
                if (übersetzerDataGridView_9.CurrentCell != null && übersetzerDataGridView_9.CurrentCell.Value != null)
                {
                    string valueCell = übersetzerDataGridView_9.CurrentCell.Value.ToString();
                    // clear content speak, then add a new sound  
                    mediaPlayer.currentPlaylist.clear();

                    try
                    {
                        teks = valueCell;
                        mp3Path = Environment.CurrentDirectory + @"\ubersetzer.mp3";
                        urltts = new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=ru-RU&q=" + teks);
                        // make download file
                        using (tts = new WebClient())
                        {
                            tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows;)");
                            tts.DownloadFile(urltts, mp3Path);
                        }
                        // play file speak
                        mediaPlayer.URL = mp3Path;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der Ton ist leider nicht verfügbar!");
                    }
                    teks = null;

                }

            }

        }

        // To get value from Cell in position 3 - Polen
        private void übersetzerDataGridView_10_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (übersetzerDataGridView_10.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
            {
                if (übersetzerDataGridView_10.CurrentCell != null && übersetzerDataGridView_10.CurrentCell.Value != null)
                {
                    string valueCell = übersetzerDataGridView_10.CurrentCell.Value.ToString();
                    // clear content speak, then add a new sound  
                    mediaPlayer.currentPlaylist.clear();

                    try
                    {
                        teks = valueCell;
                        mp3Path = Environment.CurrentDirectory + @"\ubersetzer.mp3";
                        urltts = new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=pl-PL&q=" + teks);
                        // make download file
                        using (tts = new WebClient())
                        {
                            tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows;)");
                            tts.DownloadFile(urltts, mp3Path);
                        }
                        // play file speak
                        mediaPlayer.URL = mp3Path;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der Ton ist leider nicht verfügbar!");
                    }
                    teks = null;

                }

            }

        }

        // To get value from Cell in position 3 - Arabic
        private void übersetzerDataGridView_11_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (übersetzerDataGridView_11.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
            {
                if (übersetzerDataGridView_11.CurrentCell != null && übersetzerDataGridView_11.CurrentCell.Value != null)
                {
                    string valueCell = übersetzerDataGridView_11.CurrentCell.Value.ToString();
                    // clear content speak, then add a new sound  
                    mediaPlayer.currentPlaylist.clear();

                    try
                    {
                        teks = valueCell;
                        mp3Path = Environment.CurrentDirectory + @"\ubersetzer.mp3";
                        urltts = new Uri("http://translate.google.com/translate_tts?client=tw-ob&tl=ar-AR&q=" + teks);
                        // make download file
                        using (tts = new WebClient())
                        {
                            tts.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows;)");
                            tts.DownloadFile(urltts, mp3Path);
                        }
                        // play file speak
                        mediaPlayer.URL = mp3Path;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Der Ton ist leider nicht verfügbar!");
                    }
                    teks = null;

                }

            }

        }
        /*---------------End------------------Define action on click in the word to translator-------------------------------------------------*/

        // Define the style and image of DataGridView
        void DataGridStyle()
        {
            // format font in DataGridView
            //-------------------------DataGrieView England-------------------------------
            this.übersetzerDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border
            this.AddImageColumn(übersetzerDataGridView);  // Add image of sound in all row of column for parameters
            // break line and adjust text in cell and fill
            this.übersetzerDataGridView.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;         
            this.übersetzerDataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.übersetzerDataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // this.übersetzerDataGridView.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //this.übersetzerDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            //-------------------------DataGrieView Spain-------------------------------------------
            this.übersetzerDataGridView_2.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_2.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_2.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border
            this.AddImageColumn(übersetzerDataGridView_2);  // Add image of sound in all row of column for parameters

            // break line and adjust text in cell and fill
            this.übersetzerDataGridView_2.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_2.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.übersetzerDataGridView_2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.übersetzerDataGridView_2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //--------------------------DataGrieView Portugal------------------------------------------
            this.übersetzerDataGridView_3.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_3.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_3.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border
            this.AddImageColumn(übersetzerDataGridView_3);  // Add image of sound in all row of column for parameters

            // break line and adjust text in cell and fill
            this.übersetzerDataGridView_3.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_3.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.übersetzerDataGridView_3.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.übersetzerDataGridView_3.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //--------------------------DataGrieView--France---------------------------------------
            this.übersetzerDataGridView_4.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_4.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_4.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border
            this.AddImageColumn(übersetzerDataGridView_4);  // Add image of sound in all row of column for parameters

            // break line and adjust text in cell and fill
            this.übersetzerDataGridView_4.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_4.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.übersetzerDataGridView_4.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.übersetzerDataGridView_4.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //--------------------------DataGrieView Italy------------------------------------------
            this.übersetzerDataGridView_5.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_5.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_5.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border
            this.AddImageColumn(übersetzerDataGridView_5);  // Add image of sound in all row of column for parameters

            // break line and adjust text in cell and fill
            this.übersetzerDataGridView_5.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_5.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_5.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.übersetzerDataGridView_5.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.übersetzerDataGridView_5.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //---------------------------DataGrieView Turkei-----------------------------------------
            this.übersetzerDataGridView_6.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_6.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_6.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border
            this.AddImageColumn(übersetzerDataGridView_6);  // Add image of sound in all row of column for parameters

            // break line and adjust text in cell and fill
            this.übersetzerDataGridView_6.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_6.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_6.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.übersetzerDataGridView_6.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.übersetzerDataGridView_6.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //---------------------------DataGrieView Romenien-----------------------------------------
            this.übersetzerDataGridView_7.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_7.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_7.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border
            this.AddImageColumn(übersetzerDataGridView_7);  // Add image of sound in all row of column for parameters

            // break line and adjust text in cell and fill
            this.übersetzerDataGridView_7.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_7.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_7.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.übersetzerDataGridView_7.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.übersetzerDataGridView_7.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //---------------------------DataGrieView Bulgarien-----------------------------------------
            this.übersetzerDataGridView_8.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_8.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_8.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border
            this.AddImageColumn(übersetzerDataGridView_8);  // Add image of sound in all row of column for parameters

            // break line and adjust text in cell and fill
            this.übersetzerDataGridView_8.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_8.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_8.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.übersetzerDataGridView_8.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.übersetzerDataGridView_8.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //---------------------------DataGrieView Russian-----------------------------------------
            this.übersetzerDataGridView_9.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_9.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_9.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border
            this.AddImageColumn(übersetzerDataGridView_9);  // Add image of sound in all row of column for parameters

            // break line and adjust text in cell and fill
            this.übersetzerDataGridView_9.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_9.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_9.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.übersetzerDataGridView_9.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.übersetzerDataGridView_9.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //----------------------------DataGrieView Polen----------------------------------------
            this.übersetzerDataGridView_10.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_10.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_10.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border
            this.AddImageColumn(übersetzerDataGridView_10);  // Add image of sound in all row of column for parameters

            // break line and adjust text in cell and fill
            this.übersetzerDataGridView_10.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_10.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_10.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.übersetzerDataGridView_10.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.übersetzerDataGridView_10.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //--------------------------DataGrieView Arabian------------------------------------------
            this.übersetzerDataGridView_11.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            this.übersetzerDataGridView_11.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_11.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal; // Remove all Row border
            this.AddImageColumn(übersetzerDataGridView_11);  // Add image of sound in all row of column for parameters

            // break line and adjust text in cell and fill
            this.übersetzerDataGridView_11.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_11.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True; // option only one column         
            this.übersetzerDataGridView_11.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.übersetzerDataGridView_11.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.übersetzerDataGridView_11.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //--------------------------DataGrieView in the Management Data------------------------------------------
            this.übersetzerDataGridView_data.AllowUserToAddRows = false; //disable the last blank line in DatagridView
            this.übersetzerDataGridView_data.ColumnHeadersDefaultCellStyle.Font = new Font("Consolas", 8.00F, FontStyle.Bold);
            this.übersetzerDataGridView_data.DefaultCellStyle.Font = new Font("Consolas", 8.50F);// change font in content cell
            this.übersetzerDataGridView_data.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            this.übersetzerDataGridView_data.ColumnHeadersDefaultCellStyle.BackColor = Color.LemonChiffon;

            // break line and adjust text in cell and fill - All 13 columns are formated
            for (int i=2;i<=13;i++)
            {
                this.übersetzerDataGridView_data.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                this.übersetzerDataGridView_data.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
                this.übersetzerDataGridView_data.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            
        }

        // ----Function Define Colors condiction in Rows - "geld senden" or "geld empfangen" - begin----------------
        //------Table with the all datas in Daten
        private void übersetzerDataGridView_dataFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (übersetzerDataGridView_data.Columns[e.ColumnIndex].HeaderText == "Transaction" && übersetzerDataGridView_data.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (übersetzerDataGridView_data.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Senden")
                {
                    übersetzerDataGridView_data.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
                }
                if (übersetzerDataGridView_data.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Empfangen")
                {
                    übersetzerDataGridView_data.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(204, 255, 153);
                }
            }
        }

        //------England
        private void übersetzerDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (übersetzerDataGridView.Columns[e.ColumnIndex].HeaderText == "Transaktion" && übersetzerDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (übersetzerDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Senden")
                {
                    übersetzerDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
                }
                if (übersetzerDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Empfangen")
                {
                    übersetzerDataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(204, 255, 153);
                }
            }
            // Clear Cell selection
            übersetzerDataGridView.ClearSelection();
        }
        //------Spain
        private void übersetzerDataGridView_2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (übersetzerDataGridView_2.Columns[e.ColumnIndex].HeaderText == "Transaktion" && übersetzerDataGridView_2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (übersetzerDataGridView_2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Senden")
                {
                    übersetzerDataGridView_2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
                }
                if (übersetzerDataGridView_2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Empfangen")
                {
                    übersetzerDataGridView_2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(204, 255, 153);
                }
            }
            // Clear Cell selection
            übersetzerDataGridView_2.ClearSelection();
        }
        //------Portugal
        private void übersetzerDataGridView_3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (übersetzerDataGridView_3.Columns[e.ColumnIndex].HeaderText == "Transaktion" && übersetzerDataGridView_3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (übersetzerDataGridView_3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Senden")
                {
                    übersetzerDataGridView_3.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
                }
                if (übersetzerDataGridView_3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Empfangen")
                {
                    übersetzerDataGridView_3.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(204, 255, 153);
                }
            }
            // Clear Cell selection
            übersetzerDataGridView_3.ClearSelection();
        }
        //------France
        private void übersetzerDataGridView_4_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (übersetzerDataGridView_4.Columns[e.ColumnIndex].HeaderText == "Transaktion" && übersetzerDataGridView_4.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (übersetzerDataGridView_4.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Senden")
                {
                    übersetzerDataGridView_4.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
                }
                if (übersetzerDataGridView_4.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Empfangen")
                {
                    übersetzerDataGridView_4.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(204, 255, 153);
                }
            }
            // Clear Cell selection
            übersetzerDataGridView_4.ClearSelection();
        }
        //------Italy
        private void übersetzerDataGridView_5_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (übersetzerDataGridView_5.Columns[e.ColumnIndex].HeaderText == "Transaktion" && übersetzerDataGridView_5.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (übersetzerDataGridView_5.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Senden")
                {
                    übersetzerDataGridView_5.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
                }
                if (übersetzerDataGridView_5.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Empfangen")
                {
                    übersetzerDataGridView_5.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(204, 255, 153);
                }
            }
            // Clear Cell selection
            übersetzerDataGridView_5.ClearSelection();
        }
        //------Turkei
        private void übersetzerDataGridView_6_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (übersetzerDataGridView_6.Columns[e.ColumnIndex].HeaderText == "Transaktion" && übersetzerDataGridView_6.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (übersetzerDataGridView_6.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Senden")
                {
                    übersetzerDataGridView_6.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
                }
                if (übersetzerDataGridView_6.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Empfangen")
                {
                    übersetzerDataGridView_6.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(204, 255, 153);
                }
            }
            // Clear Cell selection
            übersetzerDataGridView_6.ClearSelection();
        }
        //------Bulgarien
        private void übersetzerDataGridView_8_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (übersetzerDataGridView_8.Columns[e.ColumnIndex].HeaderText == "Transaktion" && übersetzerDataGridView_8.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (übersetzerDataGridView_8.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Senden")
                {
                    übersetzerDataGridView_8.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
                }
                if (übersetzerDataGridView_8.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Empfangen")
                {
                    übersetzerDataGridView_8.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(204, 255, 153);
                }
            }
            // Clear Cell selection
            übersetzerDataGridView_8.ClearSelection();
        }
        //------Russland
        private void übersetzerDataGridView_9_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (übersetzerDataGridView_9.Columns[e.ColumnIndex].HeaderText == "Transaktion" && übersetzerDataGridView_9.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (übersetzerDataGridView_9.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Senden")
                {
                    übersetzerDataGridView_9.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
                }
                if (übersetzerDataGridView_9.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Empfangen")
                {
                    übersetzerDataGridView_9.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(204, 255, 153);
                }
            }
            // Clear Cell selection
            übersetzerDataGridView_9.ClearSelection();
        }
        //------Polen
        private void übersetzerDataGridView_10_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (übersetzerDataGridView_10.Columns[e.ColumnIndex].HeaderText == "Transaktion" && übersetzerDataGridView_10.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (übersetzerDataGridView_10.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Senden")
                {
                    übersetzerDataGridView_10.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
                }
                if (übersetzerDataGridView_10.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Empfangen")
                {
                    übersetzerDataGridView_10.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(204, 255, 153);
                }
            }
            // Clear Cell selection
            übersetzerDataGridView_10.ClearSelection();
        }
        //------Arabian
        private void übersetzerDataGridView_11_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (übersetzerDataGridView_11.Columns[e.ColumnIndex].HeaderText == "Transaktion" && übersetzerDataGridView_11.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (übersetzerDataGridView_11.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Senden")
                {
                    übersetzerDataGridView_11.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
                }
                if (übersetzerDataGridView_11.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Empfangen")
                {
                    übersetzerDataGridView_11.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(204, 255, 153);
                }
            }
            // Clear Cell selection
            übersetzerDataGridView_11.ClearSelection();
        }
        //------Romanein
        private void übersetzerDataGridView_7_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (übersetzerDataGridView_7.Columns[e.ColumnIndex].HeaderText == "Transaktion" && übersetzerDataGridView_7.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            // if the column is bool_badge and check null value for the extra row at dgv
            {
                if (übersetzerDataGridView_7.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Senden")
                {
                    übersetzerDataGridView_7.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 204);
                }
                if (übersetzerDataGridView_7.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Geld Empfangen")
                {
                    übersetzerDataGridView_7.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(204, 255, 153);
                }
            }
            // Clear Cell selection
            übersetzerDataGridView_7.ClearSelection();
        }

        //----------------------------------------end - format color------------------------------------------------------------

        void DateTimeOut()
        {
            int day = DateTime.Now.Day;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            DateTime dateEnd = new DateTime(2017, 05, 30);//change date here
            DateTime dateCurrent = DateTime.Now;

            if (day >= 30 && month >= 5 && year == 2017)//change date here
            {
                MessageBox.Show("Tut mir leid. Die Frist dieser Anwendung erreicht das Ende!");
                Application.Exit();
            }

            TimeSpan ts = dateCurrent - dateEnd;
            int NrOfDays = ts.Days * (-1);

            if (NrOfDays == 0)
            {
                this.txtNrDays.Text = "Heute ist die Frist dieser Anmeldung!!";
                this.txtNrDays_2.Text = "Heute ist die Frist dieser Anmeldung!!";
            }

            string numdays = NrOfDays.ToString();
            this.txtNrDays.Text = numdays + " tages";
            this.txtNrDays_2.Text = numdays + " tages";
        }

    }

}
