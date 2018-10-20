﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YouTubeDataBase.Models.HomeModels;
using YouTubeDataBase.Models.MessageModels;

namespace YouTubeDataBase.Controllers.FeedbackController
{
    public class FeedbackController : Controller
    {
        //связь с контекстом базы данныч
        private DataContext db = new DataContext();

        //ДЛЯ АДМИНИСТРАТОРОВ
        public ActionResult AnswerMessage(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    //выводим список всех сактивных сообщений от пользователей, тоесть тех на которых еще администраторы не дали ответа
                    var message = db.FeedbackMessages.Where(x => x.CategoryMessageId == id && x.Status == "yes").ToList();

                    return View(message);
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
        public ActionResult AnswerMessage(FeedbackMessage feedbackMessage)
        {
            //присваиваем соответствующему сообщению ответ со стороны администратора
            FeedbackMessage answer_message = db.FeedbackMessages.Where(x => x.Id == feedbackMessage.Id).FirstOrDefault();
            answer_message.AdminEmail = feedbackMessage.AdminEmail;
            answer_message.ADescription = feedbackMessage.ADescription;
            answer_message.AStatus = "yes";
            answer_message.Status = "no";

            db.Entry(answer_message).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("IncomingMessages", "Admin");
        }
         //история сообщений, тоесть те сообщения на которые администраторы дали ответ
        public ActionResult HistoryAnswerAdminMessage()
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    //история сообщений, в порядке сначала последний ответ
                    var MessagHistory = db.FeedbackMessages.Where(x => x.Status == "no").OrderByDescending(x => x.Id).ToList();

                    return View(MessagHistory);
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

        //ДЛЯ ЮЗЕРОВ
        //всее сообщения которые отправил пользователь 
        public ActionResult UserMessage(int id)
        {
            try
            { 
                var Email = Session["Email"].ToString();
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    //при переходе на вкладку сообщения гет запросом отправляеться переменная id = 0 если єто так
                    //то это означает что пользователь увидел все сообщения
                    if (id == 0)
                    {   
                        //меняем тогда статус сообщений с активных на неактивные тоесть прочитаные
                        IEnumerable<FeedbackMessage> message = db.FeedbackMessages.Where(x => x.EmailUser == Email && x.AStatus == "yes").ToList();
                        foreach(var item in message)
                        {
                            item.AStatus = "no";     
                        }
                        
                        db.SaveChanges();
                    }
                    //формируем список сообщений конкретного юзера и передаем в представления
                    var MessageUserHistory = db.FeedbackMessages.Where(x => x.EmailUser == Email).OrderByDescending(x => x.Id).ToList();
                    return View(MessageUserHistory);
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
        //частичное представление, подсчитываем количество не прочитаных сообщений ы выводи информацию в шапке сайта
        [ChildActionOnly]
        public ActionResult CountUserMessage()
        {
            var Email = Session["Email"].ToString();
            ViewBag.CheckSend = db.FeedbackMessages.Where(x => x.EmailUser == Email).Count();
            ViewBag.countUserMessage = db.FeedbackMessages.Where(x => x.AStatus == "yes" && x.EmailUser == Email).Count();
            return PartialView();
        }
    }
}