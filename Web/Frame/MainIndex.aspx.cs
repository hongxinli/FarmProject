using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Frame
{
    public partial class MainIndex : System.Web.UI.Page
    {
        public StringBuilder menutab = new StringBuilder();
        public StringBuilder strTree = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
           // GetMenu();
        }
    }
}