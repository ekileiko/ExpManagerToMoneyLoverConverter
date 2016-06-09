using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ExpManagerToMoneyLoverConverter.Helpers
{
    public class SimpleSerializer
    {
        public static string SerializeToXml(object message)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(message.GetType());

                var xmlnsEmpty = new XmlSerializerNamespaces();
                xmlnsEmpty.Add("", "");

                var sb = new StringBuilder();
                var w = new StringWriter(sb, CultureInfo.InvariantCulture);
                xmlSerializer.Serialize(w, message, xmlnsEmpty);

                return sb.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        
        public static string SerializeToXml(object message, string encodingName)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(message.GetType());
                var ms = new MemoryStream();

                var xmlnsEmpty = new XmlSerializerNamespaces();
                xmlnsEmpty.Add("", "");

                xmlSerializer.Serialize(ms, message, xmlnsEmpty);

                var xmlDocument = new XmlDocument();
                ms.Position = 0;
                xmlDocument.Load(ms);


                if (xmlDocument.FirstChild.NodeType != XmlNodeType.XmlDeclaration)
                    return xmlDocument.OuterXml;

                var xmlDeclaration = (XmlDeclaration)xmlDocument.FirstChild;
                xmlDeclaration.Encoding = encodingName;
                xmlDeclaration.Standalone = "yes";
                return xmlDocument.OuterXml;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        
        public static string SerializeToXmlWithoutStandalone(object message, string encodingName)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(message.GetType());
                var ms = new MemoryStream();

                var xmlnsEmpty = new XmlSerializerNamespaces();
                xmlnsEmpty.Add("", "");

                xmlSerializer.Serialize(ms, message, xmlnsEmpty);

                var xmlDocument = new XmlDocument();
                ms.Position = 0;
                xmlDocument.Load(ms);

                if (xmlDocument.FirstChild.NodeType != XmlNodeType.XmlDeclaration)
                    return xmlDocument.OuterXml;

                var xmlDeclaration = (XmlDeclaration)xmlDocument.FirstChild;
                xmlDeclaration.Encoding = encodingName;
                return xmlDocument.OuterXml;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        
        public static byte[] ObjectToByteArray(object _Object)
        {
            try
            {
                var memoryStream = new MemoryStream();
                var binaryFormatter = new BinaryFormatter();

                binaryFormatter.Serialize(memoryStream, _Object);
                return memoryStream.ToArray();
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }

        public static T ByteArrayToObject<T>(byte[] bytes)
        {
            var memoryStream = new MemoryStream();
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Position = 0;
            var binaryFormatter = new BinaryFormatter();

            var result = (T)binaryFormatter.Deserialize(memoryStream);

            return result;
        }
        
        public static T DeserializeXmlToObject<T>(string xml)
        {
            if (xml == null)
                return default(T);

            if (xml == string.Empty)
                return (T)Activator.CreateInstance(typeof(T));

            var reader = new StringReader(xml);
            var sr = new XmlSerializer(typeof(T));

            return (T)sr.Deserialize(reader);
        }

    }

    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));
            
            var wasEmpty = reader.IsEmptyElement;

            reader.Read();
            
            if (wasEmpty)
                return;
            
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");

                var key = (TKey)keySerializer.Deserialize(reader);

                reader.ReadEndElement();
                reader.ReadStartElement("value");

                var value = (TValue)valueSerializer.Deserialize(reader);

                reader.ReadEndElement();
                
                this.Add(key, value);
                
                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }
        
        public void WriteXml(XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (var key in this.Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
                writer.WriteStartElement("value");

                var value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

        }

        #endregion
    }
}
