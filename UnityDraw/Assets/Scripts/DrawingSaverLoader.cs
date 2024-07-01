using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class DrawingSaverLoader : MonoBehaviour
{
    [SerializeField] private DrawScript drawScript;

    private string saveFileName = "drawing.json";

    public void SaveDrawing()
    {
        List<LineData> linesData = drawScript.GetLinesData();
        string json = JsonUtility.ToJson(new LineDataWrapper { lines = linesData }, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, saveFileName), json);
        Debug.Log("Drawing saved.");
    }

    public void LoadDrawing()
    {
        string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LineDataWrapper linesDataWrapper = JsonUtility.FromJson<LineDataWrapper>(json);
            drawScript.LoadLinesData(linesDataWrapper.lines);
            Debug.Log("Drawing loaded.");
        }
    }
}

[System.Serializable]
public class LineDataWrapper
{
    public List<LineData> lines;
}
