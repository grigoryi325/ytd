using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using YouTubeDataBase.Models.AccountModels;
using YouTubeDataBase.Models.MessageModels;

namespace YouTubeDataBase.Models.HomeModels
{
    public class DataContext:DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<CountChannelInCategory> CountChannelInCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<FeedbackMessage> FeedbackMessages { get; set; }
        public DbSet<CategoryMessage> CategoryMessages { get; set; }
    }
}