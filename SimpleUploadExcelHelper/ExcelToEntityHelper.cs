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

namespace SimpleUploadExcelHelper
{
    public class ExcelToEntityHelper 
    {
        public static List<T> GetEntities<T>(ExcelFileInfo fileInfo, out List<T> errors) where T : Entities.EntityBase
        {
            using (var npoiHelper = new NPOIExcelHelper(fileInfo.FilePathAndName))
            {

                var dtExcelData = npoiHelper.ExcelToDataTable(string.Empty, fileInfo.ColumnNameRow);//展示只支持取第一个sheet

                errors = new List<T>();

                var entities = new List<T>();

                var importType = typeof(T);
                var fields = importType.GetFields();
                var properties = importType.GetProperties();

                var isCellValid = true;

                Dictionary<string, Dictionary<string, string>> allDataSource = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, string> currentDataSource = new Dictionary<string, string>();

                foreach (System.Data.DataRow dtRow in dtExcelData.Rows)
                {
                    var concreteObject = Activator.CreateInstance<T>();

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


                            #region 记录校验错误
                            if (!isCellValid)
                            {
                                concreteObject.AppendError(excelColumnAttribute.ColumnName + "");
                            }
                            #endregion


                        }
                        #endregion
                    }

                    #region 校验不通过则加到错误实体，通过则加到正确实体
                    if (concreteObject.IsValid())
                    {
                        entities.Add(concreteObject);
                    }
                    else
                    {
                        errors.Add(concreteObject);
                    }
                    #endregion
                }

                return entities;
            }
        }

        public static List<EntityBase> GetEntities(Type importType,ExcelFileInfo fileInfo, out List<EntityBase> errors)
        {
            using (var npoiHelper = new NPOIExcelHelper(fileInfo.FilePathAndName))
            {

                var dtExcelData = npoiHelper.ExcelToDataTable(string.Empty, fileInfo.ColumnNameRow);//展示只支持取第一个sheet

                errors = new List<EntityBase>();

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
                            if (datasourceAttribute != null && datasourceFieldAttribute != null)
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

                                var dr = dt.Select(keyFieldName + "='" + cellVal + "'");

                                if (dr == null)
                                {
                                    concreteObject.AppendError(excelColumnAttribute.ColumnName + "值" + cellVal + datasourceFieldAttribute.ErrorMsg);
                                }
                                else
                                {
                                    propertySetMethod.Invoke(concreteObject, new object[] { Convert.ChangeType(dr[0][valFieldName], property.PropertyType) });
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

                            if (isCellValid)
                            {
                                propertySetMethod.Invoke(concreteObject, new object[] { Convert.ChangeType(cellVal, property.PropertyType) });
                            }

                            #endregion

                         


                        }
                        #endregion
                    }

                    #region 校验不通过则加到错误实体，通过则加到正确实体
                    if (concreteObject.IsValid())
                    {
                        entities.Add(concreteObject);
                    }
                    else
                    {
                        errors.Add(concreteObject);
                    }
                    #endregion
                }

                return entities;
            }
        }
    }
}
