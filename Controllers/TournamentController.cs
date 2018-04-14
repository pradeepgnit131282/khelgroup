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
    public class TournamentController : Controller
    {
        //
        // GET: /Tournament/

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

    }
}
