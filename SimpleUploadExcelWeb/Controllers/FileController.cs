﻿using SimpleUploadExcelHelper;
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
            var entityAssemblyConfig = System.Configuration.ConfigurationManager.AppSettings["SimpleImportExcel:EntityAssemblyName"];

            if (entityAssemblyConfig == null)
            {
                throw new Exception("没有配置SimpleImportExcel:EntityAssemblyName");
            }

            var uploadHelperAssembly = System.Reflection.Assembly.Load(entityAssemblyConfig.ToString());

            var entityAssemblyNameSpaceConfig = System.Configuration.ConfigurationManager.AppSettings["SimpleImportExcel:EntityNameSpace"];
            if (entityAssemblyNameSpaceConfig == null)
            {
                throw new Exception("没有配置SimpleImportExcel:EntityNameSpace");
            }
            var entityType =uploadHelperAssembly.GetType(entityAssemblyNameSpaceConfig.ToString() +"."+ EntityClassName);

            var fileName = DateTime.Now.ToString("yyyMMddHHmmss") + SubmitFile.FileName;

            var uploadFileRootConfig = System.Configuration.ConfigurationManager.AppSettings["SimpleImportExcel:UploadFileRoot"];

            if (uploadFileRootConfig == null)
            {
                throw new Exception("没有配置SimpleImportExcel:UploadFileRoot");
            }

            var filePath = Path.Combine(uploadFileRootConfig.ToString(), EntityClassName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

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
            var entityAssemblyConfig = System.Configuration.ConfigurationManager.AppSettings["SimpleImportExcel:EntityAssemblyName"];

            if (entityAssemblyConfig == null)
            {
                throw new Exception("没有配置SimpleImportExcel:EntityAssemblyName");
            }

            var uploadHelperAssembly = System.Reflection.Assembly.Load(entityAssemblyConfig.ToString());

            var entityAssemblyNameSpaceConfig = System.Configuration.ConfigurationManager.AppSettings["SimpleImportExcel:EntityNameSpace"];
            if (entityAssemblyNameSpaceConfig == null)
            {
                throw new Exception("没有配置SimpleImportExcel:EntityNameSpace");
            }
            var entityType = uploadHelperAssembly.GetType(entityAssemblyNameSpaceConfig.ToString() + "." + EntityClassName);

            return this.File(ErrorEntityToFileHelper.GetFilePathAndName(entityType,FileName), "application/ms-excel", FileName);
        }
    }
}