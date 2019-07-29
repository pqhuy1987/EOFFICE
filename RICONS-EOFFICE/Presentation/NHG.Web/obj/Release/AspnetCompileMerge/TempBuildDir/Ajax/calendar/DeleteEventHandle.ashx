<%@ WebHandler Language="C#" Class="DeleteEventHandle" %>
using System;
using System.Web;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using COMMONLIB;
using System.Text;

public class DeleteEventHandle : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    DT.COMMONLIB.DTLogger logger = new DT.COMMONLIB.DTLogger("DeleteEventHandle--");
    public void ProcessRequest(HttpContext context)
    {
        string strResult = "";
        if (context.Session["userid"] != null)
        {
            WEB_SOAP.calendar.SuKien010000Service services = new WEB_SOAP.calendar.SuKien010000Service();
            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(context.Request.InputStream);
                string line = "";
                line = sr.ReadToEnd();
                Newtonsoft.Json.Linq.JObject jo = Newtonsoft.Json.Linq.JObject.Parse(line);

                CustomDataSet.clSuKien clParam = new CustomDataSet.clSuKien();
                clParam.maghichu = (string)jo["id"];

                if (context.Request.QueryString["del"].ToString() == "Deny")
                {
                    clParam.nguoihieuchinh = context.Session["userid"].ToString();
                    clParam.trangthai = Constants.CST_EVENT_DENIED_FLAG;
                    strResult = services.updateSuKienRegist(clParam);
                }
                else
                    strResult = services.deleteEvent(int.Parse(clParam.maghichu));
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