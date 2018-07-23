using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CodeChange.APIModels
{
    [DataContract]
    public class Action
    {
        [DataMember]
        public string ActionName { get; set; }
        [DataMember]
        public DateTime DateBegin { get; set; }
        [DataMember]
        public DateTime DateEnd { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ActionURL { get; set; }
        [DataMember]
        public string ActionUser { get; set; }
        [DataMember]
        public string ActionUserID { get; set; }
        [DataMember]
        public string UserURL { get; set; }
        [DataMember]
        public string ActionImage { get; set; }
        [DataMember]
        public string Location { get; set; }
    }
}