using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Zenseless.Patterns
{
	/// <summary>
	/// Contains class instance serialization/deserialization methods. 
	/// Can be used for persisting class instances to disc and reading them back to memory.
	/// </summary>
	public static class Serialization
	{
		/// <summary>
		/// Deserializes from an XML file into a new class instance of a given type.
		/// </summary>
		/// <param name="fileName">The file name from which the serialized instance will be restored from.</param>
		/// <param name="type">The type of the class that will be deserialized.</param>
		/// <returns>Deserialized class instance</returns>
		public static object FromXMLFile(string fileName, Type type)
		{
			using (StreamReader inFile = new StreamReader(fileName))
			{
				XmlSerializer formatter = new XmlSerializer(type);
				return formatter.Deserialize(inFile);
			}
		}

		/// <summary>
		/// Deserializes from an XML string into a new class instance of a given type.
		/// </summary>
		/// <param name="xmlString">XML string from which to deserialize.</param>
		/// <param name="type">The type of the class that will be deserialized.</param>
		/// <returns>Deserialized class instance</returns>
		public static object FromXmlString(string xmlString, Type type)
		{
			using (StringReader input = new StringReader(xmlString))
			{
				XmlSerializer formatter = new XmlSerializer(type);
				return formatter.Deserialize(input);
			}
		}

		/// <summary>
		/// Deserializes an new obj instance from a binary file.
		/// </summary>
		/// <param name="fileName">The file name from which the serialized instance will be restored from.</param>
		/// <returns>Deserialized class instance</returns>
		public static object FromBinFile(string fileName)
		{
			using (FileStream inFile = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				return FromBinStream(inFile);
			}
		}

		/// <summary>
		/// Deserializes an new obj instance from a binary stream.
		/// </summary>
		/// <param name="binStream">The binary stream from which the serialized instance will be restored from.</param>
		/// <returns>Deserialized class instance</returns>
		public static object FromBinStream(Stream binStream)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			return formatter.Deserialize(binStream);
		}

		/// <summary>
		/// Serializes the given class instance into a XML format file.
		/// </summary>
		/// <param name="serializable">The class instance to be serialized.</param>
		/// <param name="fileName">The file name the serialized instance will be stored to.</param>
		public static void ToXMLFile(object serializable, string fileName)
		{
			if (serializable is null) new ArgumentNullException("Null object given!");
			XmlSerializer formatter = new XmlSerializer(serializable.GetType());
			using (StreamWriter outfile = new StreamWriter(fileName))
			{
				formatter.Serialize(outfile, serializable);
			}
		}

		/// <summary>
		/// Serializes the given class instance into a XML string.
		/// </summary>
		/// <param name="serializable">The class instance to be serialized.</param>
		public static string ToXmlString(object serializable)
		{
			if (serializable is null) new ArgumentNullException("Null object given!");
			XmlSerializer formatter = new XmlSerializer(serializable.GetType());
			StringBuilder builder = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings
			{
				Encoding = Encoding.Default,
				Indent = false,
				OmitXmlDeclaration = true,
				NamespaceHandling = NamespaceHandling.OmitDuplicates
			};
			using (XmlWriter writer = XmlWriter.Create(builder, settings))
			{
				formatter.Serialize(writer, serializable);
			}
			string output = builder.ToString();
			return output;
		}

		/// <summary>
		/// Serializes the given class instance into a binary file.
		/// </summary>
		/// <param name="serializable">The class instance to be serialized.</param>
		/// <param name="fileName">The file name the serialized instance will be stored to.</param>
		public static void ToBinFile(object serializable, string fileName)
		{
			using (FileStream outfile = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				ToBinStream(serializable, outfile);
			}
		}

		/// <summary>
		/// Serializes the given class instance into the given stream.
		/// </summary>
		/// <param name="serializable">The class instance to be serialized.</param>
		/// <param name="output">Stream to serialize to</param>
		public static void ToBinStream(object serializable, Stream output)
		{
			if (serializable is null) new ArgumentNullException("Null object given!");
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(output, serializable);
		}
	}
}
