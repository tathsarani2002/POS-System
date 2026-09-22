using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_System
{
    public partial class SalesHistory : Form
    {
        SqlConnection conn = new SqlConnection(
    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\hitht\OneDrive\Desktop\Projects\22APP6127-POS SYSTEM\POS System\POS System\POSdb.mdf"";Integrated Security=True");
        public SalesHistory()
        {
            InitializeComponent();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SalesHistory_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Short;

            SetupDataGridView();

            LoadSales();
        }
        private void SetupDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridView1.MultiSelect = false;

            dataGridView1.ReadOnly = true;

            dataGridView1.AllowUserToAddRows = false;
        }

        private void LoadSales()
        {
            try
            {
                conn.Open();

                string query = @"SELECT
                            s.SaleID,
                            s.SaleDate,
                            c.CustomerName,
                            s.TotalAmount
                         FROM Sales s
                         INNER JOIN Customers c
                         ON s.CustomerID = c.CustomerID
                         ORDER BY s.SaleID DESC";

                SqlDataAdapter da =
                    new SqlDataAdapter(query, conn);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns["SaleID"]
                    .HeaderText = "Bill Number";

                dataGridView1.Columns["SaleDate"]
                    .HeaderText = "Date";

                dataGridView1.Columns["CustomerName"]
                    .HeaderText = "Customer";

                dataGridView1.Columns["TotalAmount"]
                    .HeaderText = "Total Amount";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }





        private void button11_Click(object sender, EventArgs e)
        {
            
            
            if (textBox1.Text.Trim() == "" && !checkBox1.Checked)
            {
                MessageBox.Show(
                    "Please enter Bill Number or select Search by Date.");

                return;
            }

            try
            {
                conn.Open();

                string query = @"SELECT
                            s.SaleID,
                            s.SaleDate,
                            c.CustomerName,
                            s.TotalAmount
                         FROM Sales s
                         INNER JOIN Customers c
                         ON s.CustomerID = c.CustomerID
                         WHERE 1 = 1";

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;

                
                if (textBox1.Text.Trim() != "")
                {
                    int saleID;

                    if (!int.TryParse(textBox1.Text.Trim(), out saleID))
                    {
                        MessageBox.Show(
                            "Please enter a valid Bill Number.");

                        return;
                    }

                    query += " AND s.SaleID = @SaleID";

                    cmd.Parameters.AddWithValue(
                        "@SaleID",
                        saleID);
                }

                
                if (checkBox1.Checked)
                {
                    query +=
                        " AND CAST(s.SaleDate AS DATE) = @SaleDate";

                    cmd.Parameters.AddWithValue(
                        "@SaleDate",
                        dateTimePicker1.Value.Date);
                }

                query += " ORDER BY s.SaleID DESC";

                cmd.CommandText = query;

                SqlDataAdapter da =
                    new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No sales found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        
           
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.Clear();

            checkBox1.Checked = false;

            dateTimePicker1.Value = DateTime.Now;

            LoadSales();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a bill.");
                return;
            }

            int saleID = Convert.ToInt32(
                dataGridView1.SelectedRows[0]
                .Cells["SaleID"].Value);

            try
            {
                conn.Open();

                string query = @"SELECT
                            p.ProductName,
                            si.Quantity,
                            si.UnitPrice,
                            si.LineTotal
                         FROM SaleItems si
                         INNER JOIN Products p
                         ON si.ProductID = p.ProductID
                         WHERE si.SaleID = @SaleID";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@SaleID",
                    saleID);

                SqlDataReader reader =
                    cmd.ExecuteReader();

                string details = "";

                while (reader.Read())
                {
                    details +=
                        "Product: " +
                        reader["ProductName"].ToString() +
                        "\n";

                    details +=
                        "Quantity: " +
                        reader["Quantity"].ToString() +
                        "\n";

                    details +=
                        "Unit Price: Rs. " +
                        Convert.ToDecimal(
                            reader["UnitPrice"])
                        .ToString("0.00") +
                        "\n";

                    details +=
                        "Line Total: Rs. " +
                        Convert.ToDecimal(
                            reader["LineTotal"])
                        .ToString("0.00") +
                        "\n\n";
                }

                reader.Close();

                if (details == "")
                {
                    MessageBox.Show("No sale details found.");
                }
                else
                {
                    MessageBox.Show(
                        "Bill Number: " +
                        saleID +
                        "\n\n" +
                        details,
                        "Sale Details");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CategoryM form = new CategoryM();
            form.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dashboard form = new Dashboard();
            form.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
               "Do you want to Exit?",
               "Exit",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show(
                    "Thank you for using the POS System.",
                    "POS System",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Application.Exit();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BillingSystem form = new BillingSystem();
            form.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CustomersM form = new CustomersM();
            form.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProductM form = new ProductM();
            form.Show();
            this.Hide();
        }
    }
}
