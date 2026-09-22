using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace POS_System
{
    public partial class LoginForm : Form
    {   SqlConnection conn =new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\hitht\OneDrive\Desktop\Projects\22APP6127-POS SYSTEM\POS System\POS System\POSdb.mdf"";Integrated Security=True");
        public LoginForm()
        {
            InitializeComponent();
        }
        

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        
            if (textBox1.Text.Trim() == "" ||
                textBox2.Text.Trim() == "")
            {
                MessageBox.Show(
                    "Please enter username and password.",
                    "Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (textBox1.Text.Trim() == "admin" &&
                textBox2.Text.Trim() == "1234")
            {
                MessageBox.Show(
                    "Login Successful",
                    "Login Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Dashboard dashboard = new Dashboard();

                dashboard.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show(
                    "Invalid username or password.",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                textBox2.Clear();
                textBox2.Focus();
            }
        }
        

        private void PnlRight_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
