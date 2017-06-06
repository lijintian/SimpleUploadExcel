using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BatchUploadExcelHelper.Common;
using BatchUploadExcelHelper.Entities;

namespace BatchUploadExcelHelper
{
    public static class ExcelToEntityHelper 
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

                            #region 是否必填
                            if (excelColumnAttribute.IsRequire)
                            {
                                if (string.IsNullOrEmpty(cellVal))
                                {
                                    concreteObject.AppendError("请输入"+excelColumnAttribute.ColumnName);
                                    continue;
                                }
                            }
                            #endregion

                            #region 格式校验
                            switch (excelColumnAttribute.ValidType)
                            {
                                case Common.ExcelFieldValidType.Regular:
                                    #region 正则校验
                                    if (excelColumnAttribute.ValidRegular.IsMatch(dtRow[excelColumnAttribute.ColumnName].ToString()))
                                    {
                                        propertySetMethod.Invoke(concreteObject, new object[] { Convert.ChangeType(dtRow[excelColumnAttribute.ColumnName], property.PropertyType) });
                                        isCellValid = true;
                                    }
                                    else
                                    {
                                        isCellValid = false;
                                    }
                                    #endregion
                                    break;
                                case Common.ExcelFieldValidType.DataSource:
                                    //todo:各种类型的数据源校验
                                    #region 数据源校验 

                                    #region 将json数据源反序列化为实体，反序列化过的数据源不再反序列化
                                    if (allDataSource.ContainsKey(excelColumnAttribute.ColumnName))
                                    {
                                        currentDataSource = allDataSource[excelColumnAttribute.ColumnName];
                                    }
                                    else
                                    {
                                        Dictionary<string, string> dataSource = (Dictionary<string, string>)excelColumnAttribute.DataSource.ToObject(new Dictionary<string, string>());
                                        allDataSource.Add(excelColumnAttribute.ColumnName, dataSource);
                                        currentDataSource = dataSource;
                                    }
                                    #endregion

                                    if (currentDataSource.ContainsKey(dtRow[excelColumnAttribute.ColumnName].ToString()))
                                    {
                                        propertySetMethod.Invoke(concreteObject, new object[] { currentDataSource[dtRow[excelColumnAttribute.ColumnName].ToString()] });
                                        isCellValid = true;
                                    }
                                    else
                                    {
                                        isCellValid = false;
                                    }
                                    #endregion

                                    break;
                                default:
                                    #region 无须校验
                                    isCellValid = true;
                                    propertySetMethod.Invoke(concreteObject, new object[] { Convert.ChangeType(dtRow[excelColumnAttribute.ColumnName], property.PropertyType) });
                                    #endregion
                                    break;
                            }
                            #endregion

                            #region 记录校验错误
                            if (!isCellValid)
                            {
                                concreteObject.AppendError(excelColumnAttribute.ColumnName + excelColumnAttribute.ValidErrorMsg);
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

                Dictionary<string, Dictionary<string, string>> allDataSource = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, string> currentDataSource = new Dictionary<string, string>();

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

                            #region 是否必填
                            if (excelColumnAttribute.IsRequire)
                            {
                                if (string.IsNullOrEmpty(cellVal))
                                {
                                    concreteObject.AppendError("请输入" + excelColumnAttribute.ColumnName);
                                    continue;
                                }
                            }
                            #endregion

                            #region 格式校验
                            switch (excelColumnAttribute.ValidType)
                            {
                                case Common.ExcelFieldValidType.Regular:
                                    #region 正则校验
                                    if (excelColumnAttribute.ValidRegular.IsMatch(dtRow[excelColumnAttribute.ColumnName].ToString()))
                                    {
                                        propertySetMethod.Invoke(concreteObject, new object[] { Convert.ChangeType(dtRow[excelColumnAttribute.ColumnName], property.PropertyType) });
                                        isCellValid = true;
                                    }
                                    else
                                    {
                                        isCellValid = false;
                                    }
                                    #endregion
                                    break;
                                case Common.ExcelFieldValidType.DataSource:
                                    //todo:各种类型的数据源校验
                                    #region 数据源校验 

                                    #region 将json数据源反序列化为实体，反序列化过的数据源不再反序列化
                                    if (allDataSource.ContainsKey(excelColumnAttribute.ColumnName))
                                    {
                                        currentDataSource = allDataSource[excelColumnAttribute.ColumnName];
                                    }
                                    else
                                    {
                                        Dictionary<string, string> dataSource = (Dictionary<string, string>)excelColumnAttribute.DataSource.ToObject(new Dictionary<string, string>());
                                        allDataSource.Add(excelColumnAttribute.ColumnName, dataSource);
                                        currentDataSource = dataSource;
                                    }
                                    #endregion

                                    if (currentDataSource.ContainsKey(dtRow[excelColumnAttribute.ColumnName].ToString()))
                                    {
                                        propertySetMethod.Invoke(concreteObject, new object[] { currentDataSource[dtRow[excelColumnAttribute.ColumnName].ToString()] });
                                        isCellValid = true;
                                    }
                                    else
                                    {
                                        isCellValid = false;
                                    }
                                    #endregion

                                    break;
                                default:
                                    #region 无须校验
                                    isCellValid = true;
                                    propertySetMethod.Invoke(concreteObject, new object[] { Convert.ChangeType(dtRow[excelColumnAttribute.ColumnName], property.PropertyType) });
                                    #endregion
                                    break;
                            }
                            #endregion

                            #region 记录校验错误
                            if (!isCellValid)
                            {
                                concreteObject.AppendError(excelColumnAttribute.ColumnName + excelColumnAttribute.ValidErrorMsg);
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
