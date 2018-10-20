using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YouTubeDataBase.Models.HomeModels
{
    public class Channel
    {
        //щетчик
        public int Id { get; set; }
        //нужен для того чтобі в личном кабинете выводить только те каналы, которые принадлежат конкретному аккаунту
        public string Email { get; set; }
        //имя канала
        public string Name { get; set; }
        //описание канала
        public string Description { get; set; }
        //сылка на канал в ютюбе
        public string Link { get; set; }

        //связь с таблицой Category (тип связи один ко многим, тоисть одна категория будет иметь много каналов)
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}