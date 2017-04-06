using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
   public class AuditState
    {
       public string Auditstate(string state)
       {
           string stateColor = "";
           if (state == "待提交")
           {
               stateColor = "<span style='color:Blue'>待提交</span>";
           }
           else if (state == "待审批")
           {
               stateColor = "<span style='color:Brown'>待审批</span>";
           }
           else if (state == "未通过")
           {
               stateColor = "<span style='color:Red'>未通过</span>";
           }
           else if (state == "通过")
           {
               stateColor = "<span style='color:Green'>通过</span>";
           }
           else
           {
               stateColor = "<span style='color:Green'>"+state+"</span>";
           }
           return stateColor;
       }

       public string JlzJdjlYd(string state)
       {
           string stateColor = "";
            if (state == "0")
           {
               stateColor = "<span style='color:Green'>0</span>";
           } 
           else
           {
               stateColor = "<span style='color:Red'>" + state + "</span>";
           }
           return stateColor;
       }
       public string LzkTime(string time)
       {
           string stateColor = "";
           if (!string.IsNullOrEmpty(time))
           {
               string nowTime = DateTime.Now.ToString("yyyy-MM-dd");
               TimeSpan ts = DateTime.Parse(time) - DateTime.Parse(nowTime);
               double dDays = ts.TotalDays;//带小数的天数，比如1天12小时结果就是1.5
               if (dDays < 1.0)
               {
                   stateColor = "<span style='color:Red'>" + time + "</span>";
               }
               else
               {
                   stateColor = "<span style='color:Blue'>" + time + "</span>";
               }
           } 
           return stateColor;
       }
    }
}
