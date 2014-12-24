using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DublinBikes.External;
using DublinBikes.Resources;
using Microsoft.Phone.Maps;
using Microsoft.Phone.Maps.Controls;

namespace DublinBikes
{
	/// <summary>
	/// Main Page.
	/// </summary>
	public partial class MainPage
	{
		/// <summary>
		/// Defines whether the download failed at all.
		/// </summary>
		private bool m_downloadFailed;


		/// <summary>
		/// Defines whether the initial download was successful.
		/// </summary>
		private bool m_initialDownloadSuccessful;


		/// <summary>
		/// Initializes a new instance of the <see cref="MainPage"/> class.
		/// </summary>
		public MainPage()
		{
			InitializeComponent();
		}


		/// <summary>
		/// Raises the <see cref="E:NavigatedTo" /> event.
		/// </summary>
		/// <param name="eventArgs">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
		protected override void OnNavigatedTo(NavigationEventArgs eventArgs)
		{
			base.OnNavigatedTo(eventArgs);
			m_initialDownloadSuccessful = false;
			m_downloadFailed = false;
			DownloadAllStops();
		}


		/// <summary>
		/// Handles the Loaded event of the BikeMap control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="eventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
		private void BikeMap_Loaded(object sender, RoutedEventArgs eventArgs)
		{
			MapsSettings.ApplicationContext.ApplicationId = App.ApplicationId;
			MapsSettings.ApplicationContext.AuthenticationToken = App.MapAuthenticationToken;
		}


		/// <summary>
		/// Downloads all bike stops data.
		/// </summary>
		private void DownloadAllStops()
		{
			WebClient allStopsClient = new WebClient();
			allStopsClient.DownloadStringCompleted += (sender, eventData) =>
			{
				IEnumerable<BikeStop> bikeStops;
				if (!DublinBikesApi.TryParseAllStops(eventData, out bikeStops))
				{
					TagBlock.Text = AppResources.GenericErrorMessage;
					return;
				}

				foreach (BikeStop stop in bikeStops)
				{
					DownloadStopDetails(stop);
				}
			};

			// Asynchronously call the all stops API method.
			allStopsClient.DownloadStringAsync(new Uri(App.AllStopsUrl, UriKind.Absolute));
		}


		/// <summary>
		/// Downloads details from a specific bike stop.
		/// </summary>
		/// <param name="stop">The given bike stop.</param>
		private void DownloadStopDetails(BikeStop stop)
		{
			WebClient stopDetailsClient = new WebClient();
			stopDetailsClient.DownloadStringCompleted += (sender, eventData) =>
			{
				// Try parse the API call results
				BikeStopDetails details;
				if (!BikeStopDetails.TryParseDetailsEventData(eventData, out details))
				{
					TagBlock.Text = AppResources.FailedToLoadStopErrorMessage;
					if (!m_downloadFailed)
					{
						TitlePanel.Visibility = Visibility.Visible;
						m_downloadFailed = true;
					}
					return;
				}

				// If successful, remove the title panel
				if (!m_initialDownloadSuccessful && !m_downloadFailed)
				{
					TitlePanel.Visibility = Visibility.Collapsed;
					m_initialDownloadSuccessful = true;
				}

				// Create pin content from stop data
				StackPanel pinContent = stop.ToMapContent(details);

				// Add tap event for toolbar to appear
				pinContent.Tap += delegate
				{
					AddressBlock.Text = stop.Address;
					FreeBlock.Text = details.Free.ToString(CultureInfo.InvariantCulture);
					BikesBlock.Text = details.Available.ToString(CultureInfo.InvariantCulture);
					ToolbarGrid.Visibility = Visibility.Visible;
				};

				// Create pin
				MapOverlay pin = new MapOverlay
				{
					Content = pinContent,
					GeoCoordinate = stop.Location,
					PositionOrigin = new Point(0.5, 0.5),
				};

				// Add pin to map
				BikeMap.Layers.Add(new MapLayer { pin });
			};

			// Asynchronously call the stop details API method.
			stopDetailsClient.DownloadStringAsync(
				new Uri(string.Format(App.StationDetailsUrlFormat, stop.Id),
					UriKind.Absolute));
		}
	}
}