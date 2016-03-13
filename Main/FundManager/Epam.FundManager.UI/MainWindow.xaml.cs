using System.Windows;
using System.Windows.Input;
using Epam.FundManager.UI.Images;
using Epam.FundManager.UI.Presenters;

namespace Epam.FundManager.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainPresenter _presenter;

        public MainPresenter Presenter
        {
            get { return _presenter; }
        }

        public MainWindow()
        {
            //Set current windows as main
            Application.Current.MainWindow = this;

            //Set application icon
            Icon = Icons.ApplicationBitmapImage;

            //Load window settings (width, height, position
            LoadUserSettings();

            //Initialize Presenter
            _presenter = new MainPresenter();
            DataContext = _presenter;

            //Рендерим окно
            InitializeComponent();
        }

        private void LoadUserSettings()
        {
            if (Settings.Default.MainWindowSize != new System.Drawing.Size(0, 0))
            {
                Width = Settings.Default.MainWindowSize.Width;
                Height = Settings.Default.MainWindowSize.Height;
            }
            if (Settings.Default.MainWindowPosition == new System.Drawing.Point(0, 0))
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                WindowStartupLocation = WindowStartupLocation.Manual;
                Left = Settings.Default.MainWindowPosition.X;
                Top = Settings.Default.MainWindowPosition.Y;
            }
        }

        private void SaveUserSettings()
        {
            Settings.Default.MainWindowSize = new System.Drawing.Size((int)Width, (int)Height);
            Settings.Default.MainWindowPosition = new System.Drawing.Point((int)Left, (int)Top);
            Settings.Default.Save();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_presenter == null)
            {
                return;
            }
            var command = (RoutedUICommand)e.Command;
            _presenter.Execute(command);
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_presenter == null)
            {
                return;
            }
            var command = (RoutedUICommand) e.Command;
            bool canExecute = _presenter.CanExecute(command);
            e.CanExecute = canExecute;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveUserSettings();
        }
    }
}