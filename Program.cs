using System;
using System.Collections.Generic;
using System.IO;

public class Student
{
    private string firstName;
    private string lastName;
    private int age;
    private double averageGrade;

    public Student(string firstName, string lastName, int age, double averageGrade)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.age = age;
        this.averageGrade = averageGrade;
    }

    public string FirstName { get { return firstName; } }
    public string LastName { get { return lastName; } }
    public int Age { get { return age; } }
    public double AverageGrade { get { return averageGrade; } }
}

public class University
{
    private List<Student> students = new List<Student>();

    public void AddStudent(Student student)
    {
        students.Add(student);
    }

    public void RemoveStudent(Student student)
    {
        students.Remove(student);
    }

    public List<Student> GetStudents() {  return students; }
}

namespace DataAccess
{
    public class StudentsRepository
    {
        public static void SaveStudents(string filename, List<Student> students)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), filename);

            using (StreamWriter file = new StreamWriter(path))
            {
                foreach (Student student in students)
                {
                    file.WriteLine($"{student.FirstName},{student.LastName},{student.Age},{student.AverageGrade.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
                }
            }
        }

        public static List<Student> LoadStudents(string filename)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), filename);
            List<Student> students = new List<Student>();

            using (StreamReader file = new StreamReader(path))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    string firstName = data[0];
                    string lastName = data[1];
                    int age = Convert.ToInt32(data[2]);
                    double averageGrade = Convert.ToDouble(data[3], System.Globalization.CultureInfo.InvariantCulture);

                    Student student = new Student(firstName, lastName, age, averageGrade);
                    students.Add(student);
                }
            }

            return students;
        }
    }
}

class Program
{
    static void Main()
    {
        Student student1 = new Student("Иван", "Иванов", 20, 4.8);
        Student student2 = new Student("Петр", "Петров", 21, 4.9);
        Student student3 = new Student("Мария", "Сидорова", 19, 4.8);
        Student student4 = new Student("Александр", "Блок", 40, 3.6);

        University university = new University();

        university.AddStudent(student1);
        university.AddStudent(student2);
        university.AddStudent(student3);
        university.AddStudent(student4);

        DataAccess.StudentsRepository.SaveStudents("students.txt", university.GetStudents());

        List<Student> loadedStudents = DataAccess.StudentsRepository.LoadStudents("students.txt");

        foreach (var student in loadedStudents)
        {
            Console.WriteLine($"Имя: {student.FirstName}\nФамилия: {student.LastName}\nВозраст: {student.Age}\nСредний балл: {student.AverageGrade}\n");
        }
    }
}