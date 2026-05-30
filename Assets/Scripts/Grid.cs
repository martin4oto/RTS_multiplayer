using System.Collections.Generic;

using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    private GridCell[,] grid;
    private void Start()
    {
        grid = new GridCell[width,height];
        for(int i=0; i<grid.GetLength(0); i++)
        {
            for(int j=0; j<grid.GetLength(1);j++)
            {
                grid[i, j] = new();
            }
        }
    }
    public void SetBuilding(Building building, List<Vector3> allBuildingPositions)
    {
        foreach(var p in allBuildingPositions)
        {
            (int x, int y) = WorldToGridPosition(p);
            grid[x,y].SetBuilding(building);
        }
    }
    public bool CanBuild(List<Vector3> allBuildingPositions)
    {
        foreach(var p in allBuildingPositions)
        {
            (int x, int y)= WorldToGridPosition(p);
            if(x<0 || x>=width || y<0 || y>=height)
                return false;
            if(!grid[x,y].IsEmpty())
                return false;
        }
        return true;
    }
    private (int x, int y) WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition-transform.position).x/BuildingSystem.CellSize);
        int y = Mathf.FloorToInt((worldPosition-transform.position).y/BuildingSystem.CellSize);
        return (x,y);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(BuildingSystem.CellSize <= 0 || width <= 0 || height <= 0) return;
        Vector3 origin = transform.position;
        for(int i=0; i<= height; i++)
        {
            Vector3 start = origin + new Vector3(0, i*BuildingSystem.CellSize, 0.01f);
            Vector3 end = origin + new Vector3(width*BuildingSystem.CellSize, i*BuildingSystem.CellSize,0.01f); 
            Gizmos.DrawLine(start, end);
        }   
        for(int i=0; i<= width;i++)
        {
            Vector3 start = origin + new Vector3(i * BuildingSystem.CellSize, 0,0.01f); 
            Vector3 end = origin + new Vector3(i * BuildingSystem.CellSize, height*BuildingSystem.CellSize, 0.01f);
            Gizmos.DrawLine(start, end);
        }    
    }
}
public class GridCell
{
    private Building building;
    public void SetBuilding(Building building)
    {
        this.building = building;
    }
    public bool IsEmpty()
    {
        return building == null;
    }
}
