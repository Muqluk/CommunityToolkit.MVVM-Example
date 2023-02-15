using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;

using MC.UI.ViewModels;

namespace MC.UI.Views {
  public partial class MainView : Window {
    public MainView() {
      DataContext = Ioc.Default.GetService<MainViewModel>();
      InitializeComponent();
    }
  }
}
