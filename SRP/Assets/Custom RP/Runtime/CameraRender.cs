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

		PrepareBuffer();
		PrepareForSceneWindow();

		if (!Cull())
			return;

		SetUp();

		// Context are buffered (not draw before submitting)
		DrawVisibleGeometry();
		//Draw Unsupported Meshes
		DrawUnsupportedShaders();
		// Draw Gizmos
		DrawGizmos();

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

		// Clear Flag
		CameraClearFlags flags = camera.clearFlags;

		//Clearing the Render Target
		commandBuffer.ClearRenderTarget(
			flags <= CameraClearFlags.Depth, // if nothing =>  not clear
			flags == CameraClearFlags.Color,  // if it is color =>  clear with color
			flags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear // transparent or color
			);

		// Inject profiler samples
		commandBuffer.BeginSample(SampleName);

		ExcuteBuffer();	
    }

	// submit the queued work for execution
	void Submit()
    {
		commandBuffer.EndSample(SampleName);
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
