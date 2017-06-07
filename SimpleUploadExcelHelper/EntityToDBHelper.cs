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
using SimpleUploadExcelHelper.Entities;

namespace SimpleUploadExcelHelper
{
    public static class EntityToDBHelper
    {
        private static string DbConfig = "SimpleImportExcel:RepositoryDBConnStr";

        /// <summary>
        /// 用TQL插入实体，实体个数必须在1000以下
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static int ImportEntitiesWidthTSQL<T>(List<T> entities) where T : Entities.EntityBase
        {
            if (entities.Count > 1000)
            {
                throw new Exception("T-SQ持久化实体个数不得超过1000");
            }

            var importType = typeof(T);
            var fields = importType.GetFields();
            var properties = importType.GetProperties();

            var dbTableAttribute = importType.GetCustomAttribute<DBTableAttribute>(false);
            if (dbTableAttribute == null)
            {
                throw new Exception("没有配置DBTableAttribute，无法识别实体要持久化到哪个数据库。");
            }

            #region 生成插入语句
            var insertSqlHead = "INSERT INTO " + dbTableAttribute.TableName + " (";

            var insertSqlField = string.Empty;

            var isGetInsertField = false;

            var insertSqlVals = new StringBuilder("");

            foreach (var entity in entities)
            {
                var concreteObject = Activator.CreateInstance<T>();

                var insertSqlVal = new StringBuilder("(");

                foreach (var property in properties)
                {
                    var dbColumnAttribute = property.GetCustomAttribute<DBTableColumnAttribute>(false);

                    #region 拼接插入的字段头和值
                    if (dbColumnAttribute != null)
                    {
                        var propertyVal = property.GetValue(entity);

                        #region 第一次进来拼接字段
                        if (!isGetInsertField)
                        {
                            insertSqlField += dbColumnAttribute.ColumnName + ",";
                        }
                        #endregion

                        #region 每次进来拼接值
                        insertSqlVal.Append("'");
                        insertSqlVal.Append(propertyVal);
                        insertSqlVal.Append("',");
                        #endregion
                    }
                    #endregion
                }
                var insertSqlValStr= insertSqlVal.ToString().TrimEnd(',') + "),";
                insertSqlVals.Append(insertSqlValStr);

                isGetInsertField = true;
            }

            //todo:做一些安全性的东西，如防sql注入，正则检测字符串
            var inserSql = insertSqlHead + insertSqlField.TrimEnd(',') + ") VALUES " + insertSqlVals.ToString().TrimEnd(',');

            #endregion

            #region 执行插入语句持久化到数据库

            var effectRows = 0;

            var imprtDB =  ConfigHelper.GetConfig("DbConfig");
           
            var connStr = System.Configuration.ConfigurationManager.ConnectionStrings[imprtDB.ToString()];

            if (connStr == null)
            {
                throw new Exception("没有配置连接字符串");
            }

            using (var sqlConn = new SqlConnection(connStr.ConnectionString))
            {
                sqlConn.Open();
                using (var sqlCmd = new SqlCommand(inserSql, sqlConn))
                {
                    sqlCmd.CommandTimeout = 120;
                    effectRows= sqlCmd.ExecuteNonQuery();
                }
                sqlConn.Close();
            }


            #endregion

            return effectRows;
        }


        /// <summary>
        /// 用BulkCopy插入实体,适合大数据量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static void ImportEntitiesWidthBulkCopy<T>(List<T> entities) where T : Entities.EntityBase
        {
            var importType = typeof(T);
            var fields = importType.GetFields();
            var properties = importType.GetProperties();

            var dbTableAttribute = importType.GetCustomAttribute<DBTableAttribute>(false);
            if (dbTableAttribute == null)
            {
                throw new Exception("没有配置DBTableAttribute，无法识别实体要持久化到哪个数据库。");
            }

            #region 生成DataTable用于BulkCopy

            #region 生成DataTable数据结构

            var dt = new DataTable(dbTableAttribute.TableName);

            foreach (var property in properties)
            {
                var dbColumnAttribute = property.GetCustomAttribute<DBTableColumnAttribute>(false);

                if (dbColumnAttribute != null)
                {
                    dt.Columns.Add(dbColumnAttribute.ColumnName);
                }
            }

            #endregion

            #region 生成DataTable
            foreach (var entity in entities)
            {
                var concreteObject = Activator.CreateInstance<T>();

                var dr=dt.NewRow();

                foreach (var property in properties)
                {
                    var dbColumnAttribute = property.GetCustomAttribute<DBTableColumnAttribute>(false);

                    
                    if (dbColumnAttribute != null)
                    {
                        var propertyVal = property.GetValue(entity);
                        dr[dbColumnAttribute.ColumnName] = propertyVal;
                    }
                }

                dt.Rows.Add(dr);

            }
            #endregion

            #endregion

            #region 用BulkCopy持久化
            BatchInsert(dt);
            #endregion

            return;
        }


        /// <summary>
        /// 用BulkCopy插入实体,适合大数据量插入
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static void ImportEntitiesWidthBulkCopy(Type importType, List<EntityBase> entities) 
        {
            var fields = importType.GetFields();
            var properties = importType.GetProperties();

            var dbTableAttribute = importType.GetCustomAttribute<DBTableAttribute>(false);
            if (dbTableAttribute == null)
            {
                throw new Exception("没有配置DBTableAttribute，无法识别实体要持久化到哪个数据库。");
            }

            #region 生成DataTable用于BulkCopy

            #region 生成DataTable数据结构

            var dt = new DataTable(dbTableAttribute.TableName);

            foreach (var property in properties)
            {
                var dbColumnAttribute = property.GetCustomAttribute<DBTableColumnAttribute>(false);

                if (dbColumnAttribute != null)
                {
                    dt.Columns.Add(dbColumnAttribute.ColumnName);
                }
            }

            #endregion

            #region 生成DataTable
            foreach (var entity in entities)
            {
                var concreteObject = Activator.CreateInstance(importType);

                var dr = dt.NewRow();

                foreach (var property in properties)
                {
                    var dbColumnAttribute = property.GetCustomAttribute<DBTableColumnAttribute>(false);


                    if (dbColumnAttribute != null)
                    {
                        var propertyVal = property.GetValue(entity);
                        dr[dbColumnAttribute.ColumnName] = propertyVal;
                    }
                }

                dt.Rows.Add(dr);

            }
            #endregion

            #endregion

            #region 用BulkCopy持久化
            BatchInsert(dt);
            #endregion

            return;
        }


        #region Private
        private static void BatchInsert(DataTable dt)
        {
         
            var imprtDB = ConfigHelper.GetConfig(DbConfig);

            var connStr = System.Configuration.ConfigurationManager.ConnectionStrings[imprtDB.ToString()];

            if (connStr == null)
            {
                throw new Exception("没有配置连接字符串");
            }



            using (var sqlConn = new SqlConnection(connStr.ConnectionString))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn.ConnectionString, SqlBulkCopyOptions.KeepIdentity))
                {
                    //每一批次中的行数
                    bulkCopy.BatchSize = 100000;
                    //超时之前操作完成所允许的秒数
                    bulkCopy.BulkCopyTimeout = 1800;

                    //将DataTable表名作为待导入库中的目标表名
                    bulkCopy.DestinationTableName = dt.TableName;

                    //将数据集合和目标服务器库表中的字段对应 
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        //列映射定义数据源中的列和目标表中的列之间的关系
                        bulkCopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                    }
                    //将DataTable数据上传到数据表中
                    bulkCopy.WriteToServer(dt);
                }
            }
        }
        #endregion
    }
}
