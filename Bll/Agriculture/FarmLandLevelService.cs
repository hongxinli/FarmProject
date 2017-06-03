using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDal.Agriculture;
using OracleDal.Agriculture;
using Model.Agriculture;

namespace Bll.Agriculture
{
    public class FarmLandLevelService
    {
        IDal.Agriculture.IFarmLandLevel dal = new FarmLandLevelRepository();

        public Model.Agriculture.Topic_FarmLandLevel GetModel(string town, string village, string farmLevel)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append(" sum(decode(type,'cityArea',area,0)) cityArea,");
            strSql.Append(" sum(decode(type,'cityLevelArea',area,0)) cityLevelArea,");
            strSql.Append(" sum(decode(type,'townArea',area,0)) townArea,");
            strSql.Append(" sum(decode(type,'townLevelArea',area,0)) townLevelArea,");
            strSql.Append(" sum(decode(type,'villageArea',area,0)) villageArea,");
            strSql.Append(" sum(decode(type,'villageLevelArea',area,0)) villageLevelArea");
            strSql.Append(" from");
            strSql.Append("(");
            strSql.Append(" select 'cityArea' type, sum(t.area) area from topic_farmlandlevel t");
            strSql.Append(" union");
            strSql.Append(" select 'cityLevelArea' type, sum(t.area) area from topic_farmlandlevel t where t.rank='" + farmLevel + "'");
            strSql.Append(" union");
            strSql.Append(" select 'townArea' type, sum(t.area) area from topic_farmlandlevel t where t.town='" + town + "'");
            strSql.Append(" union");
            strSql.Append(" select 'townLevelArea' type, sum(t.area) area from topic_farmlandlevel t where t.town='" + town + "' and t.rank='" + farmLevel + "'");
            strSql.Append(" union");
            strSql.Append(" select 'villageArea' type, sum(t.area) area from topic_farmlandlevel t where t.village='" + village + "'");
            strSql.Append(" union");
            strSql.Append(" select 'villageLevelArea' type, sum(t.area) area from topic_farmlandlevel t where t.village='" + village + "' and t.rank='" + farmLevel + "')");
            var dt = dal.Query(strSql.ToString()).Tables[0];
            Topic_FarmLandLevel model = new Topic_FarmLandLevel();
            model.city = "贵池区";
            model.cityArea = dt.Rows[0]["cityArea"].ToString();
            model.cityLevelArea = dt.Rows[0]["cityLevelArea"].ToString();
            model.town = town;
            model.townArea = dt.Rows[0]["townArea"].ToString();
            model.townLevelArea = dt.Rows[0]["townLevelArea"].ToString();
            model.village = village;
            model.villageArea = dt.Rows[0]["villageArea"].ToString();
            model.villageLevelArea = dt.Rows[0]["villageLevelArea"].ToString();
            return model;
        }
    }
}
