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
    public partial class CategoryM : Form
    {

        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\hitht\OneDrive\Desktop\Projects\22APP6127-POS SYSTEM\POS System\POS System\POSdb.mdf"";Integrated Security=True");
        public CategoryM()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Please select a category from the table.");
                return;
            }

            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Please enter category name.");
                textBox2.Focus();
                return;
            }

                try
                {

                    conn.Open();

                    string query = @"
            UPDATE Categories
            SET CategoryName = @name,
                Description = @description
            WHERE CategoryID = @id";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue(
                        "@name",
                        textBox2.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@description",
                        textBox3.Text.Trim());

                    cmd.Parameters.AddWithValue(
                        "@id",
                        int.Parse(textBox1.Text));

                    cmd.ExecuteNonQuery();

                    conn.Close();

                    MessageBox.Show("Category updated successfully!");

                    LoadCategories();

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

        

        private void CategoryM_Load(object sender, EventArgs e)
        {
            LoadCategories();
            textBox1.ReadOnly = true;
        }


        private void LoadCategories()
        {
            try
            {
                conn.Open();

                string query = @"
            SELECT CategoryID, CategoryName, Description
            FROM Categories
            ORDER BY CategoryID";

                SqlDataAdapter adapter =
                    new SqlDataAdapter(query, conn);

                DataTable table = new DataTable();

                adapter.Fill(table);

                dataGridView1.DataSource = table;

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
                    "Error loading categories:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Please enter category name.");
                return;
            }

            try
            {
                SqlConnection conn = new SqlConnection(
                    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\hitht\OneDrive\Desktop\Projects\22APP6127-POS SYSTEM\POS System\POS System\POSdb.mdf"";Integrated Security=True");

                conn.Open();

                string query =
                    "INSERT INTO Categories (CategoryName, Description) " +
                    "VALUES (@name, @description)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@name", textBox2.Text);

                cmd.Parameters.AddWithValue(
                    "@description", textBox3.Text);

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("Category added successfully.");

                LoadCategories();

                textBox2.Clear();
                textBox3.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {


              
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Please select a category to delete.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this category?",
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

                string query = "DELETE FROM Categories WHERE CategoryID = @id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@id",
                    int.Parse(textBox1.Text));

                cmd.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show(
                    "Category deleted successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadCategories();

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
            

        private void button3_Click(object sender, EventArgs e)
        {
            ProductM form = new ProductM();
            form.Show();
            this.Hide();
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

        private void button1_Click(object sender, EventArgs e)
        {
            Dashboard form = new Dashboard();
            form.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row =
                    dataGridView1.Rows[e.RowIndex];

                textBox1.Text =
                    row.Cells["CategoryID"].Value.ToString();

                textBox2.Text =
                    row.Cells["CategoryName"].Value.ToString();

                textBox3.Text =
                    row.Cells["Description"].Value.ToString();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(
                    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\hitht\OneDrive\Desktop\22APP6127-POS SYSTEM\POS System\POS System\POSdb.mdf"";Integrated Security=True");

                conn.Open();

                string query = @"
            SELECT CategoryID, CategoryName, Description
            FROM Categories
            WHERE CategoryName LIKE @search";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@search",
                    "%" + textBox4.Text.Trim() + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

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
    }
}
