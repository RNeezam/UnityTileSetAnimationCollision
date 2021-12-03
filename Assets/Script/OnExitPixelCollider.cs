using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExitPixelCollider : MonoBehaviour
{
    public Rect globalIntersection = new Rect();
    private float delay;
    private float delayMax = 10f;
    public OnExitPixelCollider other;
    private SpriteRenderer spr;
    public Color[] colorPreview;
    private bool hasCollision = false;
    private bool isCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        this.spr = GetComponent<SpriteRenderer>();
        delay = delayMax;
    }

    // Update is called once per frame
    void Update()
    {
        DetectCollision();
        RenderCollision();
    }

    void DetectCollision()
    { 
        if (this.spr.bounds.Intersects(other.spr.bounds))
        {
      
            globalIntersection.x = Mathf.Max(this.spr.bounds.min.x, other.spr.bounds.min.x);
            globalIntersection.y = Mathf.Max(this.spr.bounds.min.y, other.spr.bounds.min.y);
            globalIntersection.xMax = Mathf.Min(this.spr.bounds.max.x, other.spr.bounds.max.x);
            globalIntersection.yMax = Mathf.Min(this.spr.bounds.max.y, other.spr.bounds.max.y);



            Rect sectionA = new Rect();
            sectionA.x = globalIntersection.xMin - this.spr.bounds.min.x;
            sectionA.width = globalIntersection.width;
            sectionA.y = globalIntersection.yMin - this.spr.bounds.min.y;
            sectionA.height = globalIntersection.height;

            Rect sectionB = new Rect();
            sectionB.x = globalIntersection.xMin - other.spr.bounds.min.x;
            sectionB.width = globalIntersection.width;
            sectionB.y = globalIntersection.yMin - other.spr.bounds.min.y;
            sectionB.height = globalIntersection.height;


            Color[] blockA = this.spr.sprite.texture.GetPixels(
                        (int)sectionA.x,
                        (int)sectionA.y,
                        (int)(sectionA.width * this.spr.sprite.pixelsPerUnit),
                        (int)(sectionA.height * this.spr.sprite.pixelsPerUnit));

            colorPreview = blockA;
            Color[] blockB = other.spr.sprite.texture.GetPixels(
                (int)sectionB.x,
                (int)sectionB.y,
                (int)(sectionB.width * other.spr.sprite.pixelsPerUnit),
                (int)(sectionB.height * other.spr.sprite.pixelsPerUnit));


      
            if (blockA.Length <= 0)
            {
                hasCollision = false;
            }
            for (int i = 0; i < blockA.Length; i++)
            {

                if (blockA[i].a == 1f && blockB[i].a == 1f)
                {
                    hasCollision = true;
                    return;
                }
                else
                {
                    hasCollision = false;
                }
            }
        }
    }

    private void RenderCollision()
    {
        if (hasCollision == true)
        {
            isCollided = true;
        }
        if (isCollided == true && hasCollision == false)
        {
            if (delay >= 0f)
            {
                this.spr.color = Color.red;
                delay -= Time.deltaTime;
            }
            isCollided = false;
            delay = delayMax;
        }
        else this.spr.color = Color.white;
    }


}
