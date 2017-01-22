using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.Helpers.Validation
{
    public abstract class ValidationMessage
    {
        public MemberInfo TargetMember { get; set; }
        public string Message { get; set; }
    }

    public class ValidationWarning : ValidationMessage
    {

    }

    public class ValidationError : ValidationMessage
    {

    }
}
