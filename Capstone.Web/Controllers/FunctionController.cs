﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Web.Models;
using Capstone.Web.DAL;

namespace Capstone.Web.Controllers
{
    public class FunctionController : PotholeController
    {
        private IPotholeDAL potholeDAL;

        public FunctionController(IPotholeDAL potholeDAL)
        {
            this.potholeDAL = potholeDAL;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Report", "Function");
        }

        [HttpGet]
        public ActionResult Report()
        {
            if (!base.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            List<PotholeModel> model = potholeDAL.GetAllPotholes();

            return View(model);
        }

        [HttpPost]
        public ActionResult Report(PotholeModel newPothole)
        {
            int userId = ((User)Session["user"]).UserId;
            DateTime now = DateTime.Now;

            newPothole.WhoReported = userId;
            newPothole.ReportDate = now;

            potholeDAL.ReportPothole(newPothole);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Review(string option)
        {
            if(((User)Session["user"]) == null || ((User)Session["user"]).UserType.ToLower() != "e")
            {
                return RedirectToAction("Index", "Home");
            }

            List<PotholeModel> model = potholeDAL.GetAllPotholes();
            Session["option"] = "all";

            if (option == "uninspected")
            {
                model = potholeDAL.GetPotholesUninspected();
                Session["option"] = "uninspected";
            }
            else if (option == "inspected")
            {
                model = potholeDAL.GetInspectedOnly();
                Session["option"] = "inspected";
            }
            else if (option == "inRepair")
            {
                model = potholeDAL.GetRepairsInProgress();
                Session["option"] = "inRepair";
            }
            else if (option == "complete")
            {
                model = potholeDAL.GetRepairedPotholes();
                Session["option"] = "complete";
            }

            return View("Review", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            potholeDAL.DeletePothole(id);

            return RedirectToAction("Review", "Function", new { option = Session["option"].ToString() });
        }

        [HttpPost]
        public ActionResult UndoInspect(int id)
        {
            potholeDAL.UndoInspect(id);

            return RedirectToAction("Review", "Function", new { option = Session["option"].ToString() });
        }

        [HttpPost]
        public ActionResult UndoStartRepair(int id)
        {
            potholeDAL.UndoStartRepair(id);

            return RedirectToAction("Review", "Function", new { option = Session["option"].ToString() });
        }

        [HttpPost]
        public ActionResult UndoRepairComplete(int id)
        {
            potholeDAL.UndoRepairComplete(id);

            return RedirectToAction("Review", "Function", new { option = Session["option"].ToString() });
        }



        [HttpPost]
        public ActionResult Update(int potholeId, string status, string severity, string comment)
        {

            int employeeId = ((User)Session["user"]).UserId;

            if (severity != null)
            {
                int intSeverity = Convert.ToInt32(severity);

                potholeDAL.UpdateSeverity(potholeId, intSeverity);
            }

            if (status == "inspect")
            {
                potholeDAL.UpdateInspectDate(employeeId, potholeId);
            }
            else if (status == "repairStart")
            {
                potholeDAL.UpdateStartRepairDate(potholeId);
            }
            else if (status == "repairEnd")
            {
                potholeDAL.UpdateEndRepairDate(potholeId);
            }

            return RedirectToAction("Review", "Function", new { option = Session["option"].ToString() });
        }

    }
}