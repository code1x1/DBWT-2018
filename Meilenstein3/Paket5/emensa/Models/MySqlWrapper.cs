using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace emensa.Models{
    public class MySqlWrapper{
        
        public static string conString = "Server=localhost;Port=3306;Database=emensa;Uid=denis;Pwd=;";
        public MySqlWrapper(IConfiguration configuration){
            Configuration = configuration;
        }
        public static IConfiguration Configuration { get; private set; }
        
        public static string exexuteReaderPreis(string query, string column){
            string retVal = "0";
            MySqlConnection con = new MySqlConnection(conString);
            try{

                con.Open();
                MySqlCommand cmd = new MySqlCommand(query,con);
                MySqlDataReader r = cmd.ExecuteReader();
                if(r.Read()){
                    retVal = r[column].ToString();
                }

            }catch(Exception e){
                Debug.Print(e.StackTrace);
            } finally{
                con.Close();
            }
            return retVal;
        }
        
        public static List<Kategorien> exexuteReaderKategorien(string query){
            List<Kategorien> retVal = new List<Kategorien>();
            MySqlConnection con = new MySqlConnection(conString);
            try{

                con.Open();
                MySqlCommand cmd = new MySqlCommand(query,con);
                MySqlDataReader r = cmd.ExecuteReader();
                if(r.Read()){
                    Kategorien k = new Kategorien();
                    k.Bezeichnung = r["Bezeichnung"].ToString();
                    k.FkOberKategorie = Convert.ToInt32(r["FkOberKategorie"]);
                    k.Id = Convert.ToInt32(r["Id"]);

                    retVal.Add(k);
                }

            }catch(Exception e){
                Debug.Print(e.StackTrace);
            } finally{
                con.Close();
            }
            return retVal;
        }
        

        public static T exexuteScalar<T>(string query){
            T retVal = default(T);
            MySqlConnection con = new MySqlConnection(conString);
            try{

                con.Open();
                MySqlCommand cmd = new MySqlCommand(query,con);
                retVal = (T)cmd.ExecuteScalar();

            }catch(Exception e){
                Debug.Print(e.StackTrace);
            } finally{
                con.Close();
            }
            return retVal;
        }

        public static List<string> exexuteColumn(string query, string column){
            List<string> retVal = new List<string>();
            MySqlConnection con = new MySqlConnection(conString);
            try{

                con.Open();
                MySqlCommand cmd = new MySqlCommand(query,con);
                MySqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    retVal.Add((string)r[column]);
                }

            }catch(Exception e){
                Debug.Print(e.StackTrace);
            } finally{
                con.Close();
            }
            return retVal;
        }

    }
}