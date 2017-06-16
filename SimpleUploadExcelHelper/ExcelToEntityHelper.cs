using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SimpleUploadExcelHelper.Common;
using SimpleUploadExcelHelper.Entities;
using System.IO;
using System.Data;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using SimpleUploadExcelHelper.Handler;

namespace SimpleUploadExcelHelper
{
    public class ExcelToEntityHelper 
    {
        public static List<T> GetEntities<T>(ExcelFileInfo fileInfo, out List<T> errors) where T : Entities.EntityBase
        {

            var baseErrors = new List<EntityBase>();
            var baseEntities = new List<EntityBase>();
            var type = typeof(T);

            baseEntities = GetEntities(type, fileInfo, out baseErrors);
            errors = new List<T>();

            foreach (var error in baseErrors)
            {
                errors.Add((T)error);
            }

            var entities = new List<T>();

            foreach (var entity in baseEntities)
            {
                entities.Add((T)entity);
            }

            return entities;

        }

        public static List<EntityBase> GetEntities(Type importType,ExcelFileInfo fileInfo, out List<EntityBase> errors)
        {
            using (var npoiHelper = new NPOIExcelHelper(fileInfo.FilePathAndName))
            {

                var dtExcelData = npoiHelper.ExcelToDataTable(string.Empty, fileInfo.ColumnNameRow);//展示只支持取第一个sheet


                if (dtExcelData.Rows.Count <= 0)
                {
                    throw new Exception("SimpleUploadExcelHelper-Exception:Excel中没有填写数据");
                }

                errors = new List<EntityBase>();

                var allEntities = new List<EntityBase>();
                var entities = new List<EntityBase>();

                var fields = importType.GetFields();
                var properties = importType.GetProperties();

                var isCellValid = true;

                Dictionary<string, DataTable> allDataSource = new Dictionary<string, DataTable>();

                foreach (System.Data.DataRow dtRow in dtExcelData.Rows)
                {
                    var concreteObject = (EntityBase)Activator.CreateInstance(importType);

                    foreach (var property in properties)
                    {
                        var excelColumnAttribute = property.GetCustomAttribute<ExcelColumnAttribute>(false);

                        #region 给原始导入属性赋值
                        if (property.Name == "OriginalData")
                        {
                            var propertySetMethod = property.GetSetMethod();
                            //BindingFlags flag = BindingFlags.Public | BindingFlags.Instance;
                            propertySetMethod.Invoke(concreteObject, new object[] { dtRow });

                        }
                        #endregion

                        #region 根据Attribute校验Excel的原始导入值是否校验通过，校验不通过则加到错误实体，通过则加到正确实体
                        if (excelColumnAttribute != null)
                        {
                            var propertySetMethod = property.GetSetMethod();
                            //BindingFlags flag = BindingFlags.Public | BindingFlags.Instance;
                            isCellValid = true;
                            var cellVal = dtRow[excelColumnAttribute.ColumnName].ToString();

                            #region DataSourceAttribute

                            var datasourceAttribute = property.GetCustomAttribute<DataSourceBaseAttribute>();
                            var datasourceFieldAttribute = property.GetCustomAttribute<DataSourceFieldAttribute>();

                            var isDatasource = datasourceAttribute != null && datasourceFieldAttribute != null;

                            if (isDatasource)
                            {//两者必须成対的出现
                                var dt = new DataTable();

                                #region 同类型的数据源拿过一次后确保不会重复拿
                                var dataSourceTypeName = datasourceAttribute.GetType().Name;

                                if (allDataSource.ContainsKey(dataSourceTypeName))
                                {
                                    dt = allDataSource[dataSourceTypeName];
                                }
                                else
                                {
                                    dt = datasourceAttribute.GetDataSource();
                                    allDataSource.Add(dataSourceTypeName, dt);
                                }
                                #endregion

                                var keyFieldName = datasourceFieldAttribute.KeyFieldName;
                                var valFieldName = datasourceFieldAttribute.ValueFieldName;

                                
                                if(!string.IsNullOrEmpty(cellVal))
                                {
                                    var dr = dt.Select(keyFieldName + "='" + cellVal + "'");

                                    if (!dr.Any())
                                    {
                                        concreteObject.AppendError(excelColumnAttribute.ColumnName + "值" + cellVal + datasourceFieldAttribute.ErrorMsg);
                                    }
                                    else
                                    {
                                        var propertyType = property.PropertyType;

                                        if (propertyType.IsEnum)
                                        {
                                            propertySetMethod.Invoke(concreteObject, new object[] { Enum.Parse(propertyType, dr[0][valFieldName].ToString()) });
                                        }
                                        else
                                        {
                                            propertySetMethod.Invoke(concreteObject, new object[] { Convert.ChangeType(dr[0][valFieldName], propertyType) });
                                        }
                                    }
                                }
                                
                            }

                            #endregion

                            #region 校验Atrribute
                            var validAttributes = property.GetCustomAttributes<ValidBaseAttribute>();

                            foreach (var validAttribute in validAttributes)
                            {
                                if (!validAttribute.Valid(cellVal))
                                {//记录校验错误
                                    isCellValid = false;
                                    concreteObject.AppendError(excelColumnAttribute.ColumnName + "值" + cellVal + validAttribute.ErrorMsg);
                                }

                            }

                            if (isCellValid && !isDatasource)
                            {
                                propertySetMethod.Invoke(concreteObject, new object[] { Convert.ChangeType(cellVal, property.PropertyType) });
                            }

                            #endregion

                        }
                        #endregion
                    }

                    allEntities.Add(concreteObject);

                  
                }

                #region 如果有用户自定义所有实体处理
                var handlerType = importType.GetCustomAttribute<EntitiesHandlerAttribute>();
                if (handlerType != null)
                {
                    allEntities = ReolveAndDoHandler(handlerType.HandlerName,  allEntities);
                }

                #endregion

                #region 区分校验正确与错误的实体
                foreach (var entity in allEntities)
                {
                    #region 校验不通过则加到错误实体，通过则加到正确实体
                    if (entity.IsValid())
                    {
                        entities.Add(entity);
                    }
                    else
                    {
                        errors.Add(entity);
                    }
                    #endregion
                }

                #endregion

                return entities;
            }
        }

        public static List<EntityBase> ReolveAndDoHandler(string handlerTypeName,List<EntityBase> entities)
        {
            var handlerType = TypeHelper.GetHandlerType(handlerTypeName);

            var concreteObject = (IEntitiesHandler)Activator.CreateInstance(handlerType);

            concreteObject.Handle(entities);

            return entities;
        }

    }
}
