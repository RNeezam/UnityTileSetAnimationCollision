using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileMapper : MonoBehaviour
{

    [SerializeField] int mapWidth;//d1 columns |
    [SerializeField] int mapHeight;//d0 rows ->
    int[,] tileMap = new int[3, 3]
        {
            {0,0,0},
            {0,0,0},
            {0,0,0}
        };

    [SerializeField] Sprite[] sprites;

    
    [ContextMenu("Randomize")]
   public void RandomizeTileMap()
    {
        //creating a new tilemap based on specified height and width
        this.tileMap = new int[mapHeight, mapWidth];

        string debugMap = string.Empty;

        for(int rows = 0;rows< tileMap.GetLength(0); rows++)
        {
            for(int col = 0; col<tileMap.GetLength(1); col++)
            {
                tileMap[rows, col] = UnityEngine.Random.RandomRange(1, 67);
                if (col == 0) debugMap += System.Environment.NewLine;

                debugMap += tileMap[rows, col] + ",";
               //debugMap += $"{tileMap[rows, col]},";
            }
        }
        print(debugMap);
        //GenerateTile(sprites[Random.Range(1, 67)]);
        RenderTileMap();
    }

    [ContextMenu("HardCode")]
    public void HardcoreTileMap()
    {

        tileMap = new int[3, 3]
        {

            {1,2,3 },
            {12,13,14 },
            {23,24,25 },

        };
        RenderTileMap();
    }

    [ContextMenu("Read From File")]
    public void SetTileMapFromFile()
    {
        TextAsset file = Resources.Load<TextAsset>("GreenHillZone1");
        string data = file.text;
        string widthText = string.Empty;
        string heightText = string.Empty;

        widthText = data.Substring(data.IndexOf("width=") + 6);
        widthText = widthText.Substring(0, widthText.IndexOf(System.Environment.NewLine));

        heightText = data.Substring(data.IndexOf("height=") + 7);
        heightText = heightText.Substring(0, heightText.IndexOf(System.Environment.NewLine));


        mapHeight =  int.Parse(heightText);
        mapWidth = int.Parse(widthText);

        string mapData = data.Substring(data.IndexOf("data=") + 5 + System.Environment.NewLine.Length);

        string[] split = mapData.Split(new Char[] {'\n'});

        
        this.tileMap = new int[mapHeight, mapWidth];
        int totalMap = mapHeight * mapWidth;

        mapData = mapData.Replace("\r", "").Replace("\n", "");
        
        for(int row = 0; row<mapHeight; row++)
        {
            for(int col = 0; col< mapWidth; col++)
            {
                string tempString = "";

                if (mapData.Contains(","))
                {
                    tempString = mapData.Substring(0, mapData.IndexOf(","));
                    mapData = mapData.Substring(mapData.IndexOf(tempString) + tempString.Length + 1);
                }
                else
                    tempString = mapData.Substring(0);

                tileMap[row, col] = int.Parse(tempString);
            }
        }
        RenderTileMap();

        print(mapData);
      
    }

    /// <summary>
    /// renders the whole map on screen
    /// </summary>
    void RenderTileMap()
    {
        //kill the children (bad practice lol)
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        for(int child = 0;child<children.Length;child++)
        {
            if (children[child] == this.transform) continue;
            DestroyImmediate(children[child].gameObject);
        }

        for (int row = 0; row < tileMap.GetLength(0); row++)
        {
            //generate tiles
            for (int col = 0; col < tileMap.GetLength(1); col++)
            {
                int spriteIndex = tileMap[row,col]; //get index of sprite
                Sprite curSprite = sprites[spriteIndex]; //get sprite using index
                GameObject newTile = GenerateTile(sprites[tileMap[row, col]]); // set SpriteRenderer using sprite
                newTile.transform.SetParent(this.transform);//Parent new tiles under Mapper
                newTile.name = "Tile " + row + "," + col; //Rename tiles

                //Offset Tiles
                float rowOffset = (tileMap.GetLength(0) / 2f) - 0.5f;
                float collOffset = tileMap.GetLength(0) / 2f - 0.5f;

                newTile.transform.Translate(col - collOffset, -row + rowOffset, 0);
            }
        }
    }

    /// <summary>
    /// Generate a tile and returns a game object
    /// </summary
    GameObject GenerateTile(Sprite sprite)
    {
        GameObject newTile = new GameObject();
        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = sprite;
        return newTile;
    }

}
