using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAnimator : MonoBehaviour
{

     bool oneShot = false;
     bool reverse = false;
     bool pingPong = false;
     bool dualcolumn = false;

    public int cols;
    public int rows;
    public float frameRate = 1f;

    private int currentFrame = 0;
    private Material mat;
    private float time = 0f;
    private Vector2 offset = Vector2.zero;

    private bool ping = false; 
    private int animCol = 1; 

    void Start()
    {
        mat = this.GetComponent<MeshRenderer>().material;

        mat.SetTextureScale("_MainTex", new Vector2(1f/(float)cols, 1f/(float)rows));
    }

    void Update()
    {
        float timeBetweenFrames = 1f / frameRate;
        time += Time.deltaTime;

        if (time >= timeBetweenFrames)
        {

            currentFrame = (currentFrame + 1) % cols;
            time = 0f;
            Selector();
        }
        

        mat.SetTextureOffset("_MainTex", offset);
    }

    public void OneShot()
    {
        oneShot = true;
        reverse = false;
        pingPong = false;
        dualcolumn = false;
        offset.x = (1f / (float) cols) * (float) currentFrame;
        offset.y = 1f / (float) rows;
   
    }


    public void Reverse()
    {
        oneShot = false;
        reverse = true;
        pingPong = false;
        dualcolumn = false;
        offset.x = -((1f / (float)cols) * (float)currentFrame);
        offset.y = 1f / (float)rows;

    }

    public void PingPong()
    {
        oneShot = false;
        reverse = false;
        pingPong = true;
        dualcolumn = false;
        if (currentFrame == 0)
        {
            ping = !ping;
        }
        if (ping)
        {
            offset.x = (1f / (float)cols) * (float)currentFrame;
        }
        else if(!ping)
        {
            offset.x = -((1f / (float)cols) * (float)currentFrame);
        }
        offset.y = 1f / (float)rows;

        
    }

    public void TwoColumn()
    {
        oneShot = false;
        reverse = false;
        pingPong = false;
        dualcolumn = true;
        if (currentFrame == 0)
        {
            if(animCol == 1)
            {
                animCol = 2;
            }
            else if(animCol == 2)
            {
                animCol = 1;
            }
        }

        offset.x = (1f / (float)cols) * (float)currentFrame;
        offset.y = 1f / (float)rows * animCol;

      
    }
    void Selector()
    {
        if(oneShot)
        {
            OneShot();
        }
        else if(reverse)
        {
            Reverse();
        }
        else if(pingPong)
        {
            PingPong();
        }
        else if(dualcolumn)
        {
            TwoColumn();
        }
    }

    private void ResetValue()
    {
        offset.x = 0;
        offset.y = offset.y = 1f / (float)rows;
        currentFrame = 0;
    }
}
