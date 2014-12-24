using System.Xml.Linq;

namespace DublinBikes.Extensions
{
	/// <summary>
	/// Extension methods for the XElement class.
	/// </summary>
	public static class XElementExtensions
	{
		/// <summary>
		/// Given an element, tries to get a named value.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="valueName">The value's name.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>True</c> if call was successful, else <c>false</c>.</returns>
		private delegate bool TryGetValue(XElement element, string valueName, out string value);


		/// <summary>
		/// Gets an attribute's value as a double.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="attributeName">Name of the attribute.</param>
		/// <returns>The value of the attribute.</returns>
		public static double GetAttributeDoubleValue(this XElement element, string attributeName)
		{
			return element.GetDoubleValue(attributeName, TryGetAttributeValue);
		}


		/// <summary>
		/// Gets an attribute's value as an integer.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="attributeName">Name of the attribute.</param>
		/// <returns>The value of the attribute.</returns>
		public static int GetAttributeIntValue(this XElement element, string attributeName)
		{
			return element.GetIntValue(attributeName, TryGetAttributeValue);
		}


		/// <summary>
		/// Gets an attribute's value as an string.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="attributeName">Name of the attribute.</param>
		/// <returns>The value of the attribute.</returns>
		public static string GetAttributeStringValue(this XElement element, string attributeName)
		{
			return element.GetStringValue(attributeName, TryGetAttributeValue);
		}


		/// <summary>
		/// Gets a child element's value as an integer.
		/// </summary>
		/// <param name="parentElement">The parent element.</param>
		/// <param name="childName">Name of the child element.</param>
		/// <returns>The value of the child element.</returns>
		public static int GetChildElementIntValue(this XElement parentElement, string childName)
		{
			return parentElement.GetIntValue(childName, TryGetChildElementValue);
		}


		#region Private Helpers

		/// <summary>
		/// Gets a double value from the element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="valueName">Name of the value.</param>
		/// <param name="tryGetValue">Method to try and get the value.</param>
		/// <returns>The value as a double.</returns>
		private static double GetDoubleValue(this XElement element, string valueName, TryGetValue tryGetValue)
		{
			string stringValue;
			double value;
			return (element.ValueExists(valueName, tryGetValue, out stringValue) && double.TryParse(stringValue, out value))
				? value
				: default(double);
		}


		/// <summary>
		/// Gets an integer value from the element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="valueName">Name of the value.</param>
		/// <param name="tryGetValue">Method to try and get the value.</param>
		/// <returns>The value as an integer.</returns>
		private static int GetIntValue(this XElement element, string valueName, TryGetValue tryGetValue)
		{
			string stringValue;
			int value;
			return (element.ValueExists(valueName, tryGetValue, out stringValue) && int.TryParse(stringValue, out value))
				? value
				: default(int);
		}


		/// <summary>
		/// Gets a string value from the element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="valueName">Name of the value.</param>
		/// <param name="tryGetValue">Method to try and get the value.</param>
		/// <returns>The value as a string.</returns>
		private static string GetStringValue(this XElement element, string valueName, TryGetValue tryGetValue)
		{
			string value;
			return (element.ValueExists(valueName, tryGetValue, out value))
				? value
				: default(string);
		}


		#region TryGetValue delegate methods

		/// <summary>
		/// Given an element, tries to get an attribute's value.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="attributeName">Name of the attribute.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>True</c> if value retrieved, else <c>false</c>.</returns>
		private static bool TryGetAttributeValue(XElement element, string attributeName, out string value)
		{
			XAttribute attribute = element.Attribute(attributeName);
			if (attribute != null)
			{
				value = attribute.Value;
				return true;
			}

			value = null;
			return false;
		}


		/// <summary>
		/// Given an element, tries to get a child element's value.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="childName">Name of the child.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>True</c> if value retrieved, else <c>false</c>.</returns>
		private static bool TryGetChildElementValue(XElement element, string childName, out string value)
		{
			XElement childElement = element.Element(childName);
			if (childElement != null)
			{
				value = childElement.Value;
				return true;
			}

			value = null;
			return false;
		}

		#endregion


		/// <summary>
		/// Checks whether a value exists in respect to a given element, and if so outputs it.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="valueName">Name of the value.</param>
		/// <param name="tryGetValue">Method to try and get the value.</param>
		/// <param name="value">The output value.</param>
		/// <returns><c>True</c> if value exists, else <c>false</c>.</returns>
		private static bool ValueExists(this XElement element, string valueName, TryGetValue tryGetValue, out string value)
		{
			value = null;
			return (element != null && !string.IsNullOrWhiteSpace(valueName) && tryGetValue.Invoke(element, valueName, out value));
		}

		#endregion
	}
}