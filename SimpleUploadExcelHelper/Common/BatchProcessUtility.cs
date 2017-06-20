using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class BatchProcessUtility
    {

        public static ProcessInfo GetProcessInfo(int totalRecord, int eachBath)
        {
            return new ProcessInfo(totalRecord,eachBath);
        }
    }


    public class ProcessInfo
    {
        public ProcessInfo(int totalRecord, int pageRecord)
        {
            this.TotalRecord = totalRecord;
            this.PageRecord = pageRecord;
            this.TotalPage = (TotalRecord % PageRecord > 0 ? TotalRecord / PageRecord + 1 : TotalRecord / PageRecord);
            this.BeginTime = DateTime.Now;
            this.CurrentPersent = 0;
            this.LastPersent = 0;
        }
        
        public int PageRecord{get;private set;}

        public int TotalRecord { get; private set; }

        public int TotalPage { get; private set; }

        public int LastPersent{get;set;}

        public int CurrentPersent{get;private set;}

        public DateTime BeginTime { get;private set; }

        public int NeedSeconds { get;private set; }


        public bool IsNewPersent(int i, int currentBatchCount)
        {
            var currentPersent = ((decimal.Parse(((i + 1) * PageRecord - (PageRecord - currentBatchCount)).ToString()) / TotalRecord) * 100);

            this.CurrentPersent = 1;
            if (currentPersent > 1)
            {
                this.CurrentPersent = Convert.ToInt32(currentPersent);
            }
          

            DateTime dtEnd = DateTime.Now;

            TimeSpan sp = dtEnd - BeginTime;

            var costSeconds = sp.TotalSeconds;

            var totalSeconds = 100 * costSeconds / this.CurrentPersent;

            var needSeconds = totalSeconds - costSeconds;

            int intNeedSeconds = needSeconds < 1 ? 1 : Convert.ToInt32(needSeconds);

            this.NeedSeconds = intNeedSeconds;

            bool isNewPersent = false;

            if (this.LastPersent != this.CurrentPersent)
            {
                isNewPersent = true;
                this.LastPersent = this.CurrentPersent;
            }

            return isNewPersent;
        }
    }
}
