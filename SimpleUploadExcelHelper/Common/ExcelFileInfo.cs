using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
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


        /// <summary>
        /// 导入是Excel所知的信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="entityClassName"></param>
        /// <param name="fileContent"></param>
        public ExcelFileInfo(string fileName,string entityClassName, byte[] fileContent)
        {
            this.FileName = fileName;
            this.EntityClassName = entityClassName;
            this.FileContent = fileContent;
        }


        public string FilePathAndName { get { return Path.Combine(this.FilePath, this.FileName); } private set { } }

        public string FilePath { get; private set;  }

        public string FileName { get; private set; }

        public byte[] FileContent { get; private set; }

        public int ColumnNameRow { get;private set; }

        public string EntityClassName { get; set; }

        public Type EntityType { get;  set; }

       
    }
}
