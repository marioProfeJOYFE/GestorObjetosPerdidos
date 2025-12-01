using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorObjetosPerdidos.MVVM.Model
{
    public class ObjetoPerdido
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public DateTime FechaHallazgo { get; set; }
        public string UbicacionHallazgo { get; set; }
        public string Descripcion { get; set; }
        public string ImagenPath { get; set; }
        public bool Devuelto { get; set; }
    }
}
