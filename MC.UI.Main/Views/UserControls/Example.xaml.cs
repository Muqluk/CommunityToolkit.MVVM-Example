using System.Windows.Controls;
using Microsoft.Extensions.Configuration;

using CommunityToolkit.Mvvm.DependencyInjection;

namespace MC.UI.Main.Views.UserControls {

  internal class ExampleViewModel : ViewModel {
    public string ExampleText { get; set; }

    public string ConfigurationText { get; set; }

    public ExampleViewModel(IInjectionService injectionService, IConfiguration configuration) {
      ExampleText = injectionService.InjectionServiceElement;
      ConfigurationText = configuration.GetValue<string>("ConfigEntry");
    }
  }

  public partial class Example : UserControl {
    public Example() {
      InitializeComponent();
      DataContext = Ioc.Default.GetService<ExampleViewModel>();
    }
  }
}
