using System.IO.IsolatedStorage;

namespace DublinBikes.Tools
{
	/// <summary>
	/// Static class for handling data storage.
	/// </summary>
	public static class Storage
	{
		/// <summary>
		/// Loads data from storage localted by a given key.
		/// </summary>
		/// <typeparam name="T">Type of data to retrieve.</typeparam>
		/// <param name="key">The key.</param>
		/// <returns>Data from storage, null if not there.</returns>
		public static T Load<T>(string key)
		{
			T result;
			TryLoad(key, out result);
			return result;
		}


		/// <summary>
		/// Saves data to the storage located by a given key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="data">The data.</param>
		public static void Save(string key, object data)
		{
			IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
			settings[key] = data;
			settings.Save();
		}


		/// <summary>
		/// Tries to load data from storage, using a given key.
		/// </summary>
		/// <typeparam name="T">Type of data to retrieve.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="data">The data.</param>
		/// <returns><c>True</c> if load was successful, else <c>false</c>.</returns>
		public static bool TryLoad<T>(string key, out T data)
		{
			IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
			return settings.TryGetValue(key, out data);
		}
	}
}