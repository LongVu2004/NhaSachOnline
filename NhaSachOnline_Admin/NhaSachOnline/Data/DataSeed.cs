using Microsoft.AspNetCore.Identity;
using NhaSachOnline.ChucNangPhanQuyen;

namespace NhaSachOnline.Data
{
    public class DataSeed
    {
        public static async Task KhoiTaoDuLieuMacDinh(IServiceProvider dichVu)
        {
            var quanLyNguoiDung = dichVu.GetService<UserManager<IdentityUser>>();
            var quanLyVaiTro = dichVu.GetService<RoleManager<IdentityRole>>();

            // Thêm 1 vai trò vào CSDL
            await quanLyVaiTro.CreateAsync(new IdentityRole(PhanQuyen.Admin.ToString()));
            await quanLyVaiTro.CreateAsync(new IdentityRole(PhanQuyen.User.ToString()));

            // Tạo thông tin tài khoản mặc định cho Admin,
            // Bao gồm Username, Email, xác thực Email
            var quanTri = new IdentityUser
            {
                UserName = "testAdmin@gmail.com",
                Email = "testAdmin@gmail.com",
                EmailConfirmed = true,
            };

            var nguoiDungTrongCSDL = await quanLyNguoiDung.FindByEmailAsync(quanTri.Email);
            // Nếu tài khoản Admin không tồn tại trong csdl (chưa có csdl)
            if(nguoiDungTrongCSDL is null)
            {
                // tạo tk Admin mới với mk = Aa@123
                await quanLyNguoiDung.CreateAsync(quanTri, "Aa@123");
                await quanLyNguoiDung.AddToRoleAsync(quanTri, PhanQuyen.Admin.ToString());
            }


        }
    }
}
