using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;
using Windows.Web.Http;

namespace LaBiciUn
{

    public class Ciudad
    {
        public string Tag { get; set; }
        public string Nombre { get; set; }
        public Geopoint Centro { get; set; }
    }


    public class StationsDataManager
    {
        private static string city;
        public static string City
        {
            get
            {
                return city;
            }

            set
            {
                city = value; 
                Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["city"] = value;
            }
        }

        public static string remoteJsonData { get; set; } = "{\"status\":1,\"msg\":\"No connection\",\"data\":[]}";
        /// https://msdn.microsoft.com/en-us/windows/uwp/app-settings/store-and-retrieve-app-data
        /// 

        public static RootObject parsedJson { get; set; }


        public static Dictionary<string, Ciudad> ciudades = new Dictionary<string, Ciudad>()
        {
            { "MU", new Ciudad { Tag="MU", Nombre="Murcia", Centro=new Geopoint(new BasicGeoposition()
                {
                    Latitude = 37.991985,
                    Longitude = -1.129554
                }) }
            },

            { "PO", new Ciudad { Tag="PO", Nombre="Ponferrada", Centro=new Geopoint(new BasicGeoposition()
                {
                    Latitude = 42.5475641,
                    Longitude = -6.600071
                }) }
            },

            { "BE", new Ciudad { Tag="BE", Nombre="Benidorm", Centro=new Geopoint(new BasicGeoposition()
                {
                    Latitude = 38.5392324,
                    Longitude = -0.1293277
                }) }
            },

            { "AL", new Ciudad { Tag="AL", Nombre="Altea", Centro=new Geopoint(new BasicGeoposition()
                {
                    Latitude = 38.602354,
                    Longitude = -0.0474582
                }) }
            },

            { "MA", new Ciudad { Tag="MA", Nombre="Majadahonda", Centro=new Geopoint(new BasicGeoposition()
                {
                    Latitude = 40.4666036,
                    Longitude = -3.8673309
                }) }
            },

            { "GA", new Ciudad { Tag="GA", Nombre="Gandía", Centro=new Geopoint(new BasicGeoposition()
                {
                    Latitude = 38.9696481,
                    Longitude = -0.180044
                }) }
            },

            { "GE", new Ciudad { Tag="GE", Nombre="Getafe", Centro=new Geopoint(new BasicGeoposition()
                {
                    Latitude = 40.3101097,
                    Longitude = -3.7332323
                }) }
            },

            { "VI", new Ciudad { Tag="VI", Nombre="Vilagarcía de Arousa", Centro=new Geopoint(new BasicGeoposition()
                {
                    Latitude = 42.5962671,
                    Longitude = -8.7674612
                }) }
            }

        };



        public async static Task GetData()
        {

            try
            {
                HttpClient http = new HttpClient();
                Uri requestUri = new Uri("http://labici.net/api-labici.php?module=parking&method=get-locations&city="+City);


                var response = await http.GetAsync(requestUri);
                var result = await response.Content.ReadAsStringAsync();

                remoteJsonData = result;
            }
            catch (Exception)
            {

                CustomNotifications.displayInfoDialog("Error de conexión", "No se pudo descargar los datos de las estaciones.\nCompruebe su conexión a Internet y pruebe a recargar o cambiar de página para ver los cambios.");
            }
            finally
            {
                parsedJson = new RootObject(remoteJsonData);
            }

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
        public bool eshabilitada { get; set; }
        public bool xactivo { get; set; }
        public int libres { get; set; }
        public string ocupados { get; set; }
        public Geopoint posicion { get; set; }

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
            eshabilitada = jo.GetNamedString(eshabilitadaKey, "") == "0" ? false : true;
            xactivo = jo.GetNamedString(xactivoKey, "") == "0" ? false : true;
            libres = Convert.ToInt32(jo.GetNamedNumber(libresKey, 0));
            ocupados = jo.GetNamedString(ocupadosKey, "");

            posicion = new Geopoint(new BasicGeoposition()
            {
                Latitude = latitude,
                Longitude = longitude
            });
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
