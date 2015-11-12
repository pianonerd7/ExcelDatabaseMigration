using Excel_Database_Migration.SQLGeneration;
using Excel_Database_Migration.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Excel_Database_Migration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Page
    {
        public MainWindow()
        {
            DataContext = new OptionsViewModel(this);
            InitializeComponent();
        }
    }
}
