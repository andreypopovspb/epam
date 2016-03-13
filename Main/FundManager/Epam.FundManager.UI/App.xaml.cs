using System;
using System.Windows;
using System.Windows.Threading;
using Epam.FundManager.Common.Extensions;

namespace Epam.FundManager.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = e.Exception.InnerException ?? e.Exception;
            //TODO: Log
            MessageBox.Show(ex.ToTraceString(), "Ошибка (DispatcherUnhandledException)", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}