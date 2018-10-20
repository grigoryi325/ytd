using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YouTubeDataBase.Models.AccountModels;
using YouTubeDataBase.Models.HomeModels;
using YouTubeDataBase.Models.MessageModels;

namespace YouTubeDataBase.Controllers.AdminControllers
{
    public class AdminController : Controller
    {
        //доступ до контекста базы данных
        private DataContext db = new DataContext();

        // GET: Admin
        public ActionResult Index()
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {   
                    //подсчет всех активных сообщений
                    ViewBag.CountNewMessae = db.FeedbackMessages.Where(x => x.Status == "yes").Count();

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

        //вход администратора в систему
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            try
            {
                var adm = db.Admins.Single(u => u.Email == admin.Email && u.Password == admin.Password);
                Session["Id"] = adm.Id.ToString();
                Session["Email"] = adm.Email;
                Session["Role"] = adm.Role;
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("ErrorLogin", "Error");
            }

        }

        //редактирование данных аккаунта
        public ActionResult EditAccount(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    return View(db.Admins.Where(x => x.Id == id).FirstOrDefault());
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
        public ActionResult EditAccount(int id, string Email, string OldPassword, string NewPassword, string ReNewPassword)
        {
            //переверяем введенные данные с теми что у базе
            //если пароли совпадают старый и новые
            if (NewPassword == ReNewPassword)
            {
                //ищем пользователя в системе
                var admin = db.Admins.Where(x => x.Id == id).FirstOrDefault();
                if (OldPassword == admin.Password)
                {
                    //то меняем старые даные пользователя на новые
                    admin.Email = Email;
                    admin.Password = NewPassword;
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Admin");
                }    
                return RedirectToAction("ErrorEditAccount", "Error");
            }
            else
            {
                return RedirectToAction("ErrorEditAccount", "Error");
            }
        }

        //Список всех категорий
        public ActionResult ListCategory()
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    return View(db.Categories);
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

        //добавление категории канала ютуба
        public ActionResult AddCategory()
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
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
        public ActionResult AddCategory(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return View();
        }


        //редактирование категории
        public ActionResult EditCategory(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    return View(db.Categories.Where(x => x.Id == id).FirstOrDefault());
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
        public ActionResult EditCategory(int id, Category category)
        {
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ListCategory", "Admin");
        }

        //Подсчет количества зарегистрированных пользователей, каналов, категорий
        public ActionResult Statistics()
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    //подсчитываем количество пользователей, каналов, категорий
                    var count_category = db.Categories.Count();
                    var count_user = db.Users.Count();
                    var count_channel = db.Channels.Count();

                    ViewBag.count_category = count_category;
                    ViewBag.count_user = count_user;
                    ViewBag.count_channel = count_channel;

                    //количество каналов на одного пользователя
                    var channel_for_one_user = (float)count_channel / count_user;

                    ViewBag.channel_for_one_user = Math.Round(channel_for_one_user, 2);

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

        //подсчет количества каналов в каждой категории
        public ActionResult CountChannelInCategory()
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {            
                    //инициилизируем щетчик
                    int i = 1;

                    //перебираем все категории
                    foreach (var item in db.Categories.ToList())
                    {
                        //проверка на наличие записей в таблице, если рядок не пуст то мы
                        //не добавляэм еще раз даные а редактируем их
                        var check = db.CountChannelInCategories.Where(x => x.Id == i).FirstOrDefault();

                        if (check != null)
                        {
                             //берем поочереди категории и подсчитываем сколько в них каналов
                            int count_channel = db.Channels.Where(x => x.CategoryId == i).Count();

                            //так как таблица не пуста то берем записи поочереди и редактируем или перезаписываем их
                            CountChannelInCategory count = db.CountChannelInCategories.Where(x => x.Id == i).FirstOrDefault();
                            count.NameCategory = item.Name;
                            count.CountChannel = count_channel;

                            db.Entry(count).State = EntityState.Modified;

                            i++;
                        }
                        //если таблица полностью пуста или добавлена новая категория то добавляем данные для нее
                        else
                        {
                            //берем поочереди категории и подсчитываем сколько в них каналов
                            int count_channel = db.Channels.Where(x => x.CategoryId == i).Count();
                            //добавляем результат в базу данных
                            CountChannelInCategory count = new CountChannelInCategory
                            {
                                 NameCategory = item.Name,
                                 CountChannel = count_channel
                            };

                            db.CountChannelInCategories.Add(count);

                            i++;
                        }
                    }

                    //сохраняем результат в бд
                    db.SaveChanges();
                    //передаем в представление эту бд для вывода с нее информации
                    return View(db.CountChannelInCategories);
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

        //страница входящих сообщений
        public ActionResult IncomingMessages()
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    //инициилизируем щетчик
                    int i = 1;

                    List<CountMessage> countMessages = new List<CountMessage>();

                    //перебираем все категории сообщений
                    foreach (var item in db.CategoryMessages.ToList())
                    {
                        //берем поочереди категории и подсчитываем сколько в них сообщений которые активны, 
                        //тоесть на них еще не было ответов
                        int count_message = db.FeedbackMessages.Where(x => x.CategoryMessageId == i && x.Status == "yes").Count();

                        //добавляемо в список id - категории, имя категории, количество сообщений в категории
                        countMessages.Add(new CountMessage(i ,item.NameCategory, count_message));

                        //увеличиваем щетчик на единицу
                        i++;
                    }

                    //присваиваем переменной ViewBag.CountMessage результатом сформированый список, и передаем в представление
                    ViewBag.CountMessage = countMessages;

                    return View(ViewBag.CountMessage);
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