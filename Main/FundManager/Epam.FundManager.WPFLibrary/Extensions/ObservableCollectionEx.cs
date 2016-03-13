using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Threading;

namespace Epam.FundManager.WPFLibrary.Extensions
{
	public class ObservableCollectionEx<T> : ObservableCollection<T> where T : INotifyPropertyChanged
	{
		private void ContainedElementChanged(object sender, PropertyChangedEventArgs e)
		{
			//using (BlockReentrancy())
			{
				OnPropertyChanged(e);
			}
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			// Be nice - use BlockReentrancy like MSDN said
			using (BlockReentrancy())
			{
				NotifyCollectionChangedEventHandler eventHandler = CollectionChanged;
				if (eventHandler == null)
				{
					return;
				}

				Delegate[] delegates = eventHandler.GetInvocationList();
				// Walk thru invocation list
				foreach (NotifyCollectionChangedEventHandler handler in delegates)
				{
					var dispatcherObject = handler.Target as DispatcherObject;
					// If the subscriber is a DispatcherObject and different thread
					if (dispatcherObject != null && dispatcherObject.CheckAccess() == false)
					{
						// Invoke handler in the target dispatcher's thread
						dispatcherObject.Dispatcher.Invoke(DispatcherPriority.DataBind, handler, this, e);
					}
					else // Execute handler as is
					{
						handler(this, e);
					}
				}
			}
		}

		// Override the event so this class can access it
		public override event NotifyCollectionChangedEventHandler CollectionChanged;

		public virtual new void Add(T item)
		{
			if (!ReferenceEquals(item, null))
			{
				item.PropertyChanged += ContainedElementChanged;
			}
			base.Add(item);
		}

		public virtual void AddRange(IEnumerable<T> items)
		{
			if (items == null)
				throw new ArgumentNullException("items");

			foreach (T item in items)
			{
				Add(item);
			}
		}

		public virtual new void Remove(T item)
		{
			base.Remove(item);
			if (!ReferenceEquals(item, null))
			{
				item.PropertyChanged -= ContainedElementChanged;
			}
		}

		public virtual new void RemoveAt(int index)
		{
			base.RemoveAt(index);
			if ((index >= 0) && (index < Count))
			{
				if (!ReferenceEquals(this[index], null))
				{
					this[index].PropertyChanged -= ContainedElementChanged;
				}
			}
		}

		public virtual new void Clear()
		{
			foreach (T item in this)
			{
				if (!ReferenceEquals(item, null))
				{
					item.PropertyChanged -= ContainedElementChanged;
				}
			}
			base.Clear();
		}

		public virtual new T this[int index]
		{
			get { return base[index]; }
			set
			{
				if ((index >= 0) && (index < Count))
				{
					if (!ReferenceEquals(this[index], null))
					{
						this[index].PropertyChanged -= ContainedElementChanged;
					}
				}
				base[index] = value;
				value.PropertyChanged += ContainedElementChanged;
			}
		}
	}
}