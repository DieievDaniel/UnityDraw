using UnityEngine;
using System.Collections.Generic;

public class DrawScript : MonoBehaviour
{
    [SerializeField] private Camera cam = null;
    [SerializeField] private LineRenderer trailPrefab = null;

    private LineRenderer currentTrail;
    private List<Vector3> points = new List<Vector3>();
    private float lineWidth = 0.1f;
    private Color lineColor = Color.white;

    #region MONO
    private void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    private void Update()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
        #elif UNITY_IOS || UNITY_ANDROID
        HandleTouchInput();
        #endif
    }
    #endregion
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateNewLine();
        }

        if (Input.GetMouseButton(0))
        {
            AddPoint(Input.mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearLines();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    CreateNewLine();
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    AddPoint(touch.position);
                    break;
            }
        }
    }

    private void CreateNewLine()
    {
        currentTrail = Instantiate(trailPrefab);
        currentTrail.transform.SetParent(transform, true);
        currentTrail.startWidth = lineWidth;
        currentTrail.endWidth = lineWidth;
        currentTrail.startColor = lineColor;
        currentTrail.endColor = lineColor;
        points.Clear();
    }

    private void UpdateLinePoints()
    {
        if (currentTrail != null && points.Count > 1)
        {
            currentTrail.positionCount = points.Count;
            currentTrail.SetPositions(points.ToArray());
        }
    }

    private void AddPoint(Vector3 screenPosition)
    {
        Ray ray = cam.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Writeable"))
            {
                points.Add(hit.point);
                UpdateLinePoints();
            }
        }
    }

    public void ClearLines()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetLineWidth(float width)
    {
        lineWidth = width;
    }

    public void SetLineColor(Color color)
    {
        lineColor = color;
    }

    public List<LineData> GetLinesData()
    {
        List<LineData> linesData = new List<LineData>();
        foreach (Transform child in transform)
        {
            LineRenderer lineRenderer = child.GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                linesData.Add(new LineData
                {
                    points = new List<Vector3>(lineRenderer.positionCount),
                    lineWidth = lineRenderer.startWidth,
                    lineColor = lineRenderer.startColor
                });
                for (int i = 0; i < lineRenderer.positionCount; i++)
                {
                    linesData[linesData.Count - 1].points.Add(lineRenderer.GetPosition(i));
                }
            }
        }
        return linesData;
    }

    public void LoadLinesData(List<LineData> linesData)
    {
        ClearLines();
        foreach (LineData lineData in linesData)
        {
            currentTrail = Instantiate(trailPrefab);
            currentTrail.transform.SetParent(transform, true);
            currentTrail.startWidth = lineData.lineWidth;
            currentTrail.endWidth = lineData.lineWidth;
            currentTrail.startColor = lineData.lineColor;
            currentTrail.endColor = lineData.lineColor;
            currentTrail.positionCount = lineData.points.Count;
            currentTrail.SetPositions(lineData.points.ToArray());
        }
    }
}

[System.Serializable]
public class LineData
{
    public List<Vector3> points;
    public float lineWidth;
    public Color lineColor;
}
