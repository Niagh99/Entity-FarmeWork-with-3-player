using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using DAL.Win;

namespace BUS
{
    public class StudentService
    {
        private readonly StudentModel context = new StudentModel();

        // Lấy tất cả sinh viên
        public List<Student> GetAll()
        {
            return context.Students.ToList();
        }

        // Lấy sinh viên chưa đăng ký chuyên ngành (MajorID == null)
        public List<Student> GetAllHasNoMajor()
        {
            return context.Students.Where(p => p.MajorID == null).ToList();
        }

        // Lấy sinh viên chưa đăng ký chuyên ngành và thuộc khoa cụ thể
        public List<Student> GetAllHasNoMajor(int facultyID)
        {
            return context.Students.Where(p => p.MajorID == null && p.FacultyID == facultyID).ToList();
        }

        // Tìm sinh viên theo ID
        public Student FindById(int studentid)
        {
            return context.Students.FirstOrDefault(p => p.StudentID == studentid);
        }

        // Thêm hoặc cập nhật sinh viên
        public void InserUpdate(Student s)
        {
            context.Students.AddOrUpdate(s);
            context.SaveChanges();
        }

        // Xóa sinh viên
        public void Delete(int studentID)
        {
            var student = context.Students.Find(studentID);
            if (student != null)
            {
                context.Students.Remove(student);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Không tìm thấy sinh viên để xóa.");
            }
        }
    }
}
