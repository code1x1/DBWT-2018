using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace emensa.Extension{

    public static class HttpRequestExtensions
    {
        public static string BestellungCounter(HttpRequest request, ViewDataDictionary viewData, ISession session)
        {
            if (request == null)
            {
                throw new System.ArgumentNullException(nameof(request));
            }

            if (request.Cookies["bestellung" + session.GetString("user")] == null && viewData["bestellungCounter"] == null){
                return "(0)";
            }
            else if(viewData["bestellungCounter"] != null){
                return $"({viewData["bestellungCounter"]})";
            }
            else{
                Dictionary<string,int> bestellungDict;
                try
                {
                    bestellungDict = JsonConvert.DeserializeObject<Dictionary<string,int>>(request.Cookies["bestellung" + session.GetString("user")]);
                }
                catch (System.Exception)
                {
                    bestellungDict = new Dictionary<string,int>();
                }
                return $"({bestellungDict.Sum(x => x.Value)})";
            }
        }
    }

}