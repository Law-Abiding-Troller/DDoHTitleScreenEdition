using System;
using BepInEx;
using HarmonyLib;
using Nautilus.Handlers.TitleScreen;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HoverfishTitleScreen;

public class TitleScreen
{
    public static void Register(BaseUnityPlugin plugin)
    {
        TitleScreenHandler.RegisterTitleScreenObject("DDOH",new TitleScreenHandler.CustomTitleData(
            "DDoH, Title Edition", new WorldTitleObjectHandler(SpawnNothing)));
    }

    public static GameObject SpawnNothing()
    {
        var _subnauticaLogo = GameObject.Find("logo");
        if (_subnauticaLogo == null) new GameObject("NRE");
        var newVector3 = new Vector3( _subnauticaLogo.transform.position.x - 20f,_subnauticaLogo.transform.position.y, _subnauticaLogo.transform.position.z);
        GameObject hoverfishObject = Object.Instantiate(Plugin.HoverFishPrefab, newVector3, _subnauticaLogo.transform.rotation);
        Vector3[] newVector33 =
        {
            new Vector3(newVector3.x,newVector3.y,newVector3.z+20),
            new Vector3(newVector3.x+5,newVector3.y,newVector3.z+20),
            new Vector3(newVector3.x+10,newVector3.y,newVector3.z+20), 
            new Vector3(newVector3.x+15,newVector3.y,newVector3.z+20),
            new Vector3(newVector3.x+20,newVector3.y,newVector3.z+20), 
            new Vector3(newVector3.x+25,newVector3.y,newVector3.z+20),
            new Vector3(newVector3.x+30,newVector3.y,newVector3.z+20),
            new Vector3(newVector3.x+35,newVector3.y,newVector3.z+20),
            new Vector3(newVector3.x+40,newVector3.y,newVector3.z+20),
            new Vector3(newVector3.x,newVector3.y,newVector3.z+15),
            new Vector3(newVector3.x+5,newVector3.y,newVector3.z+15),
            new Vector3(newVector3.x+10,newVector3.y,newVector3.z+15), 
            new Vector3(newVector3.x+15,newVector3.y,newVector3.z+15),
            new Vector3(newVector3.x+20,newVector3.y,newVector3.z+15), 
            new Vector3(newVector3.x+25,newVector3.y,newVector3.z+15),
            new Vector3(newVector3.x+30,newVector3.y,newVector3.z+15),
            new Vector3(newVector3.x+35,newVector3.y,newVector3.z+15),
            new Vector3(newVector3.x+40,newVector3.y,newVector3.z+15),
            new Vector3(newVector3.x,newVector3.y,newVector3.z+25),
            new Vector3(newVector3.x+5,newVector3.y,newVector3.z+25),
            new Vector3(newVector3.x+10,newVector3.y,newVector3.z+25), 
            new Vector3(newVector3.x+15,newVector3.y,newVector3.z+25),
            new Vector3(newVector3.x+20,newVector3.y,newVector3.z+25), 
            new Vector3(newVector3.x+25,newVector3.y,newVector3.z+25),
            new Vector3(newVector3.x+30,newVector3.y,newVector3.z+25),
            new Vector3(newVector3.x+35,newVector3.y,newVector3.z+25),
            new Vector3(newVector3.x+40,newVector3.y,newVector3.z+25),
        };
        for (int i = 0; i < Plugin.Options.HoverFishCount; i++)
        {
            foreach (var pos in newVector33)
            {
                Object.Instantiate(Plugin.HoverFishPrefab, pos, _subnauticaLogo.transform.rotation);
            }
        }
        return hoverfishObject;
    }
}

public class WorldTitleObjectHandler : WorldObjectTitleAddon
{
    private int _up = 0;
    private GameObject _subnauticaLogo;
    private GameObject _hoverishObject;
    private Vector3 _targetPosition;
    public static bool Enabled;
    private float _interloopation = 0f;
    public WorldTitleObjectHandler(Func<GameObject> spawnObject, float fadeInTime = 1, params string[] requiredGUIDs) : base(spawnObject, fadeInTime, requiredGUIDs)
    {
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _subnauticaLogo = GameObject.Find("logo");
        if (_subnauticaLogo == null) return;
        _targetPosition = new Vector3(MainCamera.camera.transform.position.x, MainCamera.camera.transform.position.y, MainCamera.camera.transform.position.z - 3);
        if (WorldObject.name == "NRE") return;
        _hoverishObject = WorldObject;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Enabled = true;
        _up = 1;
    }
    protected override void OnDisable()
    {
        base.OnEnable();
        Enabled = false;
        _up = 0;
    }

    public override void ManagedUpdate()
    {
        base.ManagedUpdate();
        if (_hoverishObject == null) return;
        if (_up != 0)
        {
            
            _hoverishObject.transform.position = Vector3.Lerp(_hoverishObject.transform.position, _targetPosition, Time.deltaTime);
            _interloopation += Time.deltaTime;
        }

        if (_hoverishObject.transform.position == _targetPosition )
        {
            switch (_up)
            {
                case 1:
                    _up = 2;
                    _targetPosition = new Vector3(_subnauticaLogo.transform.position.x + 3f,
                        _subnauticaLogo.transform.position.y, _subnauticaLogo.transform.position.z);
                    break;
                case 2:
                    _up = 3;
                    _targetPosition = new Vector3(MainCamera.camera.transform.position.x, MainCamera.camera.transform.position.y, MainCamera.camera.transform.position.z - 3);
                    break;
                case 3:
                    _up = 1;
                    _targetPosition = new Vector3( _subnauticaLogo.transform.position.x - 3f,_subnauticaLogo.transform.position.y, _subnauticaLogo.transform.position.z);
                    break;
            }
        }
    }
}

[HarmonyPatch(typeof(BehaviourLOD))]
public class BehaviourLODPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(BehaviourLOD.Update))]
    public static bool Update_Prefix(BehaviourLOD __instance)
    {
        if (!WorldTitleObjectHandler.Enabled) return true;
        __instance.current = LODState.Full;
        return false;
    }
}