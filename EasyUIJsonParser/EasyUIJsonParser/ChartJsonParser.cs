using EasyUIJsonParser.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyUIJsonParser
{
    public class ChartJsonParser
    {

        /// <summary>
        /// 获取带DataGrid的仪表盘
        /// </summary>
        /// <param name="title">表盘标题</param>
        /// <param name="unit">表盘单位</param>
        /// <param name="actualValue">实际值</param>
        /// <param name="alarmValue">报警设定值</param>
        /// <param name="maximunValue">表盘上限（可选）</param>
        /// <param name="minimumValue">表盘下限（可选）</param>
        /// <returns></returns>
        public static string GetGridMeterGaugeJsonString(string title, string unit, decimal actualValue, decimal alarmValue, decimal maximunValue = 0, decimal minimumValue = 0)
        {
            string[] myColumnsName = new string[] { "项目", "值" };
            string[] myRowsName = new string[] { "表盘实际值", "报警设定值", "表盘的上限值", "表盘下限值" };
            decimal[] myRowsValue = new decimal[] { actualValue, alarmValue, maximunValue, minimumValue };
            string myUnitX = unit;
            string myUnitY = "";
            int myFrozenColumns = 0;

            DataTable dt = new DataTable();
            dt.Columns.Add("RowName", typeof(string));
            dt.Columns.Add("Value", typeof(decimal));

            for (int i = 0; i < myRowsName.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr["RowName"] = myRowsName[i];
                dr["Value"] = myRowsValue[i];
                dt.Rows.Add(dr);
            }

            dt.AcceptChanges();

            return GetGridChartJsonString(dt, myColumnsName, myRowsName, myUnitX, myUnitY, myFrozenColumns);
        }

        /// <summary>
        /// Chart相关数据
        /// </summary>
        /// <param name="myDataTable">chart数据源</param>
        /// <param name="myColumnsName">Grid列汉语名</param>
        /// <param name="myRowsName">Grid每行第一列表示行名，用来表示每条曲线的名字</param>
        /// <param name="myUnitX">X轴坐标文本</param>
        /// <param name="myUnitY">Y轴坐标文本</param>
        /// <param name="myFrozenCount">冻结列的列数,从第一列开始排</param>
        /// <returns></returns>
        public static string GetGridChartJsonString(DataTable myDataTable, string[] myColumnsName, string[] myRowsName, string myUnitX, string myUnitY, int myFrozenCount)
        {
            if (myColumnsName != null && myRowsName != null && myDataTable != null)
            {
                int m_MaxRowCount = myRowsName.Length <= myDataTable.Rows.Count ? myRowsName.Length : myDataTable.Rows.Count;
                int m_MaxColumnCount = myColumnsName.Length <= myDataTable.Columns.Count ? myColumnsName.Length : myDataTable.Columns.Count;
                int m_MaxFrozenCount = myFrozenCount <= m_MaxColumnCount ? myFrozenCount : m_MaxColumnCount;

                StringBuilder Json = new StringBuilder();
                Json.Append("{\"total\":" + myDataTable.Rows.Count + ",");
                Json.Append("\"FrozenCount\":" + m_MaxFrozenCount + ",");
                Json.Append("\"rows\":[");
                for (int i = 0; i < m_MaxRowCount; i++)
                {
                    Json.Append("{");
                    if (myRowsName != null)
                    {
                        if (i < myRowsName.Length)
                        {
                            Json.Append("\"RowName\":\"" + myRowsName[i] + "\""); //增加行的名称
                        }
                        else
                        {
                            Json.Append("\"RowName\":\"RowName" + (i + 1).ToString() + "\""); //增加行的名称
                        }
                    }
                    else
                    {
                        Json.Append("\"RowName\":\"RowName" + (i + 1).ToString() + "\""); //增加行的名称
                    }
                    for (int j = 0; j < myDataTable.Columns.Count; j++)
                    {
                        string m_ColumnValue = GetConfigInfo.FormatDecimalPlaces(myDataTable.Rows[i][j], myDataTable.Columns[j].DataType);  //增加保留小数点功能
                        Json.Append(",");
                        Json.Append("\"" + myDataTable.Columns[j].ColumnName.ToString() + "\":\"" + m_ColumnValue + "\"");
                    }
                    Json.Append("}");
                    if (i < myDataTable.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
                Json.Append("],\"columns\":[");
                /////////////////////////////////增加一列
                Json.Append("{");
                Json.Append("\"title\":\"名称\"");
                Json.Append(", \"field\":\"RowName\",\"width\":100");
                Json.Append("}");
                /////////////////////////////////增加一列
                for (int i = 0; i < m_MaxColumnCount; i++)
                {
                    Json.Append(",");
                    Json.Append("{");
                    if (myColumnsName != null)
                    {
                        if (i < myColumnsName.Length)
                        {
                            Json.Append("\"title\":\"" + myColumnsName[i] + "\"");
                        }
                        else
                        {
                            Json.Append("\"title\":\"数据" + (i + 1).ToString() + "\"");
                        }
                    }
                    else
                    {
                        Json.Append("\"title\":\"数据" + (i + 1).ToString() + "\"");
                    }
                    Json.Append(", \"field\":\"" + myDataTable.Columns[i].ColumnName.ToString() + "\",\"width\":100");
                    Json.Append("}");
                }
                Json.Append("],\"Units\":{");
                Json.Append("\"UnitX\":\"" + myUnitX + "\"");
                Json.Append(",\"UnitY\":\"" + myUnitY + "\"");
                Json.Append("}}");
                return Json.ToString();
            }
            else
            {
                return "\"rows\":[]";
            }
        }
    }
}
