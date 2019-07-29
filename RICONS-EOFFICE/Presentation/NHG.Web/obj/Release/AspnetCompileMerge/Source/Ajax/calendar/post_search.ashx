<%@ WebHandler Language="C#" Class="post_cmnd" %>
using System;
using System.IO;
using System.Web;
using System.Text;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using CustomDataSet;
using Newtonsoft.Json.Linq;
using WEB_SOAP.calendar;

public class post_cmnd : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    DT.COMMONLIB.DTLogger logger = new DT.COMMONLIB.DTLogger("post_cmnd");
    HttpContext contextt;
    public void ProcessRequest (HttpContext context) {
        contextt = context;
        if (contextt.Session != null)
        {
            if (contextt.Session["userid"] != null)
            {
                StringBuilder sbResult = new StringBuilder();
                try
                {
                   System.IO.StreamReader sr = new System.IO.StreamReader(context.Request.InputStream);
                    if (context.Request.InputStream != null)
                    {
                        JObject jo = JObject.Parse(sr.ReadToEnd());
                        //noi dung                      
                        
                        sbResult.Append("[");
                        sbResult.Append(SelectDanhSach(jo));
                        sbResult.Append("]");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    sbResult = new StringBuilder();
                    sbResult.Append("{\"error\":\"error\"}");
                }
                context.Response.Write(sbResult);
                context.Response.ContentType = "application/json";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                context.Response.End();
            }
        }
        context.Response.Redirect(CMSDOTNET.Utils.CommonHelper.GetLoginPageURL(), false);
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private StringBuilder SelectDanhSach(JObject jo)
    {
        StringBuilder sbResult = new StringBuilder();
        try
        {
            ThanhPhan010000Service serviceThanhPhan = new ThanhPhan010000Service();

            #region tao tham so
            clThanhPhan clParam = new clThanhPhan();
            clParam.tenthanhphan = ((string)jo["q"]).ToLower();
            #endregion
            List<clThanhPhan> lstThanhPhan = serviceThanhPhan.Select(clParam);
            if (lstThanhPhan.Count > 0)
            {
                foreach (var item in lstThanhPhan)
                {
                    sbResult.Append("{");
                    sbResult.Append("\"mathanhphan\":\"" + item.mathanhphan + "\",");
                    sbResult.Append("\"tenthanhphan\":\"" + item.tenthanhphan + "\",");
                    sbResult.Append("\"maphongban\":\"" + item.maphongban + "\",");
                    sbResult.Append("\"capdo\":\"" + item.capdo + "\"");
                    sbResult.Append("},");
                }
                if (sbResult.Length > 0)
                    sbResult.Remove(sbResult.Length - 1, 1);
            }
            else
            {
                
            }            
        }
        catch (Exception ex)
        {
            logger.Error(ex);
            sbResult = new StringBuilder();
        }
        return sbResult;
    }
}