using EasyUIJsonParser.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyUIJsonParser
{
    public class DataGridJsonParser
    {
        public static string DataTableToJson(DataTable table, params string[] columnsToParse)
        {
            if (table == null || table.Rows.Count == 0)
                return "{\"total\":0,\"rows\":[]}";

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"total\":").Append(table.Rows.Count).Append(",");
            sb.Append("\"rows\":[");

            foreach (DataRow row in table.Rows)
            {
                sb.Append("{");

                if (columnsToParse.Count() == 0)
                {
                    columnsToParse = ParserHelper.GetColumnName(table);
                }
                foreach (string column in columnsToParse)
                {
                    sb.Append("\"").Append(column).Append("\":").Append("\"").Append(row[column].ToString().Trim()).Append("\",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]}");

            return sb.ToString();
        }

        /// <summary>
        /// 自定义行数Table转json
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="myRowCount">行数</param>
        /// <param name="columnsToParse">其它字段</param>
        /// <returns>table的json</returns>
        public static string DataTableToJson(DataTable table, int myRowCount, params string[] columnsToParse)
        {
            string m_RowJson = GetDataRowJson(table, columnsToParse);
            string m_TotalJson = "\"total\":" + myRowCount;
            return "{" + m_TotalJson + "," + m_RowJson + "}";
        }
        /// <summary>
        /// 只获得行的json
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="columnsToParse">其它字段</param>
        /// <returns>行的json</returns>
        public static string GetDataRowJson(DataTable table, params string[] columnsToParse)
        {
            if (table == null || table.Rows.Count == 0)
            {
                return "\"rows\":[]";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("\"rows\":[");

            foreach (DataRow row in table.Rows)
            {
                sb.Append("{");

                if (columnsToParse.Count() == 0)
                {
                    columnsToParse = ParserHelper.GetColumnName(table);
                }
                foreach (string column in columnsToParse)
                {
                    sb.Append("\"").Append(column).Append("\":").Append("\"").Append(row[column].ToString().Trim()).Append("\",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            return sb.ToString();
        }
        /// <summary>
        /// 列的json
        /// </summary>
        /// <param name="table">数据表</param>
        /// <param name="myDataTableColumns">额外的表信息</param>
        /// <returns>列的json</returns>
        public static string GetColumnsJson(DataTable table, params Model_DataTableColumns[] myDataTableColumns)
        {
            if (table == null)
            {
                return "\"columns\":[]";
            }
            StringBuilder m_Columns = new StringBuilder();
            m_Columns.Append("\"columns\":[");

            if (myDataTableColumns == null)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (i == 0)
                    {
                        m_Columns.Append("{");
                    }
                    else
                    {
                        m_Columns.Append(",{");
                    }
                    m_Columns.Append("\"title\":\"" + table.Columns[i].ColumnName + "\"");
                    m_Columns.Append(",\"field\":\"" + table.Columns[i].ColumnName + "\"");
                    m_Columns.Append("}");
                }
            }
            else
            {
                int m_MaxCount = table.Columns.Count <= myDataTableColumns.Length ? table.Columns.Count : myDataTableColumns.Length;
                for (int i = 0; i < m_MaxCount; i++)
                {
                    if (i == 0)
                    {
                        m_Columns.Append("{");
                    }
                    else
                    {
                        m_Columns.Append(",{");
                    }
                    m_Columns.Append("\"title\":\"" + myDataTableColumns[i].ColumnText + "\"");
                    m_Columns.Append(",\"field\":\"" + table.Columns[i].ColumnName + "\"");
                    if (myDataTableColumns[i].ColumnWidth > 0)
                    {
                        m_Columns.Append(",\"width\":" + myDataTableColumns[i].ColumnWidth.ToString());
                    }
                    if (myDataTableColumns[i].ColumnHeaderAlign != "")
                    {
                        m_Columns.Append(",\"headeralign\":\"" + myDataTableColumns[i].ColumnHeaderAlign + "\"");
                    }
                    if (myDataTableColumns[i].ColumnAlign != "")
                    {
                        m_Columns.Append(",\"align\":\"" + myDataTableColumns[i].ColumnAlign + "\"");
                    }
                    m_Columns.Append("}");

                }
            }
            m_Columns.Append("]");
            return m_Columns.ToString();
        }

        public static DataTable JsonToDataTable(string[] json, DataTable resourceTable)
        {
            List<string> columns = GetColumnNames(resourceTable);
            foreach (var item in json)
            {
                DataRow newRow = resourceTable.NewRow();
                foreach (var column in columns)
                {
                    string value = item.JsonPick(column);
                    if (value == "")
                    {
                        newRow[column] = DBNull.Value;
                    }
                    else
                    {
                        newRow[column] = value;
                    }
                }
                resourceTable.Rows.Add(newRow);
            }
            return resourceTable;
        }

        private static List<string> GetColumnNames(DataTable table)
        {
            List<string> result = new List<string>();
            foreach (DataColumn item in table.Columns)
            {
                result.Add(item.ColumnName);
            }
            return result;
        }
    }
}
