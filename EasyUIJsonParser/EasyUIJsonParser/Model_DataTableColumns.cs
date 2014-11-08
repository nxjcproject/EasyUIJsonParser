using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyUIJsonParser
{
    public enum AlignText
    {
        left,
        right,
        center
    }
    public class Model_DataTableColumns
    {
        /// <summary>
        /// 列汉语名称
        /// </summary>
        private string _ColumnText;
        /// <summary>
        /// 列标题对齐方式
        /// </summary>
        private string _ColumnHeaderAlign;
        /// <summary>
        /// 行单元格对齐方式
        /// </summary>
        private string _ColumnAlign;
        /// <summary>
        /// 列宽
        /// </summary>
        private int _ColumnWidth;
        public Model_DataTableColumns()
        {
            _ColumnText = "";
            _ColumnHeaderAlign = AlignText.center.ToString();
            _ColumnAlign = AlignText.right.ToString();
            _ColumnWidth = 100;
        }
        public string ColumnText
        {
            get
            {
                return _ColumnText;
            }
            set
            {
                _ColumnText = value;
            }
        }
        public string ColumnHeaderAlign
        {
            get
            {
                return _ColumnHeaderAlign;
            }
            set
            {
                _ColumnHeaderAlign = value;
            }
        }
        public string ColumnAlign
        {
            get
            {
                return _ColumnAlign;
            }
            set
            {
                _ColumnAlign = value;
            }
        }
        public int ColumnWidth
        {
            get
            {
                return _ColumnWidth;
            }
            set
            {
                _ColumnWidth = value;
            }
        }
    }
}
