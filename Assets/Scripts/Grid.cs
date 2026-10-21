using Unity.VisualScripting.FullSerializer;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour {
    public int width;
    public int height;
    public float tileSize;
    public Vector3 adjustOrigin;
    public GameObject[,] gridArray;
    public Vector3 tileCenterOffset;
    public Grid(int width, int height, float tileSize)
    {
        this.width = width;
        this.height = height;
        this.tileSize = tileSize;

        adjustOrigin = new Vector3((float)-width / 2f, (float)-height / 2f) ;

        gridArray = new GameObject[width, height];

        Debug.Log("width: " + width + ". Height: " + height);

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), Color.white, 100f);
    
    }

    public void setTile(int x, int y, GameObject insTile)
    {
        insTile.transform.position = GetWorldPos(x,y) + new Vector3(tileSize / 2, tileSize / 2);
        gridArray[x,y] = insTile;

    }

    public Vector3 getTileCenterPos(int x, int y){
        return GetWorldPos(x,y) + new Vector3(tileSize / 2, tileSize / 2);
    }

    public Vector3 getGridCenter(){
        return new Vector3(0f, 0f);
    }

    public Vector3 GetWorldPos(int x, int y)
    {
        return (new Vector3(x,y) + adjustOrigin)* tileSize;
    }

    public Vector3 GetWorldPos(GridPosition gp)
    {
        return GetWorldPos(gp.x, gp.y);
    }

    public bool inGrid(GridPosition gp) {
        return System.Math.Abs(gp.x) <= width / 2 && System.Math.Abs(gp.y) <= height / 2;
    }
}
