using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;

public class TestCreateFile : MonoBehaviour
{
    public void SaveByXML()
    {
        string directoryPath = Application.dataPath + "/PlayerData/";
        string filePath = directoryPath + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".xml";

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (File.Exists(filePath))
        {
            Debug.LogWarning("File already exists. Overwriting...");
        }

        XmlDocument xmlDocument = new XmlDocument();
        XmlElement root = xmlDocument.CreateElement("Root");
        xmlDocument.AppendChild(root);
        xmlDocument.Save(filePath);
        Debug.Log("XML FILE SAVED");
    }
}

