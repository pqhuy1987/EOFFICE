<%@ WebHandler Language="C#" Class="download" %>

using System;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using NHG.Web.Models;
using NHG.Logger;
using NHG.Core.Functions;
using NHG.Web.Controllers;

public class download : IHttpHandler, System.Web.SessionState.IRequiresSessionState {
    private List<clHTMLCodeBehide> htmlCodeBehide = new List<clHTMLCodeBehide>();   
    
    public void ProcessRequest (HttpContext context) {
        //
        htmlCodeBehide = new List<clHTMLCodeBehide>();
        htmlCodeBehide.Add(new clHTMLCodeBehide() { html = "vb1", dataColumn = "vanbanden" }); //van ban den local
        htmlCodeBehide.Add(new clHTMLCodeBehide() { html = "vb2", dataColumn = "vanbanden-saved" }); //van ban den local
        htmlCodeBehide.Add(new clHTMLCodeBehide() { html = "vb3", dataColumn = "vanbandi" });
        htmlCodeBehide.Add(new clHTMLCodeBehide() { html = "vb5", dataColumn = "vanbandi-saved" });
        htmlCodeBehide.Add(new clHTMLCodeBehide() { html = "vb4", dataColumn = "ykienlanhdao" });
        htmlCodeBehide.Add(new clHTMLCodeBehide() { html = "bc140000", dataColumn = "baocaonhanh" });
        //tham so
        string pathUpload = "";
        System.Text.StringBuilder strCities = new System.Text.StringBuilder();
        Log4Net logger = new Log4Net("download");
        string strUserID = context.Session["userid"].ToString();
        if (Functions.CheckSession(strUserID, "Int32"))
        {
            string strFileName = context.Request.Form["fileName"];
            string strType = context.Request.Form["tt"];
            //get path
            List<clHTMLCodeBehide> columDB =
               (from p in htmlCodeBehide
               where p.html == strType
               select p).ToList();
            if (columDB.Count() > 0)
            {
                strType = columDB[0].dataColumn;
            }
            switch (strType)
            {
                case "vanbanden":
                    pathUpload = CommonHelper.GetFolderProfileUser() + strFileName;
                    break;
               
                
                default:
                    break;   
            }
            
            FileInfo fi = new FileInfo(pathUpload);
            if (fi.Exists)
            {
                byte[] Buffer = null;
                using (FileStream MyFileStream = new FileStream(pathUpload, FileMode.Open))
                {
                    // Total bytes to read: 
                    long size;
                    size = MyFileStream.Length;
                    Buffer = new byte[size];
                    MyFileStream.Read(Buffer, 0, int.Parse(MyFileStream.Length.ToString()));
                }

                context.Response.ContentType = FunctionsFile.GetStringFileContentType(pathUpload);
                string header = "attachment; filename=\"" + strFileName + "\"";
                context.Response.AddHeader("content-disposition", header);
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                context.Response.BinaryWrite(Buffer);
                context.Response.End();
                context.Response.Flush();
            } 
            else
                context.Response.End();
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
