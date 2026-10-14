using System;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Testing : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  [SerializeField] int width;
  [SerializeField] int height;
  [SerializeField] float tileSize;
  [SerializeField] GameObject testTile;
  void Start()
  {
    Grid grid = new Grid(width,height,tileSize);  
    for(int x  = 0; x < width; x++)
    {
      for(int y = 0; y < height; y++)
      {
        if(x != Math.Ceiling((double)width/2) && y != Math.Ceiling((double)height / 2))
        {
          if(UnityEngine.Random.Range(0, 5) == 1)
          {
            GameObject insTile = Instantiate(testTile);
            insTile.transform.parent = transform;
            grid.setTile(x,y, insTile);
          }
        }
        
      }
    }
  }

}
