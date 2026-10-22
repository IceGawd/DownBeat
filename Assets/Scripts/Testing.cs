using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Testing : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  [SerializeField] int width;
  [SerializeField] int height;
  [SerializeField] float tileSize;
  [SerializeField] GameObject testTile;
  public int tileX;
  public int tileY;
  private Grid tgrid;  
  void Start()
  {
    tgrid = new Grid(width,height,tileSize);  
    for(int x  = 0; x < width; x++)
    {
      for(int y = 0; y < height; y++)
      {
        // if(x != Math.Floor((double)width/2) && y != Math.Floor((double)height / 2))
        // {
          GameObject insTile = Instantiate(testTile);
          insTile.transform.parent = transform;
          tgrid.setTile(x,y, insTile);

          // if(UnityEngine.Random.Range(0, 5) == 1)
          // {
          //   GameObject insTile = Instantiate(testTile);
          //   insTile.transform.parent = transform;
          //   grid.setTile(x,y, insTile);
          // }
        // }
        
      }
    }
  }

  void Update()
  {
    if (Keyboard.current.tKey.wasPressedThisFrame)
    {
      Debug.Log("Tile at " + tileX + ", " + tileY + "has its center at coordinates" + tgrid.getTileCenterPos(tileX, tileY));
    }
    if (Keyboard.current.yKey.wasPressedThisFrame)
    {
      Debug.Log("Grid center is at: ");
    }
  }

}
