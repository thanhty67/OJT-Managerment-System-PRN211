using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library.Models;
using Library.Repository;

namespace WinFormsApplication
{
    public partial class FrmStudentEditProfile : Form
    {
        public IRepositoryTblStudent RepositoryTblStudent { get; set; }
        public IRepositoryTblAccount RepositoryTblAccount { get; set; }
        public TblStudent StudentInfo { get; set; }
        public TblAccount AccountInfo { get; set; }
        public TblSemester SemesterInfo { get; set; }

        public FrmStudentProfile parentForm { get; set; }
        private bool isAuto { get; set; } = true;
        private bool isClose { get; set; } = true;
        public FrmStudentEditProfile()
        {
            InitializeComponent();
        }

        //Method: Khi nhấn nút cancel thì tắt form edit profile
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Method: khi nhấn nút Update thì thực hiện chức năng Update
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult rs = MessageBox.Show("Are you sure that you want to update?", "Update a student",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.OK)
                {
                    string fullName = TxtFullName.Text.Trim();
                    string password = TxtPassword.Text.Trim();
                    DateTime? dateOfBirth = DateTime.Now;
                    //if (MtxtDateOfBirth.Text.Equals("  /  /"))
                    //{

                    //    //MtxtDateOfBirth.Text = ("  /  /");
                    //    dateOfBirth = Convert.ToDateTime(MtxtDateOfBirth.Text);
                    //    //DOB = dateOfBirth?.ToString("dd/MM/yyyy");
                    //}
                    //else
                    //{
                    //    dateOfBirth = Convert.ToDateTime(MtxtDateOfBirth.Text);
                    //    DOB = dateOfBirth?.ToString("dd/MM/yyyy");
                    //}


                    string address = TxtAddress.Text.Trim();
                    bool found = false;
                    string error = "";

                    if (fullName.Length == 0)
                    {
                        found = true;
                        error += "Full name is required\n";
                    }

                    if (fullName.Length > 100)
                    {
                        found = true;
                        error += "Full name less than 100 characters\n";
                    }

                    if (password.Length == 0)
                    {
                        found = true;
                        error += "Password is required\n";
                    }

                    if (address.Length == 0)
                    {
                        found = true;
                        error += "Address is required\n";
                    }

                    //if (DateTime.Parse(DOB) >= DateTime.Today)
                    //{
                    //    found = true;
                    //    error += "Birthday is illegal\n";
                    //}

                    //Regex regex = new Regex(@"\d");
                    //if (!regex.IsMatch(MtxtDateOfBirth.Text))
                    //{
                    //    found = true;
                    //    error += "\n - Date Of Birth is Empty.";
                    //}

                    if (DateTime.Today.Year - DateTime.ParseExact(MtxtDateOfBirth.Text, "dd/MM/yyyy", null).Year < 18)
                    {
                        found = true;
                        error += $"student's birthdate must be before {DateTime.Today.Year - 18} .\n";
                    }

                    if (found)
                    {
                        MessageBox.Show(error, "Update Profile - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        dateOfBirth = DateTime.ParseExact(MtxtDateOfBirth.Text, "dd/MM/yyyy", null);

                        //dateOfBirth = Convert.ToDateTime(MtxtDateOfBirth.Text);
                        //DOB = dateOfBirth?.ToString("dd/MM/yyyy");
                        StudentInfo.StudentName = fullName;
                        StudentInfo.Address = address;
                        StudentInfo.BirthOfDate = dateOfBirth;
                        AccountInfo.Password = password;

                        RepositoryTblAccount = new RepositoryTblAccount();
                        RepositoryTblAccount.UpdateAccount(AccountInfo);

                        RepositoryTblStudent = new RepositoryTblStudent();
                        RepositoryTblStudent.UpdateStudent(StudentInfo);

                        parentForm.LoadStudentProfile();
                        isClose = false;
                        MessageBox.Show("Updated successfully", "Update a student", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        this.Close();

                    }
                    
                }
                else
                {
                    isAuto = true;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("DateTime"))
                {
                    MessageBox.Show("Date Of Birth is invalid.", "Update Profile - Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                }
                else
                    MessageBox.Show(ex.Message, "Update Profile - Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }

        }

        public void LoadEditProFileStudent()
        {
            TxtFullName.Text = StudentInfo.StudentName;
            TxtStudentID.Text = StudentInfo.StudentCode;
            TxtEmail.Text = StudentInfo.Username;
            TxtAddress.Text = StudentInfo.Address;
            TxtPassword.Text = AccountInfo.Password;
            if (StudentInfo.Gender == false)
            {
                CbGender.Text = "Female";
            }
            else CbGender.Text = "Male";
            MtxtDateOfBirth.Text = StudentInfo.BirthOfDate?.ToString("dd/MM/yyyy");
            CbMajor.Text = StudentInfo.Majorname;
            CbSemester.Text = SemesterInfo.SemesterName;

        }
        private void FrmStudentEditProfile_Load(object sender, EventArgs e)
        {
            LoadEditProFileStudent();
        }

        private void FrmStudentEditProfile_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (isAuto) { e.Cancel = true; }
            if (isClose)
            {
                DialogResult rs = MessageBox.Show("Are you sure that you want to eixt?", "Edit a student information " +
                    "- Exit confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
