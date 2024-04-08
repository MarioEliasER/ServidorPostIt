﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServidorPostIt.Models
{
    public class Notas
    {
        public string Titulo { get; set; } = "";
        public string Contenido { get; set; } = "";
        public string Remitente {  get; set; } = "";
        public double X {  get; set; }
        public double Y { get; set; }
        public double Angulo { get; set; }
    }
}
