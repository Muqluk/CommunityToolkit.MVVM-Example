
1. Add the following packages via Nuget Package Manager:
    - `Microsoft.Extensions.DependencyInjection`
    - `CommunityToolkit.Mvvm.DependencyInjection`
    <br>
1. Add the following `using statements` to your <u>`App.xaml.cs`</u> file:
    ```c#
    using Microsoft.Extensions.DependencyInjection;
    using CommunityToolkit.Mvvm.DependencyInjection;
    using MC.Wpf.Core;
    ```
    <br>
1. Replace your <u>`App.xaml.cs`</u> constructor with the following OnStartup method:
    ```c#
    protected override void OnStartup(StartupEventArgs args) 
    {
        base.OnStartup(args);

        var services = new ServiceCollectionBuilder()
            .AddMainWindow<MainWindow>()
            .ConfigureServices(services => 
            {
                // Add your desired services here, eg:
                // services.AddTransient<IInjectionService, InjectionService>()
            })
            // .AddConfiguration() // uncomment to add support for appsettings.json
            .AddViewModels()
            .Build();

        Ioc.Default.GetService<MainWindow>()!.Show();
    }
    ```
    <br>
1. Remove\* `StartupUri="MainWindow.xaml"` from your App.xaml:
    \* <sup><sub>_We remove the StartupURI property because removal of the constructor will cause the MainWindow view to be launched twice._</sub></sup>
    ```xml
    <Application 
          x:Class="MC.UI.App"
          xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
          xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
          xmlns:local="clr-namespace:MC.UI">
        <Application.Resources />
    </Application>
    ```

    

