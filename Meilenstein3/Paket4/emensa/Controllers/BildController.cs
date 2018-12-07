using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MyApp.Namespace
{
    public class BildController : Controller
    {

        public string imShow(byte[] data, int type)
        {
                //data:[<mime type>][;charset=<Zeichensatz>][;base64],<Daten>
                string b64 = Convert.ToBase64String(data, 0, data.Length);
                switch(type)
                {
                        case 0: //jpg
                                return "data:image/jpg;base64," + b64;
        
                        case 1: //png
                                return "data:image/png;base64," + b64;
        
                        case 2: //bmp
                                return "data:image/bmp;base64," + b64;
        
                        case 4: //gif
                                return "data:image/gif;base64," + b64;
        
                        default:
                                return "data:image/jpg;base64," + b64;
                }
                
        }

        // 
        // GET: /Bild/id
        public FileContentResult Index(int id)
        {
            byte[] bindaten = null;
            MySqlConnection con = new MySqlConnection("Server=localhost;Port=3306;Database=emensa;Uid=denis;Pwd=;");

            try
            {
                con.Open();
                String query = @"SELECT b.`Binärdaten` 
                                FROM Bilder b
                                WHERE b.ID=" + id;

                MySqlCommand command = new MySqlCommand(query, con);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                bindaten = (byte[])reader["Binärdaten"];
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {

                con.Close();

            }

            return File(bindaten, "image/png");
        }
    }
}
