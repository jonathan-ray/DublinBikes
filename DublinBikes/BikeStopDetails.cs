using System.Net;
using System.Xml.Linq;
using DublinBikes.Extensions;

namespace DublinBikes
{
	/// <summary>
	/// Object representation of the live numbers of a bike stop.
	/// </summary>
	public class BikeStopDetails
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="BikeStopDetails"/> class from being created.
		/// </summary>
		private BikeStopDetails()
		{
		}


		/// <summary>
		/// The amount of bikes available.
		/// </summary>
		public int Available { get; set; }


		/// <summary>
		/// The amount of bike slots free.
		/// </summary>
		public int Free { get; set; }


		/// <summary>
		/// The total amount of bike slots at this stop.
		/// </summary>
		public int Total { get; set; }


		/// <summary>
		/// Parses the event data of the station details API call.
		/// </summary>
		/// <param name="detailsEventData">The <see cref="DownloadStringCompletedEventArgs" /> instance containing the event data.</param>
		/// <param name="details">Parsed bike stop details instance.</param>
		/// <returns><c>True</c> if parse was successful, else <c>false</c>.</returns>
		public static bool TryParseDetailsEventData(DownloadStringCompletedEventArgs detailsEventData, out BikeStopDetails details)
		{
			// Default out assignment
			details = null;

			// Check parameter
			if (detailsEventData == null || detailsEventData.Error != null || detailsEventData.Result == null)
			{
				return false;
			}

			// Try parse the results as valid XML
			XElement stationXml;
			try
			{
				stationXml = XElement.Parse(detailsEventData.Result);
			}
			catch
			{
				return false;
			}

			// Retrieve the numbers from the parsed XML
			details = new BikeStopDetails
			{
				Available = stationXml.GetChildElementIntValue("available"),
				Free = stationXml.GetChildElementIntValue("free"),
				Total = stationXml.GetChildElementIntValue("total")
			};

			return true;
		}
	}
}