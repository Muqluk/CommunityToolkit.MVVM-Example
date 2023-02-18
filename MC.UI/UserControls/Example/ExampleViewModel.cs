using Microsoft.Extensions.Configuration;

using MC.UI.Services;
using MC.UI.Core;

namespace MC.UI.UserControls {

  internal class ExampleViewModel : ViewModelBase {
    public string ViewModelText = "from ViewModel Constant";
    public string ExampleText { get; set; } = "Injexted ExampleText";
    public string? ConfigurationText { get; set; } = "ConfigurationText";

    public ExampleViewModel(IInjectionService injectionService, IConfiguration configuration) {
      ExampleText = injectionService.InjectionServiceElement;
      ConfigurationText = configuration.GetValue<string>("example_appsettings_content_key");
    }
  }
}
