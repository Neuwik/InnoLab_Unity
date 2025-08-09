using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class UMLSaveSystem
{
    private static string folderName = "UMLTrees";
    public static void SaveTreeBin(UMLTree tree)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);

        UMLTreeData data = new UMLTreeData(tree);

        string filePath = Path.Combine(folderPath, $"{data.TreeName}.{data.ID}.bin");

        FileStream stream = new FileStream(filePath, FileMode.Create);
        
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, data);
        stream.Close();
    }
    
    public static void DeleteSaves(string timestamp = "")
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);

        if (!Directory.Exists(folderPath))
        {
            return;
        }

        if (String.IsNullOrEmpty(timestamp)) timestamp = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")}";

        // Save a new Backup every time
        string newFolderPath = Path.Combine(Application.persistentDataPath, $"{folderName}_{timestamp}");

        // Only save latest Backup
        // string newFolderPath = Path.Combine(Application.persistentDataPath, $"{folderName}_prev");

        if (Directory.Exists(newFolderPath))
        {
            Directory.Delete(newFolderPath, true);
        }

        Directory.Move(folderPath, newFolderPath);
    }

    public static void SaveTree(UMLTree tree)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);

        Directory.CreateDirectory(folderPath);

        UMLTreeData data = new UMLTreeData(tree);

        if (data.IsValid())
        {
            string filePath = Path.Combine(folderPath, $"{tree.UTreeName}.json");

            Debug.Log($"Saving Tree {tree.UTreeName} with {data.elements.Length} Elements");

            string json = JsonUtility.ToJson(data, true);

            File.WriteAllText(filePath, json);
        }
    }

    public static List<UMLTreeData> LoadTrees()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        List<UMLTreeData> trees = new List<UMLTreeData>();

        if (!Directory.Exists(folderPath))
        {
            return trees;
        }

        foreach (string filePath in Directory.GetFiles(folderPath, "*.json"))
        {
            string json = File.ReadAllText(filePath);
            UMLTreeData tree = JsonUtility.FromJson<UMLTreeData>(json);
            trees.Add(tree);
        }

        trees.OrderBy(t => t.TreeName);

        return trees;
    }
}
