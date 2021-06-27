using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Avansight.Domain
{

    public class DataAccessService
    {
        private static string connStr;
        SqlTransaction transaction;
        SqlConnection conn;
        static DataAccessService instance;
        public DataAccessService()
        {   
        }
        public static DataAccessService getInstance()
        {
            if (instance == null)
            {
                var projectPath = String.Format("{0}\\{1}", System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

                var config = new ConfigurationBuilder()
                    .SetBasePath(projectPath)
                    .AddJsonFile("appsettings.json").Build();
                connStr = config.GetConnectionString("DefaultConnection");
                instance = new DataAccessService();
            }
            return instance;
        }
        public List<Patient> patientSet(List<Patient> patients)
        {
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                transaction = conn.BeginTransaction();

                var deletePatientSql = "delete from TreatmentReading";
                conn.Execute(deletePatientSql, null, transaction);

                //Delete TreatmentReading
                var TreatmentReadingSql = "delete from Patient";
                conn.Execute(TreatmentReadingSql, null, transaction);

                //begin set Patient
                var command = conn.CreateCommand();
                var json = JsonConvert.SerializeObject(patients);
                DataTable table = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                command.CommandText = "dbo.PatientSet";
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;
                var parameters = command.CreateParameter();
                parameters.TypeName = "dbo.PatientTableType";
                parameters.Value = table;
                parameters.ParameterName = "@Patients";
                command.Parameters.Add(parameters);
                SqlDataReader reader;
                reader = command.ExecuteReader();
                patients.Clear();
                while (reader.Read())
                {
                    IDataRecord record = (IDataRecord)reader;
                    Patient patient = new Patient();
                    patient.PatientId = (int)record[0];
                    patient.Age = (Int16)record[1];
                    patient.Gender = (string)record[2];
                    patients.Add(patient);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                throw ex;
            }
            // Easy way not use Procedure
            //int ret = conn.Execute("INSERT INTO patient( Age, Gender) VALUES(@Age, @Gender)", patients);
            return patients;
        }
        public List<Patient> getPatients()
        {
            if (transaction != null)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }  
            }
            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                conn.Open();
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            List<Patient> patientList = new List<Patient>();
            var multi = conn.QueryMultiple("exec PatientGet");
            patientList = multi.Read<Patient>().AsList();
            conn.Close();
            return patientList;
        }
        public int SaveRecord(List<TreatmentReading> treatmentReadings)
        {   
            try
            {
                var json = JsonConvert.SerializeObject(treatmentReadings);
                var table = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                var command = conn.CreateCommand();
                var parameters = command.CreateParameter();
                parameters.TypeName = "dbo.TreatmentReadingTableType";
                parameters.Value = table;
                parameters.ParameterName = "@TreatmentReadings";
                command.CommandText = "dbo.TreatmentReadingSet";
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;
                command.Parameters.Add(parameters);
                List<int> treatmentReadingIdList = new List<int>();
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
                // if it was successful, commit the transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                throw ex;
            }
            conn.Close();
            return 0;
        }
        public List<TreatmentReading> getTreatmentReadings()
        {
            if (transaction != null)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            List<TreatmentReading> treatmentReadingList = new List<TreatmentReading>();
            var multi = conn.QueryMultiple("exec TreatmentReadingGet");
            treatmentReadingList = multi.Read<TreatmentReading>().AsList();
            conn.Close();
            return treatmentReadingList;
        }
    }

}
