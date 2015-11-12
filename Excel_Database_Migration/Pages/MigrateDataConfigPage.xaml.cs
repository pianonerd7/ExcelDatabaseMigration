using Excel_Database_Migration.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Excel_Database_Migration.Pages
{
    /// <summary>
    /// Interaction logic for MigrateDataConfigPage.xaml
    /// </summary>
    public partial class MigrateDataConfigPage : Page
    {
        public MigrateDataConfigPage()
        {
            DataContext = new MigrationDataConfigViewModel(this);
            InitializeComponent();
        }
    }
}
