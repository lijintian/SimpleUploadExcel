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
        public static readonly string ErrorFileRootConfig = "SimpleImportExcel:ErrorFileRoot";

        public  static ExcelFileInfo GetFile<T>(List<T> errors) where T : Entities.EntityBase
        {

            var importType = typeof(T);
            var fields = importType.GetFields();
            var properties = importType.GetProperties();

            #region 生成DataTable用于生成文件

            #region 生成DataTable数据结构
            var dt = new DataTable("导入错误文件");


            var firstError = errors.FirstOrDefault();

            dt=firstError.OriginalData.Table.Clone();

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

            var saveErroRoot = ConfigurationManager.AppSettings[ErrorFileRootConfig];
            if (saveErroRoot == null)
            {
                throw new Exception("没有配置导入错误文件保存路径");
            }

            var filePath =  Path.Combine(saveErroRoot.ToString(), importType.Name);
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss")+ "导入错误文件.xlsx";
            var npoiHelper = new NPOIExcelHelper(Path.Combine(filePath, fileName));

            npoiHelper.DataTableToExcel(dt, "导入错误信息", true);

            return new ExcelFileInfo(filePath, fileName);
          
        }

        public static ExcelFileInfo GetFile(Type importType, List<EntityBase> errors) 
        {
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

            var saveErroRoot = ConfigurationManager.AppSettings[ErrorFileRootConfig];
            if (saveErroRoot == null)
            {
                throw new Exception("没有配置导入错误文件保存路径");
            }

            var filePath = Path.Combine(saveErroRoot.ToString(), importType.Name);
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "导入错误文件.xlsx";
            var npoiHelper = new NPOIExcelHelper(Path.Combine(filePath, fileName));

            npoiHelper.DataTableToExcel(dt, "导入错误信息", true);

            return new ExcelFileInfo(filePath, fileName,importType.Name);

        }

        public static string GetFilePathAndName<T>(string fileName)
        {

            var importType = typeof(T);
        

            var saveErroRoot = ConfigurationManager.AppSettings[ErrorFileRootConfig];
            if (saveErroRoot == null)
            {
                throw new Exception("没有配置导入错误文件保存路径");
            }

            var filePath = Path.Combine(saveErroRoot.ToString(), importType.Name);
            return Path.Combine(filePath, fileName);
        }


        public static string GetFilePathAndName(Type importType,string fileName)
        {
            var saveErroRoot = ConfigurationManager.AppSettings[ErrorFileRootConfig];
            if (saveErroRoot == null)
            {
                throw new Exception("没有配置导入错误文件保存路径");
            }

            var filePath = Path.Combine(saveErroRoot.ToString(), importType.Name);
            return Path.Combine(filePath, fileName);
        }

    }
}
