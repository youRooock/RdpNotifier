using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RdpNotifier
{
    public class RequestFieldAttribute : Attribute
    {
        public string RequestField { get; private set; }
        public bool IsJson { get; private set; }

        public RequestFieldAttribute(string field, bool isJson = false)
        {
            this.RequestField = field;
            this.IsJson = isJson;
        }
    }
}
