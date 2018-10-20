using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YouTubeDataBase.Models.HomeModels;

namespace YouTubeDataBase.Models.PageModels
{
    public class ChannelsListViewModel
    {
        public IEnumerable<Channel> Channels { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}