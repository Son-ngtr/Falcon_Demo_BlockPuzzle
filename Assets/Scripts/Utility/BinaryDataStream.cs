using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

// Lưu và đọc dữ liệu nhị phân
public class BinaryDataStream
{
    // Lưu dữ liệu nhị phân của đối tượng vào tệp fileName
    public static void Save<T>(T serializedObject, string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);

        BinaryFormatter formater = new BinaryFormatter();
        FileStream fileStream = new FileStream(path + fileName + ".dat", FileMode.Create);

        try
        {
            formater.Serialize(fileStream, serializedObject);
        }
        catch (SerializationException e)
        {
            Debug.Log("Save filed. Error: " + e.Message);
        }
        finally { fileStream.Close(); }
    }

    // Kiểm tra tệp tên "fileName" có tồn tại không
    public static bool Exist(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        string fullFileName = fileName + ".dat";
        return File.Exists(path + fullFileName);
    }

    // Độc dữ liệu từ tệp "fileName"
    public static T Read<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        BinaryFormatter formater = new BinaryFormatter();
        FileStream fileStream = new FileStream(path + fileName + ".dat", FileMode.Open);
        T returnType = default(T);

        try
        {
            returnType = (T)formater.Deserialize(fileStream);
        }
        catch (SerializationException e)
        {
            Debug.Log("Read filed. Error: " + e.Message);
        }
        finally { fileStream.Close(); }

        return returnType;
    }
}
