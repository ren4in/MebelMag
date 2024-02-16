using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MebelMag
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Store.SetBaseAddress(Store.APP_PATH);
            MainFrame.Navigate(new AuthorizationPage());
            Manager.MainFrame = MainFrame;
        }
        
        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if (MainFrame.CanGoBack)
                BtnBack.Visibility = Visibility.Visible;
            else
                BtnBack.Visibility = Visibility.Hidden;
        }


        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.MainFrame.Content.ToString() == "Измайлов_работа.InventoryContentPage")
            {
                if (MessageBox.Show("Сохранить введенные данные?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                 //   InventoryContentPage.db.SaveChanges();
                }
            }
            Manager.MainFrame.GoBack();
        }
    }
}