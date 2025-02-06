using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace StudentRecords_ADO.NET_with_sql
{
    public partial class Form1 : Form
    {
        // Read the connection string from App.config
        string connectionString = ConfigurationManager.ConnectionStrings["StudentDBConnectionString"].ConnectionString;

        public Form1()
        {
            InitializeComponent();
        }

        // Load Data into DataGridView
        private void LoadData()
        {
            try
            {
                // Clear existing rows and columns
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                // Add columns to the DataGridView
                dataGridView1.Columns.Add("Id", "ID");
                dataGridView1.Columns.Add("Name", "Name");
                dataGridView1.Columns.Add("Age", "Age");
                dataGridView1.Columns.Add("Course", "Course");

                // Load data from the database
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand("SELECT Id, Name, Age, Course FROM Students", con))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(reader["Id"], reader["Name"], reader["Age"], reader["Course"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        // Save new record
        private void Savebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtName.Text) || string.IsNullOrWhiteSpace(TxtAge.Text) || string.IsNullOrWhiteSpace(TxtCourse.Text))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                if (!int.TryParse(TxtAge.Text, out int age))
                {
                    MessageBox.Show("Age must be a valid number.");
                    return;
                }

                // Save the student record
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand("INSERT INTO Students (Name, Age, Course) VALUES (@Name, @Age, @Course)", con))
                    {
                        cmd.Parameters.AddWithValue("@Name", TxtName.Text);
                        cmd.Parameters.AddWithValue("@Age", age);
                        cmd.Parameters.AddWithValue("@Course", TxtCourse.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Refresh DataGridView to reflect changes
                LoadData();
                MessageBox.Show("Record saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving record: {ex.Message}");
            }
        }

        // Update existing record
        private void Updatebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a student to update.");
                    return;
                }

                if (!int.TryParse(TxtAge.Text, out int age))
                {
                    MessageBox.Show("Age must be a valid number.");
                    return;
                }

                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand("UPDATE Students SET Name = @Name, Age = @Age, Course = @Course WHERE Id = @Id", con))
                    {
                        cmd.Parameters.AddWithValue("@Name", TxtName.Text);
                        cmd.Parameters.AddWithValue("@Age", age);
                        cmd.Parameters.AddWithValue("@Course", TxtCourse.Text);
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Refresh DataGridView to reflect changes
                LoadData();
                MessageBox.Show("Record updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating record: {ex.Message}");
            }
        }

        // Delete existing record
        private void Deletebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a student to delete.");
                    return;
                }

                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand("DELETE FROM Students WHERE Id = @Id", con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Refresh DataGridView to reflect changes
                LoadData();
                MessageBox.Show("Record deleted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting record: {ex.Message}");
            }
        }

        // DataGridView Cell Click event to load data into Textboxes
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dataGridView1.Rows[e.RowIndex];

                    // Access columns by name instead of index
                    TxtName.Text = row.Cells["Name"].Value.ToString();
                    TxtAge.Text = row.Cells["Age"].Value.ToString();
                    TxtCourse.Text = row.Cells["Course"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data into textboxes: {ex.Message}");
            }
        }

        // Load Data button click
        private void LoadDatabtn_Click(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Empty event handler
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Empty event handler
        }
    }
}
