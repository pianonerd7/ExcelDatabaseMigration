using Excel_Database_Migration.ViewModel;
using System.Windows;

namespace Excel_Database_Migration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new OptionsViewModel(this);
            InitializeComponent();
        }
    }
}
