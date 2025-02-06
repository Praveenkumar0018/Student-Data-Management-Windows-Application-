CREATE TABLE Students (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    Course NVARCHAR(100) NOT NULL
);
INSERT INTO Students (Name, Age, Course) VALUES 
('Praveen', 21, 'Computer Science'),
('Sai', 22, 'Mechanical Engineering'),
('Shan', 20, 'Electrical Engineering');
