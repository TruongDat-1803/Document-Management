using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System;
using System.Threading.Tasks;
using Upload.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using Upload.Entity;
using Upload.Interface;

namespace Upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        public IBaseBL _baseBL;

        [HttpPost("verify-email")]
        public async Task<ServiceResponse> VerifyEmail1([FromBody] EmailRequest request)
        {
            var res = new ServiceResponse() { };
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    throw new ArgumentException("Email không được để trống.", nameof(request.Email));
                }

                var email = "datalink.3107@gmail.com";
                var password = "omau qbna lxky jubz";

                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(email, password)
                };

                // Content
                var emailSubject = "Confirm your email address";
                Random random = new Random();
                int randomNumber = random.Next(100000, 999999);
                string randomString = "Mã xác thực của bạn là: ";
                randomString += randomNumber.ToString();
                string value = randomNumber.ToString();
                res.Data = value;

                var message = new MailMessage(email, request.Email, emailSubject, randomString);
                await smtpClient.SendMailAsync(message);
                return res;
            }
            catch (Exception ex)
            {
                // Ghi log để xem thông tin chi tiết về lỗi
                Console.WriteLine($"Lỗi: {ex.Message}");
                throw; // Ném lại ngoại lệ để máy chủ trả về lỗi
            }
        }
    }
}
