using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avansight.Domain
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

    public class PatientService
    {
        DataAccessService das = null;
        public PatientService()
        {
            if(das == null)
                das = DataAccessService.getInstance();
        }
        public List<Patient> patientSet(string data)
        {
            GeneratePatients generatePatients = JsonConvert.DeserializeObject<GeneratePatients>(data);
            int generateMaleNumbers = Convert.ToInt16((generatePatients.male / (generatePatients.male + generatePatients.female)) * generatePatients.size);
            int generateFemaleNumbers = Convert.ToInt16(generatePatients.size - generateMaleNumbers);
            int totalAgeNumbers = Convert.ToInt16(generatePatients.age20 + generatePatients.age30 + generatePatients.age40 + generatePatients.age50 + generatePatients.age60);
            int generateMaleAge20Numbers = Convert.ToInt16((generatePatients.age20 / totalAgeNumbers) * generateMaleNumbers);
            int generateMaleAge30Numbers = Convert.ToInt16((generatePatients.age30 / totalAgeNumbers) * generateMaleNumbers);
            int generateMaleAge40Numbers = Convert.ToInt16((generatePatients.age40 / totalAgeNumbers) * generateMaleNumbers);
            int generateMaleAge50Numbers = Convert.ToInt16((generatePatients.age50 / totalAgeNumbers) * generateMaleNumbers);
            int generateMaleAge60Numbers = generateMaleNumbers - generateMaleAge20Numbers - generateMaleAge30Numbers - generateMaleAge40Numbers - generateMaleAge50Numbers;

            List<Patient> patientList = new List<Patient>();
            Random _random = new Random();
            int patientId = 1;
            for (int j = 0; j < generateMaleAge20Numbers; j++)
            {
                Patient patient = new Patient();
                patient.Gender = "Male";
                patient.Age = _random.Next(21, 30);
                patient.PatientId = patientId;
                patientList.Add(patient);
                patientId++;
            }
            for (int j = 0; j < generateMaleAge30Numbers; j++)
            {
                Patient patient = new Patient();
                patient.Gender = "Male";
                patient.Age = _random.Next(31, 40);
                patient.PatientId = patientId;
                patientList.Add(patient);
                patientId++;
            }
            for (int j = 0; j < generateMaleAge40Numbers; j++)
            {
                Patient patient = new Patient();
                patient.Gender = "Male";
                patient.Age = _random.Next(41, 50);
                patient.PatientId = patientId;
                patientList.Add(patient);
                patientId++;
            }
            for (int j = 0; j < generateMaleAge50Numbers; j++)
            {
                Patient patient = new Patient();
                patient.Gender = "Male";
                patient.Age = _random.Next(51, 60);
                patient.PatientId = patientId;
                patientList.Add(patient);
                patientId++;
            }
            for (int j = 0; j < generateMaleAge60Numbers; j++)
            {
                Patient patient = new Patient();
                patient.Gender = "Male";
                patient.Age = _random.Next(61, 70);
                patient.PatientId = patientId;
                patientList.Add(patient);
                patientId++;
            }
            int generateFemaleAge20Numbers = Convert.ToInt16((generatePatients.age20 / totalAgeNumbers) * generateFemaleNumbers);
            int generateFemaleAge30Numbers = Convert.ToInt16((generatePatients.age30 / totalAgeNumbers) * generateFemaleNumbers);
            int generateFemaleAge40Numbers = Convert.ToInt16((generatePatients.age40 / totalAgeNumbers) * generateFemaleNumbers);
            int generateFealeAge50Numbers = Convert.ToInt16((generatePatients.age50 / totalAgeNumbers) * generateFemaleNumbers);
            int generateFealeAge60Numbers = generateFemaleNumbers - generateFemaleAge20Numbers - generateFemaleAge30Numbers - generateFemaleAge40Numbers - generateFealeAge50Numbers;
            //List<Patient> femalePatientList = new List<Patient>();
            for (int j = 0; j < generateFemaleAge20Numbers; j++)
            {
                Patient patient = new Patient();
                patient.Gender = "Female";
                patient.Age = _random.Next(21, 30);
                patient.PatientId = patientId;
                patientList.Add(patient);
                patientId++;
            }
            for (int j = 0; j < generateFemaleAge30Numbers; j++)
            {
                Patient patient = new Patient();
                patient.Gender = "Female";
                patient.Age = _random.Next(31, 40);
                patient.PatientId = patientId;
                patientList.Add(patient);
                patientId++;
            }
            for (int j = 0; j < generateFemaleAge40Numbers; j++)
            {
                Patient patient = new Patient();
                patient.Gender = "Female";
                patient.Age = _random.Next(41, 50);
                patient.PatientId = patientId;
                patientList.Add(patient);
                patientId++;
            }
            for (int j = 0; j < generateFealeAge50Numbers; j++)
            {
                Patient patient = new Patient();
                patient.Gender = "Female";
                patient.Age = _random.Next(51, 60);
                patient.PatientId = patientId;
                patientList.Add(patient);
                patientId++;
            }
            for (int j = 0; j < generateFealeAge60Numbers; j++)
            {
                Patient patient = new Patient();
                patient.Gender = "Female";
                patient.Age = _random.Next(61, 70);
                patient.PatientId = patientId;
                patientList.Add(patient);
                patientId++;
            }
            //SessionHelper.SetObjectAsJson(HttpContext.Session, "maleList", malePatientList);
            //SessionHelper.SetObjectAsJson(HttpContext.Session, "femaleList",femalePatientList );
            //List<Patient> maleList = SessionHelper.GetObjectFromJson<List<Patient>>(HttpContext.Session, "maleList");
            List<Patient> pList =  das.patientSet(patientList);
            return pList;
        }
        public List<Patient> getPatients()
        {
            return das.getPatients();
        }
        public int saveRecord(List<Patient> patients)
        {
            Random _random = new Random();
            List<TreatmentReading> treatmentReadings = new List<TreatmentReading>();
            foreach (var patient in patients)
            {
                var patientId = patient.PatientId;
                if (patientId % 3 == 0)
                {
                    for(int i =1;i<=4;i++)
                    {
                        TreatmentReading treatmentReading = new TreatmentReading();
                        treatmentReading.PatientId = patientId;
                        int reading = _random.Next(1, 4);
                        treatmentReading.VisitWeek = "V" + i;
                        treatmentReading.Reading = Math.Round(reading + _random.NextDouble(), 2);
                        treatmentReadings.Add(treatmentReading);
                    }
                }
                if (patientId % 3 == 1)
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        TreatmentReading treatmentReading = new TreatmentReading();
                        treatmentReading.PatientId = patientId;
                        int reading = _random.Next(1, 7);
                        treatmentReading.VisitWeek = "V" + i;
                        treatmentReading.Reading = Math.Round(reading + _random.NextDouble(), 2);
                        treatmentReadings.Add(treatmentReading);
                    }
                    
                }
                if (patientId % 3 == 2)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        TreatmentReading treatmentReading = new TreatmentReading();
                        treatmentReading.PatientId = patientId;
                        int reading = _random.Next(1, 10);
                        treatmentReading.VisitWeek = "V" + i;
                        treatmentReading.Reading = Math.Round(reading + _random.NextDouble(), 2);
                        treatmentReadings.Add(treatmentReading);
                    }
                }
                
            }
            int ret = das.SaveRecord(treatmentReadings);
            return ret;
        }
        public List<TreatmentReading> getTreatmentReading()
        {
            return das.getTreatmentReadings();
        }
    }
}
