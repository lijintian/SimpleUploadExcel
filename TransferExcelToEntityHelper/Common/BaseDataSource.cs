using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchUploadExcelHelper.Common
{
    public class BaseDataSource
    {
        private static BaseDataSource _instance = new BaseDataSource();

        private static readonly object lockHelper = new object();

        static BaseDataSource(){}

        private BaseDataSource() {

            var countries= new Dictionary <string, string>();
            countries.Add("广州", "GZ");
            countries.Add("深圳", "SZ");

            this.City = countries.ToJson();

        }

        public static BaseDataSource CreateInstance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new BaseDataSource();
                }
            }
            return _instance;
        }


        public string City { get; set; }
    }
}
