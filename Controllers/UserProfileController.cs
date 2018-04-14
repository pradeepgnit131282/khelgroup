using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Khel.DataContract;
using Khel.DAL;
using Khel.Business;
using System.Web.Security;
using System.Web.Routing;
using System.IO;
namespace KhelGroup.Controllers
{
    public class UserProfileController : Controller
    {
        //
        // GET: /UserProfile/

        public ActionResult Index()
        {
            FillDropdown();
            CommonUser commonUser = new CommonUser();

            if (Session["UserId"] != null)
            {
                Int64 UserId = Convert.ToInt64(Session["UserId"]);
                commonUser = DAL.GetUserDetails(UserId);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
            return View(commonUser);
        }
        public ActionResult MyInfo()
        {
            CommonUser commonUser = new CommonUser();

            if (Session["UserId"] != null)
            {
                Int64 UserId = Convert.ToInt64(Session["UserId"]);
                commonUser = DAL.GetUserDetails(UserId);
                if (commonUser != null)
                {
                    if (commonUser.customer.StateId == 0 && commonUser.customer.CityId == 0)
                    {
                        return RedirectToAction("MyProfile", "UserProfile");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
            return View(commonUser);

        }

        public ActionResult MyFriends()
        {
            return View();
        }
        public ActionResult MyPhoto()
        {
            return View();
        }
        public ActionResult MyProfile()
        {
            FillDropdown();
            CommonUser commonUser = new CommonUser();

            if (Session["UserId"] != null)
            {
                Int64 UserId = Convert.ToInt64(Session["UserId"]);
                commonUser = DAL.GetUserDetails(UserId);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
            return View(commonUser);
        }

        [HttpPost]
        public ActionResult UpdateUserProfile(CommonUser commonUser, HttpPostedFileBase file)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            Int64 UserId = Convert.ToInt64(Session["UserId"]);
            // Upload your profie pic............

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var userpic = UserId + fileName;
                var path = Path.Combine(Server.MapPath("~/Content/UserProfile"), userpic);
                file.SaveAs(path);
                ModelState.Clear();
                commonUser.customer.UserImage = userpic;
            }
            //Save Customer....................... 
            commonUser.customer.Id = UserId;

            DAL.SaveCustomerDetails(commonUser.customer);
            //Save SportDeatils.......................  
            commonUser.sportDeatils.SportId = 1;
            commonUser.sportDeatils.PlayingLevelId = commonUser.playingLavel.Id;
            commonUser.sportDeatils.MajorSkillId = commonUser.skillInformation.Id;
            if (commonUser.skillInformation.Id == 1)
            {
                commonUser.sportDeatils.MajorSkillTypeId = commonUser.batsman.Id;
            }
            else if (commonUser.skillInformation.Id == 2)
            {
                commonUser.sportDeatils.MajorSkillTypeId = commonUser.bowler.Id;
            }
            else if (commonUser.skillInformation.Id == 3)
            {
                commonUser.sportDeatils.MajorSkillTypeId = commonUser.allRounderInformation.AllRounderTypeId;
            }

            DAL.SaveSportTypeInformation(commonUser.sportDeatils);
            //Save SportDeatils.......................  
            DAL.SaveUserSport(UserId, 1);
            //Favorite Information.......................
            commonUser.favoriteInformation.UserId = UserId;
            DAL.SaveFavoriteInformation(commonUser.favoriteInformation);
            return RedirectToAction("MyInfo", "UserProfile");
            // return View("Index");
        }
        public void FillDropdown()
        {
            ViewBag.MajorSkill = DAL.GetMajorSkill();
            ViewBag.Playinglevel = DAL.GetPlayinglevel();
            ViewBag.AllRounderInformation = DAL.GetAllRounderInformation();
            ViewBag.BatsmanInformation = DAL.GetBatsmanInformation();
            ViewBag.BowlerInformation = DAL.GetBowlerInformation();
            ViewBag.SportName = DAL.GetSportName();
            ViewBag.State = DAL.GetState();
        }
        public JsonResult getCity(string StateId)
        {
            var citylist = DAL.GetCity(Convert.ToInt32(StateId));
            return Json(citylist, JsonRequestBehavior.AllowGet);
        }
        
        #region Blog...................#########################$$$$$$$$$$$$$$$$$$$$$$$$$44
        [HttpPost]
        public ActionResult Blog(Blog blog)
        {
            //if (Session["UserId"] == null)
            //{
            //    return RedirectToAction("Index", "User");
            //}
            //Int64 UserId = Convert.ToInt64(Session["UserId"]);
            //blog.UserId = UserId;
            blog.UserId = 1;
            DAL.SaveBlogDetails(blog);
            return View();
        }
        public ActionResult Blog()
        {
            //if (Session["UserId"] == null)
            //{
            //    return RedirectToAction("Index", "User");
            //}
            //Int64 UserId = Convert.ToInt64(Session["UserId"]);
            // List<Blog> bloglst= new List<Blog>() ;           
            //bloglst= DAL.GetBlogInformation(1);
            // return View(bloglst);
            return View();
        }
        #endregion

        
        public ActionResult AddTournament()
        {
            CommonUser commonUser = new CommonUser();
            commonUser.tournament = new Tournament();
            if (Session["UserId"] != null)
            {
                Int64 UserId = Convert.ToInt64(Session["UserId"]);
                commonUser.tournament = DAL.GetTournament(0, UserId);
                string refURL = Request.UrlReferrer.AbsolutePath;
                if (refURL == "/UserProfile/MyTournament")
                {
                }
                else
                {
                    if (commonUser.tournament != null)
                    {
                        if (!string.IsNullOrEmpty(commonUser.tournament.TournamentName) && !string.IsNullOrEmpty(commonUser.tournament.MobileNumber))
                        {
                            return RedirectToAction("MyTournament", "UserProfile");
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
            return View(commonUser);
        }
        public ActionResult MyTournament()
        {
            CommonUser commonUser = new CommonUser();
            commonUser.tournament = new Tournament();
            if (Session["UserId"] != null)
            {
                Int64 UserId = Convert.ToInt64(Session["UserId"]);
                commonUser.tournament = DAL.GetTournament(0, UserId);

                List<Team> lstPlayer = new List<Team>();
                lstPlayer = DAL.GetListofTournament_Team(commonUser.tournament.TournamentId);
                commonUser.tournament.team = lstPlayer;
                List<ScheduleInfo> lstshcedule = new List<ScheduleInfo>();
                commonUser.tournament.scheduleInfo = new ScheduleInfo();
                commonUser.tournament.scheduleInfo.TournamentId = commonUser.tournament.TournamentId;
                lstshcedule = DAL.GetScheduleInfo(commonUser.tournament.scheduleInfo.TournamentId);
                commonUser.tournament.lstScheduleInfo = lstshcedule;
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
            return View(commonUser);
        }
        public ActionResult AddSchedule()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveTournament(Tournament tournament)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            Int64 UserId = Convert.ToInt64(Session["UserId"]);
            tournament.SportsId = 1;
            tournament.Country = "India";
            tournament.UserId = UserId;
            DAL.SaveTournament(tournament);
            return View("AddTournament");
        }
        [HttpPost]
        public ActionResult SaveTournamentWithTeam(Tournament tournament)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            Int64 UserId = Convert.ToInt64(Session["UserId"]);
            Team_Tournament team_Tournament = new Team_Tournament();
            CommonUser commonUser = new CommonUser();
            commonUser.tournament = new Tournament();
            commonUser.tournament = DAL.GetTournament(0, UserId);
            team_Tournament.TeamId = tournament.TeamId;
            team_Tournament.TournamentId = tournament.TournamentId;
            DAL.SaveTournamentWithTeam(team_Tournament);
            List<Team> lstPlayer = new List<Team>();
            lstPlayer = DAL.GetListofTournament_Team(commonUser.tournament.TournamentId);
            commonUser.tournament.team = lstPlayer;
            return View("MyTournament", commonUser);
        }
        public JsonResult GetFavoriteteam(string teamlist)
        {
            var result = DAL.GetM_Favoriteteam(teamlist);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFavoriteplayer(string playerlist)
        {
            var result = DAL.GetM_FavoritePlayer(playerlist);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveTournamentWithSchedule(Tournament tournament)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            Int64 UserId = Convert.ToInt64(Session["UserId"]);
            //tournament.scheduleInfo = new ScheduleInfo();
            CommonUser commonUser = new CommonUser();
            commonUser.tournament = new Tournament();
            commonUser.tournament = DAL.GetTournament(0, UserId);
            tournament.scheduleInfo.TournamentId = tournament.TournamentId;
            DAL.SaveScheduleInfo(tournament.scheduleInfo);
            List<Team> lstPlayer = new List<Team>();
            lstPlayer = DAL.GetListofTournament_Team(commonUser.tournament.TournamentId);
            commonUser.tournament.team = lstPlayer;
            List<ScheduleInfo> lstshcedule = new List<ScheduleInfo>();
            lstshcedule = DAL.GetScheduleInfo(tournament.scheduleInfo.TournamentId);
            commonUser.tournament.lstScheduleInfo = lstshcedule;
            return View("MyTournament", commonUser);
        }
    }
    public class SessionExpire : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


            if (HttpContext.Current.Session["UserId"] == null)
            {
                FormsAuthentication.SignOut();
                filterContext.Result =
               new RedirectToRouteResult(new RouteValueDictionary   
            {  
             { "action", "Index" },  
            { "controller", "User" },  
            { "returnUrl", filterContext.HttpContext.Request.RawUrl}  
             });

                return;
            }
        }

    } 

    
}
