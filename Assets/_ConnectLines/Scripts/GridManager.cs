using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 originPosition;
    [SerializeField] private Gem[] gems;

    
    private GridSystem2D<GridObject<Gem>> grid;

    private void Start()
    {
        grid = GridSystem2D<GridObject<Gem>>.VerticalGrid(width, height, cellSize, originPosition);
        PopulateGrid();
    }
    
    public void DestroyGems(List<Gem> gemsToDestroy)
    {
        foreach (Gem gem in gemsToDestroy)
        {
            gem.PlayExplosionEffect();

            foreach (PopGemsCondition condition in GameManager.Instance.GetCurrentConditions())
            {
                if (condition.gemType == gem.GemType)
                {
                    condition.IncrementCount(gem.GemType);
                    GameManager.Instance.NotifyConditionProgress(condition);
                }
            }
            
            gem.SetSpriteDisable();
            
            gem.SetHiglight(false);
            
            float delay = gem.GetExplosionDuration();
            
            // Remove gem from grid and destroy it
            grid.SetValue(gem.X, gem.Y, null);
            
            
            Destroy(gem.gameObject, delay);
        }

        Invoke(nameof(DropGems),0.5f);
    }

    void DropGems()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid.GetValue(x,y) == null)
                {
                    for (int dropY = y + 1; dropY < height; dropY++)
                    {
                        Gem gemAbove = grid.GetValue(x, dropY)?.Content;
                        if (gemAbove != null)
                        {
                            grid.SetValue(x, y, grid.GetValue(x, dropY));
                            gemAbove.Y = y;
                            grid.SetValue(x, dropY, null);

                            gemAbove.transform.DOMove(grid.GetWorldPositionCenter(x, y), 0.5f);
                            break;
                        }
                    }
                }
            }
        }

        SpawnNewGems();
    }

    void SpawnNewGems()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = height - 1; y >= 0; y--)
            {
                if (grid.GetValue(x, y) == null)
                {
                    Gem prefab = gems[Random.Range(0, gems.Length)];
                    Vector3 worldPosition = grid.GetWorldPositionCenter(x, y);
                    Gem newGem = Instantiate(prefab, worldPosition, Quaternion.identity);
                    newGem.X = x;
                    newGem.Y = y;
                    
                    grid.SetValue(x, y, new GridObject<Gem>(grid, x, y) { Content = newGem });
                }
            }
        }
    }
    
    void PopulateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var gridObject = new GridObject<Gem>(grid, x, y);
                
                //Select random gem
                Gem prefab = gems[Random.Range(0, gems.Length)];
                
                //Instantiate gem at grid position
                Vector3 worldPosition = grid.GetWorldPositionCenter(x, y);
                Gem gemInstance = Instantiate(prefab, worldPosition, Quaternion.identity);
                gemInstance.transform.parent = transform;
                gemInstance.X = x;
                gemInstance.Y = y;

                // set gem as content 
                gridObject.SetContent(gemInstance);
                
                // store gridObject in grid
                grid.SetValue(x,y, gridObject);
            }
        }
    }
}
