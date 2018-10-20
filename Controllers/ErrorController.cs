using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YouTubeDataBase.Controllers
{
    public class ErrorController : Controller
    {
        //Ошибка при вхлде в аккаунт
        public ActionResult ErrorLogin()
        {
            return View();
        }
        //ошибка при регистрации
        public ActionResult ErrorRegister()
        {
            return View();
        }
        //ошибка редактирования данных аккаунта
        public ActionResult ErrorEditAccount()
        {
            return View();
        }
        //ошибка доступа
        public ActionResult ErrorAccess()
        {
            return View();
        }
        //ошибка подтверждения пароля
        public ActionResult ErrorConfirmPassword()
        {
            return View();
        }
        //ошибка добавления нового канала(если такой канал уже существует в базе)
        public ActionResult ErrorAddNewChannel()
        {
            return View();
        }
    }
}