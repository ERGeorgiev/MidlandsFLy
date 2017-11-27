using MidlandsFly;
using MidlandsFly.Sql;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteMaster : MasterPage
{
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;

    protected void Page_Init(object sender, EventArgs e)
    {
        // The code below helps to protect against XSRF attacks
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;
        if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            // Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value;
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        else
        {
            // Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                HttpOnly = true,
                Value = _antiXsrfTokenValue
            };
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += master_Page_PreLoad;
    }

    protected void master_Page_PreLoad(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set Anti-XSRF token
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
        }
        else
        {
            // Validate the Anti-XSRF token
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            {
                throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
    {
        Context.GetOwinContext().Authentication.SignOut();
    }

    protected void AddRecords(object sender, EventArgs e)
    {
        if ((System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
        {
            try
            {
                if (SqlMidlandsFly.Instance.TableExists(
                    SqlMidlandsFly.Instance.Table_Passenger,
                    SqlMidlandsFly.Instance.Table_Cargo,
                    SqlMidlandsFly.Instance.Table_Employees,
                    SqlMidlandsFly.Instance.Table_Assignment,
                    SqlMidlandsFly.Instance.Table_Maintenance,
                    SqlMidlandsFly.Instance.Table_MaintenanceHistory,
                    SqlMidlandsFly.Instance.Table_Stage))
                {
                    Simulation.Demo();
                    Response.Redirect(Request.Url.AbsoluteUri);
                }
                else
                {
                    ErrMessage = String.Format("Table {0} does not exist. Please restart the simulation or contact an administrator.", SqlMidlandsFly.Instance.Table_Employees.Name);
                }
            }
            catch (Exception ex)
            {
                ErrMessage = String.Format("Message: {0}", ex.Message);
            }
        }
    }

    protected void ClearTables(object sender, EventArgs e)
    {
        if ((System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
        {
            try
            {
                SqlMidlandsFly.Instance.RecreateTables();
                SqlMidlandsFly.Instance.Execute();
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            catch (Exception ex)
            {
                ErrMessage = String.Format("Message: {0}", ex.Message);
            }
        }
    }

    protected void AddHours_24(object sender, EventArgs e)
    {
        if ((System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
        {
        }
    }

    protected void AddHours_100(object sender, EventArgs e)
    {
        if ((System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
        {
        }
    }

    public string ErrMessage
    {
        get
        {
            return ErrorMessage.Text;
        }
        set
        {
            ErrorMessage.Text = value;
        }
    }

    //protected void AddHoursToPlane(object sender, EventArgs e)
    //{
    //    SqlMidlandsFly.Instance.AddHours((uint)Int32.Parse(TextBox_AddHoursToPlane_Hours.Text), TextBox_AddHoursToPlane_RegNumber.Text);
    //    SqlMidlandsFly.Instance.Execute();
    //}
}