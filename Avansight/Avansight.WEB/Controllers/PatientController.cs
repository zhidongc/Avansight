using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Avansight.WEB.Controllers
{
    public class GeneratePatients
    {
        public float size { get; set; }
        public float male { get; set; }
        public float female { get; set; }
        public float age20 { get; set; }
        public float age30 { get; set; }
        public float age40 { get; set; }
        public float age50 { get; set; }
        public float age60 { get; set; }
    }
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult JsonGeneratePatients(string data)
        {
            GeneratePatients generatePatients = JsonConvert.DeserializeObject<GeneratePatients>(data);
            float generateMaleNumbers =( generatePatients.male / (generatePatients.male + generatePatients.female) ) * generatePatients.size;
            float generateFemaleNumbers = generatePatients.size - generateMaleNumbers;
            float totalAgeNumbers = generatePatients.age20 + generatePatients.age30 + generatePatients.age40 + generatePatients.age50 + generatePatients.age60;
            float generateAge20Numbers = (generatePatients.age20 / totalAgeNumbers) * generatePatients.size;
            float generateAge30Numbers = (generatePatients.age30 / totalAgeNumbers) * generatePatients.size;
            float generateAge40Numbers = (generatePatients.age40 / totalAgeNumbers) * generatePatients.size;
            float generateAge50Numbers = (generatePatients.age50 / totalAgeNumbers) * generatePatients.size;
            float generateAge60Numbers = (generatePatients.age60 / totalAgeNumbers) * generatePatients.size;

            return Json(data);
        }
    }
}