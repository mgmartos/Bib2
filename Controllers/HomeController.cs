using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Net.Http;

//using System.Runtime.Serialization.json
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Bib2.Models;


namespace Bib2.Controllers
{
    public class HomeController : Controller
    {
        private static string Token = "";
        string WSRest = "http://localhost:1722/api/";// System.Web.Configuration.WebConfigurationManager.AppSettings["RestWS"].ToString();


        public ActionResult Index()
        {

            var request = (HttpWebRequest)WebRequest.Create(WSRest + "biblos/inicio");
            string json = GetWS(request);
            JArray jsonArray = JArray.Parse(json);
            List<string> cantidades = jsonArray.ToObject<List<string>>();

            ViewData["CantidadLibros"] = cantidades[0];
            ViewData["CantidadAutores"] = cantidades[1];
            ViewData["CantidadEditoriales"] = cantidades[2];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //public ActionResult Alta()
        //{
        //    var request = (HttpWebRequest)WebRequest.Create(WSRest + "biblosauth/todos");
        //    string json = GetWS(request);
        //    JArray jsonArray = JArray.Parse(json);
        //    List<Autores> autores = jsonArray.ToObject<List<Autores>>();

        //    ViewBag.autores = autores.OrderBy(a => a.NombreAutor).Select(a =>
        //                 new System.Web.Mvc.SelectListItem { /*Selected = (a.idAutor == idAutor),*/ Value = a.idAutor.ToString(), Text = a.NombreAutor.ToString() });
        //    request = null;
        //    request = (HttpWebRequest)WebRequest.Create(WSRest + "biblos/temas");
        //    json = GetWS(request);
        //    jsonArray = null;
        //    jsonArray = JArray.Parse(json);
        //    List<string> temas = jsonArray.ToObject<List<string>>();
        //    ViewBag.temas = temas.Select(t => new System.Web.Mvc.SelectListItem { /*Selected = (a.idAutor == idAutor),*/ Value = t.ToString(), Text = t.ToString() });
        //    return View();
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alta([Bind(Include = "idLibro,titulo,autor,editorial,coleccion,fecha,tema,calificacion,paginas,comentario,prestamo,fecha_prestamo,numobras,origen,CodAutor")] mlib nuevoLibro)
        {

            if (string.IsNullOrEmpty(nuevoLibro.CodAutor.ToString()))
            {
                // 
            }
            string xdate = "";
            string xdatep = "";
            if (nuevoLibro.fecha.HasValue)
            {
                xdate = ((DateTime)nuevoLibro.fecha).ToString("u");     // "2020-03-22 23:59:59"; 
            }
            if (nuevoLibro.fecha_prestamo.HasValue)
            {
                xdatep = ((DateTime)nuevoLibro.fecha_prestamo).ToString("u");     // "2020-03-22 23:59:59"; 
            }



            string jsonLibro = "{\"idLibro\":\"" + nuevoLibro.idLibro + "\"," +
                               "\"titulo\":\"" + nuevoLibro.titulo + "\"," +
                               "\"autor\":\"" + nuevoLibro.autor + "\"," +
                               "\"editorial\":\"" + nuevoLibro.editorial + "\"," +
                               "\"coleccion\":\"" + nuevoLibro.coleccion + "\"," +
                               //"\"fecha\":\"" + nuevoLibro.fecha + "\"," +
                               "\"fecha\":\"" + xdate + "\"," +
                               "\"tema\":\"" + nuevoLibro.tema + "\"," +
                               "\"calificacion\":\"" + nuevoLibro.calificacion + "\"," +
                               "\"paginas\":\"" + nuevoLibro.paginas + "\"," +
                               "\"comentario\":\"" + nuevoLibro.comentario + "\"," +
                               "\"prestamo\":\"" + nuevoLibro.prestamo + "\"," +
                               "\"fecha_prestamo\":\"" + xdatep + "\"," +
                               "\"origen\":\"" + nuevoLibro.origen + "\"," +
                               "\"CodAutor\":\"" + nuevoLibro.CodAutor + "\"}";

            string ret = PostWS("biblos/altalibro", "letra=" + jsonLibro);
            //JArray jsonArray = JArray.Parse(ret);
            string letra = nuevoLibro.titulo.Substring(0, 1);

            //return RedirectToAction("Libros", "Home", "letra=" + letra);
            return Redirect("Libros?letra=" + letra);
        }

        public ActionResult Alta(int codigo = 0)
        {
            var request = (HttpWebRequest)WebRequest.Create(WSRest + "biblosauth/todos");
            string json = GetWS(request);
            JArray jsonArray = JArray.Parse(json);
            List<Autores> autores = jsonArray.ToObject<List<Autores>>();

            //ViewBag.autores = autores.OrderBy(a => a.NombreAutor).Select(a =>
            //             new System.Web.Mvc.SelectListItem { Selected = (a.idAutor == idAutor), Value = a.idAutor.ToString(), Text = a.NombreAutor.ToString() });
            request = null;
            request = (HttpWebRequest)WebRequest.Create(WSRest + "biblos/temas");
            json = GetWS(request);
            jsonArray = null;
            jsonArray = JArray.Parse(json);
            List<string> temas = jsonArray.ToObject<List<string>>();
            ViewBag.Modificar = 0;
            if (codigo > 0)
            {
                request = null;
                request = (HttpWebRequest)WebRequest.Create(WSRest + "biblos/libro?id=" + codigo.ToString());
                json = GetWS(request);
                JObject js = JObject.Parse(json);
                mlib libro = js.ToObject<mlib>();
                // List<string> temas = jsonArray.ToObject<List<string>>();
                ViewBag.temas = temas.Select(t => new System.Web.Mvc.SelectListItem { Selected = (t.ToString().Trim() == libro.tema.Trim()), Value = t.ToString(), Text = t.ToString() });
                ViewBag.autores = autores.OrderBy(a => a.NombreAutor).Select(a =>
                new System.Web.Mvc.SelectListItem { Selected = (a.idAutor == libro.CodAutor), Value = a.idAutor.ToString(), Text = a.NombreAutor.ToString() });

                ViewBag.Modificar = 1;
                return View(libro);
            }
            else
            {
                ViewBag.temas = temas.Select(t => new System.Web.Mvc.SelectListItem { /*Selected = (t.ToString() == idAutor),*/ Value = t.ToString(), Text = t.ToString() });
                ViewBag.autores = autores.OrderBy(a => a.NombreAutor).Select(a =>
                        new System.Web.Mvc.SelectListItem { /*Selected = (a.idAutor == libro.CodAutor),*/ Value = a.idAutor.ToString(), Text = a.NombreAutor.ToString() });

                return View(new mlib());
            }

        }


        [HttpPost]
        public ActionResult AltaAutor([Bind(Include = "idAutor,NombreAutor")] Autores nuevoAutor)
        {
            string jsonAutor = "{\"idAutor\":\"" + nuevoAutor.idAutor + "\"," +
                               "\"NombreAutor\":\"" + nuevoAutor.NombreAutor + "\"}";

            string ret = PostWS("biblosauth/altaAutor", "letra=" + jsonAutor);
            //JArray jsonArray = JArray.Parse(ret);
            string letra = nuevoAutor.NombreAutor.Substring(0, 1);

            //return RedirectToAction("Libros", "Home", "letra=" + letra);
            return Redirect("Index");
        }

        public ActionResult AltaAutor(int codigo = 0)
        {
            ViewBag.Modificar = 0;
            return View(new Autores());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AltaLectura([Bind(Include = "titulo,autor,CodAutor,fecha,calificacion,comentario,Ebook")] Lecturas nuevaLectura)
        {
            string xdate = "";
            if (nuevaLectura.fecha.HasValue)
            {
                xdate = ((DateTime)nuevaLectura.fecha).ToString("u");     // "2020-03-22 23:59:59"; 
            }
            string jsonLibro = "{\"titulo\":\"" + nuevaLectura.titulo + "\"," +
                               "\"autor\":\"" + nuevaLectura.autor + "\"," +
                               "\"CodAutor\":\"" + nuevaLectura.CodAutor + "\"," +
                              "\"fecha\":\"" + xdate + "\"," +
                               "\"calificacion\":\"" + nuevaLectura.calificacion + "\"," +
                               "\"comentario\":\"" + nuevaLectura.comentario + "\"," +
            //"\"EBook\":\" \"}";
                                "\"EBook\":\"" + nuevaLectura.Ebook + "\"}";

            string ret = PostWS("Lecturas/altalectura", "letra=" + jsonLibro);
            string letra = nuevaLectura.titulo.Substring(0, 1);
            return Redirect("AltaLectura");
        }
        public ActionResult AltaLectura()
        {
            ViewBag.Modificar = 0;
            var request = (HttpWebRequest)WebRequest.Create(WSRest + "biblosauth/todos");
            string json = GetWS(request);
            JArray jsonArray = JArray.Parse(json);
            List<Autores> autores = jsonArray.ToObject<List<Autores>>();
            ViewBag.autores = autores.OrderBy(a => a.NombreAutor).Select(a =>
                 new System.Web.Mvc.SelectListItem { /*Selected = (a.idAutor == libro.CodAutor),*/ Value = a.idAutor.ToString(), Text = a.NombreAutor.ToString() });


            return View(new Bib2.Models.Lecturas());
        }

        [HttpGet]
        public ActionResult ListaLecturas()
        {
            var request = (HttpWebRequest)WebRequest.Create(WSRest + "lecturas/todos");
            string json = GetWS(request);
            JArray jsonArray = JArray.Parse(json);
            List<Lecturas> lecturas = jsonArray.ToObject<List<Lecturas>>();
            return View(lecturas);
        }


        public ActionResult autores(char letra='A')
        {
            //var request = (HttpWebRequest)WebRequest.Create(WSRest + "biblosauth/autorletra/" + letra);
            //string json = GetWS(request);
            //JArray jsonArray = JArray.Parse(json);
            //List<Autores> cantidades = jsonArray.ToObject<List<Autores>>();
            //return View();
            string ret = PostWS("biblosauth/autorletralibros", "letra=" + letra);
            JArray jsonArray = JArray.Parse(ret);
            List<AutoresL> autors = jsonArray.ToObject<List<AutoresL>>();
            //ViewData["listaAutores"] = autors;
            return View(autors);

        }

        public ActionResult LibrosAutor(int codigo)
        {
             string ret = PostWS("biblosauth/librosautor", "letra=" + codigo.ToString());
            JArray jsonArray = JArray.Parse(ret);
            List<mlib> libros = jsonArray.ToObject<List<mlib>>();
            ViewData["CodAutor"] = codigo;
            ViewData["NomAutor"] = libros[0].autor;
             return View(libros);
        }

        public PartialViewResult _LibrosAutor(int codigo)
        {
            string ret = PostWS("biblosauth/librosautor", "letra=" + codigo.ToString());
            JArray jsonArray = JArray.Parse(ret);
            List<mlib> libros = jsonArray.ToObject<List<mlib>>();
            ViewData["CodAutor"] = codigo;
            ViewData["NomAutor"] = libros[0].autor;
            return PartialView(libros);
            //return (this.LibrosAutor(codigo));
        }


        public ActionResult Libros(string letra="A")
        {
            string ret = PostWS("biblos/LibrosLetra", "letra=" + letra.ToString());
            JArray jsonArray = JArray.Parse(ret);
            //List<mlibU> libros = jsonArray.ToObject<List<mlibU>>();
            List<mlibU> libros = new List<mlibU>();
            for (int i=0;i<jsonArray.Count;i++)
            {
                mlib l = jsonArray[i]["c"].ToObject<mlib>();
                mlibU u = new mlibU();
                u.libro = l;
                int nums = 0;
                int.TryParse(jsonArray[i]["urls"].ToString(), out nums);
                u.numurls = nums;
                libros.Add(u);
            }
            return View(libros);
        }


        private Bib2.Models.mlib Obtener_Libro(int codigo)
        {
            var request = (HttpWebRequest)WebRequest.Create(WSRest + "biblos/libro?id=" + codigo.ToString());
            string json = GetWS(request);
            //JArray jsonArray = JArray.Parse(json);
            JObject js = JObject.Parse(json);
            mlib libro = js.ToObject<mlib>();
            return libro;
        }
        private Bib2.Models.Autores Obtener_Autor(int codigo)
        {
            var request = (HttpWebRequest)WebRequest.Create(WSRest + "biblosauth/autor?id=" + codigo.ToString());
            string json = GetWS(request);
            //JArray jsonArray = JArray.Parse(json);
            JObject js = JObject.Parse(json);
            Autores autor = js.ToObject<Autores>();
            return autor;
        }

        public PartialViewResult _Libro(int  codigo)
        {
            mlib libro = Obtener_Libro(codigo);

            return PartialView(libro);
        }

        public PartialViewResult _Links(int tipo, int codigo)
        {
            var request2 = (HttpWebRequest)WebRequest.Create(WSRest + "biblos/enlaces?tipo=" + tipo.ToString()+"&padre=" + codigo.ToString());
            string json2 = GetWS(request2);

            JArray jsonArray = JArray.Parse(json2);
            List<Urls> links = jsonArray.ToObject<List<Urls>>();

            return PartialView(links);
        }

        public PartialViewResult AltaLink(int tipo, int codigo_padre)
        {
            ViewData["tipo"] = tipo.ToString();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AltaLink([Bind(Include = "tipo,codigo_padre,direccion,descripcion")] Urls nuevoLink)
        {
            string jsonLink = "{\"tipo\":\"" + nuevoLink.tipo + "\"," +
                              "\"codigo_padre\":\"" + nuevoLink.codigo_padre + "\"," +
                              "\"direccion\":\"" + nuevoLink.direccion + "\"," +
                              "\"descripcion\":\"" + nuevoLink.descripcion + "\"}";

            string ret = PostWS("biblos/altalink", "letra=" + jsonLink);
            if (nuevoLink.tipo == 1) // libro
            {
                mlib libro = Obtener_Libro(nuevoLink.codigo_padre);
                return RedirectToAction("Libros", "Home", new { letra = libro.titulo.ToString().Substring(0, 1).ToUpper() });
            }
            else
            {
                Autores autor = Obtener_Autor(nuevoLink.codigo_padre);
                return RedirectToAction("Autores", "Home", new { letra = autor.NombreAutor.ToString().Substring(0, 1).ToUpper() });
            }
        }


            public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            string ret = PostWS("biblos/LibrosLetra", "letra="+"A");
            JArray jsonArray = JArray.Parse(ret);
            List<mlib> cantidades = jsonArray.ToObject<List<mlib>>();
            return View();
        }

        public string GetWS(HttpWebRequest request)
        {

            //if (Token.Length <= 0)
                ObtenerToken();
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + Token);

            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            var content = string.Empty;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }

            return content;
        }

        public string PostWS(string ruta, string param)
        {
            string retorno = "";
            if (Token.Length <= 0)
                ObtenerToken();
            HttpWebRequest request = WebRequest.Create(WSRest + ruta) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Authorization", "Bearer " + Token);
           // byte[] byteArray = System.Text.Encoding.UTF8.GetBytes("letra=" + param);
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes( param);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                retorno = reader.ReadToEnd();
                
            }
            return retorno;
        }

        private void ObtenerToken()
        {
            HttpWebRequest request = WebRequest.Create(WSRest + "login/authenticate") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // Metodo modificado
            string postData = "username=mgmartos&password=trijaka&grant_type=password";
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            //using (
            Stream dataStream = request.GetRequestStream();
            {
                //using (StreamWriter stmw = new StreamWriter(dataStream))
                //{
                //    stmw.Write(postData);
                //}
                dataStream.Write(byteArray, 0, byteArray.Length);
            }


            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                Token = reader.ReadToEnd();
                Token = Token.Substring(1, Token.Length - 2);
            }
            //Token = Token.Substring(1);
            //Token = Token.Substring(0, Token.Length - 1);
        }



    }
}