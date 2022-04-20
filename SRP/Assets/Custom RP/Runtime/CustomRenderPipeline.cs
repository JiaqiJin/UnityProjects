using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Defines a series of commands and settings that describes how Unity renders a frame.
// https://docs.unity3d.com/ScriptReference/Rendering.RenderPipeline.html
public class CustomRenderPipeline : RenderPipeline 
{
	CameraRender renderer = new CameraRender();

	protected override void Render(ScriptableRenderContext context, Camera[] cameras)
	{
		foreach (Camera camera in cameras)
        {
			renderer.Render(context, camera);
        }
	}
}
