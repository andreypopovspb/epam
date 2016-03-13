using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Epam.FundManager.WPFLibrary.Extensions
{
	public static class WindowCustomizer
	{
		#region CanMaximize

		public static readonly DependencyProperty CanMaximize = DependencyProperty.RegisterAttached("CanMaximize", typeof(bool), typeof(Window), new PropertyMetadata(true, OnCanMaximizeChanged));

		private static void OnCanMaximizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var window = d as Window;

			if (window != null)
			{
				RoutedEventHandler loadedHandler = null;

				loadedHandler = delegate
				                	{
				                		if ((bool)e.NewValue)
				                		{

				                			WindowHelper.EnableMaximize(window);

				                		}

				                		else
				                		{

				                			WindowHelper.DisableMaximize(window);

				                		}

				                		window.Loaded -= loadedHandler;
				                	};

				if (!window.IsLoaded)
				{
					window.Loaded += loadedHandler;
				}
				else
				{
					loadedHandler(null, null);
				}
			}
		}

		public static void SetCanMaximize(DependencyObject d, bool value)
		{
			d.SetValue(CanMaximize, value);
		}

		public static bool GetCanMaximize(DependencyObject d)
		{
			return (bool)d.GetValue(CanMaximize);
		}

		#endregion CanMaximize

		#region CanMinimize

		public static readonly DependencyProperty CanMinimize = DependencyProperty.RegisterAttached("CanMinimize", typeof(bool), typeof(Window), new PropertyMetadata(true, OnCanMinimizeChanged));

		private static void OnCanMinimizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var window = d as Window;

			if (window != null)
			{

				RoutedEventHandler loadedHandler = null;

				loadedHandler = delegate
				                	{

				                		if ((bool)e.NewValue)
				                		{

				                			WindowHelper.EnableMinimize(window);

				                		}

				                		else
				                		{

				                			WindowHelper.DisableMinimize(window);

				                		}

				                		window.Loaded -= loadedHandler;

				                	};



				if (!window.IsLoaded)
				{

					window.Loaded += loadedHandler;

				}

				else
				{

					loadedHandler(null, null);

				}

			}

		}

		public static void SetCanMinimize(DependencyObject d, bool value)
		{
			d.SetValue(CanMinimize, value);
		}

		public static bool GetCanMinimize(DependencyObject d)
		{
			return (bool)d.GetValue(CanMinimize);
		}

		#endregion CanMinimize

		#region RemoveIcon

		public static void RemoveIcon(Window window)
		{
			WindowHelper.DisableIcon(window);
		}
		
		#endregion RemoveIcon

		#region WindowHelper Nested Class

		internal static class WindowHelper
		{
			private const Int32 GWL_STYLE = -16;
			private const Int32 WS_MAXIMIZEBOX      = 0x00010000;
			private const Int32 WS_MINIMIZEBOX      = 0x00020000;

			private const Int32 GWL_EXSTYLE = -20;
			private const Int32 WS_EX_DLGMODALFRAME = 0x00000001;
			private const Int32 SWP_NOSIZE = 0x00000001;
			private const Int32 SWP_NOMOVE = 0x00000002;
			private const Int32 SWP_NOZORDER = 0x00000004;
			private const Int32 SWP_FRAMECHANGED = 0x00000020;

			[DllImport("User32.dll", EntryPoint = "GetWindowLong")]
			private extern static Int32 GetWindowLongPtr(IntPtr hWnd, Int32 nIndex);

			[DllImport("User32.dll", EntryPoint = "SetWindowLong")]
			private extern static Int32 SetWindowLongPtr(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);

			[DllImport("user32.dll")]
			private extern static bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

			/// <summary>
			/// Disables the maximize functionality of a WPF window.
			/// </summary>
			/// <param name="window">The WPF window to be modified.</param>
			public static void DisableMaximize(Window window)
			{
				lock (window)
				{
					IntPtr hWnd = new WindowInteropHelper(window).Handle;
					Int32 windowStyle = GetWindowLongPtr(hWnd, GWL_STYLE);
					SetWindowLongPtr(hWnd, GWL_STYLE, windowStyle & ~WS_MAXIMIZEBOX);
				}
			}

			/// <summary>
			/// Disables the minimize functionality of a WPF window.
			/// </summary>
			/// <param name="window">The WPF window to be modified.</param>
			public static void DisableMinimize(Window window)
			{
				lock (window)
				{
					IntPtr hWnd = new WindowInteropHelper(window).Handle;
					Int32 windowStyle = GetWindowLongPtr(hWnd, GWL_STYLE);
					SetWindowLongPtr(hWnd, GWL_STYLE, windowStyle & ~WS_MINIMIZEBOX);
				}
			}

			/// <summary>
			/// Disables the WPF windows icon.
			/// </summary>
			/// <param name="window">The WPF window to be modified.</param>
			public static void DisableIcon(Window window)
			{
				lock (window)
				{
					// Get this window's handle
					IntPtr hWnd = new WindowInteropHelper(window).Handle;

					// Change the extended window style to not show a window icon
					int extendedStyle = GetWindowLongPtr(hWnd, GWL_EXSTYLE);
					SetWindowLongPtr(hWnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

					//Update the window's non-client area to reflect the changes
					SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
				}
			}

			/// <summary>
			/// Enables the maximize functionality of a WPF window.
			/// </summary>
			/// <param name="window">The WPF window to be modified.</param>
			public static void EnableMaximize(Window window)
			{
				lock (window)
				{
					IntPtr hWnd = new WindowInteropHelper(window).Handle;
					Int32 windowStyle = GetWindowLongPtr(hWnd, GWL_STYLE);
					SetWindowLongPtr(hWnd, GWL_STYLE, windowStyle | WS_MAXIMIZEBOX);
				}
			}

			/// <summary>
			/// Enables the minimize functionality of a WPF window.
			/// </summary>
			/// <param name="window">The WPF window to be modified.</param>
			public static void EnableMinimize(Window window)
			{
				lock (window)
				{
					IntPtr hWnd = new WindowInteropHelper(window).Handle;
					Int32 windowStyle = GetWindowLongPtr(hWnd, GWL_STYLE);
					SetWindowLongPtr(hWnd, GWL_STYLE, windowStyle | WS_MINIMIZEBOX);
				}
			}
			
			/// <summary>
			/// Toggles the enabled state of a WPF window's maximize functionality.
			/// </summary>
			/// <param name="window">The WPF window to be modified.</param>
			public static void ToggleMaximize(Window window)
			{
				lock (window)
				{
					IntPtr hWnd = new WindowInteropHelper(window).Handle;
					Int32 windowStyle = GetWindowLongPtr(hWnd, GWL_STYLE);

					if ((windowStyle | WS_MAXIMIZEBOX) == windowStyle)
					{
						SetWindowLongPtr(hWnd, GWL_STYLE, windowStyle & ~WS_MAXIMIZEBOX);
					}
					else
					{
						SetWindowLongPtr(hWnd, GWL_STYLE, windowStyle | WS_MAXIMIZEBOX);
					}
				}
			}

			/// <summary>
			/// Toggles the enabled state of a WPF window's minimize functionality.
			/// </summary>
			/// <param name="window">The WPF window to be modified.</param>
			public static void ToggleMinimize(Window window)
			{
				lock (window)
				{
					IntPtr hWnd = new WindowInteropHelper(window).Handle;
					Int32 windowStyle = GetWindowLongPtr(hWnd, GWL_STYLE);

					if ((windowStyle | WS_MINIMIZEBOX) == windowStyle)
					{
						SetWindowLongPtr(hWnd, GWL_STYLE, windowStyle & ~WS_MINIMIZEBOX);
					}
					else
					{
						SetWindowLongPtr(hWnd, GWL_STYLE, windowStyle | WS_MINIMIZEBOX);
					}
				}
			}
		}
		
		#endregion WindowHelper Nested Class
	}
}