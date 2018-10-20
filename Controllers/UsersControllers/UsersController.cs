using System;
using System.Linq;
using System.Web.Mvc;
using YouTubeDataBase.Models.HomeModels;
using System.Data.Entity;

namespace YouTubeDataBase.Controllers
{
    public class UsersController : Controller
    {
        //связь с контекстом базы данныч
        private DataContext db = new DataContext();

        // GET: Users
        public ActionResult Index(int id = 0)
        {
            try
            {
                var Email = Session["Email"].ToString();
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    var chanels = db.Channels.Include(p => p.Category).OrderByDescending(x => x.Id).ToList();

                    if (id == 0)
                    {
                        return View(chanels.Where(x => x.Email == Email));
                    }
                    else
                    {
                        return View(chanels.Where(x => x.Email == Email && x.CategoryId == id));
                    }
                }
                else
                {
                    return RedirectToAction("ErrorAccess", "Error");
                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("ErrorAccess", "Error");
            }

        }

        //часткове представлення
        //ChildActionOnly гарантирует вызов метода только в качестве дочернего метода

        [ChildActionOnly]
        public ActionResult FilterCategory()
        {
            return View(db.Categories);
        }

        //добавление канала ютюба
        public ActionResult AddChannel()
        {
            try
            {
                var Email = Session["Email"].ToString();
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    SelectList categories = new SelectList(db.Categories, "Id", "Name");
                    ViewBag.Categories = categories;
                    ViewBag.Email = Email;

                    return View();
                }
                else
                {
                    return RedirectToAction("ErrorAccess", "Error");
                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("ErrorAccess", "Error");
            }
        }
        [HttpPost]
        public ActionResult AddChannel(Channel channel)
        {
            var check_channel = db.Channels.Where(x => x.Link == channel.Link).FirstOrDefault();
            //проверка на наличие идентичных ссылок на один и 
            //той же канал, тоесть ищем есть ли добавляемый канал уже в базе
            if (check_channel != null) {
                return RedirectToAction("ErrorAddNewChannel", "Error");
            } else
            {
                db.Channels.Add(channel);
                db.SaveChanges();
                return RedirectToAction("Index", "Users");
            }
        }

        //редактирование канала пользователя
        public ActionResult EditChannel(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    // Находим в бд футболиста
                    //Channel channels = db.Channels.Find(id);
                    Channel channel = db.Channels.Where(x => x.Id == id).FirstOrDefault();

                    // Создаем список команд для передачи в представление
                    SelectList category = new SelectList(db.Categories, "Id", "Name", channel.CategoryId);
                    ViewBag.Categories = category;
                    return View(channel);
                }
                else
                {
                    return RedirectToAction("ErrorAccess", "Error");
                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("ErrorAccess", "Error");
            }
        }


        [HttpPost]
        public ActionResult EditChannel(Channel channel)
        {
            db.Entry(channel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Users");
        }

        public ActionResult DeleteChannel(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    Channel channel = db.Channels.Where(x => x.Id == id).FirstOrDefault();
                    db.Channels.Remove(channel);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("ErrorAccess", "Error");
                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("ErrorAccess", "Error");
            }
        }
    }
}