using System;
using System.Collections.Generic;
using System.Text;

namespace Fuvar
{
    class Fuvar
    {
        public int taxi_id { get; set; }
        public DateTime indulas { get; set; }
        public int idotartam { get; set; }
        public double tavolsag { get; set; }
        public double viteldij { get; set; }
        public double borravalo { get; set; }
        public string fizetes_modja { get; set; }

        public Fuvar(string line)
        {
            string[] lineSplitted = line.Split(";");

            taxi_id = Convert.ToInt32(lineSplitted[0]);
            indulas = Convert.ToDateTime(lineSplitted[1]);
            idotartam = Convert.ToInt32(lineSplitted[2]);
            tavolsag = Convert.ToDouble(lineSplitted[3].Replace(',','.'));
            viteldij = Convert.ToDouble(lineSplitted[4].Replace(',', '.'));
            borravalo = Convert.ToDouble(lineSplitted[5].Replace(',', '.'));
            fizetes_modja = lineSplitted[6];
        }



    }
}
