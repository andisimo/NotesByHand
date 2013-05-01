using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesByHand.Utilities
{
    public static class Fetches
    {
        public static FetchExpression ContactsForBasicPage1
        {
            get
            {
                return new FetchExpression
                    (@"<fetch distinct='false'>
                    <entity name='contact'>
                        <attribute name='fullname' />
                        <attribute name='parentcustomerid' />
                        <attribute name='telephone1' />
                        <attribute name='emailaddress1' />
                        <attribute name='contactid' />
                        <order attribute='fullname' descending='false' />
                        <filter type='and'>
                            <condition attribute='statecode' operator='eq' value='0' />
                        </filter>
                    </entity>
                </fetch>");
            }
        }
    }
}
