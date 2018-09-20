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
        //////////////////////////////////////自定义小数点保留杜永文添加函数开始
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataType">数据类型</param>
        /// <param name="myPointReservation">第一个参数为decimal小数点保留位数</param>
        /// <returns></returns>
        private static int GetCustomPointReservation(Type myDataType,int myPointReservationCount )
        {
            int m_DecimalPlaces = -1;
            try
            {
                if (myDataType == typeof(System.Decimal))
                {
               
                  m_DecimalPlaces = myPointReservationCount;
                                    
                }
                else if (myDataType == typeof(System.Double))
                {
                    m_DecimalPlaces = 2;
                }
                else if (myDataType == typeof(System.Single))
                {
                    m_DecimalPlaces = 2;
                }
                return m_DecimalPlaces;
            }
            catch
            {
                return m_DecimalPlaces;
            }
        }
       // ///杜永文添加函数
        public static string CustomFormatDecinalPlaces(object myData, Type myDataType, int myDeciamlResCount)
        {
            string m_ReturnValue = "";
            int m_DataPlaces = GetCustomPointReservation(myDataType, myDeciamlResCount);
            if (m_DataPlaces > 0)
            {
                m_ReturnValue = string.Format("{0:F" + m_DataPlaces.ToString() + "}", myData);
            }
            else if (m_DataPlaces == 0)
            {
                string[] mStrArry = myData.ToString().Split('.');
                m_ReturnValue = mStrArry[0];
            }
            else
            {
                m_ReturnValue = myData.ToString();
            }
            return m_ReturnValue;
        }

       //////////////////////////////////杜永文添加函数结束
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
