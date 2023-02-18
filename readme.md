# CommunityToolkit.MVVM with .Net 7 with appsettings.json

## Steps for a new (or existing) project.

1. <u>Packages to Add (via Nuget package manager)</u>
    <details>
        <summary>Package List</summary>

    `CommunityToolkit.Mvvm`<br />
    `Microsoft.Extensions.Configuration.Binder`<br />
    `Microsoft.Extensions.Configuration.FileExtensions`<br />
    `Microsoft.Extensions.Configuration.Json`<br />
    `Microsoft.Extensions.DependencyInjection`<br />
    </details>

1. <u>Build the ServiceCollectionBuilder class</u>

    <sup>\- Create a new folder in the root of your project called `Core`</sup>
    <sup>\- Add a new class file within that folder called **_`ServiceCollectionBuilder.cs`_**</sup>
    <details>
        <summary>ServiceCollectionBuilder.cs</summary>

    ```c#
        using System;
        using System.Linq;

        using Microsoft.Extensions.Configuration;
        using Microsoft.Extensions.DependencyInjection;

        using CommunityToolkit.Mvvm.DependencyInjection;


        namespace MC.UI.Core {
            public class ServiceCollectionBuilder {
                private readonly IServiceCollection _serviceCollection;

                public ServiceCollectionBuilder() 
                {
                    _serviceCollection = new ServiceCollection();
                }

                public ServiceCollectionBuilder AddMainWindow<T>() where T : class 
                {
                    _serviceCollection.AddTransient<T>();

                    return this;
                }

                public ServiceCollectionBuilder ConfigureServices(Action<IServiceCollection> action) 
                {
                    action.Invoke(_serviceCollection);

                    return this;
                }

                public ServiceCollectionBuilder AddViewModels() 
                {
                    var viewModels = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(assembly => assembly.GetTypes())
                        .Where(type => type.IsSubclassOf(typeof(ViewModel)));

                    foreach (var viewModel in viewModels) 
                    {
                        _serviceCollection.AddTransient(viewModel);
                    }

                    return this;
                }

                public ServiceCollectionBuilder AddConfiguration(string fileName = "appsettings.json")
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile(fileName, false, true);

                    var configuration = builder.Build() as IConfiguration;

                    _serviceCollection.AddSingleton(configuration);

                    return this;
                }

                public IServiceProvider Build() 
                {
                    var serviceProvider = _serviceCollection.BuildServiceProvider();

                    Ioc.Default.ConfigureServices(serviceProvider);

                    return serviceProvider;
                }
            }
        }
    ```
    </details>

1. <u>Build the ViewModelBase class</u>     
    <sup><sub>_All of your viewmodels will inherit this class. It will allow the `ServiceCollectionBuilder` to find and add them automatically._</sub></sup>
        
    <sup>\- Within the previously created Core folder, create a new class file called _`ViewModelBase.cs`_</sup>
    <sup>\- Add the following code</sup>
    
    <details>
        <summary>ViewModelBase.cs</summary>

    ```c#
        using CommunityToolkit.Mvvm.ComponentModel;

        namespace MC.UI.Core 
        {
            public class ViewModelBase : ObservableObject { }
        }
    ```
    </details>
    
1. <u>Modify the existing App.xaml File</u>

    <sup>\- Remove the _`StartupUri="MainWindow.xaml"`_ property</sup>

    <details>
        <summary>App.xaml</summary>

    ```xml
        <Application 
            x:Class="MC.UI.App"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:local="clr-namespace:MC.UI">
            <Application.Resources>
                <!--  -->
            </Application.Resources>
        </Application>
    ```
    </details>    

1. <u>Modify the existing App.xaml.cs</u>

    <sup>\- Remove the default constructor</sup>
    <sup>\- Add a new OnStartup method</sup>

    <details>
        <summary>App.xaml.cs</summary>

    ```c#
        using Microsoft.Extensions.DependencyInjection;
        using CommunityToolkit.Mvvm.DependencyInjection;

        using MC.UI.Core; // 
        using MC.UI.Views;
        using MC.UI.Services;
        
        namespace MC.UI {
            public partial class App : Application {
                protected override void OnStartup(StartupEventArgs args) 
                {
                    base.OnStartup(args);

                    var services = new ServiceCollectionBuilder()
                        .AddMainWindow<MainWindow>()
                        .ConfigureServices(services => 
                            {
                                // Add desired services here
                                services.AddTransient<IExampleInjectionService, ExampleInjectionService>() 
                            })
                        .AddConfiguration() // Comment this line out if you don't need/want an appsettings.json
                        .AddViewModels()
                        .Build();

                    Ioc.Default.GetService<MainWindow>()!.Show(); // opens the MainWindow
                }
            }
        }
    ```
    </details>

1. <u>Move the existing MainWindow.xaml to the _`Views`_ folder</u>

    <sup>\- Create a new folder in the root of your project called Views</sup>
    <sup>\- Move MainWindow.xaml and MainWindow.xaml.cs into this newly created Views folder.</sup>
    ```
       📦MC.UI
        ┣ 📂Views
        ┃ ┣ 📜MainWindow.xaml
        ┃ ┣ 📜MainWindow.xaml.cs
    ```

