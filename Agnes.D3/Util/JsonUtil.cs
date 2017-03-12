// ------------------------------------------
// <copyright file="JsonUtil.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Agnes.D3
//    Last updated: 2017/03/10
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Agnes.D3.Util
{
    public static class JsonUtil
    {
        #region Static Fields & Constants

        public const BindingFlags ALL_MEMBERS =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public const BindingFlags DEFAULT_MEMBERS = BindingFlags.Instance | BindingFlags.Public;

        public static JsonSerializerSettings ArgsSettings =
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

        public static JsonSerializerSettings ConfigSettings =
            new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

        public static JsonSerializerSettings PreserveReferencesSettings =
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };

        public static JsonSerializerSettings TypeSpecifySettings =
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            };

        #endregion

        #region Public Methods

        public static JsonSerializer CreateJsonSerializer(JsonSerializerSettings settings)
        {
            var jsonSerializer = new JsonSerializer();
            if (settings == null) return jsonSerializer;
            jsonSerializer.TypeNameHandling = settings.TypeNameHandling;
            jsonSerializer.TypeNameAssemblyFormat = settings.TypeNameAssemblyFormat;
            jsonSerializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
            jsonSerializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
            jsonSerializer.ContractResolver = settings.ContractResolver;
            return jsonSerializer;
        }

        public static T DeserializeJson<T>(
            string objStr, JsonSerializerSettings settings = null, bool nested = false)
        {
            using (var stringReader = new StringReader(objStr))
                return DeserializeJson<T>(new JsonTextReader(stringReader), settings, nested);
        }

        public static T DeserializeJsonFile<T>(
            string fileName, JsonSerializerSettings settings = null, bool nested = false, bool binary = false)
        {
            if (!File.Exists(fileName)) return default(T);
            if (binary)
                using (var reader = new BinaryReader(File.OpenRead(fileName)))
                    return DeserializeJson<T>(new BsonReader(reader), settings, nested);

            using (var streamReader = new StreamReader(fileName))
                return DeserializeJson<T>(new JsonTextReader(streamReader), settings, nested);
        }

        public static void SaveToXmlFile<T>(string filePath, T obj)
        {
            var jsonSettings = new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore};
            var xmlDocument = JsonConvert.DeserializeXmlNode(
                JsonConvert.SerializeObject(new NestedObject<T>(obj), Formatting.Indented, jsonSettings));
            xmlDocument.Save(filePath);
        }

        public static string SerializeJson<T>(
            this T obj, JsonSerializerSettings settings = null, Formatting formatting = Formatting.Indented,
            bool nest = false)
        {
            using (var stringWriter = new StringWriter())
            {
                SerializeJson(obj, new JsonTextWriter(stringWriter) {Formatting = formatting}, settings, nest);
                return stringWriter.ToString();
            }
        }

        public static void SerializeJsonFile<T>(
            this T obj, string fileName, JsonSerializerSettings settings = null,
            Formatting formatting = Formatting.Indented, bool nest = false, bool binary = false)
        {
            if (!PathUtil.VerifyFilePathCreation(fileName)) return;

            if (binary)
                using (var writer = new BinaryWriter(File.OpenWrite(fileName)))
                    SerializeJson(obj, new BsonWriter(writer), settings, nest);
            else
                using (var sw = new StreamWriter(fileName))
                    SerializeJson(obj, new JsonTextWriter(sw) {Formatting = formatting}, settings, nest);
        }

        #endregion

        #region Nested type: NestedObject

        #region Private Classes

        private sealed class NestedObject<T>
        {
            #region Properties & Indexers

            #region Public Properties

            [JsonProperty(TypeNameHandling = TypeNameHandling.All, Order = 1000000)]
            public T Value { get; }

            #endregion Public Properties

            #endregion

            #region Constructors

            public NestedObject(T value)
            {
                this.Value = value;
            }

            #endregion
        }

        #endregion Private Classes

        #endregion

        #region Private Methods

        private static T DeserializeJson<T>(
            JsonReader reader, JsonSerializerSettings settings = null, bool nested = false)
        {
            var objectType = nested ? typeof(NestedObject<T>) : typeof(T);
            var serializer = CreateJsonSerializer(settings);
            var obj = serializer.Deserialize(reader, objectType);
            return nested ? ((NestedObject<T>) obj).Value : (T) obj;
        }

        private static void SerializeJson<T>(
            this T obj, JsonWriter writer, JsonSerializerSettings settings = null, bool nest = false)
        {
            var jsonSerializer = CreateJsonSerializer(settings);
            jsonSerializer.Serialize(writer, nest ? (object) new NestedObject<T>(obj) : obj);
        }

        #endregion Private Methods
    }
}