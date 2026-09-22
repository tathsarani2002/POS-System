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
    public partial class BillingSystem : Form
    {
        SqlConnection conn = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\hitht\OneDrive\Desktop\Projects\22APP6127-POS SYSTEM\POS System\POS System\POSdb.mdf"";Integrated Security=True");

        decimal subTotal = 0;
        decimal grandTotal = 0;
        public BillingSystem()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {



                   
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No items added to the bill.");
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            try
            {
                conn.Open();

                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    int customerID =
                        Convert.ToInt32(comboBox1.SelectedValue);

                    string saleQuery = @"INSERT INTO Sales
                                (SaleDate, CustomerID, TotalAmount)
                                VALUES
                                (@SaleDate, @CustomerID, @TotalAmount);

                                SELECT SCOPE_IDENTITY();";

                    SqlCommand saleCmd =
                        new SqlCommand(
                            saleQuery,
                            conn,
                            transaction);

                    saleCmd.Parameters.AddWithValue(
                        "@SaleDate",
                        dateTimePicker1.Value);

                    saleCmd.Parameters.AddWithValue(
                        "@CustomerID",
                        customerID);

                    saleCmd.Parameters.AddWithValue(
                        "@TotalAmount",
                        grandTotal);

                    int saleID =
                        Convert.ToInt32(saleCmd.ExecuteScalar());

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow)
                            continue;

                        int productID =
                            Convert.ToInt32(
                                row.Cells["ProductID"].Value);

                        int quantity =
                            Convert.ToInt32(
                                row.Cells["Quantity"].Value);

                        decimal unitPrice =
                            Convert.ToDecimal(
                                row.Cells["UnitPrice"].Value);

                        decimal lineTotal =
                            Convert.ToDecimal(
                                row.Cells["LineTotal"].Value);

                        string itemQuery = @"INSERT INTO SaleItems
                                    (SaleID, ProductID, Quantity,
                                     UnitPrice, LineTotal)
                                    VALUES
                                    (@SaleID, @ProductID, @Quantity,
                                     @UnitPrice, @LineTotal)";

                        SqlCommand itemCmd =
                            new SqlCommand(
                                itemQuery,
                                conn,
                                transaction);

                        itemCmd.Parameters.AddWithValue(
                            "@SaleID",
                            saleID);

                        itemCmd.Parameters.AddWithValue(
                            "@ProductID",
                            productID);

                        itemCmd.Parameters.AddWithValue(
                            "@Quantity",
                            quantity);

                        itemCmd.Parameters.AddWithValue(
                            "@UnitPrice",
                            unitPrice);

                        itemCmd.Parameters.AddWithValue(
                            "@LineTotal",
                            lineTotal);

                        itemCmd.ExecuteNonQuery();

                        string stockQuery = @"UPDATE Products
                                      SET StockQuantity =
                                      StockQuantity - @Quantity
                                      WHERE ProductID = @ProductID";

                        SqlCommand stockCmd =
                            new SqlCommand(
                                stockQuery,
                                conn,
                                transaction);

                        stockCmd.Parameters.AddWithValue(
                            "@Quantity",
                            quantity);

                        stockCmd.Parameters.AddWithValue(
                            "@ProductID",
                            productID);

                        stockCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    MessageBox.Show(
                        "Sale completed successfully.\n" +
                        "Sale ID: " + saleID);

                    dataGridView1.Rows.Clear();

                    subTotal = 0;
                    grandTotal = 0;

                    label16.Text = "Rs. 0.00";
                    label17.Text = "Rs. 0.00";

                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;

                    textBox5.Clear();

                    label13.Text = "0.00";
                    label15.Text = "0";
                }
                catch
                {
                    transaction.Rollback();
                    throw;
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

            
            LoadProducts();
        }
        
        

        private void BillingSystem_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;

            label13.Text = "0.00";
            label15.Text = "0";

            label16.Text = "Rs. 0.00";
            label17.Text = "Rs. 0.00";

            LoadCustomers();
            LoadProducts();
            SetupDataGridView();
        }


        private void LoadCustomers()
        {
            try
            {
                conn.Open();

                string query = "SELECT CustomerID, CustomerName FROM Customers";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "CustomerName";
                comboBox1.ValueMember = "CustomerID";

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

        private void LoadProducts()
        {
            try
            {
                conn.Open();

                string query = @"SELECT ProductID, ProductName,
                                UnitPrice, StockQuantity
                         FROM Products
                         WHERE StockQuantity > 0";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "ProductName";
                comboBox2.ValueMember = "ProductID";

                comboBox2.SelectedIndex = -1;
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

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1)
            {
                label13.Text = "0.00";
                label15.Text = "0";
                return;
            }

            DataRowView row = comboBox2.SelectedItem as DataRowView;

            if (row != null)
            {
               label13.Text =
                    Convert.ToDecimal(row["UnitPrice"]).ToString("0.00");

                label15.Text =
                    row["StockQuantity"].ToString();
            }
        }

        private void SetupDataGridView()
        {
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("ProductID", "Product ID");
            dataGridView1.Columns.Add("ProductName", "Product");
            dataGridView1.Columns.Add("Quantity", "Quantity");
            dataGridView1.Columns.Add("UnitPrice", "Unit Price");
            dataGridView1.Columns.Add("LineTotal", "Line Total");

            dataGridView1.Columns["ProductID"].Visible = false;

            dataGridView1.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridView1.MultiSelect = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {


            
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a product.");
                return;
            }

            if (textBox5.Text.Trim() == "")
            {
                MessageBox.Show("Please enter quantity.");
                textBox5.Focus();
                return;
            }

            int quantity;

            if (!int.TryParse(textBox5.Text.Trim(), out quantity))
            {
                MessageBox.Show("Please enter a valid quantity.");
                textBox5.Focus();
                return;
            }

            
            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.");
                textBox5.Focus();
                return;
            }

            int productID =
                Convert.ToInt32(comboBox2.SelectedValue);

            
            int stock = 0;

            try
            {
                conn.Open();

                string stockQuery =
                    "SELECT StockQuantity FROM Products WHERE ProductID = @ProductID";

                SqlCommand stockCmd =
                    new SqlCommand(stockQuery, conn);

                stockCmd.Parameters.AddWithValue(
                    "@ProductID",
                    productID);

                object result = stockCmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    stock = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                conn.Close();
            }

            
            if (stock <= 0)
            {
                MessageBox.Show("This product is out of stock.");
                return;
            }

            if (quantity > stock)
            {
                MessageBox.Show(
                    "Quantity exceeds available stock.\n\n" +
                    "Available stock: " + stock);

                textBox5.Focus();
                return;
            }

            string productName =
                comboBox2.Text;

            decimal unitPrice =
                Convert.ToDecimal(label13.Text);

            decimal lineTotal =
                quantity * unitPrice;

            
            dataGridView1.Rows.Add(
                productID,
                productName,
                quantity,
                unitPrice.ToString(),
                lineTotal.ToString());

            
            subTotal += lineTotal;

            grandTotal = subTotal;

            label16.Text =
                "Rs. " + subTotal.ToString();

            label17.Text =
                "Rs. " + grandTotal.ToString();

            
            textBox5.Clear();

            
            comboBox2.SelectedIndex = -1;

            label13.Text = "";
            label15.Text = "";
        }

        

        private void button11_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item.");
                return;
            }

            DataGridViewRow row =
                dataGridView1.SelectedRows[0];

            decimal lineTotal =
                Convert.ToDecimal(row.Cells["LineTotal"].Value);

            subTotal -= lineTotal;

            grandTotal = subTotal;

            dataGridView1.Rows.Remove(row);

            label16.Text =
                "Rs. " + subTotal.ToString("0.00");

            label17.Text =
                "Rs. " + grandTotal.ToString("0.00");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            LoadCustomers();
            LoadProducts();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CategoryM form = new CategoryM();
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

        private void button1_Click(object sender, EventArgs e)
        {
            Dashboard form = new Dashboard();
            form.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SalesHistory form = new SalesHistory();
            form.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }
    
}
