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
    public partial class CustomersM : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\hitht\OneDrive\Desktop\Projects\22APP6127-POS SYSTEM\POS System\POS System\POSdb.mdf"";Integrated Security=True");
        

        public CustomersM()
        {
            InitializeComponent();
        }

        private void CustomersM_Load(object sender, EventArgs e)
        {
            LoadCustomers();

            textBox1.ReadOnly = true;
        }
        private void LoadCustomers()
        {

            try
            {
                conn.Open();

                string query = "SELECT CustomerID, CustomerName, ContactNumber, Address FROM Customers";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
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

private void button8_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Customer name is required.");
                textBox2.Focus();
                return;
            }

            if (textBox5.Text.Trim() == "")
            {
                MessageBox.Show("Contact number is required.");
                textBox5.Focus();
                return;
            }

            if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("Address is required.");
                textBox3.Focus();
                return;
            }

            try
            {
                conn.Open();

                string query = @"INSERT INTO Customers
                         (CustomerName, ContactNumber, Address)
                         VALUES
                         (@CustomerName, @ContactNumber, @Address)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CustomerName", textBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@ContactNumber", textBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", textBox3.Text.Trim());

                cmd.ExecuteNonQuery();

                MessageBox.Show("Customer added successfully.");

                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            LoadCustomers();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textBox1.Text = row.Cells["CustomerID"].Value.ToString();
                textBox2.Text = row.Cells["CustomerName"].Value.ToString();
               textBox5.Text = row.Cells["ContactNumber"].Value.ToString();
                textBox3.Text = row.Cells["Address"].Value.ToString();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Customer name is required.");
                return;
            }

            if (textBox5.Text.Trim() == "")
            {
                MessageBox.Show("Contact number is required.");
                return;
            }

            if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("Address is required.");
                return;
            }

            try
            {
                conn.Open();

                string query = @"UPDATE Customers
                         SET CustomerName = @CustomerName,
                             ContactNumber = @ContactNumber,
                             Address = @Address
                         WHERE CustomerID = @CustomerID";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CustomerID", textBox1.Text);
                cmd.Parameters.AddWithValue("@CustomerName", textBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@ContactNumber", textBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", textBox3.Text.Trim());

                cmd.ExecuteNonQuery();

                MessageBox.Show("Customer updated successfully.");

                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            LoadCustomers();
        }

        private void button10_Click(object sender, EventArgs e)
        {
           
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this customer?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    conn.Open();

                    string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerID", textBox1.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Customer deleted successfully.");

                   
                    
                }
                catch (Exception ex)
                {


                   
                    MessageBox.Show(
                        "This customer cannot be deleted because sales records exist for this customer.",
                        "Cannot Delete",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }

         }


    

        private void button12_Click(object sender, EventArgs e)
        {
           
            try
            {
                conn.Open();

                string query = @"SELECT CustomerID, CustomerName, ContactNumber, Address
                         FROM Customers
                         WHERE CustomerID LIKE @Search
                         OR CustomerName LIKE @Search
                         OR ContactNumber LIKE @Search";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);

                da.SelectCommand.Parameters.AddWithValue(
                    "@Search", "%" + textBox4.Text.Trim() + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
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
            LoadCustomers();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CategoryM form = new CategoryM();
            form.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProductM form = new ProductM();
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

        private void button1_Click(object sender, EventArgs e)
        {

            Dashboard form = new Dashboard();
            form.Show();
            this.Hide();
        }
    }
}
