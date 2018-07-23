using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CodeChange.APIModels
{
    [DataContract]
    public class ActionStat
    {
        [IgnoreDataMember]
        public System.Guid IDAction { get; set; }
        [DataMember]
        public string IDStatus { get; set; }
        [DataMember]
        public string IDStr { get; set; }
        [DataMember]
        public int ReplyCount { get; set; }
        [DataMember]
        public int RetweetCount { get; set; }
        [DataMember]
        public int FavoriteCount { get; set; }
        [DataMember]
        public string Location { get; set; }
    }
}