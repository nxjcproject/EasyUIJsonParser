using EasyUIJsonParser.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyUIJsonParser
{
    public class TreeGridJsonParser
    {
        /// <summary>
        /// 按有LevelCode的表格生成json
        /// </summary>
        /// <param name="table"></param>
        /// <param name="levelCodeColumn"></param>
        /// <param name="columnsToParse"></param>
        /// <returns></returns>
        public static string DataTableToJsonByLevelCode(DataTable myTable, string levelCodeColumn, params string[] columnsToParse)
        {
            myTable.DefaultView.Sort = levelCodeColumn + " asc";
            DataTable table = myTable.DefaultView.ToTable();

            // 当表为空时，返回空json
            if (table == null || table.Rows.Count == 0)
                return "[]";

            if (columnsToParse.Count() == 0)
                columnsToParse = ParserHelper.GetColumnName(table);

            // 结果builder
            StringBuilder result = new StringBuilder();
            result.Append("[");

            // 获取层次码前缀
            string prefix = table.Rows[0][levelCodeColumn].ToString().Substring(0, table.Rows[0][levelCodeColumn].ToString().Length-2);
            // 递归生成节点
            DataTableToJsonByLevelCodeAppend(result, table, levelCodeColumn, prefix, columnsToParse);

            result.Append("]");

            return result.ToString();
        }

        private static void DataTableToJsonByLevelCodeAppend(StringBuilder result, DataTable table, string levelCodeColumn, string parentLevelCode, params string[] otherColumns)
        {
            // 子节点筛选器，规则：以parentLevelCode开头，长度为parentLevelCode长度+2
            string childrenFilter = levelCodeColumn + " like '" + parentLevelCode + "*' and len(" + levelCodeColumn + ") = " + (parentLevelCode.Length + 2);
            // 获取子节点集合
            DataRow[] children = table.Select(childrenFilter);

            if (children.Count() == 0)
                return;

            foreach (DataRow child in children)
            {
                result.Append("{\"id\":\"" + child[levelCodeColumn] + "\"");
                foreach (string column in otherColumns)
                {
                    string m_ColumnValue = GetConfigInfo.FormatDecimalPlaces(child[column], table.Columns[column].DataType);  //增加保留小数点功能
                    //result.Append(",\"" + column + "\":\"" + child[column] + "\"");
                    result.Append(",\"" + column + "\":\"" + m_ColumnValue + "\"");
                }
                //result.Append(",\"state\":\"open\",\"children\":[");
                result.Append(",\"children\":[");
                // 递归获取当前节点的子节点
                DataTableToJsonByLevelCodeAppend(result, table, levelCodeColumn, child[levelCodeColumn].ToString(), otherColumns);
                result.Append("]},");
            }
            // 移除json中最后一个元素跟着的多余逗号
            result.Remove(result.Length - 1, 1);
        }

        /// <summary>
        /// 按groupBy列生成带有一级children的json
        /// </summary>
        /// <param name="table"></param>
        /// <param name="groupBy"></param>
        /// <param name="columnsToParse"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable table, string groupBy, params string[] columnsToParse)
        {
            if (table == null || table.Rows.Count == 0)
                return "[]";

            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            DataTable rootTable = table.DefaultView.ToTable(true, groupBy);

            foreach (DataRow rootRow in rootTable.Rows)
            {
                sb.Append("{");
                sb.Append("\"guid\":\"").Append(Guid.NewGuid()).Append("\",");
                string m_ColumnValue = GetConfigInfo.FormatDecimalPlaces(rootRow[0], rootTable.Columns[0].DataType);  //增加保留小数点功能
                //sb.Append("\"").Append(columnsToParse[0]).Append("\":").Append("\"").Append(rootRow[0].ToString().Trim()).Append("\",");
                sb.Append("\"").Append(columnsToParse[0]).Append("\":").Append("\"").Append(m_ColumnValue.Trim()).Append("\",");
                sb.Append("\"state\":\"closed\",");
                sb.Append("\"children\":[");

                string filter = string.Format("{0}='{1}'", groupBy, rootRow[0].ToString());
                DataRow[] children = table.Select(filter);

                if (children.Length > 0)
                {
                    foreach (DataRow child in children)
                    {
                        sb.Append("{");
                        sb.Append("\"guid\":\"").Append(Guid.NewGuid()).Append("\",");
                        foreach (string column in columnsToParse)
                        {
                            m_ColumnValue = GetConfigInfo.FormatDecimalPlaces(child[column], table.Columns[column].DataType);  //增加保留小数点功能
                            //sb.Append("\"").Append(column).Append("\":").Append("\"").Append(child[column].ToString().Trim()).Append("\",");
                            sb.Append("\"").Append(column).Append("\":").Append("\"").Append(m_ColumnValue.Trim()).Append("\",");
                        }
                        sb.Remove(sb.Length - 1, 1);

                        sb.Append("},");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }

                sb.Append("]");
                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1).Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// 按parentIdColumn列生成带有_parentId的json
        /// </summary>
        /// <param name="table"></param>
        /// <param name="idColumn"></param>
        /// <param name="parentIdColumn"></param>
        /// <param name="columnsToParse"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable table, string idColumn, string parentIdColumn, params string[] columnsToParse)
        {
            if (table == null || table.Rows.Count == 0)
                return "[]";
            if (columnsToParse.Count() == 0)
            {
                columnsToParse = ParserHelper.GetColumnName(table);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"total\":").Append(table.Rows.Count).Append(",");
            sb.Append("\"rows\":[");

            foreach (DataRow row in table.Rows)
            {
                sb.Append("{\"").Append(idColumn).Append("\":\"");
                sb.Append(row[idColumn].ToString().Trim());
                sb.Append("\",");

                if (row[parentIdColumn].ToString().Trim().Length > 0)
                {
                    sb.Append("\"_parentId\":\"");
                    sb.Append(row[parentIdColumn].ToString().Trim());
                    sb.Append("\",");
                }

                foreach (string column in columnsToParse)
                {
                    string m_ColumnValue = GetConfigInfo.FormatDecimalPlaces(row[column], table.Columns[column].DataType);  //增加保留小数点功能
                    //sb.Append("\"").Append(column).Append("\":").Append("\"").Append(row[column].ToString().Trim()).Append("\",");
                    sb.Append("\"").Append(column).Append("\":").Append("\"").Append(m_ColumnValue.Trim()).Append("\",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]}");

            return sb.ToString();
        }

        /// <summary>
        /// 按含有嵌套children的json生成datatable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string json)
        {
            DataTable dt = new DataTable();

            // 行元素集合
            IList<string> components = new List<string>();
            // 列元素集合
            IList<string> columns = new List<string>();

            // 行元素匹配
            var rgComponent = new Regex(@"{[^{}]+}");
            // 列元素匹配
            var rgColumn = new Regex(@"""([^""]+)"":");

            // 匹配行元素
            MatchCollection mc;
            do
            {
                mc = rgComponent.Matches(json);
                for (int i = 0; i < mc.Count; i++)
                {
                    components.Add(mc[i].Value);
                }
                json = rgComponent.Replace(json, "");

            } while (mc.Count > 0);

            // 匹配列元素
            foreach (string component in components)
            {
                mc = rgColumn.Matches(component);
                for (int i = 0; i < mc.Count; i++)
                {
                    if (columns.Contains(mc[i].Groups[1].Value) == false)
                        columns.Add(mc[i].Groups[1].Value);
                }
            }

            // 去除treegrid生成列
            if (columns.Contains("children"))
                columns.Remove("children");
            if (columns.Contains("_parentId"))
                columns.Remove("_parentId");
            if (columns.Contains("state"))
                columns.Remove("state");

            // 构建datatable
            foreach (string column in columns)
            {
                DataColumn col = new DataColumn(column);
                dt.Columns.Add(col);
            }

            foreach (string component in components)
            {
                DataRow row = dt.NewRow();

                foreach (string column in columns)
                {
                    var rgValue = new Regex(@"""" + column + @""":""([^""""]+)""");
                    mc = rgValue.Matches(component);
                    if (mc.Count > 0)
                        row[column] = mc[0].Groups[1].Value;
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        public static string DataTableToJson(DataTable table, string idColumn, string textColumn, string relativeColumn, object parentId, params string[] otherColumns)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder temp = new StringBuilder();

            DataTableToJson(result, temp, table, idColumn, textColumn, relativeColumn, parentId, otherColumns);

            return result.ToString();
        }

        private static void DataTableToJson(StringBuilder result, StringBuilder temp, DataTable table, string idColumn, string textColumn, string relativeColumn, object parentId, params string[] otherColumns)
        {
            result.Append(temp.ToString());
            temp.Clear();
            if (table.Rows.Count > 0)
            {
                temp.Append("[");
                string filer = string.Format("{0}='{1}'", relativeColumn, parentId);
                DataRow[] rows = table.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        temp.Append("{\"id\":\"" + row[idColumn] + "\",\"text\":\"" + row[textColumn] + "\",\"state\":\"open\"");
                        if (otherColumns != null)
                        {
                            foreach (string myOtherColumnId in otherColumns)
                            {
                                string m_ColumnValue = GetConfigInfo.FormatDecimalPlaces(row[myOtherColumnId], table.Columns[myOtherColumnId].DataType);  //增加保留小数点功能
                                //temp.Append(string.Format(",\"{0}\":\"{1}\"", myOtherColumnId, row[myOtherColumnId]));
                                temp.Append(string.Format(",\"{0}\":\"{1}\"", myOtherColumnId, m_ColumnValue));
                            }
                        }
                        else
                        {
                            foreach (string myTableColumnId in table.Columns)
                            {
                                if (myTableColumnId != idColumn && myTableColumnId != textColumn)
                                {
                                    string m_ColumnValue = GetConfigInfo.FormatDecimalPlaces(row[myTableColumnId], table.Columns[myTableColumnId].DataType);  //增加保留小数点功能
                                    //temp.Append(string.Format(",\"{0}\":\"{1}\"", myTableColumnId, row[myTableColumnId]));
                                    temp.Append(string.Format(",\"{0}\":\"{1}\"", myTableColumnId, m_ColumnValue));
                                }
                            }
                        }
                        if (table.Select(string.Format("{0}='{1}'", relativeColumn, row[idColumn])).Length > 0)
                        {
                            temp.Append(",\"children\":");
                            DataTableToJson(result, temp, table, idColumn, textColumn, relativeColumn, row[idColumn], otherColumns);
                            result.Append(temp.ToString());
                            temp.Clear();
                        }
                        result.Append(temp.ToString());
                        temp.Clear();
                        temp.Append("},");
                    }
                    temp = temp.Remove(temp.Length - 1, 1);
                }
                temp.Append("]");
                result.Append(temp.ToString());
                temp.Clear();
            }
        }
    }
}
