using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using GestorObjetosPerdidos.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestorObjetosPerdidos.MVVM.ViewModel;

public class AppViewModel : INotifyPropertyChanged
{
    public ObservableCollection<ObjetoPerdido> ListObjetosPerdidos { get; set; }

    private readonly FileService _fileService;
    
    public ICommand GuardarDatosCommand { get; }

    public AppViewModel()
    {
        _fileService = new FileService();
        ListObjetosPerdidos = new ObservableCollection<ObjetoPerdido>()
        {
            new ObjetoPerdido
            {
                Nombre = "Objeto Perdido 1",
                Categoria = "Categoría 1",
                FechaHallazgo = DateTime.Now,
                UbicacionHallazgo = "Ubicación 1",
                Descripcion = "Descripción del objeto perdido 1",
                ImagenPath = "https://picsum.photos/seed/picsum/200/300",
                Devuelto = false
            },
            new ObjetoPerdido
            {
                Nombre = "Objeto Perdido 2",
                Categoria = "Categoría 2",
                FechaHallazgo = DateTime.Now.AddDays(-5),
                UbicacionHallazgo = "Ubicación 2",
                Descripcion = "Descripción del objeto perdido 2",
                ImagenPath = "https://picsum.photos/seed/picsum/200/300",
                Devuelto = true
            },
            new ObjetoPerdido
            {
                Nombre = "Objeto Perdido 3",
                Categoria = "Categoría 1",
                FechaHallazgo = DateTime.Now.AddDays(-10),
                UbicacionHallazgo = "Ubicación 3",
                Descripcion = "Descripción del objeto perdido 3",
                ImagenPath = "https://picsum.photos/seed/picsum/200/300",
                Devuelto = false
            }
        };
        
        GuardarDatosCommand = new RelayCommand(async async => await GuardarDatos());

    }

    public async Task GuardarDatos()
    {
        await _fileService.GuardarObjetosAsync(ListObjetosPerdidos.ToList());
    }
    
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}