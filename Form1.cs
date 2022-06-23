using Npgsql;
using System.Data;
using System.Diagnostics;
namespace KosKu
{
    public partial class Form1 : Form
    {
        String connStr = "Host=localhost;Port=5433;Database=kosku;Username=admin;Password=adminadmin;";
        int selectedRoomId = 0;
        int selectedPenghuniId = 0;
        int selectedPemesananId = 0;
        public Form1()
        {
            InitializeComponent();
            bindData();
            loadComboboxKamarItem();
            loadComboboxPenghuniItem();
        }

        void bindData()
        {
            
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "select id_kamar, nama_kamar, harga, keterangan, fasilitas from kamar join status using(id_status)";
                    cmd.CommandType = CommandType.Text;
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    
                    dataGridView1.DataSource = dt;
                    
                    cmd.Dispose();
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            ///////////////     TABEL PENGHUNI  //////////
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "select id_penghuni, nama_lengkap, alamat, nik, no_hp from penghuni";
                    cmd.CommandType = CommandType.Text;
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView_Penghuni.DataSource = dt;

                    cmd.Dispose();
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                label10.Text = ex.Message;
            }

            ////TABEL PEMESANAN///

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "select id_pemesanan, nama_kamar, nama_lengkap, tanggal_pemesanan, nominal, keterangan from pemesanan join status using (id_status)join kamar using (id_kamar) join penghuni using (id_penghuni) ";
                    cmd.CommandType = CommandType.Text;
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView2.DataSource = dt;

