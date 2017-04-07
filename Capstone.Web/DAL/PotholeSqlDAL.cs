﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Capstone.Web.Models;
using System.Data.SqlClient;

namespace Capstone.Web.DAL
{
    public class PotholeSqlDAL : IPotholeDAL
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["potholeDB"].ConnectionString;

        public bool ReportPothole(PotholeModel newPothole)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand addPothole = new SqlCommand($"INSERT INTO pothole(longitude, latitude, whoReported, reportDate) VALUES(@longitude, @latitude, @whoReported, @reportDate);", conn);

                    addPothole.Parameters.AddWithValue("@longitude", newPothole.Longitude);
                    addPothole.Parameters.AddWithValue("@latitude", newPothole.Latitude);
                    addPothole.Parameters.AddWithValue("@whoReported", newPothole.WhoReported);
                    addPothole.Parameters.AddWithValue("@reportDate", newPothole.ReportDate);
          

                    int result = addPothole.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return false;
        }

        public List<PotholeModel> GetAllPotholes()
        {
            List<PotholeModel> potholeList = new List<PotholeModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand($"SELECT * FROM pothole WHERE repairEndDate > GETDATE()- 180 OR repairEndDate IS NULL;", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PotholeModel ph = new PotholeModel();

                        int potholeID = Convert.ToInt32(reader["potholeID"]);
                        double longitude = Convert.ToDouble(reader["longitude"]);
                        double latitude = Convert.ToDouble(reader["latitude"]);
                        int whoReported = Convert.ToInt32(reader["whoReported"]);
                        int whoInspected = (reader["whoInspected"] != DBNull.Value) ? Convert.ToInt32(reader["whoInspected"]) : -1;
                        string picture = Convert.ToString(reader["picture"]);
                        DateTime reportDate = Convert.ToDateTime(reader["reportDate"]);

                        DateTime? inspectDate = null;
                        if (reader["inspectDate"] != DBNull.Value)
                        {
                            inspectDate = Convert.ToDateTime(reader["inspectDate"]);
                        }

                        DateTime? repairStartDate = null;
                        if (reader["repairStartDate"] != DBNull.Value)
                        {
                            inspectDate = Convert.ToDateTime(reader["repairStartDate"]);
                        }

                        DateTime? repairEndDate = null;
                        if (reader["repairEndDate"] != DBNull.Value)
                        {
                            inspectDate = Convert.ToDateTime(reader["repairEndDate"]);
                        }

                        int severity = (reader["severity"] != DBNull.Value) ? Convert.ToInt32(reader["severity"]) : -1;
                        string comment = Convert.ToString(reader["comment"]);

                        ph.PotholeID = potholeID;
                        ph.Longitude = longitude;
                        ph.Latitude = latitude;
                        ph.WhoReported = whoReported;
                        ph.WhoInspected = whoInspected;
                        ph.Picture = picture;
                        ph.ReportDate = reportDate;
                        ph.InspectDate = inspectDate;
                        ph.RepairStartDate = repairStartDate;
                        ph.RepairEndDate = repairEndDate;
                        ph.Severity = severity;
                        ph.Comment = comment;

                        potholeList.Add(ph);
                    }
                    return potholeList;
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public PotholeModel GetOnePothole(string id)
        {
            PotholeModel pothole = new PotholeModel();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand($"SELECT * FROM pothole WHERE potholeId=@pothId", conn);
                    cmd.Parameters.AddWithValue("@pothId", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        
                        int potholeID = Convert.ToInt32(reader["potholeID"]);
                        double longitude = Convert.ToDouble(reader["longitude"]);
                        double latitude = Convert.ToDouble(reader["latitude"]);
                        int whoReported = Convert.ToInt32(reader["whoReported"]);
                        int whoInspected = Convert.ToInt32(reader["whoInspected"]);
                        string picture = Convert.ToString(reader["picture"]);
                        DateTime reportDate = Convert.ToDateTime(reader["reportDate"]);
                        DateTime inspectDate = Convert.ToDateTime(reader["inspectDate"]);
                        DateTime repairStartDate = Convert.ToDateTime(reader["repairStartDate"]);
                        DateTime repairEndDate = Convert.ToDateTime(reader["repairEndDate"]);
                        int severity = Convert.ToInt32(reader["severity"]);
                        string comment = Convert.ToString(reader["comment"]);

                        pothole.PotholeID = potholeID;
                        pothole.Longitude = longitude;
                        pothole.Latitude = latitude;
                        pothole.WhoReported = whoReported;
                        pothole.WhoInspected = whoInspected;
                        pothole.Picture = picture;
                        pothole.ReportDate = reportDate;
                        pothole.InspectDate = inspectDate;
                        pothole.RepairStartDate = repairStartDate;
                        pothole.RepairEndDate = repairEndDate;
                        pothole.Severity = severity;
                        pothole.Comment = comment;
                      
                    }
                    return pothole;
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }



    }
}