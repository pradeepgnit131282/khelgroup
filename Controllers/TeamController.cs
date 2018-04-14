using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Khel.DataContract;
using Khel.DAL;
using System.Web.Helpers;
//using System.Web.UI.DataVisualization;
using System.Web.Security;
using System.Web.Routing;
using System.IO;

namespace KhelGroup.Controllers
{
    public class TeamController : Controller
    {
        //
        // GET: /Team/
       
        public JsonResult ViewTeamJsonData()
        {
            var teamlist = DAL.GetTeamByCity(0);
            return Json(teamlist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewTeam()
        {
            Session["UserId"] = 1;
            CommonUser commonUser = new CommonUser();
            commonUser.team = new Team();
            if (Session["UserId"] != null)
            {
                Int64 UserId = Convert.ToInt64(Session["UserId"]);
                List<Team> lstteam = new List<Team>();
                lstteam = DAL.GetTeam();
                commonUser.GetTeamYouMayBeInterested = lstteam;
                List<Player> lstPlayer = new List<Player>();
                lstPlayer = DAL.GetPlayerByTeam(commonUser.team.TeamId);
                commonUser.team.player = lstPlayer;
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
            return View(commonUser);
        }
        public JsonResult GetTeamListbyCity(string CityId)
        {
            var teamlist = DAL.GetTeamByCity(Convert.ToInt32(CityId));
            return Json(teamlist, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult getTeam(string TeamName)
        {
            var teamlist = DAL.GetTeam(TeamName);
            return Json(teamlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getTeamList(string TeamName)
        {
            var TeamList = DAL.GetTeam(TeamName);
            return Json(TeamList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MyTeam()
        {
            CommonUser commonUser = new CommonUser();
            commonUser.team = new Team();
            if (Session["UserId"] != null)
            {
                Int64 UserId = Convert.ToInt64(Session["UserId"]);
                commonUser.team = DAL.GetTeam(0, UserId);
                List<Player> lstPlayer = new List<Player>();
                lstPlayer = DAL.GetPlayerByTeam(commonUser.team.TeamId);
                commonUser.team.player = lstPlayer;
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
            return View(commonUser);
        }
        [HttpPost]
        public ActionResult SaveTeamPlayer(Player player)
        {
            player.TeamId = 1;
            DAL.SavePlayerByTeam(player);
            var playerdetail = DAL.GetPlayerByTeam(1);
            CommonUser c = new CommonUser();
            c.team = new Team();
            c.team.player = playerdetail;
            return View("Team", c);
        }
        public ActionResult AddTeam()
        {
            //var player = DAL.GetPlayerByTeam(1);
            //Business.Entity.CommonUser c = new CommonUser();
            //c.team = new Team();
            //c.team.player = player;
            //return View(c); 
            CommonUser commonUser = new CommonUser();
            commonUser.team = new Team();
            if (Session["UserId"] != null)
            {
                Int64 UserId = Convert.ToInt64(Session["UserId"]);
                commonUser.team = DAL.GetTeam(0, UserId);
                string refURL = Request.UrlReferrer.AbsolutePath;
                if (refURL == "/UserProfile/MyTeam")
                {
                }
                else
                {
                    if (commonUser.team != null)
                    {
                        if (!string.IsNullOrEmpty(commonUser.team.TeamName) && !string.IsNullOrEmpty(commonUser.team.MobileNumber))
                        {
                            return RedirectToAction("MyTeam", "UserProfile");
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
        [HttpPost]
        public ActionResult SaveTeam(Team team)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            Int64 UserId = Convert.ToInt64(Session["UserId"]);
            team.SportsId = 1;
            team.Country = "India";
            team.UserId = UserId;
            DAL.SaveTeam(team);
            ModelState.AddModelError("Save Data", "Team has been created successfully.");
            return View();
        }

    }
}
