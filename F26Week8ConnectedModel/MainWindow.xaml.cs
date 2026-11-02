using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;


namespace F26Week8ConnectedModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // read the conn str from App.config
        string connStr = ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadEmployees()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "select * from Employees";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                DataTable tbl = new DataTable();
                tbl.Load(reader);
                grdEmployees.ItemsSource = tbl.DefaultView;
            }
            //conn.Close();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadEmployees();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // paramterized query - GOOD PRACTICE
                string query = "select * from Employees where FirstName = @fn";

                // string concatenation - BAD PRACTICE
                //string query = "select * from Employees where FirstName = '" + txtFirstname.Text + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("fn", txtFirstname.Text);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                DataTable tbl = new DataTable();
                tbl.Load(reader);
                grdEmployees.ItemsSource = tbl.DefaultView;
            }
        }

        // ExecuteScalar example
        private void btnCount_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "select Count(*) from Employees";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();

                int numOfRows = (int)cmd.ExecuteScalar();

                MessageBox.Show("Total rows = " + numOfRows);
            }
        }

        // ExecuteNonQuery example
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "insert into Employees (FirstName, LastName) values (@fn, @ln)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("fn", txtFirstname.Text);
                cmd.Parameters.AddWithValue("ln", txtLastname.Text);

                conn.Open();

                int result = cmd.ExecuteNonQuery();

                if (result == 1)
                {
                    LoadEmployees();
                    MessageBox.Show("New employee added");
                }
                else
                    MessageBox.Show("New employee not added");
            }
        }
    }
}