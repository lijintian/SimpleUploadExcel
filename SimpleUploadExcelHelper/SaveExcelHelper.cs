using SimpleUploadExcelHelper;
using SimpleUploadExcelHelper.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class SaveExcelHelper
    {
        private static string UploadFileRoot = ConfigHelper.GetConfig("SimpleImportExcel:UploadFileRoot");

        private Type EntityType { get; set; }

        public ExcelFileInfo ImportExcelInfo { get; private set; }

        public SaveExcelHelper(ExcelFileInfo importExcelInfo)
        {          
            this.EntityType = TypeHelper.GetType(importExcelInfo.EntityClassName);
            this.ImportExcelInfo = importExcelInfo;
        }

        public ExcelFileInfo SaveFile()
        {
            var fileName = DateTime.Now.ToString("yyyMMddHHmmss") + this.ImportExcelInfo.FileName;

            var filePath = Path.Combine(UploadFileRoot.ToString(), this.EntityType.Name);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var saveFilePathAndName = Path.Combine(filePath, fileName);

            using (FileStream fs = new FileStream(saveFilePathAndName, FileMode.CreateNew))
            {
                fs.Write(this.ImportExcelInfo.FileContent, 0, this.ImportExcelInfo.FileContent.Length);
                fs.Flush();
                fs.Close();
            }

            return new ExcelFileInfo(filePath, fileName) { EntityType=this.EntityType};
        }

        public static string GetFilePathAndName(Type importType, string fileName)
        {
            return GetFilePathAndName(importType.Name, fileName);
        }

        public static string GetFilePathAndName(string EntityTypeName, string fileName)
        {
            var filePath = Path.Combine(UploadFileRoot, EntityTypeName);
            return Path.Combine(filePath, fileName);
        }

        public static ExcelFileInfo GetExcelFileInfo(string EntityTypeName, string fileName)
        {
            var filePath = Path.Combine(UploadFileRoot.ToString(), EntityTypeName);

            var excelFileInfo = new ExcelFileInfo(filePath, fileName);

            return excelFileInfo;
        }

    }
}
