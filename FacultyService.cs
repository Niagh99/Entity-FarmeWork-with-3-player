using DAL.Win;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class FacultyService
    {
        public List<Faculty> GetAll()
        {
            using (StudentModel context = new StudentModel())
            {
                var faculties = context.Faculties.ToList();
                Console.WriteLine($"Số lượng khoa lấy được: {faculties.Count}");
                return faculties;
            }
        }
    }
}
