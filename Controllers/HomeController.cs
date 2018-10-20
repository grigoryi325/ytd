using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YouTubeDataBase.Models.HomeModels;
using System.Data.Entity;
using YouTubeDataBase.Models.MessageModels;
using YouTubeDataBase.Models.PageModels;

namespace YouTubeDataBase.Controllers
{
    public class HomeController : Controller
    {
        //связь с контекстом базы данныч
        private DataContext db = new DataContext();
        public int PageSize = 40;

        public ActionResult Index(int id = 0, int page = 1, string time = "desc", string txt = "")
        {
            Session["category"] = id;
            Session["page"] = page;
            Session["time"] = time;
            Session["txt"] = txt;

            string text = Session["txt"].ToString();

            int category = Convert.ToInt32(Session["category"]);

            var allchannels_name = db.Channels.Where(a => a.Name.Contains(text)).ToList();
            var allchannels_description = db.Channels.Where(a => a.Description.Contains(text)).ToList();

            if (text != "")
            {
                if (allchannels_name.Count() == 0)
                {
                    if (allchannels_description.Count() == 0)
                    {
                        ViewBag.Message = "Поиск не дал результатов...";
                        return View();
                    }
                    else
                    {
                        ChannelsListViewModel search = new ChannelsListViewModel
                        {                          
                            Channels = db.Channels.Where(a => a.Description.Contains(text)).ToList()
                            .OrderBy(x => x.Id)
                            .Skip((Convert.ToInt32(Session["page"]) - 1) * PageSize)
                            .Take(PageSize),
                            PagingInfo = new PagingInfo
                            {
                                CurrentPage = Convert.ToInt32(Session["page"]),
                                ItemsPerPage = PageSize,
                                TotalItems = allchannels_description.Count()
                            }
                        };
                        return View(search);
                    }
                }
                else
                {
                    ChannelsListViewModel search = new ChannelsListViewModel
                    {
                        Channels = db.Channels.Where(a => a.Name.Contains(text)).ToList()
                        .OrderBy(x => x.Id)
                        .Skip((Convert.ToInt32(Session["page"]) - 1) * PageSize)
                        .Take(PageSize),
                        PagingInfo = new PagingInfo
                        {
                            CurrentPage = Convert.ToInt32(Session["page"]),
                            ItemsPerPage = PageSize,
                            TotalItems = allchannels_name.Count()
                        }
                    };
                    return View(search);
                }
            }
            else
            {
                if (Convert.ToInt32(Session["category"]) == 0)
                {
                    if (Session["time"].ToString() == "desc")
                    {
                        ChannelsListViewModel model = new ChannelsListViewModel
                        {
                            Channels = db.Channels.Include(p => p.Category).ToList()
                        .OrderByDescending(x => x.Id)
                        .Skip((Convert.ToInt32(Session["page"]) - 1) * PageSize)
                        .Take(PageSize),
                            PagingInfo = new PagingInfo
                            {
                                CurrentPage = Convert.ToInt32(Session["page"]),
                                ItemsPerPage = PageSize,
                                TotalItems = db.Channels.Count()
                            }
                        };
                        return View(model);
                    }
                    else
                    {
                        ChannelsListViewModel model = new ChannelsListViewModel
                        {
                            Channels = db.Channels.Include(p => p.Category).ToList()
                        .OrderBy(x => x.Id)
                        .Skip((Convert.ToInt32(Session["page"]) - 1) * PageSize)
                        .Take(PageSize),
                            PagingInfo = new PagingInfo
                            {
                                CurrentPage = Convert.ToInt32(Session["page"]),
                                ItemsPerPage = PageSize,
                                TotalItems = db.Channels.Count()
                            }
                        };
                        return View(model);
                    }
                }

                if ((Convert.ToInt32(Session["category"]) != 0) && (Session["time"].ToString() == "desc"))
                {
                    ChannelsListViewModel model = new ChannelsListViewModel
                    {
                        Channels = db.Channels.Where(x => x.CategoryId == id)
                        .OrderByDescending(x => x.Id)
                        .Skip((Convert.ToInt32(Session["page"]) - 1) * PageSize)
                        .Take(PageSize),
                        PagingInfo = new PagingInfo
                        {
                            CurrentPage = Convert.ToInt32(Session["page"]),
                            ItemsPerPage = PageSize,
                            TotalItems = db.Channels.Where(x => x.CategoryId == category).Count()
                        }
                    };
                    return View(model);
                }
                else
                {
                    ChannelsListViewModel model = new ChannelsListViewModel
                    {
                        Channels = db.Channels.Where(x => x.CategoryId == id)
                       .OrderBy(x => x.Id)
                       .Skip((Convert.ToInt32(Session["page"]) - 1) * PageSize)
                       .Take(PageSize),
                        PagingInfo = new PagingInfo
                        {
                            CurrentPage = Convert.ToInt32(Session["page"]),
                            ItemsPerPage = PageSize,
                            TotalItems = db.Channels.Where(x => x.CategoryId == category).Count()
                        }
                    };
                    return View(model);
                }
            }
        }

        [ChildActionOnly]
        public ActionResult Search()
        {
            return PartialView();
        }

        //часткове представлення
        [ChildActionOnly]
        public ActionResult Filter()
        {
            return PartialView(db.Categories);
        }

        public ActionResult About()
        {
            return View();
        }

        //страница контактов с формой обратной связи
        public ActionResult Contact()
        {        
            SelectList categoryMessages = new SelectList(db.CategoryMessages, "Id", "NameCategory");
            ViewBag.CategoriesMessages = categoryMessages;

            return View();
        }

        [HttpPost]
        public ActionResult Contact(FeedbackMessage feedbackMessage)
        {
            try
            {
                var Email = Session["Email"].ToString();
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    db.FeedbackMessages.Add(feedbackMessage);
                    db.SaveChanges();
                    return RedirectToAction("Contact", "Home");
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