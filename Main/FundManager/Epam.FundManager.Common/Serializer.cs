using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Epam.FundManager.Common
{
    public static class Serializer
    {
        /// <summary>
        /// Тип сериализатора
        /// </summary>
        [Serializable]
        [DataContract]
        public enum Type
        {
            [EnumMember]
            DataContractSerializer,

            [EnumMember]
            XmlSerializer,
        }

        /// <summary>
        /// Serialize the input object using "XmlSerializer" or "DataContractSerializer"
        /// </summary>
        /// <param name="type">The type of object to serialize</param>
        /// <param name="source">The serialazable object</param>
        /// <param name="encoding">The output xml encoding</param>
        /// <param name="serializer">Serialization type</param>
        /// <returns>XML document containing the serialized data</returns>
        public static XmlDocument Serialize(System.Type type, object source, Encoding encoding, Type serializer = Type.DataContractSerializer)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (ReferenceEquals(source, null))
                throw new ArgumentNullException("source");

            var sb = new StringBuilder();
            var doc = new XmlDocument();
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                Indent = true
            };
            using (var writer = XmlWriter.Create(sb, settings))
            {
                if (serializer == Type.XmlSerializer)
                {
                    new XmlSerializer(type).Serialize(writer, source);
                    writer.Flush();
                    doc.LoadXml(sb.ToString());
                }
                else
                {
                    new DataContractSerializer(type).WriteObject(writer, source);
                    writer.Flush();
                    doc.LoadXml(sb.ToString());
                }
                return XmlUtility.Canonize(doc, encoding);
            }
        }

        public static XmlDocument Serialize(System.Type type, object source, Type serializer = Type.DataContractSerializer)
        {
            return Serialize(type, source, Encoding.UTF8, serializer);
        }

        /// <summary>
        /// Serialize the input object using "XmlSerializer" or "DataContractSerializer"
        /// </summary>
        /// <typeparam name="T">The type of object to serialize</typeparam>
        /// <param name="source">The serialazable object</param>
        /// <param name="encoding">The output xml encoding</param>
        /// <param name="serializer">Serialization type</param>
        /// <returns>XML document containing the serialized data</returns>
        public static XmlDocument Serialize<T>(T source, Encoding encoding, Type serializer = Type.DataContractSerializer)
        {
            return Serialize(typeof(T), source, encoding, serializer);
        }

        public static XmlDocument Serialize<T>(T source, Type serializer = Type.DataContractSerializer)
        {
            return Serialize(source, Encoding.UTF8, serializer);
        }

        public static T Deserialize<T>(XmlDocument doc, Type serializer = Type.DataContractSerializer)
        {
            if (doc == null)
                throw new ArgumentNullException("doc");
            if (doc.DocumentElement == null)
                throw new ArgumentOutOfRangeException("doc", "doc.DocumentElement can not be null");

            using (var reader = new XmlNodeReader(doc.DocumentElement))
            {
                if (serializer == Type.XmlSerializer)
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                }
                return (T)new DataContractSerializer(typeof(T)).ReadObject(reader);
            }
        }

        public static T Deserialize<T>(string xml, Type serializer = Type.DataContractSerializer)
        {
            if (xml == null)
                throw new ArgumentNullException("xml");
            if (string.IsNullOrWhiteSpace("xml"))
                throw new ArgumentOutOfRangeException("xml", "Xml is empty or whitespace.");

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return Deserialize<T>(doc, serializer);
        }

        public static T Deserialize<T>(Stream stream, Type serializer = Type.DataContractSerializer)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            var doc = new XmlDocument();
            doc.Load(stream);
            return Deserialize<T>(doc, serializer);
        }

        public static void SerializeToFile<T>(T source, string filename, Encoding encoding, bool avoidSelfClosedTags, Type serializer = Type.DataContractSerializer)
        {
            if (ReferenceEquals(source, null))
                throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                Indent = true,
                Encoding = encoding,
            };

            using (var file = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                XmlWriter writer = (avoidSelfClosedTags)
                                       ? new EncodedXmlTextWriter(file, encoding, true)
                                       : XmlWriter.Create(file, settings);

                using (writer)
                {
                    if (serializer == Type.XmlSerializer)
                    {
                        new XmlSerializer(typeof(T)).Serialize(writer, source);
                        writer.Flush();
                    }
                    else
                    {
                        var dataContractSerializer = new DataContractSerializer(typeof(T));
                        dataContractSerializer.WriteObject(writer, source);
                        writer.Flush();
                    }
                }
            }
        }

        public static void SerializeToFile<T>(T source, string filename, bool avoidSelfClosedTags, Type serializer = Type.DataContractSerializer)
        {
            SerializeToFile(source, filename, Encoding.UTF8, avoidSelfClosedTags, serializer);
        }

        public static void SerializeToFile<T>(T source, string filename, Type serializer = Type.DataContractSerializer)
        {
            SerializeToFile(source, filename, Encoding.UTF8, false, serializer);
        }

        public static T DeserializeFromFile<T>(string filename, Type serializer = Type.DataContractSerializer)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");
            if (!File.Exists(filename))
                throw new ArgumentOutOfRangeException("filename", string.Format("File \"{0}\" does not exist.", filename));

            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (XmlReader reader = XmlReader.Create(file))
                {
                    if (serializer == Type.XmlSerializer)
                    {
                        return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                    }
                    return (T)new DataContractSerializer(typeof(T)).ReadObject(reader);
                }
            }
        }

        public static string SerializeToJson<T>(T source)
        {
            if (ReferenceEquals(source, null))
                throw new ArgumentNullException("source");

            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, source);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string SerializeToJson(this object source)
        {
            if (ReferenceEquals(source, null))
                throw new ArgumentNullException("source");

            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(source.GetType());
                serializer.WriteObject(stream, source);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static T DeserializeFromJson<T>(string json)
        {
            if (json == null)
                throw new ArgumentNullException("json");

            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(json);
                    writer.Flush();
                    stream.Position = 0;
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    return (T)serializer.ReadObject(stream);
                }
            }
        }
    }
}
