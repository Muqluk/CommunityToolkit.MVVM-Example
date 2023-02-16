using System.Windows.Controls;

using CommunityToolkit.Mvvm.DependencyInjection;

namespace MC.UI.UserControls {

  public partial class Example : UserControl {
    public Example() {
      InitializeComponent();
      DataContext = Ioc.Default.GetService<ExampleViewModel>();
    }
  }
}
