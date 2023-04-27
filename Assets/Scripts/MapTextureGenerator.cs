using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTextureGenerator : MonoBehaviour
{
    public static Texture2D Generate(bool[,] map, Vector2 playerRoom)
    {
        // create a new Texture2D of the map's size
        Texture2D tex = new Texture2D(map.GetLength(0), map.GetLength(1));
        // pixellate the image
        tex.filterMode = FilterMode.Point;

         // make a color array of the map's size
        Color[] pixels = new Color[map.GetLength(0) * map.GetLength(1)];

        // for each pixel inside the array
        for(int i = 0; i<pixels.Length; i++)
        {  
            //get the current column
            int x = i % map.GetLength(0);
            // get the current row rounding to the neareest
            int y = Mathf.FloorToInt(i / map.GetLength(0));

            // is this the room that the player is in?
            if(playerRoom == new Vector2(x, y))
            {
                // set the color to green
                pixels[i] = Color.green;
            }
            else
            {
                // otherwise, set it to be white if the room exists, or set it to be invisible if it doesn't exist.
                pixels[i] = map[x, y] == true ? Color.white : Color.clear;
            }

        }
            // apply the pixels to the actual texture.
            tex.SetPixels(pixels);
            tex.Apply();

        return tex;
    }
}
