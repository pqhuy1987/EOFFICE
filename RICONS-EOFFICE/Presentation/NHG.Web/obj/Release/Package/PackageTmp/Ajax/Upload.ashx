<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Web;
using System.IO;
using NHG.Logger;
using NHG.Core.Functions;
using NHG.Web.Controllers;

public class Upload : IHttpHandler, System.Web.SessionState.IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        //tham so
        string kq = "", pathUpload = "", pathDown = "", fileLinkName = "";
        System.Text.StringBuilder strCities = new System.Text.StringBuilder();
        Log4Net logger = new Log4Net("Upload");
        string strUserID = context.Session["userid"].ToString();
        if (Functions.CheckSession(strUserID, "Int32"))
        {            
            pathUpload = CommonHelper.GetFolderProfileUser();
            if (!Directory.Exists(pathUpload))
                Directory.CreateDirectory(pathUpload);
            try
            {
                strCities.Append("[");
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    HttpPostedFile postedFile = context.Request.Files[i];
                    string filename = postedFile.FileName;
                    postedFile.SaveAs(pathUpload + filename);
                    strCities.Append("{");
                    fileLinkName = context.Server.HtmlEncode(string.Format("{0}{1}", pathDown, filename));
                    strCities.Append("\"filename\":\"" + context.Server.HtmlEncode(filename) + "\",");
                    strCities.Append("\"length\":\"" + postedFile.ContentLength + "\",");
                    strCities.Append("\"link\":\"" + fileLinkName + "\",");
                    strCities.Append("\"icon\":\"" + FunctionsImage.GetExtensionFile(filename.Substring(filename.LastIndexOf('.'))) + "\"");
                    strCities.Append("},");
                }
                if (strCities.Length > 0)
                    strCities.Remove(strCities.Length - 1, 1);
                strCities.Append("]");
                kq = strCities.ToString();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                kq = "-1";
            }      
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write(kq);
        }
        else
        {
            context.Response.Redirect(CommonHelper.GetLoginPageURL(), false);
        }  
    } 
    public bool IsReusable {
        get {
            return false;
        }
    }
}
