using System;
using System.Linq;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;


namespace MC.Wpf.Core {
  public class ServiceCollectionBuilder {
    private readonly IServiceCollection _serviceCollection;

    public ServiceCollectionBuilder() {
      _serviceCollection = new ServiceCollection();
    }

    public ServiceCollectionBuilder AddMainWindow<T>() where T : class {
      _serviceCollection.AddTransient<T>();

      return this;
    }

    public ServiceCollectionBuilder ConfigureServices(Action<IServiceCollection> action) {
      action.Invoke(_serviceCollection);
      return this;
    }

    public ServiceCollectionBuilder AddViewModels() {
      var viewModels = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(type => type.IsSubclassOf(typeof(ViewModel)));

      foreach (var viewModel in viewModels) {
        _serviceCollection.AddTransient(viewModel);
      }

      return this;
    }

    public ServiceCollectionBuilder AddConfiguration(string fileName = "appsettings.json") {
      var builder = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile(fileName, false, true);
      var configuration = builder.Build() as IConfiguration;
      _serviceCollection.AddSingleton(configuration);

      return this;
    }

    public IServiceProvider Build() {
      var serviceProvider = _serviceCollection.BuildServiceProvider();
      Ioc.Default.ConfigureServices(serviceProvider);

      return serviceProvider;
    }
  }
}
