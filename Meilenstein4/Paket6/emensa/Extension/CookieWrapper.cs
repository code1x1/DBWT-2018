using emensa.Controllers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using emensa.Models;
using System;

namespace emensa.Extension{

    public class CookieWrapper
    {
        private HttpRequest _request;
        private HttpResponse _response;
        private ViewDataDictionary _viewData;

        public CookieWrapper(HttpRequest request, HttpResponse response, ViewDataDictionary viewData)
        {
            _request = request;
            _response = response;
            _viewData = viewData;
        }

        internal Dictionary<string,int> addMahlzeit(Mahlzeiten mz)
        {
            string bestellung = _request.Cookies["bestellung"];
            Dictionary<string,int> bestellungDict;
            try
            {
                bestellungDict = JsonConvert.DeserializeObject<Dictionary<string,int>>(bestellung);
            }
            catch (System.Exception)
            {
                bestellungDict = new Dictionary<string,int>();
            }
            
            if(bestellung == null || !bestellungDict.ContainsKey(mz.Id.ToString()) ){
                bestellungDict[mz.Id.ToString()] = 1;
            } else{
                bestellungDict[mz.Id.ToString()] += 1;
            }
            _viewData["bestellungCounter"] = bestellungDict.Sum(x => x.Value);
            _response.Cookies.Append("bestellung",JsonConvert.SerializeObject(bestellungDict));
            return bestellungDict;
        }

        internal Dictionary<string,int> getMahlzeiten()
        {
            string bestellung = _request.Cookies["bestellung"];
            Dictionary<string,int> bestellungDict;
            try
            {
                bestellungDict = JsonConvert.DeserializeObject<Dictionary<string,int>>(bestellung);
            }
            catch (System.Exception)
            {
                bestellungDict = new Dictionary<string,int>();
            }
            
            return bestellungDict;
        }

    }

}