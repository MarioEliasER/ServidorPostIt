using ServidorPostIt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace ServidorPostIt.Services
{
    public class PostItServer
    {
        HttpListener server = new();
        public PostItServer()
        {
            //*: todas mis interfaces, mientras entren por el puerto 12345.
            server.Prefixes.Add("http://*:12345/notas/");
        }

        public void Iniciar()
        {
            if (!server.IsListening)
            {
                server.Start();
                new Thread(EscucharPeticiones)
                {
                    IsBackground = true
                }.Start();
            }
        }

        public EventHandler<Notas>? NotaRecibida;

        void EscucharPeticiones()
        {
            while (true)
            {
                //GetContext espera por una petición y hace una pausa hasta que reciba el request.
                var context = server.GetContext();
                var pagina = File.ReadAllText("Assets/Index.html");
                var bufferPagina = Encoding.UTF8.GetBytes(pagina);

                if (context.Request.Url != null)
                {
                    //cuando se usa localpath siempre necesita el diagonal al final: notas/
                    if (context.Request.Url.LocalPath == "/notas/")
                    {
                        context.Response.ContentLength64 = bufferPagina.Length;
                        context.Response.OutputStream.Write(bufferPagina, 0, bufferPagina.Length);
                        context.Response.StatusCode = 200;//OK
                        context.Response.Close();
                    }
                    else if (context.Request.HttpMethod == "POST" && context.Request.Url.LocalPath == "/notas/crear") // Me mandan los datos del formulario
                    {
                        byte[] bufferDatos = new byte[context.Request.ContentLength64];
                        context.Request.InputStream.Read(bufferDatos, 0, bufferDatos.Length);
                        string datos = Encoding.UTF8.GetString(bufferDatos);
                        var diccionario = HttpUtility.ParseQueryString(datos);

                        Notas nota = new()
                        {
                            Titulo = diccionario["titulo"] ?? "N/A",
                            Contenido = diccionario["contenido"] ?? "N/A",
                            X = double.Parse(diccionario["x"] ?? "0"),
                            Y = double.Parse(diccionario["y"] ?? "0"),
                            Remitente = context.Request.RemoteEndPoint.Address.ToString()
                        };

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            NotaRecibida?.Invoke(this, nota);
                        });
                        context.Response.StatusCode = 200;
                        context.Response.Close();
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        context.Response.Close();
                    }
                }
            }
        }

        public void Detener()
        {
            server.Stop();
        }
    }
}
