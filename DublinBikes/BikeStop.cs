using System.Device.Location;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using DublinBikes.Content;
using DublinBikes.Extensions;

namespace DublinBikes
{
	/// <summary>
	/// Object representation of a Bike Stop
	/// </summary>
	public class BikeStop
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="BikeStop"/> class from being created.
		/// </summary>
		private BikeStop()
		{
		}


		/// <summary>
		/// Street address of the bike stop.
		/// </summary>
		public string Address { get; set; }


		/// <summary>
		/// Unique identifier for the bike stop.
		/// </summary>
		public int Id { get; set; }


		/// <summary>
		/// Latitude/longitude of the bike stop.
		/// </summary>
		public GeoCoordinate Location { get; set; }


		/// <summary>
		/// Tries to parse the all stops' stop element as a bike stop object.
		/// </summary>
		/// <param name="stopElement">The stop element.</param>
		/// <param name="stop">The stop.</param>
		/// <returns><c>True</c> if successful, else <c>false</c>.</returns>
		public static bool TryParseStopElement(XElement stopElement, out BikeStop stop)
		{
			if (stopElement == null)
			{
				stop = null;
				return false;
			}

			stop = new BikeStop
			{
				Id = stopElement.GetAttributeIntValue("number"),
				Location = new GeoCoordinate(
					stopElement.GetAttributeDoubleValue("lat"),
					stopElement.GetAttributeDoubleValue("lng")),
				Address = stopElement.GetAttributeStringValue("address")
			};

			return stop.Id != default(int);
		}


		/// <summary>
		/// Converts the bike stop data alongside its corresponding details data to map pin content.
		/// </summary>
		/// <param name="details">The details data.</param>
		/// <returns>Map pin content.</returns>
		public StackPanel ToMapContent(BikeStopDetails details)
		{
			if (details == null)
			{
				return null;
			}

			// Define base panel
			StackPanel content = new StackPanel
			{
				Background = Colours.Teal,
				Width = 30
			};

			// Add text block for available bikes
			content.Children.Add(new TextBlock
			{
				Text = details.Available.ToString(CultureInfo.InvariantCulture),
				FontSize = 18,
				HorizontalAlignment = HorizontalAlignment.Center,
				Foreground = Colours.White,
				Padding = new Thickness(2, 0, 2, 0)
			});

			// Define a box to contain the free bike slots text block
			// This is so we can change the background colour
			Grid grid = new Grid
			{
				Background = Colours.White,
				Width = 30
			};

			// Add text block for free bike slots
			grid.Children.Add(new TextBlock
			{
				Text = details.Free.ToString(CultureInfo.InvariantCulture),
				FontSize = 14,
				HorizontalAlignment = HorizontalAlignment.Center,
				Foreground = Colours.Teal,
				Padding = new Thickness(2, 0, 2, 2)
			});
			content.Children.Add(grid);

			return content;
		}
	}
}