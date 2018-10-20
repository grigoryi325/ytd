using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YouTubeDataBase.Models.AccountModels
{
    public class CountChannelInCategory
    {
        public int Id { get; set; }
        public string NameCategory { get; set; }
        public int CountChannel { get; set; }
    }
}