using Microsoft.Extensions.Configuration;

using MC.UI.Services;
using MC.Wpf.Core;

namespace MC.UI.UserControls {

  internal class ExampleViewModel : ViewModel {
    public string ExampleText { get; set; }
    public string? ConfigurationText { get; set; } = "";

    public ExampleViewModel(IInjectionService injectionService, IConfiguration configuration) {
      ExampleText = injectionService.InjectionServiceElement;
      ConfigurationText = configuration.GetValue<string>("ConfigEntry");
    }
  }
}
