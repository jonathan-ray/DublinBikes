using System.Windows.Media;

namespace DublinBikes.Content
{
	/// <summary>
	/// Pre-defined set of colour brushes.
	/// </summary>
	public class Colours
	{
		/// <summary>
		/// Opaque transparency.
		/// </summary>
		private const byte OpaqueTransparency = 0xFF;


		/// <summary>
		/// Translucent transprancy.
		/// </summary>
		private const byte TranslucentTransparency = 0xCC;


		/// <summary>
		/// 'Dublin Bike Teal' Colour Brush.
		/// </summary>
		public static SolidColorBrush Teal
		{
			get
			{
				return new SolidColorBrush(DefineTeal(OpaqueTransparency));
			}
		}


		/// <summary>
		/// 'Dublin Bike Teal' Translucent Colour Brush.
		/// </summary>
		public static SolidColorBrush TranslucentTeal
		{
			get
			{
				return new SolidColorBrush(DefineTeal(TranslucentTransparency));
			}
		}


		/// <summary>
		/// White Translucent Colour Brush.
		/// </summary>
		public static SolidColorBrush TranslucentWhite
		{
			get
			{
				return new SolidColorBrush(Color.FromArgb(TranslucentTransparency, 0xFF, 0xFF, 0xFF));
			}
		}


		/// <summary>
		/// 'White' Colour Brush.
		/// </summary>
		public static SolidColorBrush White
		{
			get
			{
				return new SolidColorBrush(Colors.White);
			}
		}


		/// <summary>
		/// Defines the colour teal given a transparency level.
		/// </summary>
		/// <param name="transparency">The transparency level.</param>
		/// <returns>Teal colour.</returns>
		private static Color DefineTeal(byte transparency)
		{
			return Color.FromArgb(transparency, 0x36, 0x75, 0x88);
		}
	}
}