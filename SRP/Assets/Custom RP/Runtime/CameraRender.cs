using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRender 
{
    ScriptableRenderContext context;
    Camera camera;
	public void Render(ScriptableRenderContext context, Camera camera)
	{
		this.context = context;
		this.camera = camera;

		DrawVisibleGeometry();
		Submit();
	}

	void DrawVisibleGeometry()
    {
		context.DrawSkybox(camera);
    }

	// submit the queued work for execution
	void Submit()
    {
		context.Submit();
    }
}
