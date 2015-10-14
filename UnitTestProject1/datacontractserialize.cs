using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;


public static class SerializationExtensions {
    public static string Serialize<T>(this T obj) {
        var serializer = new DataContractSerializer(obj.GetType());
        using(var writer = new StringWriter())
        using(var stm = new XmlTextWriter(writer)) {
            stm.Formatting = Formatting.Indented;
            serializer.WriteObject(stm, obj);
            return writer.ToString();
        }
    }
    public static T Deserialize<T>(this string serialized) {
        var serializer = new DataContractSerializer(typeof(T));
        using(var reader = new StringReader(serialized))
        using(var stm = new XmlTextReader(reader)) {
            return (T)serializer.ReadObject(stm);
        }
    }
}