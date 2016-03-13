using System.Windows.Input;

namespace Epam.FundManager.UI.Presenters
{
    public class DataCommands
    {
        private static readonly RoutedUICommand _addStock;
        private static readonly RoutedUICommand _cancel;
        private static readonly RoutedUICommand _buyStock;

        static DataCommands()
        {
            //Add stock
            var inputs = new InputGestureCollection { new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl+S") };
            _addStock = new RoutedUICommand("Add stock", "AddStock", typeof(DataCommands), inputs);

            //Cancel
            inputs = new InputGestureCollection { new KeyGesture(Key.Escape, ModifierKeys.None, "Escape") };
            _cancel = new RoutedUICommand("Cancel", "Cancel", typeof(DataCommands), inputs);

            //Add Function
            inputs = new InputGestureCollection { new KeyGesture(Key.Enter, ModifierKeys.None, "Enter") };
            _buyStock = new RoutedUICommand("Buy stock", "BuyStock", typeof(DataCommands), inputs);
        }

        public static RoutedUICommand AddStock
        {
            get { return _addStock; }
        }

        public static RoutedUICommand Cancel
        {
            get { return _cancel; }
        }

        public static RoutedUICommand BuyStock
        {
            get { return _buyStock; }
        }
    }
}