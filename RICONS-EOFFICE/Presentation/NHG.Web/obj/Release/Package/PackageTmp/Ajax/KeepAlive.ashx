<%@ WebHandler Language="C#" Class="KeepAlive" %>

using System;
using System.Web;

public class KeepAlive : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.Write("");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}