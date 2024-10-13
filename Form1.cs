using BUS;
using DAL.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form1 : Form

    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
      //  private readonly MajorService majorService = new MajorService();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dtgStudent);
                dtgStudent.Columns.Clear();
                dtgStudent.Columns.Add("StudentID", "Student ID");
                dtgStudent.Columns.Add("FullName", "Full Name");
                dtgStudent.Columns.Add("FacultyName", "Faculty");
                dtgStudent.Columns.Add("AverageScore", "ĐTB");
                dtgStudent.Columns.Add("MajorName", "Major");

                var listFacultys = facultyService.GetAll();
                //var listMajors = majorService.GetAll(); 
                var listStudents = studentService.GetAll();

                FillFacultyCombobox(listFacultys);
               // FillMajorCombobox(listMajors); 
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        private void FillFacultyCombobox(List<Faculty> listFacultys)
        {
            if (listFacultys != null && listFacultys.Count > 0)
            {
                // In ra số lượng khoa trước khi thêm vào ComboBox
                Console.WriteLine($"Số lượng khoa trước khi thêm vào ComboBox: {listFacultys.Count}");

                listFacultys.Insert(0, new Faculty { FacultyName = "Chọn khoa", FacultyID = 0 });
                this.CBKhoa.DataSource = listFacultys;
                this.CBKhoa.DisplayMember = "FacultyName";
                this.CBKhoa.ValueMember = "FacultyID";
            }
            else
            {
                MessageBox.Show("Không có dữ liệu khoa nào để hiển thị.");
            }
        }

        private void BindGrid(List<Student> listStudent)
        {
            dtgStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dtgStudent.Rows.Add();
                dtgStudent.Rows[index].Cells[0].Value = item.StudentID;
                dtgStudent.Rows[index].Cells[1].Value = item.FullName;

                if (item.Faculty != null)
                {
                    dtgStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                }

                dtgStudent.Rows[index].Cells[3].Value = item.ĐTB.ToString();

                if (item.Major != null)
                {
                    dtgStudent.Rows[index].Cells[4].Value = item.Major.MajorName;
                }
            }
        }

        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void dtgStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dtgStudent.Rows.Count)
            {
                var row = dtgStudent.Rows[e.RowIndex];

                if (row.Cells.Count > 0)
                {
                    txtID.Text = row.Cells[0].Value.ToString();
                    txtName.Text = row.Cells[1].Value.ToString();
                    if (row.Cells.Count > 4 && int.TryParse(row.Cells[2].Value.ToString(), out int facultyId))
                    {
                        CBKhoa.SelectedValue = facultyId;
                    }

                    txtDiem.Text = row.Cells[3].Value.ToString();
                }
            }
        }



        private void btnAdd_up_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtID.Text.Trim(), out int studentID))
                {
                    MessageBox.Show("Student ID không hợp lệ.");
                    return;
                }

                string fullName = txtName.Text.Trim();

                if (!double.TryParse(txtDiem.Text.Trim(), out double dtb))
                {
                    MessageBox.Show("Điểm trung bình không hợp lệ.");
                    return;
                }

                if (!(CBKhoa.SelectedValue is int facultyID))
                {
                    MessageBox.Show("Khoa không hợp lệ.");
                    return;
                }

               

                Student newStudent = new Student
                {
                    StudentID = studentID,
                    FullName = fullName,
                    ĐTB = dtb,
                    FacultyID = facultyID,
                    
                };

                studentService.InserUpdate(newStudent);

                BindGrid(studentService.GetAll());

                MessageBox.Show("Thêm sinh viên thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtID.Text.Trim(), out int studentID))
                {
                    MessageBox.Show("Vui lòng chọn sinh viên để xóa.");
                    return;
                }

                DialogResult confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?",
                                                             "Xác nhận xóa",
                                                             MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    studentService.Delete(studentID);

                    BindGrid(studentService.GetAll());

                    MessageBox.Show("Xóa sinh viên thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        private void CheckMajor_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = new List<Student>();
            if (this.CheckMajor.Checked)
                listStudents = studentService.GetAllHasNoMajor();
            else
                listStudents = studentService.GetAll();
            BindGrid(listStudents);
        }
    }
}
