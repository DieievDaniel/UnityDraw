using UnityEngine;
using System.Collections.Generic;

namespace Shady
{
    public class Draw : MonoBehaviour
    {
        [SerializeField] private Camera cam = null;
        [SerializeField] private LineRenderer trailPrefab = null;

        private LineRenderer currentTrail;
        private List<Vector3> points = new List<Vector3>();

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

        private void ClearLines()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
