using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Defines a series of commands and settings that describes how Unity renders a frame.
// https://docs.unity3d.com/ScriptReference/Rendering.RenderPipeline.html
public class CustomRenderPipeline : RenderPipeline 
{
	CameraRender cameraRenderer = new CameraRender();

    //Each frame Unity invokes Render on the RP instance
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            var camera = cameras[i];
            cameraRenderer.Render(context, camera);
        }
    }
}
