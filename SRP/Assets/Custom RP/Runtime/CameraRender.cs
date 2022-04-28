using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRender 
{
    ScriptableRenderContext context;
    Camera camera;

	CullingResults cullingResults;

	// Some commands have to be issued indirectly, via a separate command buffer
	const string bufferName = "Camera Buffer";

	static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");

	CommandBuffer commandBuffer = new CommandBuffer()
	{
		name = bufferName 
    };

	public void Render(ScriptableRenderContext context, Camera camera)
	{
		this.context = context;
		this.camera = camera;

		if (!Cull())
			return;

		SetUp();
		DrawVisibleGeometry();
		DrawUnsupportedShaders();
		Submit();
	}

	void DrawVisibleGeometry()
    {
		// used to determine whether orthographic or distance-based sorting applies.
		var sortingSettings = new SortingSettings(camera)
		{
			criteria = SortingCriteria.CommonOpaque
		};
		var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
		var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

		context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

		context.DrawSkybox(camera);

		// Draw transparency
		sortingSettings.criteria = SortingCriteria.CommonTransparent;
		drawingSettings.sortingSettings = sortingSettings;
		filteringSettings.renderQueueRange = RenderQueueRange.transparent;

		context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
	}

	void SetUp()
    {
		// Set Up view-projection matrix.
		context.SetupCameraProperties(camera);

		//Clearing the Render Target
		commandBuffer.ClearRenderTarget(true, true, Color.clear);

		// Inject profiler samples
		commandBuffer.BeginSample(bufferName);

		ExcuteBuffer();	
    }

	// submit the queued work for execution
	void Submit()
    {
		commandBuffer.EndSample(bufferName);
		ExcuteBuffer();
		context.Submit();
    }

	// That copies the commands from the buffer but doesn't clear it
	void ExcuteBuffer()
    {
		context.ExecuteCommandBuffer(commandBuffer);
		commandBuffer.Clear();
    }

	private bool Cull()
    {
		if(camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
			cullingResults = context.Cull(ref p);
			return true;
        }

		return false;
    }
}
