using SimpleUploadExcelHelper.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleImportEntity
{
    public class GetDataSourceInstance
    {
        private static GetDataSourceInstance _instance = new GetDataSourceInstance();

        private static readonly object lockHelper = new object();

        static GetDataSourceInstance(){}

        private GetDataSourceInstance() {

            var dt = new DataTable("Citys");

            dt.Columns.Add("Name");
            dt.Columns.Add("Code");

            var dr = dt.NewRow();
            dr["Name"] = "广州";
            dr["Code"] = "GZ";

            dt.Rows.Add(dr);

            var dr1 = dt.NewRow();
            dr1["Name"] = "深圳";
            dr1["Code"] = "SZ";

            dt.Rows.Add(dr1);


            this.City = dt;

        }

        public static GetDataSourceInstance CreateInstance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new GetDataSourceInstance();
                }
            }
            return _instance;
        }


        public DataTable City { get; set; }
    }
}
