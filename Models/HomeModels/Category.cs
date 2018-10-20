using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YouTubeDataBase.Models.HomeModels
{
    public class Category
    {
        //щетчик
        public int Id { get; set; }
        //имя категории
        public string Name { get; set; }

        public ICollection<Channel> Channels { get; set; }
        public Category()
        {
            Channels = new List<Channel>();
        }
    }
}