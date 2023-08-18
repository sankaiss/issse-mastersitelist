using System;
using System.Collections.Generic;
using System.Text.Json;

namespace DotNetCoreSqlDb.Data
{
    public static class ConvertData<T>
    {
        public static List<T> ByteArrayToObjectList(byte[] inputByteArray)
        {
            var deserializedList = JsonSerializer.Deserialize<List<T>>(inputByteArray);
            if(deserializedList == null) 
            {
                throw new InvalidOperationException("Failed to deserialize byte array to List<T>.");
            }
            return deserializedList;
        }

        public static byte[] ObjectListToByteArray(List<T> inputList)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(inputList);
            return bytes;
        }

        public static T ByteArrayToObject(byte[] inputByteArray)
        {
            var deserializedObject = JsonSerializer.Deserialize<T>(inputByteArray);
            if (deserializedObject == null)
            {
                throw new InvalidOperationException("Failed to deserialize byte array to object.");
            }
            return deserializedObject;
        }

        public static byte[] ObjectToByteArray(T input)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(input);
            return bytes;
        }
    }
}

