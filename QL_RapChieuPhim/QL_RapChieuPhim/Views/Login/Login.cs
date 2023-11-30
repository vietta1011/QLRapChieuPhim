using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace QL_RapChieuPhim.Views
{
    public partial class Login : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);
        Database.DatabaseAccess dtb = new Database.DatabaseAccess();

        //bool Matkhau = false;

        public Login()
        {
            InitializeComponent();
        }
        int count_poster = 0;
        private void timer_transfer_Tick(object sender, EventArgs e)
        {
            if (count_poster < 11)
            {
                image_slider.Image = imageList_movie.Images[count_poster];
                count_poster++;
            }
            else
            {
                count_poster = 0;
            }
        }


        private void Login_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            /*              txt_user.Text = hidetextTK;
                          txt_user.ForeColor = Color.Gray;

                          txt_password.Text = hidetextMK;
                          txt_password.ForeColor = Color.Gray;

                      */
        }


        private void btn_dangnhap_Click(object sender, EventArgs e)
        {
            FormMain frm = new FormMain();
            frm.Show();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btn_DangNhap_Click_1(object sender, EventArgs e)
        {
            string loginname = txt_TaiKhoan.Text;
            string password = txt_MatKhau.Text;

            if (loginname == "")
            {
                MessageBox.Show("Bạn chưa nhập tên tài khoản");
                txt_TaiKhoan.Focus();
            }
            else if (password == "")
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu");
                txt_MatKhau.Focus();
            }
            else
            {
                string connectionString = "Data Source=WAVYZ-LAPTOP\\VIET;Initial Catalog=QLRapChieuPhim2023_Update;Integrated Security=True;MultipleActiveResultSets=true";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Tạo câu truy vấn SQL để kiểm tra đăng nhập và lấy ChucVu
                    string query = "SELECT COUNT(*), ChucVu FROM tbTaiKhoan WHERE TenDN = @loginname AND MatKhau = @password GROUP BY ChucVu";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@loginname", loginname);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int count = reader.GetInt32(0);
                                    string chucVu = reader.GetString(1);

                                    if (count > 0)
                                    {
                                        // Lấy mã nhân viên
                                        string LayMaNV = "Select MaNV from tbTaiKhoan where TenDN = @loginname";
                                        SqlCommand layMaNVCmd = new SqlCommand(LayMaNV, connection);
                                        layMaNVCmd.Parameters.AddWithValue("@loginname", loginname);
                                        object maNVObject = layMaNVCmd.ExecuteScalar();
                                        string MaNV = (maNVObject != DBNull.Value) ? maNVObject.ToString() : string.Empty;
                                        // Lấy tên từ bảng nhân viên
                                        string LayTenNV = "Select TenNV from tbNhanVien where MaNV = @MaNV";
                                        SqlCommand LayTenNVCmd = new SqlCommand(LayTenNV, connection);
                                        LayTenNVCmd.Parameters.AddWithValue("@MaNV", MaNV);
                                        object tenNVObject = LayTenNVCmd.ExecuteScalar();
                                        string TenNV = (tenNVObject != null) ? tenNVObject.ToString() : string.Empty;
                                        
                                        // Thực hiện hành động sau khi đăng nhập thành công
                                        

                                        if (chucVu == "Nhân Viên")
                                        {
                                            Views.formMainNV fmNV = new formMainNV(TenNV,MaNV);
                                            this.Hide();
                                            fmNV.ShowDialog();
                                            //MessageBox.Show("Đăng nhập thành công!");
                                            
                                            this.Show();
                                        }
                                        else if (chucVu == "Admin")
                                        {
                                            FormMain fm = new FormMain("Admin");
                                            this.Hide();
                                            fm.ShowDialog();
                                            //MessageBox.Show("Đăng nhập thành công!");
                                            this.Show();
                                        }

                                        
                                    }
                                    else
                                    {
                                        MessageBox.Show("Đăng nhập không thành công. Hãy thử lại.");
                                    }
                                }
                            }
                            
                            else
                            {
                                MessageBox.Show("Đăng nhập không thành công. Hãy thử lại.");
                            }

                        }
                    }
                }
            }
        }


        private void txt_MatKhau_Enter(object sender, EventArgs e)
        {
            txt_MatKhau.UseSystemPasswordChar = true;
            txt_MatKhau.PlaceholderText = "";
        }

        private void txt_MatKhau_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_MatKhau.Text))
            {
                txt_MatKhau.UseSystemPasswordChar = false;
                txt_MatKhau.PlaceholderText = "Mật khẩu";
            }
        }

        private void Login_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                btn_DangNhap_Click_1(sender, e);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
           /* if (Matkhau == false) {

                txt_MatKhau.UseSystemPasswordChar = false;
                Matkhau = true;
            }
            else if(Matkhau == true)
            {
                txt_MatKhau.UseSystemPasswordChar = true;
                Matkhau = false;
            }
            */
        }
        
    }
}
