using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PasswordSecurity;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace emensa.Models
{
    public partial class Benutzer
    {
        public Benutzer()
        {
            FhAngehörige = new HashSet<FhAngehörige>();
            FreundeFreundNavigation = new HashSet<Freunde>();
            FreundeNutzerNavigation = new HashSet<Freunde>();
            Gäste = new HashSet<Gäste>();
        }

        

        [Key]
        public int Nummer { get; set; }
        [DisplayName("Name")]
        [Required(ErrorMessage = "Required.")]
        public string Nutzername { get; set; }
        [ScaffoldColumn(false)]
        public DateTimeOffset? LetzerLogin { get; set; }
        [DisplayName("E-Mail")]
        [Required(ErrorMessage = "Required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EMail { get; set; }
        [ScaffoldColumn(false)]
        public string Salt { get; set; }
        [ScaffoldColumn(false)]
        public string Hash { get; set; }        
        [MinLength(4,ErrorMessage="Das Passwort muss mindestens 4 Zeichen lang sein.")]
        [DisplayName("Passwort")]
        [NotMapped]
        [ScaffoldColumn(true)]
        [Required(ErrorMessage = "Required.")]
        [Category("Security")]
        [Description("Demonstrates PasswordPropertyTextAttribute.")]
        [PasswordPropertyText(true)]
        public string Password { get; set; }
        [MinLength(4,ErrorMessage="Das Passwort muss mindestens 4 Zeichen lang sein.")]
        [DisplayName("Passwort wiederholen")]
        [NotMapped]
        [ScaffoldColumn(true)]
        [Required(ErrorMessage = "Required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Category("Security")]
        [Description("Demonstrates PasswordPropertyTextAttribute.")]
        [PasswordPropertyText(true)]
        public string PasswordRepeat { get; set; }
        [ScaffoldColumn(false)]
        public DateTime AnlegeDatum { get; set; }
        public byte Aktiv { get; set; }
        [DisplayName("Vorname")]
        [Required(ErrorMessage = "Required.")]
        public string Vorname { get; set; }
        [DisplayName("Nachname")]
        [Required(ErrorMessage = "Required.")]
        public string Nachname { get; set; }
        [DateAttribute(ErrorMessage="Das angegeben Datum kann nicht verwendet werden.")]
        public DateTime? Geburtsdatum { get; set; }
        public int? Alter { get; set; }

        public virtual ICollection<FhAngehörige> FhAngehörige { get; set; }
        public virtual ICollection<Freunde> FreundeFreundNavigation { get; set; }
        public virtual ICollection<Freunde> FreundeNutzerNavigation { get; set; }
        public virtual ICollection<Gäste> Gäste { get; set; }

        internal string getRole(string con)
        {
            MySqlConnection logincon = new MySqlConnection(con);
            string role = "";
            try{
                logincon.Open();
                MySqlCommand logincmd = new MySqlCommand("LoginProcedure", logincon);
                logincmd.CommandType = System.Data.CommandType.StoredProcedure;
                logincmd.Parameters.AddWithValue("@Lname", this.Nutzername);
                MySqlDataReader loginresult = logincmd.ExecuteReader();
                if(loginresult.Read()){
                    role = loginresult["role"].ToString();
                }
            } catch(Exception e){
                Debug.Print(message: e.StackTrace);
                role = "Gast";
            } finally{
                logincon.Close();    
            }
            return role;    
        }

        public void createHashSalt(){
            lock (this)
            {
                string createhash = PasswordStorage.CreateHash(this.Password);
                string[] splittedhash = createhash.Split(':');
                string salt = splittedhash[3];
                string hash = splittedhash[4];
                this.Salt = salt;
                this.Hash = hash;
            }
        }

        public Boolean verifyPassword(string password){
            lock (this)
            {
                return PasswordStorage.VerifyPassword(password,String.Format("{0}:{1}:{2}:{3}:{4}",
                                                "sha1",
                                                "64000",
                                                Convert.ToString(18),
                                                this.Salt,
                                                this.Hash));                
            }
        }

    }

    public class DateAttribute : RangeAttribute
    {
        public DateAttribute()
            : base(typeof(DateTime), DateTime.Now.AddYears(-120).ToShortDateString(),     DateTime.Now.ToShortDateString()) { } 
    }

}
