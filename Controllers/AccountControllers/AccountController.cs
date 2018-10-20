using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YouTubeDataBase.Models.AccountModels;
using YouTubeDataBase.Models.HomeModels;

namespace YouTubeDataBase.Controllers.AccountControllers
{
    public class AccountController : Controller
    {
        DataContext db = new DataContext();
        // GET: Account
        /*public ActionResult Index()
        {
                return View();           
        }*/

        //вход пользователя в систему
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            try {
                var usr = db.Users.Single(u => u.Email == user.Email && u.Password == user.Password);
                Session["Id"] = usr.Id.ToString();
                Session["Email"] = usr.Email;
                Session["Role"] = usr.Role;
                return RedirectToAction("Index", "Users");
            }
            catch (InvalidOperationException) {
                return RedirectToAction("ErrorLogin", "Error");
            }

        }

        //регистрация нового пользователя
        public ActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(User users, string Password,  string RePassword)
        {
            //проверяем, совпадает ли подтверждение паролей
            if (Password != RePassword)
            {
                return RedirectToAction("ErrorConfirmPassword", "Error");
            }
            else
            {
                //ищем пользователя в базе
                var usr = db.Users.Where(x => x.Email == users.Email).FirstOrDefault();
                //если такой пользователь уже зарегистрирован то выдаем ошибку
                if(usr != null)
                {
                    return RedirectToAction("ErrorRegister", "Error");
                }
                //если такого пользователя нет в системе то регистрируем его
                else
                {
                    db.Users.Add(users);
                    db.SaveChanges();

                    return RedirectToAction("Login");
                }               
            }
        }
        
        //редактирование данных аккаунта
       public ActionResult EditAccount(int id)
       {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    return View(db.Users.Where(x => x.Id == id).FirstOrDefault());
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
            //ищем пользователя в системе
            var usr = db.Users.Where(x => x.Id == id).FirstOrDefault();
            //переверяем введенные данные с теми что у базе
            //если пароли совпадают старый и новые
            if ((OldPassword == usr.Password)&&(NewPassword == ReNewPassword))
            {
                //то меняем старые даные пользователя на новые
                User user = db.Users.Where(x => x.Id == id).FirstOrDefault();
                user.Email = Email;
                user.Password = NewPassword;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            } else
            {
                return RedirectToAction("ErrorEditAccount", "Error");
            }
        }

        //выход
        public ActionResult DelSession()
        {
            if (Session["Id"] != null)
            {
                Session.Clear();
                return RedirectToAction("Index", "Home");
            } else
            {
                return RedirectToAction("ErrorAccess", "Error");
            }
        }
    }
}