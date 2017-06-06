using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchUploadExcelHelper
{
    public class ExcelFileInfo
    {
        public ExcelFileInfo(string filePath,string fileName,int columnNameRow=0)
        {
            this.FilePath = filePath;
            this.FileName = fileName;
            this.ColumnNameRow = columnNameRow;
        }


        public ExcelFileInfo(string filePath, string fileName, string entityClassName)
        {
            this.FilePath = filePath;
            this.FileName = fileName;
            this.EntityClassName = entityClassName;
        }

        public string FilePathAndName { get { return Path.Combine(this.FilePath, this.FileName); } private set { } }

        public string FilePath { get; private set;  }

        public string FileName { get; private set; }

        public int ColumnNameRow { get;private set; }

        public string EntityClassName { get; set; }

       
    }
}
