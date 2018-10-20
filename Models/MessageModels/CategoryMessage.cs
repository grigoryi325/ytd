using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YouTubeDataBase.Models.MessageModels
{
    public class CategoryMessage
    {
        public int Id { get; set; }
        public string NameCategory { get; set; }

        public ICollection<FeedbackMessage> FeedbackMessages { get; set; }
        public CategoryMessage()
        {
            FeedbackMessages = new List<FeedbackMessage>();
        }
    }
}