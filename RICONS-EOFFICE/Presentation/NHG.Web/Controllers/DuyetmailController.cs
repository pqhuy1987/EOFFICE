using NHG.Core.Functions;
using NHG.Web.Data.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NHG.Web.Controllers
{
    public class DuyetmailController : Controller
    {
        //
        // GET: /Duyetmail/
        public ActionResult Index()
        {
            return View();
        }

        public string send_Mail(string mailcanhan,string dongy)
        {
            try
            {
                string smtp_host = string.Format(Functiontring.ReturnStringFormatthongtincauhinhmail("smtp_host"));
                string smtp_user = string.Format(Functiontring.ReturnStringFormatthongtincauhinhmail("smtp_user"));
               
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(smtp_user, mailcanhan.Trim());
                message.From = new MailAddress(smtp_user.Trim(), "Đơn xin nghỉ phép", System.Text.Encoding.UTF8);
                message.Subject = "Đơn xin nghỉ phép";
                message.Body = dongy;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient(smtp_host);
                client.UseDefaultCredentials = true;
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                                ex.ToString());
                }
                return "Successfull!";
            }
            catch (Exception ms)
            {
                return ms.Message;
            }
        }

        public string send_Mail(string mailto, string hotento, string hovaten,string tenphongban,string songayxinnghi,string ngayxinnghitu,string ngayxinnghiden
            , string lydoxinnghi, string sodienthoai, string mailcanhan, string strEncryptCode, string duyenlan, string buoinghi)
        {
            try
            {
                string linkname = string.Format(Functiontring.ReturnStringFormatthongtincauhinhmail("linkname")); 
                    strEncryptCode = linkname.Trim() + strEncryptCode + "&mailcanhan=" + mailcanhan + "&duyenlan=2" + "&emailto=0&hotento=0";

                string smtp_host = string.Format(Functiontring.ReturnStringFormatthongtincauhinhmail("smtp_host"));
                string smtp_user = string.Format(Functiontring.ReturnStringFormatthongtincauhinhmail("smtp_user"));

                StringBuilder sb = new StringBuilder();
                sb.Append("<html><head>");
                sb.Append("<link rel='stylesheet' type='text/css' href='theme.css' />");
                sb.Append("</head>");
                sb.Append("<body style='margin-top: 20px; padding: 0; width: 650px; font-size: 1em;color:black;'>"); //margin: 0 auto;  de canh giua

                sb.Append("<table cellpadding='0' cellspacing='0' width='650px' >");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td height='76px' width='650px' >");
                sb.Append("<table cellpadding='0' cellspacing='0' width='100%'>");

                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td>");

                sb.Append("<div style='width:150px;float:left;height :75px; line-height:75px; padding-top:17px;'>");
                sb.Append("<img src='http://i.imgur.com/oYQcVBO.png'  alt='ddd' style='width:150px; height:68px;'/>");
                sb.Append("</div>");

                sb.Append("<div style='text-align:center;font-weight: bold;float:left;line-height:75px'>");
                sb.Append("<p style= 'text-align:center;font-size:18px;font-weight:bold;line-height:75px;padding-left:80px;'>ĐƠN XIN NGHỈ PHÉP</p>");
                sb.Append("</div>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("<hr style=border: 1px solid #000; width: 100% />");

                sb.Append("<table style='width:650px; font-size:14px;'>");
                //sb.Append("<tr><td style='font-weight:bold'>" + "Kính gởi Anh/chị:" + "</td></tr>");
                sb.Append("<tr><td style='padding-left:10px;'><p><strong><em><u>Kính gửi Anh/Chị:</u></em>&nbsp;" + hotento + "</strong></p></td></tr>");

                sb.Append("<tr><td style='float:left;height :22px; line-height:22px; padding-left:10px;'>" + "Họ và tên:             " + hovaten + "" + "</td></tr>");
                sb.Append("<tr><td style='float:left;height :22px; line-height:22px; padding-left:10px;'>" + "Công tác tại:          " + tenphongban + "" + "</td></tr>");
                if (buoinghi.Trim()=="")
                    sb.Append("<tr><td style='float:left;height :22px; line-height:22px; padding-left:10px;'>" + "Số ngày xin nghỉ:      " + songayxinnghi + "" + "</td></tr>");
                else
                    sb.Append("<tr><td style='float:left;height :22px; line-height:22px; padding-left:10px;'>" + "Số ngày xin nghỉ:      " + songayxinnghi + " (" + buoinghi + ")" + "" + "</td></tr>");
                sb.Append("<tr><td style='float:left;height :22px; line-height:22px; padding-left:10px;'>" + "Từ ngày:               " + ngayxinnghitu + " đến ngày: " + ngayxinnghiden + "" + "</td></tr>");
                sb.Append("<tr><td style='float:left;height :22px; line-height:22px; padding-left:10px;'>" + "Lý do xin nghỉ:        " + lydoxinnghi + "" + "</td></tr>");
                sb.Append("<tr><td style='float:left;height :22px; line-height:22px; padding-left:10px;'>" + "Số điện thoại:         " + sodienthoai + "" + "</td></tr>");
                sb.Append("<tr><td style='float:left;height :22px; line-height:22px; padding-left:10px;'>" + "Email:                 " + mailcanhan + "" + "</td></tr>");  //style='background-color:blue;color:white'

                sb.Append("<tr><td style='float:left; padding-left:10px; padding-top:10px;'><a href='" + strEncryptCode + "&dongy=1'> Đồng ý</a>&nbsp;&nbsp;<a href='" + strEncryptCode + "&dongy=2'>Không đồng ý</a></td></tr>");

                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");


                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(smtp_user, mailto.Trim());
                message.From = new MailAddress(smtp_user.Trim(), "Đơn xin nghỉ phép", System.Text.Encoding.UTF8);
                message.Subject = "Đơn xin nghỉ phép";
                message.Body = sb.ToString(); 
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient(smtp_host);
                client.UseDefaultCredentials = true;
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                                ex.ToString());
                }

                return "Successfull!";
            }
            catch (Exception ms)
            {
                return ms.Message;
            }
        }

        public string Duyet1(string madangky, string emailto, string hotento, string dongy,string hovaten,string tenphongban
                           , string songayxinnghi, string ngayxinnghitu, string ngayxinnghiden, string lydoxinnghi, string sodienthoai, string mailcanhan
                            , string duyenlan, string hieuchinh, string cap1, string cap2, string buoinghi)
        {
            string Duyetphep = "Đã duyệt phép thành công";
            string strEncryptCode = madangky.Replace("0070pXQSeNsQRuzoCmUYfuX/vA==6", "");
            AbsentServices service = new AbsentServices();
            string kq ="";
            if (duyenlan == "2")
                kq = service.UpdateRow_Duyetnghiphep2(strEncryptCode.Trim(), dongy.Trim(),hieuchinh); 
            else kq = service.UpdateRow_Duyetnghiphep1(strEncryptCode.Trim(), dongy.Trim(),hieuchinh); 

            if (kq=="1" && dongy == "1")
            {
                if (emailto == "0" && hotento == "0")
                {
                    send_Mail(mailcanhan, "Đồng ý cho nghỉ phép");
                    Duyetphep = "Đồng ý cho nghỉ phép"; // +strEncryptCode + "kq" + kq + mailcanhan + "mailcanhan" + mailcanhan;
                }
                else
                {
                    send_Mail(emailto, hotento, hovaten, tenphongban, songayxinnghi, ngayxinnghitu, ngayxinnghiden, lydoxinnghi, sodienthoai, mailcanhan, madangky, duyenlan, buoinghi);
                }
            }
            else if (kq=="1" && dongy == "2")
            {
                send_Mail(mailcanhan, "Không đồng ý nghỉ phép");
                Duyetphep = "Không đồng ý nghỉ phép";
            }
            else if(kq=="2")
            {
                Duyetphep = "Phép đã duyệt không chỉnh sữa được";
            }
            else if (kq == "3")
            {
                Duyetphep = "Nội dung nghỉ phép đã thay đổi. Vui lòng chọn lại Email.";
            }
            else if (kq == "-1")
            {
                Duyetphep = "Đã xảy ra lỗi duyệt phép. Liên hệ IT để chỉnh sữa";
            }
            return Duyetphep;
        }


        public string send_Mail_chamcong(string mailcanhan, string dongy)
        {
            try
            {
                string smtp_host = string.Format(Functiontring.ReturnStringFormatthongtincauhinhmail("smtp_host"));
                string smtp_user = string.Format(Functiontring.ReturnStringFormatthongtincauhinhmail("smtp_user"));

                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(smtp_user, mailcanhan.Trim());
                message.From = new MailAddress(smtp_user.Trim(), "Chấm công online", System.Text.Encoding.UTF8);
                message.Subject = "Chấm công online";
                message.Body = dongy;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient(smtp_host);
                client.UseDefaultCredentials = true;
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                                ex.ToString());
                }
                return "Successfull!";
            }
            catch (Exception ms)
            {
                return ms.Message;
            }
        }


        public string Duyet2(string maphongban, string thang, string dy, string mca)
        {
            string Duyetphep = "Duyệt chấm công online";
            string strEncryptCode = maphongban.Replace("0070pXQSeNsQRuzoCmUYfuX", "");
            TimekeepingServices service = new TimekeepingServices();
            string kq = service.UpdateRow_Duyetpheponline(strEncryptCode, thang, dy, "0");

            if (kq=="1" && dy == "1")
            {
                send_Mail_chamcong(mca, "Đã duyệt chấm công online");
                Duyetphep = "Đã duyệt chấm công online thành công";
            }
            else if (kq=="1" && dy == "2")
            {
                send_Mail_chamcong(mca, "Không duyệt chấm công online");
                Duyetphep = "Không duyệt chấm công online thành công";
            }
            else if (kq == "2" && dy == "2")
            {
                Duyetphep = "Chấm công online đã duyệt không thay đổi được. Vui lòng liên hệ IT để điều chỉnh";
            }
            else Duyetphep = "Lỗi trong quá trình Duyệt. Liên hệ IT để hổ trợ.";
            return Duyetphep;
        }
	}
}