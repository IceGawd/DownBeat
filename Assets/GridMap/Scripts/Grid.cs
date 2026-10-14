using Unity.VisualScripting.FullSerializer;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid {
    private int width;
    private int height;
    private float tileSize;
    private Vector3 adjustOrigin;
    private GameObject[,] gridArray;
    private Vector3 tileCenterOffset;
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
        insTile.transform.position = GetWorldPos(x,y) + new Vector3(tileSize, tileSize) * .5f;
        gridArray[x,y] = insTile;

    }

    private Vector3 GetWorldPos(int x, int y)
    {
        return (new Vector3(x,y) + adjustOrigin)* tileSize;
    }

}