                    cmd.Dispose();
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
            private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "insert into kamar (nama_kamar, harga, id_status, fasilitas) values(@nama_kamar, @harga, @id_status, @fasilitas)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@nama_kamar", textBox1.Text));
                    cmd.Parameters.Add(new NpgsqlParameter("@harga", Convert.ToInt64(textBox2.Text)));
                    cmd.Parameters.Add(new NpgsqlParameter("@id_status", Convert.ToInt32(comboBox1.SelectedIndex + 1)));
                    cmd.Parameters.Add(new NpgsqlParameter("@fasilitas", richTextBox1.Text));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                label4.Text = ex.Message;
            }
            loadComboboxKamarItem();
            bindData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            String nama = row.Cells[1].Value.ToString();
            String harga = row.Cells[2].Value.ToString();
            String status = row.Cells[3].Value.ToString();
            status = status[0].ToString().ToUpper() + status.Substring(1);
            String fasilitas = row.Cells[4].Value.ToString();

            this.selectedRoomId = Convert.ToInt32(row.Cells[0].Value.ToString());
            comboBox1.SelectedItem = status;
            textBox1.Text = nama;
            textBox2.Text = harga;
            richTextBox1.Text = fasilitas;
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string idStatus = comboBox1.SelectedIndex.ToString();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "update kamar set nama_kamar=@nama_kamar, harga=@harga, id_status=@id_status, fasilitas=@fasilitas where id_kamar=@id_kamar";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@nama_kamar", textBox1.Text));
                    cmd.Parameters.Add(new NpgsqlParameter("@harga", Convert.ToInt64(textBox2.Text)));
                    cmd.Parameters.Add(new NpgsqlParameter("@id_status", Convert.ToInt32(comboBox1.SelectedIndex + 1)));
                    cmd.Parameters.Add(new NpgsqlParameter("@fasilitas", richTextBox1.Text));
                    cmd.Parameters.Add(new NpgsqlParameter("@id_kamar", this.selectedRoomId));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            loadComboboxKamarItem();
            bindData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "delete from kamar where id_kamar=@id_kamar";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@id_kamar", this.selectedRoomId));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                }

                textBox1.Text = "";
                textBox2.Text = "";
                richTextBox1.Text = "";
                
                comboBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            loadComboboxKamarItem();
            bindData();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string idStatus = comboBox3.SelectedIndex.ToString();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "update pemesanan set id_kamar=@id_kamar, id_penghuni=@id_penghuni, tanggal_pemesanan=@tanggal, nominal=@nominal, id_status=@id_status where id_pemesanan=@id_pemesanan";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@id_kamar", Convert.ToInt32(comboBox2.SelectedValue)));
                    cmd.Parameters.Add(new NpgsqlParameter("@id_penghuni", Convert.ToInt32(comboBox4.SelectedValue)));
                    cmd.Parameters.Add(new NpgsqlParameter("@tanggal", dateTimePicker1.Value));
                    cmd.Parameters.Add(new NpgsqlParameter("@nominal", Convert.ToInt64(textBox5.Text)));
                    cmd.Parameters.Add(new NpgsqlParameter("@id_status", Convert.ToInt32(comboBox3.SelectedIndex + 3)));
                    cmd.Parameters.Add(new NpgsqlParameter("@id_pemesanan", this.selectedPemesananId));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                label12.Text = ex.Message;
            }
            bindData();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
            String nama_kamar  = Convert.ToString(row.Cells[1].FormattedValue.ToString());
            String nama_penghuni = row.Cells[2].Value.ToString();
            DateTime tanggal = DateTime.Parse(row.Cells[3].Value.ToString());
            String nominal = row.Cells[4].Value.ToString();
            String status = row.Cells[5].Value.ToString();
            status = status[0].ToString().ToUpper() + status.Substring(1);
            
            this.selectedPemesananId = Convert.ToInt32(row.Cells[0].Value.ToString());
            dateTimePicker1.Value = tanggal;
            comboBox3.SelectedItem = status;
            comboBox2.SelectedIndex = comboBox2.FindStringExact(nama_kamar);
            comboBox4.SelectedIndex = comboBox4.FindStringExact(nama_penghuni);
            textBox5.Text = nominal;
        }

        private void button9_Click(object sender, EventArgs e)
        {

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "insert into pemesanan (id_kamar, id_penghuni, tanggal_pemesanan, nominal, id_status) values(@id_kamar, @id_penghuni, @tanggal, @nominal, @id_status)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@id_kamar", Convert.ToInt32(comboBox2.SelectedValue)));
                    cmd.Parameters.Add(new NpgsqlParameter("@id_penghuni", Convert.ToInt32(comboBox4.SelectedValue)));
                    cmd.Parameters.Add(new NpgsqlParameter("@tanggal", dateTimePicker1.Value));
                    cmd.Parameters.Add(new NpgsqlParameter("@nominal", Convert.ToInt64(textBox5.Text)));
                    cmd.Parameters.Add(new NpgsqlParameter("@id_status", Convert.ToInt32(comboBox3.SelectedIndex + 3)));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                label12.Text = ex.Message;
            }
            bindData();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "delete from pemesanan where id_pemesanan=@id_pemesanan";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@id_pemesanan", this.selectedPemesananId));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                }
                comboBox2.SelectedIndex = -1;
                comboBox4.SelectedIndex = -1;
                textBox5.Text = "";
                comboBox3.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                label10.Text = ex.Message;
            }
            bindData();
        }

        private void textBox_NamaPenghuni_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_TambahPenghuni_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "insert into penghuni (nama_lengkap, alamat, nik, no_hp) values(@nama_lengkap, @alamat, @nik, @no_hp)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@nama_lengkap", textBox_NamaPenghuni.Text));
                    cmd.Parameters.Add(new NpgsqlParameter("@alamat", textBox_AlamatPenghuni.Text));
                    cmd.Parameters.Add(new NpgsqlParameter("@nik", Convert.ToInt64(textBox_NikPenghuni.Text)));
                    cmd.Parameters.Add(new NpgsqlParameter("@no_hp", Convert.ToInt64(textBox_NoHpPenghuni.Text)));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                label10.Text = ex.Message;
            }
            loadComboboxPenghuniItem();
            bindData();
        }

        private void dataGridView_Penghuni_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView_Penghuni.Rows[e.RowIndex];
            String nama_lengkap = row.Cells[1].Value.ToString();
            String alamat = row.Cells[2].Value.ToString();
            String nik = row.Cells[3].Value.ToString();
            String no_hp = row.Cells[4].Value.ToString();

            this.selectedPenghuniId = Convert.ToInt32(row.Cells[0].Value.ToString());
            textBox_NamaPenghuni.Text = nama_lengkap;
            textBox_AlamatPenghuni.Text = alamat;
            textBox_NikPenghuni.Text = nik;
            textBox_NoHpPenghuni.Text = no_hp;
        }

        private void btn_UbahPenghuni_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "update penghuni set nama_lengkap=@nama_lengkap, alamat=@alamat, nik=@nik, no_hp=@no_hp where id_penghuni=@id_penghuni";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@nama_lengkap", textBox_NamaPenghuni.Text));
                    cmd.Parameters.Add(new NpgsqlParameter("@alamat", textBox_AlamatPenghuni.Text));
                    cmd.Parameters.Add(new NpgsqlParameter("@nik", Convert.ToInt64(textBox_NikPenghuni.Text)));
                    cmd.Parameters.Add(new NpgsqlParameter("@no_hp", Convert.ToInt32(textBox_NoHpPenghuni.Text)));
                    cmd.Parameters.Add(new NpgsqlParameter("@id_penghuni", this.selectedPenghuniId));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                label10.Text = ex.Message;
            }
            loadComboboxPenghuniItem();
            bindData();
        }

        private void btn_HapusPenghuni_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "delete from penghuni where id_penghuni=@id_penghuni";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@id_penghuni", this.selectedPenghuniId));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                }

                textBox_NamaPenghuni.Text = "";
                textBox_AlamatPenghuni.Text = "";
                textBox_NikPenghuni.Text = "";
                textBox_NoHpPenghuni.Text = "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                label10.Text = ex.Message;
            }
            loadComboboxPenghuniItem();
            bindData();
        }

        private void loadComboboxKamarItem()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "select * from kamar";
                    cmd.CommandType = CommandType.Text;
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    comboBox2.DisplayMember = "nama_kamar";
                    comboBox2.ValueMember = "id_kamar";
                    comboBox2.DataSource = dt;
                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void loadComboboxPenghuniItem()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection())
                {
                    connection.ConnectionString = connStr;
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "select * from penghuni";
                    cmd.CommandType = CommandType.Text;
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    connection.Close();
                    comboBox4.DataSource = ds.Tables[0];
                    comboBox4.DisplayMember = "nama_lengkap";
                    
                    comboBox4.ValueMember = "id_penghuni";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label12.Text = comboBox2.SelectedItem.ToString();
        }
    } 
}