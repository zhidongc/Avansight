using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Avansight.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.NodeServices;

namespace Avansight.WEB.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult PatientGet(string data)
        {
            PatientService patientService = new PatientService();
            List<Patient> patients = patientService.getPatients();
            return Json(patients);
        }
        [HttpPost]
        public JsonResult PatientSet(string data)
        {
            PatientService patientService = new PatientService();
            List<Patient> patientList =  patientService.patientSet(data);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "patientList", patientList);
            return Json(patientList);
        }
        [HttpPost]
        public JsonResult SaveRecord(string data)
        {
            List<Patient> patients = SessionHelper.GetObjectFromJson<List<Patient>>(HttpContext.Session, "patientList");
            PatientService patientService = new PatientService();
            int treatmentReadingIdList = patientService.saveRecord(patients);
            return Json(treatmentReadingIdList);
        }
        [HttpPost]
        public JsonResult TreatmentReadingGet(string data)
        {
            PatientService patientService = new PatientService();
            List<TreatmentReading> treatmentReadings = patientService.getTreatmentReading();
            return Json(treatmentReadings);
        }
    }
}