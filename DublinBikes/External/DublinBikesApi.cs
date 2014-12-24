using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace DublinBikes.External
{
	/// <summary>
	/// Layer around the Dublin Bikes API
	/// </summary>
	public static class DublinBikesApi
	{
		/// <summary>
		/// Tries to parse the event data of the all stops call.
		/// </summary>
		/// <param name="eventData">The <see cref="DownloadStringCompletedEventArgs"/> instance containing the event data.</param>
		/// <param name="stops">All bike stops.</param>
		/// <returns><c>True</c> if parsing was successful, else <c>false</c>.</returns>
		public static bool TryParseAllStops(DownloadStringCompletedEventArgs eventData, out IEnumerable<BikeStop> stops)
		{
			stops = null;
			if (eventData == null || eventData.Error != null || eventData.Result == null)
			{
				return false;
			}

			// Try and parse the result's XML
			XElement stopListElement;
			try
			{
				stopListElement = XElement.Parse(eventData.Result);
			}
			catch
			{
				return false;
			}

			// Dig one level down to the markers element
			XElement markersElement = stopListElement.Element("markers");
			if (markersElement == null)
			{
				return false;
			}

			// Parse all stops where possible
			stops = markersElement.Elements().Select(element =>
			{
				BikeStop stop;
				return BikeStop.TryParseStopElement(element, out stop) ? stop : null;
			}).Where(stop => stop != null);

			// Success is defined as having at least one stop.
			return stops.Any();
		}
	}
}