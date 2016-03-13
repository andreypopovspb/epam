using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Epam.FundManager.WPFLibrary.Extensions
{
	public class GifImage : Image
	{
		private bool _inited;
		private GifBitmapDecoder _gf;
		private Int32Animation _anim;
		private bool _animationIsWorking;

		public int FrameIndex
		{
			get { return (int)GetValue(FrameIndexProperty); }
			set { SetValue(FrameIndexProperty, value); }
		}

		public static readonly DependencyProperty FrameIndexProperty = DependencyProperty.Register("FrameIndex", typeof(int), typeof(GifImage), new UIPropertyMetadata(0, ChangingFrameIndex));

		private static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
		{
			var ob = obj as GifImage;
			if (ob != null)
			{
				ob.InitAnimation();
				ob.Source = ob._gf.Frames[(int)ev.NewValue];
				ob.InvalidateVisual();
			}
		}

		private void InitAnimation()
		{
			if(!_inited)
			{
				var uri = new Uri(Source.ToString());
				_gf = new GifBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
				int mlsec = 3000 * _gf.Frames.Count / 24;
				var duration = new Duration(new TimeSpan(0, 0, 0, 0, mlsec));
				_anim = new Int32Animation(0, _gf.Frames.Count - 1, duration) { RepeatBehavior = RepeatBehavior.Forever };
				Source = _gf.Frames[0];
				_inited = true;
			}
		}
		
		protected override void OnRender(DrawingContext dc)
		{
			InitAnimation();
			base.OnRender(dc);
			if (!_animationIsWorking)
			{
				BeginAnimation(FrameIndexProperty, _anim);
				_animationIsWorking = true;
			}
		}
	}
}