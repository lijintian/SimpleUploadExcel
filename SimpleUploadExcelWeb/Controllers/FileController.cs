using SimpleUploadExcelHelper;
using SimpleUploadExcelHelper.Common;
using SimpleUploadExcelHelper.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SimpleUploadExcelWeb.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload(string TemplateFileName,string EntityClassName)
        {
            var tempFileInfo = new ExcelFileInfo(string.Empty, TemplateFileName, EntityClassName);
            return View(tempFileInfo);
        }

        [HttpPost]
        public ActionResult SubmitUpload(HttpPostedFileBase SubmitFile,string EntityClassName)
        {
            #region 已知类型调用组件实现导入

            /*var fileName = DateTime.Now.ToString("yyyMMddHHmmss") + SubmitFile.FileName;
            var filePath = Request.MapPath("~/UploadFiles");
            var saveFilePathAndName = Path.Combine(filePath, fileName);

            SubmitFile.SaveAs(saveFilePathAndName);

            var type = Type.GetType("TestDataSourceModel");

            #region 通过组件获取实体

            var fileInfo = new ExcelFileInfo(filePath,fileName);

            var errorEntities = new List<TestDataSourceModel>();

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var validEntities= ExcelToEntityHelper.GetEntities<TestDataSourceModel>(fileInfo, out errorEntities);
            stopWatch.Stop();
            #endregion

            #region 通过组件持久化实体
            var stopWatch1 = new Stopwatch();
            stopWatch1.Start();
            EntityToDBHelper.ImportEntitiesWidthBulkCopy<TestDataSourceModel>(validEntities);
            stopWatch1.Stop();
            #endregion

            #region 通过组件生成导入错误文件
            var errorFileInfo = ErrorEntityToFileHelper.GetFile<TestDataSourceModel>(errorEntities);
            #endregion

            return View("UploadResult", errorFileInfo);*/

            #endregion

            #region 只知道类型名，调用组件实现导入

            #region 通过组件保存文件
            var fileName = SubmitFile.FileName;
            var fileContent = new byte[SubmitFile.ContentLength];
            SubmitFile.InputStream.Read(fileContent, 0, SubmitFile.ContentLength);
            var submitExcelInfo = new ExcelFileInfo(fileName, EntityClassName, fileContent);
            var saveExcelHelper = new SaveExcelHelper(submitExcelInfo);
            var saveFileInfo = saveExcelHelper.SaveFile();
            #endregion

            #region 通过组件获取实体
            var errorEntities = new List<EntityBase>();
            var validEntities = ExcelToEntityHelper.GetEntities(saveFileInfo.EntityType, saveFileInfo, out errorEntities);
            #endregion

            #region 通过组件持久化实体
            EntityToDBHelper.ImportEntitiesWidthBulkCopy(saveFileInfo.EntityType, validEntities);
            #endregion

            #region 通过组件生成导入错误文件
            var errorFileInfo = ErrorEntityToFileHelper.WriteFile(saveFileInfo.EntityType, errorEntities);
            #endregion

            return View("UploadResult", errorFileInfo);
            #endregion
        }


        public ActionResult UploadResult()
        {
            var excelFileInfo = new ExcelFileInfo(string.Empty, string.Empty);
            return View(excelFileInfo);
        }

    
        public ActionResult ImportError(string EntityClassName,string FileName)
        {           
            var entityType = TypeHelper.GetType(EntityClassName);
            return this.File(ErrorEntityToFileHelper.GetFilePathAndName(entityType,FileName), "application/ms-excel", FileName);
        }
    }
}