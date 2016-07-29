using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Data;
using Windows.Web.Http;

namespace LaBiciUn
{

    public class StationsDataManager
    {
        public static string data { get; set; } = "Hola";
        /// https://msdn.microsoft.com/en-us/windows/uwp/app-settings/store-and-retrieve-app-data

        public async static Task GetData()
        {
            HttpClient http = new HttpClient();
            Uri requestUri = new Uri("http://labici.net/api-labici.php?module=parking&method=get-locations&city=MU");
            var response = await http.GetAsync(requestUri);
            var result = await response.Content.ReadAsStringAsync();
            ///TODO: manejar errores

            data = result;

        }

    }



    public class Datum
    {
        public string id_aparcamiento { get; set; }
        public string descripcion { get; set; }
        public string num_puestos { get; set; }
        public string nombrecorto { get; set; }
        public string id_poblacion { get; set; }
        public string xocupados { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string eshabilitada { get; set; }
        public string xactivo { get; set; }
        public int libres { get; set; }
        public string ocupados { get; set; }

        private const string id_aparcamientoKey = "id_aparcamiento";
        private const string descripcionKey = "descripcion";
        private const string num_puestosKey = "num_puestos";
        private const string nombrecortoKey = "nombrecorto";
        private const string id_poblacionKey = "id_poblacion";
        private const string xocupadosKey = "xocupados";
        private const string latitudeKey = "latitude";
        private const string longitudeKey = "longitude";
        private const string eshabilitadaKey = "eshabilitada";
        private const string xactivoKey = "xactivo";
        private const string libresKey = "libres";
        private const string ocupadosKey = "ocupados";


        public Datum(JsonObject jo)
        {
            id_aparcamiento = jo.GetNamedString(id_aparcamientoKey, "");
            descripcion = jo.GetNamedString(descripcionKey, "");
            num_puestos = jo.GetNamedString(num_puestosKey, "");
            nombrecorto = jo.GetNamedString(nombrecortoKey, "");
            id_poblacion = jo.GetNamedString(id_poblacionKey, "");
            xocupados = jo.GetNamedString(xocupadosKey, "");
            latitude = jo.GetNamedNumber(latitudeKey, 0.0);
            longitude = jo.GetNamedNumber(longitudeKey, 0.0);
            eshabilitada = jo.GetNamedString(eshabilitadaKey, "");
            xactivo = jo.GetNamedString(xactivoKey, "");
            libres = Convert.ToInt32(jo.GetNamedNumber(libresKey, 0));
            ocupados = jo.GetNamedString(ocupadosKey, "");
        }

        
        public override string ToString()
        {
            return "Aparcamiento " + descripcion + " " + nombrecorto + " " + longitude + " " + latitude;
        }

    }

    public class RootObject
    {
        public int status { get; set; }
        public string msg { get; set; }
        public List<Datum> data { get; set; }

        private const string statusKey = "status";
        private const string msgKey = "msg";
        private const string dataKey = "data";


        /// https://github.com/Microsoft/Windows-universal-samples/blob/master/Samples/Json/cs/User.cs


        public RootObject(string jsonstring)
        {

            data = new List<Datum>();

            JsonObject jsonObject = JsonObject.Parse(jsonstring);
            status = Convert.ToInt32(jsonObject.GetNamedNumber(statusKey));
            msg = jsonObject.GetNamedString(msgKey, "");

            if(status != 1)
            {
                CustomNotifications.displayInfoDialog("Error al descargar los datos",
                    "Status code = " + status + Environment.NewLine + "Message = " + msg +
                    Environment.NewLine + "Compuebe la conectividad o reporte el error al desarrollador.");
                return;
            }


            foreach (IJsonValue jsonValue in jsonObject.GetNamedArray(dataKey, new JsonArray()))
            {

                if (jsonValue.ValueType == JsonValueType.Object)
                {
                    data.Add(new Datum(jsonValue.GetObject()));
                }
            }

        }



        public override string ToString()
        {
            string result = "status " + status + " msg: " + msg;
            foreach (var estacion in data)
            {
                result += Environment.NewLine;
                result += estacion.ToString();
            }

            return result;
        }


    }

}
