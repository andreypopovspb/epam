using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Epam.FundManager.WPFLibrary.Extensions
{
	public class TreeViewEx : TreeView
	{
		public TreeViewEx()
		{
			SelectedItemChanged += OnSelectedItemHandler;
		}

		protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private static TreeViewItem GetSelectedItem(ItemsControl items)
		{
			if (items == null)
			{
				return null;
			}
			for (int i = 0; i < items.Items.Count; i++)
			{
				var item = (TreeViewItem)items.ItemContainerGenerator.ContainerFromIndex(i);
				if((item != null) && (item.IsSelected))
				{
					return item;
				}
			}
			for (int i = 0; i < items.Items.Count; i++)
			{
				var item = (TreeViewItem)items.ItemContainerGenerator.ContainerFromIndex(i);
				if ((item != null) && (item.IsExpanded))
				{
					item = GetSelectedItem(item);
					if (item != null)
					{
						return item;
					}
				}
			}
			return null;
		}

		private void OnSelectedItemHandler(object sender, RoutedEventArgs e)
		{
			ScrollToSelectedItem();
		}

		private static readonly DispatcherOperationCallback exitFrameCallback = ExitFrame;

		private static object ExitFrame(object state)
		{
			var frame = (DispatcherFrame)state;
			// Exit the nested message loop.
			frame.Continue = false;
			return null;
		}

		public void WaitForPriority()
		{
			// Create new nested message pump.
			var nestedFrame = new DispatcherFrame();

			// Dispatch a callback to the current message queue, when getting called,
			// this callback will end the nested message loop.
			// The priority of this callback should be lower than that of event message you want to process.
			DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(
				DispatcherPriority.ApplicationIdle, exitFrameCallback, nestedFrame);

			// pump the nested message loop, the nested message loop will immediately
			// process the messages left inside the message queue.
			Dispatcher.PushFrame(nestedFrame);

			// If the "exitFrame" callback is not finished, abort it.
			if (exitOperation.Status != DispatcherOperationStatus.Completed)
			{
				exitOperation.Abort();
			}
		}

		private static ScrollViewer GetScrollView(DependencyObject item)
		{
			// Get the TreeView's ScrollViewer
			DependencyObject parent = VisualTreeHelper.GetParent(item);
			while (parent != null && !(parent is ScrollViewer))
			{
				parent = VisualTreeHelper.GetParent(parent);
			}
			return (ScrollViewer)parent;
		}
		
		public void ScrollToSelectedItem()
		{
			//// Allow UI Rendering to Refresh
			//WaitForPriority();

			//var item = GetSelectedItem((TreeViewItem)ItemContainerGenerator.ContainerFromIndex(0));
			//if (item != null)
			//{
			//    ScrollViewer scroll = GetScrollView(item);
			//    //item.BringIntoView();
			//    Point offset = item.TransformToAncestor(scroll).Transform(new Point(0, 0));
			//    scroll.ScrollToVerticalOffset(offset.Y);
			//}
		}
	}
}