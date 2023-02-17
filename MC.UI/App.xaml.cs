using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;

using MC.UI.Core;
using MC.UI.Views;
using MC.UI.Services;

namespace MC.UI {
  public partial class App : Application {
    protected override void OnStartup(StartupEventArgs args) {
      base.OnStartup(args);

      var services = new ServiceCollectionBuilder();

      services.AddMainWindow<MainWindow>();
      services.ConfigureServices(services => {
        services.AddTransient<IInjectionService, InjectionService>();
      });
      services.AddConfiguration();
      services.AddViewModels();
      services.Build();

      Ioc.Default.GetService<MainWindow>()!.Show();
    }
  }
}
