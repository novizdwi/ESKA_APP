using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Models.Api
{
    public class ServiceResult
    {
        public string ResultType { get; set; }
        public string ResultDescription { get; set; }
        public IEnumerable ResultData { get; set; }
    }

    public class ServiceApiResult
    {
        public string ResultType { get; set; }
        public string ResultDescription { get; set; }
        public List<DocEntryResult> ResultData { get; set; }
    }

    public class DocEntryResult
    {
        public string NewDocEntry { get; set; }
        public string NewDocNum { get; set; }
    }


}
