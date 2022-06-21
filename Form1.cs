using Npgsql;
using System.Data;
using System.Diagnostics;
namespace KosKu
{
    public partial class Form1 : Form
    {
        String connStr = "Host=localhost;Port=5432;Database=kosku;Username=postgres;Password=fadil071100;";
        int selectedRoomId = 0;
        public Form1()
        {
            InitializeComponent();
            bindData();
            
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

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    } 
}