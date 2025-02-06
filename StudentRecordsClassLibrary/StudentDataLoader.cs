using System;
using System.Collections.Generic;
using System.Configuration; // Add this namespace for reading the config
using System.Data.SqlClient;

namespace StudentRecordsClassLibrary
{
    public class StudentDataLoader
    {
        // Read connection string from App.config
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentDbConnection"].ConnectionString;

        // Load all students
        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT Id, Name, Age, Course FROM Students", con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Course = reader["Course"].ToString()
                        });
                    }
                }
            }
            return students;
        }

        // Add a new student
        public void AddStudent(string name, int age, string course)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("INSERT INTO Students (Name, Age, Course) VALUES (@Name, @Age, @Course)", con))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Course", course);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Update an existing student
        public void UpdateStudent(int id, string name, int age, string course)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("UPDATE Students SET Name = @Name, Age = @Age, Course = @Course WHERE Id = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Course", course);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Delete a student
        public void DeleteStudent(int id)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("DELETE FROM Students WHERE Id = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    // Student model class to represent student data
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Course { get; set; }
    }
}