1. <u>Modify the existing MainWindow.xaml</u>

    <sup>\- Copy the following over the current contents:</sup>

    <details>
        <summary>MainWindow.xaml</summary>

    ```xml
        <Window x:Class="MC.UI.Views.MainWindow"
                xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
                xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
                xmlns:local="clr-namespace:MC.UI.Views"
                xmlns:Example="clr-namespace:MC.UI.UserControls"
                mc:Ignorable="d"
                Title="Main Window">
            <StackPanel>
                <Example:Example />
            </StackPanel>

        </Window>
    ```
    </details>

1. <u>Modify the existing MainWindow.xaml.cs</u>

    <sup>\- Copy the following over the current contents:</sup>

    <details>
        <summary>MainWindow.xaml.cs</summary>

    ```c#
        using System.Windows;

        using CommunityToolkit.Mvvm.DependencyInjection;

        using MC.UI.Core;

        namespace MC.UI.Views 
        {
            public partial class MainWindow : Window 
            {
                public MainWindow() 
                {
                    DataContext = Ioc.Default.GetService<MainWindowViewModel>(); // new
                    InitializeComponent();
                }
            }
        }
    ```
    </details>

1. <u>Add the MainWindow View Model</u>

    <sup>\- Create a new class file within the Views folder called _`MainWindowViewModel.cs`_</sup>
    <sup>\- Add the following code</sup>
    <details>
        <summary>MainWindowViewModel.cs</summary>

    ```c#
        using MC.UI.Core;

        namespace MC.UI.Views 
        {
            public class MainWindowViewModel : ViewModelBase { }
        }
    ```
    </details>

## Optional, Example Service and UserControl (consuming the service)

1. <u>Build the Example service</u>

    <sup>\- Create a new folder in the root of your project called Services</sup>
    <sup>\- Now create a new class file called _`ExampleInjectionService.cs`_</sup>
    <sup>\- Add the following code</sup>

    <details>
        <summary>ExampleInjectionService.cs</summary>

    ```c#
    namespace MC.UI.Services 
    {
        public interface IExampleInjectionService 
        {
            public string InjectionServiceElement { get; }
        }

        internal class ExampleInjectionService : IExampleInjectionService 
        {
            public string InjectionServiceElement => "Example text from injected service";
        }
    }
    ```
    </details>

1. <u>Build the Example UserControl</u>

    <sup>\- Create a new folder in the root of your project called UserControls</sup>
    <sup>\- Within the newly created folder create a new <u>UserControl</u> called _`Example`_</sup>

    <details>
        <summary>Example.xaml</summary>

    ```xml
        <UserControl
            x:Class="MC.UI.UserControls.Example"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
            xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
            mc:Ignorable="d">
            <StackPanel>
                <TextBlock>Static, from View (Example.xaml)</TextBlock>
                <TextBlock Text="{Binding ExampleText}" />
                <TextBlock Text="{Binding ConfigurationText}" />
            </StackPanel>
        </UserControl>
    ```
    </details>


    <details>
        <summary>Example.xaml.cs</summary>

    ```c#
        using System.Windows.Controls;

        using CommunityToolkit.Mvvm.DependencyInjection;

        namespace MC.UI.UserControls 
        {
            public partial class Example : UserControl 
            {
                public Example() {
                    InitializeComponent();
                    DataContext = Ioc.Default.GetService<ExampleViewModel>();
                }
            }
        }
    ```
    </details>

1. <u>Build the Example View Model</u>

    <sup>\- Now create an additional class file called _`ExampleViewModel.cs`_ within the UserControls folder</sup>

    <details>
        <summary>ExampleViewModel.cs</summary>

    ```c#
        using Microsoft.Extensions.Configuration;

        using MC.UI.Services;
        using MC.UI.Core;

        namespace MC.UI.UserControls 
        {

            internal class ExampleViewModel : ViewModel 
            {
                public string ExampleText { get; set; } = "Injexted ExampleText";
                public string? ConfigurationText { get; set; } = "ConfigurationText";

                public ExampleViewModel(IInjectionService injectionService, IConfiguration configuration) 
                {
                    ExampleText = injectionService.InjectionServiceElement;
                    ConfigurationText = configuration.GetValue<string>("example_appsettings_content_key");
                }
            }
        }
    ```
    </details>

----

### Wrapping up
    
* Your final project folder & file structure should resemble this: 

    ```
       📦MC.UI
        ┣ 📂Core
        ┃ ┣ 📜ServiceCollectionBuilder.cs
        ┃ ┗ 📜ViewModelBase.cs
        ┣ 📂Services
        ┃ ┣ 📜IExampleInjectionService.cs
        ┃ ┗ 📜ExampleInjectionService.cs
        ┣ 📂UserControls
        ┃ ┣ 📜Example.xaml
        ┃ ┣ 📜Example.xaml.cs
        ┃ ┗ 📜ExampleViewModel.cs
        ┣ 📂Views
        ┃ ┣ 📜MainWindow.xaml
        ┃ ┣ 📜MainWindow.xaml.cs
        ┃ ┗ 📜MainWindowViewModel.cs
        ┣ 📜App.xaml
        ┣ 📜App.xaml.cs
        ┣ 📜appsettings.json
        ┣ 📜AssemblyInfo.cs
        ┗ 📜MC.UI.csproj
    ```
