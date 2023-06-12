using System.Collections.Generic;
using UnityEngine;
/**-----------------------------------------------序列化&反序列化List工具类----------------------------------------------*/
public static class SerializeList
{
    // 序列化 List<T> 对象为 JSON 字符串
    public static string ListToJson<T>(List<T> list)
    { 
        return JsonUtility.ToJson(new SerializationList<T>(list));
    }

    // 反序列化 JSON 字符串为 List<T> 对象
    public static List<T> ListFromJson<T>(string json)
    {
        return JsonUtility.FromJson<SerializationList<T>>(json).ToList();
    }
}

/**-----------------------------------------------序列化&反序列化中间类----------------------------------------------*/
[System.Serializable]
public class SerializationList<T>
{
    [SerializeField]
    List<T> target;
    public List<T> ToList() { return target; }

    public SerializationList(List<T> target)
    {
        this.target = target;
    }
}
