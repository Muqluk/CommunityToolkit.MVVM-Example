using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;

using MC.UI.Views;


namespace MC.UI.Main {

  public partial class App : Application {

    // https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/ioc
    // https://github.com/gosukretess/WPF.Core

    protected override void OnStartup(StartupEventArgs args) {
      base.OnStartup(args);

      var svcCollection = new ServiceCollectionBuilder();
      svcCollection.AddMainWindow<MainView>();
      svcCollection.ConfigureServices(services => {
        services.AddTransient<IInjectionService, InjectionService>();
      });
      svcCollection.AddConfiguration();
      svcCollection.AddViewModels();
      svcCollection.Build();

      Ioc.Default.GetService<MainView>()!.Show();
    }
  }
}
