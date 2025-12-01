using GestorObjetosPerdidos.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestorObjetosPerdidos
{
    public class FileService
    {
        private readonly string _filePath = "datos.json";

        // Carga los objetos desde el archivo JSON
        // Sustituir objectos por la clase concreta que se esté utilizando
        // Ejemplo: List<ObjetoPerdido>
        public async Task<List<ObjetoPerdido>> CargarObjetosAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<ObjetoPerdido>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<ObjetoPerdido>>(json) ?? new List<ObjetoPerdido>();
        }

        // Guarda los objetos en el archivo JSON
        // Sustituir objectos por la clase concreta que se esté utilizando
        // Ejemplo: List<ObjetoPerdido>
        public async Task GuardarObjetosAsync(List<ObjetoPerdido> objetos)
        {
            var json = JsonSerializer.Serialize(objetos, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
