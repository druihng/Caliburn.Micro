using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Features.Avalonia.ViewModels;

namespace Features.CrossPlatform.Views
{
    public partial class TabView : UserControl
    {


        public TabView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
