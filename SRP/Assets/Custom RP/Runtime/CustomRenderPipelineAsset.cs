using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// RP asset is to give Unity a way to get a hold of a pipeline object instance that is responsible for rendering
[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")] 
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
	[SerializeField]
	bool useSRPBatcher = true;

	protected override RenderPipeline CreatePipeline()
	{
		return new CustomRenderPipeline(useSRPBatcher);
	}
}
