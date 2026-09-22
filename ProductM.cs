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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace POS_System
{
    public partial class ProductM : Form
    {

        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\hitht\OneDrive\Desktop\Projects\22APP6127-POS SYSTEM\POS System\POS System\POSdb.mdf"";Integrated Security=True");
        public ProductM()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void ProductM_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadCategories();
        }

        private void LoadProducts()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                conn.Open();

                string query = @"
            SELECT ProductID,
                   ProductName,
                   CategoryID,
                   UnitPrice,
                   StockQuantity
            FROM Products
            ORDER BY ProductID";

                SqlDataAdapter da =
                    new SqlDataAdapter(query, conn);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dataGridView1.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                dataGridView1.MultiSelect = false;

                dataGridView1.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading products:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void LoadCategories()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                conn.Open();

                string query = @"
            SELECT CategoryID, CategoryName
            FROM Categories
            ORDER BY CategoryName";

                SqlDataAdapter da =
                    new SqlDataAdapter(query, conn);

                DataTable dt = new DataTable();

                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "CategoryName";
                comboBox1.ValueMember = "CategoryID";
                comboBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading categories:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Please enter product name.");
                textBox2.Focus();
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category.");
                comboBox1.Focus();
                return;
            }

            if (textBox5.Text.Trim() == "")
            {
                MessageBox.Show("Please enter unit price.");
                textBox5.Focus();
                return;
            }

            if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("Please enter stock quantity.");
                textBox3.Focus();
                return;
            }

            decimal unitPrice;
            int stock;

            if (!decimal.TryParse(textBox5.Text.Trim(), out unitPrice))
            {
                MessageBox.Show("Please enter a valid unit price.");
                textBox5.Focus();
                return;
            }

            if (unitPrice <= 0)
            {
                MessageBox.Show("Unit price must be greater than zero.");
                textBox5.Focus();
                return;
            }

            if (!int.TryParse(textBox3.Text.Trim(), out stock))
            {
                MessageBox.Show("Please enter a valid stock quantity.");
                textBox3.Focus();
                return;
            }

            if (stock < 0)
            {
                MessageBox.Show("Stock quantity cannot be negative.");
                textBox3.Focus();
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                conn.Open();

                string query = @"
            INSERT INTO Products
            (ProductName, CategoryID, UnitPrice, StockQuantity)
            VALUES
            (@ProductName, @CategoryID, @UnitPrice, @StockQuantity)";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@ProductName",
                    textBox2.Text.Trim());

                cmd.Parameters.AddWithValue(
                    "@CategoryID",
                    comboBox1.SelectedValue);

                cmd.Parameters.AddWithValue(
                    "@UnitPrice",
                    unitPrice);

                cmd.Parameters.AddWithValue(
                    "@StockQuantity",
                    stock);

                cmd.ExecuteNonQuery();

                MessageBox.Show(
                    "Product added successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            
            LoadProducts();
        }


        private void button9_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Please select a product from the table.");
                return;
            }

            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Please enter product name.");
                textBox2.Focus();
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category.");
                comboBox1.Focus();
                return;
            }

            decimal unitPrice;
            int stock;

            if (!decimal.TryParse(textBox5.Text.Trim(), out unitPrice))
            {
                MessageBox.Show("Please enter a valid unit price.");
                textBox5.Focus();
                return;
            }

            if (unitPrice <= 0)
            {
                MessageBox.Show("Unit price must be greater than zero.");
                textBox5.Focus();
                return;
            }

            if (!int.TryParse(textBox3.Text.Trim(), out stock))
            {
                MessageBox.Show("Please enter a valid stock quantity.");
                textBox3.Focus();
                return;
            }

            if (stock < 0)
            {
                MessageBox.Show("Stock quantity cannot be negative.");
                textBox3.Focus();
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                conn.Open();

                string query = @"
            UPDATE Products
            SET ProductName = @ProductName,
                CategoryID = @CategoryID,
                UnitPrice = @UnitPrice,
                StockQuantity = @StockQuantity
            WHERE ProductID = @ProductID";

                SqlCommand cmd =
                    new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@ProductID",
                    int.Parse(textBox1.Text));

                cmd.Parameters.AddWithValue(
                    "@ProductName",
                    textBox2.Text.Trim());

                cmd.Parameters.AddWithValue(
                    "@CategoryID",
                    comboBox1.SelectedValue);

                cmd.Parameters.AddWithValue(
                    "@UnitPrice",
                    unitPrice);

                cmd.Parameters.AddWithValue(
                    "@StockQuantity",
                    stock);

                cmd.ExecuteNonQuery();

                MessageBox.Show(
                    "Product updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

           
            LoadProducts();
        }
        

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this product?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            try
            {
                conn.Open();

                string query = "DELETE FROM Products WHERE ProductID = @ProductID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductID", textBox1.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Product deleted successfully.");

                LoadProducts();

                textBox1.Clear();
                textBox2.Clear();
                textBox5.Clear();
                textBox3.Clear();
                comboBox1.SelectedIndex = -1;
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
            LoadProducts();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                MessageBox.Show("Please enter a product name to search.");
                return;
            }

            try
            {
                conn.Open();

                string query = @"SELECT ProductID, ProductName, CategoryID, UnitPrice, StockQuantity
                         FROM Products
                         WHERE ProductName LIKE @Search";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);

                da.SelectCommand.Parameters.AddWithValue(
                    "@Search", "%" + textBox4.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No product found.");
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["ProductID"].Value.ToString();
                textBox2.Text = row.Cells["ProductName"].Value.ToString();
                textBox5.Text = row.Cells["UnitPrice"].Value.ToString();
               textBox3.Text = row.Cells["StockQuantity"].Value.ToString();

                comboBox1.SelectedValue = row.Cells["CategoryID"].Value;
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

        private void button4_Click(object sender, EventArgs e)
        {
            CustomersM form = new CustomersM();
            form.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BillingSystem form = new BillingSystem();
            form.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SalesHistory form = new SalesHistory();
            form.Show();
            this.Hide();
        }
    }
}
