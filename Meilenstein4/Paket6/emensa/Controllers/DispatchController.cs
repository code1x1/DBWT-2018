using System;
using System.Linq;
using System.Collections.Generic;
using emensa.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using emensa.ViewModels;

namespace emensa.Controllers
{

    public class DispatchController : Controller {

        
        private readonly emensaContext _context;

        public DispatchController(emensaContext context)
        {
            _context = context;
        }

        public JsonResult Bestellungen(){
            
            try {
                List<Benutzer> hash = _context.Benutzer
                
                //.Include("BestellungEnthältMahlzeit.Mahlzeiten")
                .ToList();
                return Json(hash);
            }
            catch (Exception e) {
                var x = e.Message;
                return Json(x);
            }

            //if(Request.Headers["X-Authorize"] == "ttest"){
            //    return new AcceptedResult();
            //} else {
            //    return NotFound();
            //}


        }
        

    }
}