using UnityEditor;
using System.Linq;

public class CI
{
    [MenuItem("CI/Build")]
    public static void Build()
    {
        BuildPipeline.BuildPlayer(ScenePaths, "Build/Live-V.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    private static string[] ScenePaths => EditorBuildSettings.scenes.Select(scene => scene.path)
        .ToArray();
}
