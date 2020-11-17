using UnityEditor;
using System.Linq;

public class CI
{
    [MenuItem("CI/WindowsBuild")]
    public static void WindowsBuild()
    {
        BuildPipeline.BuildPlayer(ScenePaths, "Build/Live-V.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    [MenuItem("CI/WebGLBuild")]
    public static void WebGLBuild()
    {
        BuildPipeline.BuildPlayer(ScenePaths, "docs", BuildTarget.WebGL, BuildOptions.None);
    }

    private static string[] ScenePaths => EditorBuildSettings.scenes.Select(scene => scene.path)
        .ToArray();
}
