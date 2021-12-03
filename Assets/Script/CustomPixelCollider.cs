using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPixelCollider : MonoBehaviour
{
    public Rect intersection = new Rect();
    public Color[] previewColors = new Color[0];

    private SpriteRenderer thisSpr;
    [SerializeField] SpriteRenderer[] otherSpr;

    private bool isCollidingExit = false;
    private bool isCollidingEnter = false;
    private bool isCollidingStay = false;
    

    void Start()
    {
        

        thisSpr = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        DetectCollision();
        RenderCollision();

    }

    private void DetectCollision()
    {
        for(int i = 0; i < otherSpr.Length; i++)
        { 
             if (thisSpr.bounds.Intersects(otherSpr[i].bounds))
             {

             //2.Find the intersection
              intersection.xMin = Mathf.Max(thisSpr.bounds.min.x, otherSpr[i].bounds.min.x);
              intersection.xMax = Mathf.Min(thisSpr.bounds.max.x, otherSpr[i].bounds.max.x);
              intersection.yMin = Mathf.Max(thisSpr.bounds.min.y, otherSpr[i].bounds.min.y);
              intersection.yMax = Mathf.Min(thisSpr.bounds.max.y, otherSpr[i].bounds.max.y);

            // 2.A Convert GlobalIntersection to LocalIntersection for both sprites
            // intersection.min - spritebounds.min = localized intersection
            Rect thisInt = new Rect();
            thisInt.xMin = intersection.xMin - thisSpr.bounds.min.x;
            thisInt.yMin = intersection.yMin - thisSpr.bounds.min.y;
            thisInt.width = intersection.width;
            thisInt.height = intersection.height;

            Rect otherInt = new Rect();
            otherInt.xMin = intersection.xMin - otherSpr[i].bounds.min.x;
            otherInt.yMin = intersection.yMin - otherSpr[i].bounds.min.y;
            otherInt.width = intersection.width;
            otherInt.height = intersection.height;

            // 3.Get the array of colors from intersection for both bounding boxes

            float ppu = thisSpr.sprite.pixelsPerUnit; // Pixel density

            // Get this sprite's localized color array
            Color[] thisColorArray = thisSpr.sprite.texture.GetPixels
                (
                    (int)(thisInt.xMin * ppu), (int)(thisInt.yMin * ppu),
                    (int)(thisInt.width * ppu), (int)(thisInt.height * ppu)
                );

            // Preview array
            previewColors = new Color[thisColorArray.Length]; // Set max size
            previewColors = thisColorArray; // Set value for preview

            // Get other sprite's localized color array
            float otherPpu = otherSpr[i].sprite.pixelsPerUnit; // Pixel density
            Color[] otherColorArray = otherSpr[i].sprite.texture.GetPixels
                (
                    (int)(otherInt.xMin * otherPpu), (int)(otherInt.yMin * otherPpu),
                    (int)(otherInt.width * otherPpu), (int)(otherInt.height * otherPpu)
                );

                // 4.compare the two arrays
                for (int k = 0; k < thisColorArray.Length; k++)
                {
                    if (thisColorArray[k].a == 1f && otherColorArray[k].a == 1f)
                    {
                        // 5.Collide if alpha for same index is 1

                        if (!isCollidingEnter)
                            isCollidingEnter = true;
                        if (isCollidingStay)
                            isCollidingEnter = false;
                        if (isCollidingExit)
                            isCollidingExit = false;
                        isCollidingStay = true;
                        return;

                    }
                }
                    if (!isCollidingExit)
                        isCollidingExit = true;
                    if (!isCollidingStay)
                        isCollidingExit = false;
                    if (isCollidingEnter)
                        isCollidingEnter = false;
                    isCollidingStay = false;
                }
            else
            {
                    if (!isCollidingExit)
                        isCollidingExit = true;
                    if (!isCollidingStay)
                        isCollidingExit = false;
                    if (isCollidingEnter)
                        isCollidingEnter = false;
                    isCollidingStay = false;
                }
            }
        }

        private void RenderCollision()
    {
        // Color red if colliding, else color it white
        for (int i = 0; i < otherSpr.Length; i++)
        {
            otherSpr[i].color = (isCollidingStay) ? Color.red : Color.white;
        }

    }
}