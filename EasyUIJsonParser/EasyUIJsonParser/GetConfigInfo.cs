using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EasyUIJsonParser
{
    public class GetConfigInfo
    {
        /// <summary>
        /// 获取保留小数点的位数
        /// </summary>
        /// <returns></returns>
        private static int GetDecimalPlaces(Type myDataType)
        {
            int m_DecimalPlaces = -1;
            try
            {
                if (myDataType == typeof(System.Decimal))
                {
                    //m_DecimalPlaces = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DecimalPlaces"]);
                    m_DecimalPlaces = 2;
                }
                else if (myDataType == typeof(System.Double))
                {
                    //m_DecimalPlaces = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DecimalPlaces"]);
                    m_DecimalPlaces = 2;
                }
                else if (myDataType == typeof(System.Single))
                {
                    //m_DecimalPlaces = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DecimalPlaces"]);
                    m_DecimalPlaces = 2;
                }
                return m_DecimalPlaces;
            }
            catch
            {
                return m_DecimalPlaces;
            }
        }
        public static string FormatDecimalPlaces(object myData, Type myDataType)
        {
            string m_ReturnValue = "";
            int m_DecimalPlaces = GetDecimalPlaces(myDataType);
            if (m_DecimalPlaces > 0)
            {
                m_ReturnValue = string.Format("{0:F" + m_DecimalPlaces.ToString() + "}", myData);
            }
            else
            {
                m_ReturnValue = myData.ToString();
            }
            return m_ReturnValue;
        } 
    }
}
