using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEngine.Profiling;

public partial class CameraRender
{
#if UNITY_EDITOR
    // all Unity's default shaders ID
    static ShaderTagId[] legacyShaderTagIds = {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };
#endif
    void DrawUnsupportedShaders()
    {
        var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera));
     
        for (int i = 1; i < legacyShaderTagIds.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }

        var filteringSettings = FilteringSettings.defaultValue;
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }

}
