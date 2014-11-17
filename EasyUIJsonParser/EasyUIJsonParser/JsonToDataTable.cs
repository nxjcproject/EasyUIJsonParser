using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EasyUIJsonParser
{
    public class JsonToDataTable
    {
        //public static DataTable ConvertToTable(string[] json, DataTable resourceTable)
        //{
        //    List<string> columns = GetColumnNames(resourceTable);
        //    foreach (var item in json)
        //    {
        //        DataRow newRow = resourceTable.NewRow();
        //        foreach (var column in columns)
        //        {
        //            string value = item.JsonPick(column);
        //            if (value == "")
        //            {
        //                newRow[column] = DBNull.Value;
        //            }
        //            else
        //            {
        //                newRow[column] = value;
        //            }
        //        }
        //        resourceTable.Rows.Add(newRow);
        //    }
        //    return resourceTable;
        //}

        //private static List<string> GetColumnNames(DataTable table)
        //{
        //    List<string> result = new List<string>();
        //    foreach (DataColumn item in table.Columns)
        //    {
        //        result.Add(item.ColumnName);
        //    }
        //    return result;
        //}
    }
}
