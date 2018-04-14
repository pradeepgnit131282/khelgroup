using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KhelGroup.Controllers
{
    public class ScoreBoardController : Controller
    {
        //
        // GET: /ScoreBoard/

        public ActionResult ViewScoreBoard()
        {
            return View();
        }
        public ActionResult CreateScoreBoard()
        {
            return View();
        }       

    }
}
