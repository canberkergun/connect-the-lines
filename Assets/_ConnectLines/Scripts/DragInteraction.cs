using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragInteraction : MonoBehaviour
{
    [SerializeField] private GameObject lineRendererPrefab;
    
    private List<LineRenderer> staticLines = new List<LineRenderer>();
    private LineRenderer dynamicLine; // Current dragging line
    private Vector3 lastGemPosition;
    
    private List<Gem> selectedGems = new List<Gem>();
    private bool isDragging = false;
    private GemType? currentGemType = null;

    [SerializeField] private LayerMask gemLayer;
    [SerializeField] private GridManager gridManager;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag();
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            ContinueDrag();
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            EndDrag();
        }
    }

    void StartDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, gemLayer))
        {
            Gem gem = hit.collider.GetComponent<Gem>();
            if (gem != null)
            {
                isDragging = true;
                AddGemToSelection(gem);
                currentGemType = gem.GemType;
            }
        }
    }

    void ContinueDrag()
    {
        // raycast to detect gems during drag
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, gemLayer))
        {
            Gem gem = hit.collider.GetComponent<Gem>();
            if (gem != null && gem.GemType == currentGemType && !selectedGems.Contains(gem))
            {
                AddGemToSelection(gem);

                if (dynamicLine != null)
                {
                    Destroy(dynamicLine.gameObject);
                    dynamicLine = null;
                }
            }
            
            if (selectedGems.Count > 0)
            {
                UpdateDynamicLine(lastGemPosition, hit.point);
            }
        }
    }

    void EndDrag()
    {
        if (dynamicLine != null)
        {
            Destroy(dynamicLine.gameObject);
            dynamicLine = null;
        }

        foreach (var line in staticLines)
        {
            Destroy(line.gameObject);
        }
        
        staticLines.Clear();
        
        // Pop selected gems
        if (selectedGems.Count > 1)
        {
            gridManager.DestroyGems(selectedGems);
        }
        selectedGems[0].SetHiglight(false);
        selectedGems.Clear();
        isDragging = false;
        currentGemType = null;
    }

    void AddGemToSelection(Gem gem)
    {
        if (currentGemType != null && currentGemType == gem.GemType)
        {
            if (selectedGems.Count > 0 && !selectedGems.Contains(gem))
            {
                Gem lastSelectedGem = selectedGems[selectedGems.Count - 1];

                if (IsAdjacent(lastSelectedGem, gem))
                {
                    AddGem(gem);
                }
            }
        }
        else
        {
            AddGem(gem);
        }
    }

    bool IsAdjacent(Gem a, Gem b)
    {
        return (Mathf.Abs(a.X - b.X) == 1 && Mathf.Abs(a.Y - b.Y) == 0) || (Mathf.Abs(a.X - b.X) == 0 && Mathf.Abs(a.Y - b.Y) == 1);
    }

    void AddGem(Gem gem)
    {
        selectedGems.Add(gem);
        
        gem.SetHiglight(true);
        
        Vector3 gemPosition = gem.transform.position;

        if (selectedGems.Count > 1)
        {
            CreateStaticLine(lastGemPosition, gemPosition);
        }
        
        lastGemPosition = gemPosition;
    }

    void CreateStaticLine(Vector3 start, Vector3 end)
    {
        GameObject lineObject = Instantiate(lineRendererPrefab);
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        staticLines.Add(lineRenderer);
    }

    void UpdateDynamicLine(Vector3 start, Vector3 currentMousePosition)
    {
        if (dynamicLine == null)
        {
            GameObject lineObject = Instantiate(lineRendererPrefab);
            dynamicLine = lineObject.GetComponent<LineRenderer>();
        }
        
        dynamicLine.SetPosition(0, start);
        dynamicLine.SetPosition(1, currentMousePosition);
    }
}