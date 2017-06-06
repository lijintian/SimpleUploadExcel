using BatchUploadExcelHelper;
using BatchUploadExcelHelper.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BatchUploadExcelWeb.Controllers
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

            #region 只知道类型名，调用组件实现导入，约定导入的实体只能在组件内定义

            var uploadHelperAssembly=System.Reflection.Assembly.GetAssembly(typeof(BatchUploadExcelHelper.Entities.EntityBase));
            var entityType=uploadHelperAssembly.GetType("BatchUploadExcelHelper."+EntityClassName);

            var fileName = DateTime.Now.ToString("yyyMMddHHmmss") + SubmitFile.FileName;
            var filePath = Request.MapPath("~/UploadFiles");
            var saveFilePathAndName = Path.Combine(filePath, fileName);

            SubmitFile.SaveAs(saveFilePathAndName);

            #region 通过组件获取实体

            var fileInfo = new ExcelFileInfo(filePath, fileName);

            var errorEntities = new List<EntityBase>();

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var validEntities = ExcelToEntityHelper.GetEntities(entityType, fileInfo, out errorEntities);
            stopWatch.Stop();
            #endregion
             
             #region 通过组件持久化实体
             var stopWatch1 = new Stopwatch();
             stopWatch1.Start();
             EntityToDBHelper.ImportEntitiesWidthBulkCopy(entityType, validEntities);
             stopWatch1.Stop();
             #endregion

             #region 通过组件生成导入错误文件
             var errorFileInfo = ErrorEntityToFileHelper.GetFile(entityType,errorEntities);
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
            var uploadHelperAssembly = System.Reflection.Assembly.GetAssembly(typeof(BatchUploadExcelHelper.Entities.EntityBase));
            var entityType = uploadHelperAssembly.GetType("BatchUploadExcelHelper." + EntityClassName);

            return this.File(ErrorEntityToFileHelper.GetFilePathAndName(entityType,FileName), "application/ms-excel", FileName);
        }
    }
}