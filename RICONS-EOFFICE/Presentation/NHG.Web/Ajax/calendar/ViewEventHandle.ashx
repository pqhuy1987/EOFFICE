
<%@ WebHandler Language="C#" Class="ViewEventHandle" %>

using System;
using System.Web;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using COMMONLIB;
using System.Text;

public class ViewEventHandle : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    DT.COMMONLIB.DTLogger logger = new DT.COMMONLIB.DTLogger("ViewEventHandle--");
    public void ProcessRequest(HttpContext context)
    {
        string strResult = "";
        //HttpContext.Current.Server.Transfer("Login.aspx");
        if (context.Session["userid"] != null)
        {
            WEB_SOAP.calendar.SuKien010000Service services = new WEB_SOAP.calendar.SuKien010000Service();
            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(context.Request.InputStream);
                string line = "";
                line = sr.ReadToEnd();
                Newtonsoft.Json.Linq.JObject jo = Newtonsoft.Json.Linq.JObject.Parse(line);
                
                int masukien = int.Parse((string)jo["strMaSuKien"]);

                List<CustomDataSet.clSuKien> lstSuKien = services.selectEventForIdEvent(masukien).ToList();
                
                strResult = "[{\"strMaGhiChu\":\"" + lstSuKien[0].maghichu + "\",\"strTieuDe\":\"" + lstSuKien[0].tieude + "\""
                        + ",\"strNoiDung\":\"" + lstSuKien[0].noidung + "\""
                        + ",\"strPhongHop\":\"" + lstSuKien[0].vitrikhac + "\""
                        + ",\"strNgayBatDau\":\"" + lstSuKien[0].ngaybatdau + "\""
                        + ",\"strNgayKetThuc\":\"" + lstSuKien[0].ngayketthuc + "\""
                        + ",\"strDeXuat\":\"" + lstSuKien[0].dexuat + "\""
                        + ",\"strSoNgayNhacNho\":\"" + lstSuKien[0].songaynhacnho + "\""
                        + ",\"strLoaiNhacNho\":\"" + lstSuKien[0].loainhacnho + "\""
                        + ",\"strDoUuTien\":\"" + lstSuKien[0].mucdouutien + "\""
                        + ",\"strTrangThai\":\"" + lstSuKien[0].trangthai
                        + "\"}]";
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        else
        {
            context.Response.Redirect("index.aspx");
            return;
        }

        context.Response.ContentType = "application/json";
        context.Response.Write(strResult);
        context.Response.End();

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}