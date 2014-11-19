using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EasyUIJsonParser.Infrastructure
{
    public class ParserHelper
    {
        public static string[] GetColumnName(DataTable dt)
        {
            IList<string> columns = new List<string>();
            foreach (DataColumn item in dt.Columns)
            {
                columns.Add(item.ColumnName);
            }
            return columns.ToArray();
        }
    }
}
