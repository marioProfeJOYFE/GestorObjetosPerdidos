# **APUNTES  –  (Primera Evaluación)**


# 1. Arquitectura MVVM (resumen avanzado)

### **Model**

* Clases de datos → no contienen lógica UI
* Pueden incluir validaciones básicas o anotaciones
* Suelen ser serializadas (JSON)

### **ViewModel**

* Expone propiedades que la vista va a Binding
* Implementa INotifyPropertyChanged
* Contiene Commands
* Puede comunicarse con servicios externos (ficheros, navegación…)

### **View**

* Solo XAML
* Sin eventos tipo Click (excepto casos justificados como navegar)
* Se conecta al ViewModel mediante **DataContext**

---

# 2. Cómo se enlaza el DataContext (muy importante)

Hay 3 formas correctas de asignar un DataContext.

---

## ✔ A) En el code-behind de la View (lo más habitual)

```csharp
public PaginaObjetos()
{
    InitializeComponent();
    DataContext = new PaginaObjetosViewModel();
}
```

---

## ✔ B) Mediante inyección en constructor

```csharp
public PaginaObjetos(PaginaObjetosViewModel vm)
{
    InitializeComponent();
    DataContext = vm;
}
```

Útil cuando tienes un **NavigationService** o un **contenedor de dependencias**.

---

## ✔ C) En XAML usando recursos

```xml
<Page.DataContext>
    <vm:ObjetosViewModel />
</Page.DataContext>
```

> ⚠ Importante: Esto NO permite pasar parámetros al ViewModel.

---

# 3. DataTemplates

Los **DataTemplate** definen cómo se representa visualmente un objeto de la colección.

Ejemplo: Mostrar tarjetas de objetos encontrados.

```xml
<ListBox ItemsSource="{Binding Objetos}">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <Border Margin="5" Padding="10" CornerRadius="10" Background="#EEE">
                <StackPanel>
                    <TextBlock Text="{Binding Nombre}" FontWeight="Bold" FontSize="16"/>
                    <TextBlock Text="{Binding Categoria}" Foreground="Gray"/>
                    <TextBlock Text="{Binding FechaHallazgo, StringFormat='{}{0:dd/MM/yyyy}'}"/>
                </StackPanel>
            </Border>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
```

### Conceptos importantes:

* El Binding del DataTemplate se hace **al elemento de la colección**, no al ViewModel.
* Dentro del DataTemplate, `DataContext = item actual`.

---

# 4. DataTemplateSelector

Permite elegir un template u otro según una condición.

### A) Crear los templates en XAML

```xml
<Window.Resources>
    <DataTemplate x:Key="DevueltoTemplate">
        <TextBlock Text="{Binding Nombre}" Foreground="Green"/>
    </DataTemplate>

    <DataTemplate x:Key="NoDevueltoTemplate">
        <TextBlock Text="{Binding Nombre}" Foreground="Red"/>
    </DataTemplate>
</Window.Resources>
```

### B) Crear el selector en C#

```csharp
public class EstadoTemplateSelector : DataTemplateSelector
{
    public DataTemplate DevueltoTemplate { get; set; }
    public DataTemplate NoDevueltoTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        var obj = item as ObjetoPerdido;
        return obj.Devuelto ? DevueltoTemplate : NoDevueltoTemplate;
    }
}
```

### C) Usarlo en XAML

```xml
<Window.Resources>
    <local:EstadoTemplateSelector x:Key="EstadoSelector"
        DevueltoTemplate="{StaticResource DevueltoTemplate}"
        NoDevueltoTemplate="{StaticResource NoDevueltoTemplate}" />
</Window.Resources>

<ListBox ItemsSource="{Binding Objetos}"
         ItemTemplateSelector="{StaticResource EstadoSelector}" />
```

---

# 5. Estilos implícitos

Un estilo implícito no necesita `x:Key`.

```xml
<Style TargetType="TextBox">
    <Setter Property="Padding" Value="8"/>
    <Setter Property="BorderBrush" Value="#4A90E2"/>
    <Setter Property="BorderThickness" Value="2"/>
</Style>
```

**Afectará a TODOS los TextBox** de la vista.

---

# 6. ControlTemplates

Permiten rediseñar completamente un control.

Ejemplo: dar forma circular a un botón:

```xml
<Style x:Key="BotonRedondo" TargetType="Button">
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="Button">
                <Border Background="{TemplateBinding Background}"
                        CornerRadius="50"
                        Padding="{TemplateBinding Padding}">
                    <ContentPresenter HorizontalAlignment="Center"
                                      VerticalAlignment="Center"/>
                </Border>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```

---

# 7. Lectura y escritura de ficheros (JSON + MVVM correctos)

### A) Servicio

```csharp
public class ArchivoService
{
    private const string Ruta = "datos.json";

    public async Task<List<ObjetoPerdido>> CargarAsync()
    {
        if (!File.Exists(Ruta)) return new List<ObjetoPerdido>();

        string json = await File.ReadAllTextAsync(Ruta);
        return JsonSerializer.Deserialize<List<ObjetoPerdido>>(json);
    }

    public async Task GuardarAsync(List<ObjetoPerdido> objetos)
    {
        string json = JsonSerializer.Serialize(objetos, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(Ruta, json);
    }
}
```

---

### B) ViewModel que usa el servicio

```csharp
public class ObjetosViewModel : ViewModelBase
{
    private readonly ArchivoService _servicio;

    public ObservableCollection<ObjetoPerdido> Objetos { get; set; }

    public ICommand GuardarCommand { get; }

    public ObjetosViewModel()
    {
        _servicio = new ArchivoService();
        GuardarCommand = new RelayCommand(async _ => await Guardar());
        Cargar();
    }

    private async void Cargar()
    {
        var datos = await _servicio.CargarAsync();
        Objetos = new ObservableCollection<ObjetoPerdido>(datos);
    }

    private async Task Guardar() =>
        await _servicio.GuardarAsync(Objetos.ToList());
}
```

---

# 8. Plantilla típica para un ViewModel

```csharp
public class ObjetoViewModel : ViewModelBase
{
    private string _nombre;
    public string Nombre
    {
        get => _nombre;
        set { _nombre = value; OnPropertyChanged(); }
    }

    public ObservableCollection<ObjetoPerdido> Objetos { get; set; }

    public ICommand AgregarCommand { get; }

    public ObjetoViewModel()
    {
        Objetos = new ObservableCollection<ObjetoPerdido>();
        AgregarCommand = new RelayCommand(Agregar);
    }

    private void Agregar(object obj)
    {
        Objetos.Add(new ObjetoPerdido { Nombre = this.Nombre });
    }
}
```

---

# 9. Chuleta final

**MVVM → View (XAML) – ViewModel (Binding/Commands) – Model (Datos)**

**DataContext** → dónde está el ViewModel

**DataTemplate** → cómo se dibuja un elemento

**ItemTemplate** → DataTemplate para listas

**Estilos implícitos** → sin x:Key, afectan a todos

**ResourceDictionary** → estilos externos

**INotifyPropertyChanged** → refresca la UI

**RelayCommand** → lógica en ViewModel, no en la vista

**ObservableCollection** → listas dinámicas

**JSON + Servicios** → leer/escribir datos

**TemplateSelector** → dibujar según condición
