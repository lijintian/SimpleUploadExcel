using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SimpleUploadExcelHelper.Common;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Configuration;
using SimpleUploadExcelHelper.Entities;

namespace SimpleUploadExcelHelper
{
    public static class ErrorEntityToFileHelper
    {
        public static readonly string ErrorFileRootConfig = ConfigHelper.GetConfig("SimpleImportExcel:ErrorFileRoot") ;

        public static ExcelFileInfo WriteFile<T>(List<T> errors) where T : Entities.EntityBase
        {
            var importType = typeof(T);

            var baseErrors = new List<EntityBase>();

            foreach (var error in errors)
            {
                baseErrors.Add(error);
            }
            var excelFileInfo = WriteFile(importType, baseErrors);
            return excelFileInfo;
        }

        public static ExcelFileInfo WriteFile(Type importType, List<EntityBase> errors) 
        {
            if (errors.Count <= 0)
            {
                throw new Exception("SimpleUploadExcelHelper-Exception：错误实体个数等于0不能写错误文件");
            }

            var fields = importType.GetFields();
            var properties = importType.GetProperties();

            #region 生成DataTable用于生成文件

            #region 生成DataTable数据结构
            var dt = new DataTable("导入错误文件");


            var firstError = errors.FirstOrDefault();

            dt = firstError.OriginalData.Table.Clone();

            #endregion

            #region 生成DataTable
            foreach (var error in errors)
            {
                var dr = dt.NewRow();
                dr.ItemArray = error.OriginalData.ItemArray;
                dr["错误信息"] = error.ErrorDiscription;
                dt.Rows.Add(dr);
            }
            #endregion

            #endregion


            var filePath = Path.Combine(ErrorFileRootConfig.ToString(), importType.Name);
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "导入错误文件.xlsx";
            var npoiHelper = new NPOIExcelHelper(Path.Combine(filePath, fileName));

            npoiHelper.DataTableToExcel(dt, "导入错误信息", true);

            return new ExcelFileInfo(filePath, fileName,importType.Name);

        }

        public static string GetFilePathAndName<T>(string fileName)
        {
            var importType = typeof(T);

            return GetFilePathAndName(importType, fileName);
        }

        public static string GetFilePathAndName(Type importType,string fileName)
        {
            var filePath = Path.Combine(ErrorFileRootConfig.ToString(), importType.Name);
            return Path.Combine(filePath, fileName);
        }

    }
}
