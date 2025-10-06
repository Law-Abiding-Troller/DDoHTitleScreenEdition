using System.Collections;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UWE;

namespace HoverfishTitleScreen;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static GameObject HoverFishPrefab;

    public static IEnumerator GetHoverFishPrefab(BaseUnityPlugin plugin)
    {
        CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.Hoverfish);
        yield return task;
        HoverFishPrefab = task.GetResult();
        TitleScreen.Register(plugin);
    }

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;
        StartCoroutine(GetHoverFishPrefab(this));
        // Initialize custom prefabs
        InitializePrefabs();
        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void InitializePrefabs()
    {
    }
}