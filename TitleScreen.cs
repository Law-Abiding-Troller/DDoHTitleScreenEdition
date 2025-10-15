using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using Nautilus.Handlers.TitleScreen;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace HoverfishTitleScreen;

public class TitleScreen
{
    public static WorldTitleObjectHandler HoverfishWTOBH = new(SpawnNothing);
    public static void Register(BaseUnityPlugin plugin)
    {
        TitleScreenHandler.RegisterTitleScreenObject("DDOH",new TitleScreenHandler.CustomTitleData(
            "DDoH, Title Edition", HoverfishWTOBH));
    }

    public static GameObject SpawnNothing()
    {
        var _subnauticaLogo = GameObject.Find("logo");
        GameObject NRE = new GameObject("Hoverfish Parent");
        if (_subnauticaLogo == null) return new GameObject("NRE");
        var newVector3 = new Vector3( _subnauticaLogo.transform.position.x,_subnauticaLogo.transform.position.y, _subnauticaLogo.transform.position.z+30);
        GameObject hoverfishObject = Object.Instantiate(Plugin.HoverFishPrefab, newVector3, _subnauticaLogo.transform.rotation, NRE.transform);
        return NRE;
    }
}

public class WorldTitleObjectHandler : WorldObjectTitleAddon
{
    private int _up = 0;
    private GameObject _subnauticaLogo;
    private GameObject _hoverishObject;
    private Vector3 _targetPosition;
    private Vector3 _targetScale;
    public static bool Enabled;
    private List<GameObject> hoverfishesObj = new List<GameObject>();
    private List<Renderer> _renderers = new List<Renderer>();
    private List<Graphic> _graphics = new List<Graphic>();
    private bool _fadingIn;
    private float _currentFadeInTime;
    private GameObject _customSky;
    public WorldTitleObjectHandler(Func<GameObject> spawnObject,  float fadeInTime = 1, params string[] requiredGUIDs) : base(spawnObject, fadeInTime, requiredGUIDs)
    {
    }

    protected override void OnInitialize()
    {
        int lineRunning = 54;
        try
        {
            if (!Plugin.Options.CauseException) base.OnInitialize();
        _subnauticaLogo = GameObject.Find("logo");
        if (_subnauticaLogo == null || _subnauticaLogo.transform == null || _subnauticaLogo.transform.position == Vector3.zero) return;
        _customSky = _subnauticaLogo.GetComponent<SkyApplier>().customSkyPrefab;
        lineRunning = 57;
        var basefishSpawnPoint = new Vector3( _subnauticaLogo.transform.position.x,_subnauticaLogo.transform.position.y, _subnauticaLogo.transform.position.z);
        if (basefishSpawnPoint.Equals(null) || basefishSpawnPoint == Vector3.zero) return;
        lineRunning = 60;
        Vector3[] hoverfishSpawnPoints =
        {
            new (basefishSpawnPoint.x,basefishSpawnPoint.y+1,basefishSpawnPoint.z-20),
            new (basefishSpawnPoint.x+5,basefishSpawnPoint.y+1,basefishSpawnPoint.z-20),
            new Vector3(basefishSpawnPoint.x+10,basefishSpawnPoint.y+1,basefishSpawnPoint.z-20), 
            new Vector3(basefishSpawnPoint.x+15,basefishSpawnPoint.y+1,basefishSpawnPoint.z-20),
            new Vector3(basefishSpawnPoint.x+20,basefishSpawnPoint.y+1,basefishSpawnPoint.z-20), 
            new Vector3(basefishSpawnPoint.x+25,basefishSpawnPoint.y+1,basefishSpawnPoint.z-20),
            new Vector3(basefishSpawnPoint.x+30,basefishSpawnPoint.y+1,basefishSpawnPoint.z-20),
            new Vector3(basefishSpawnPoint.x+35,basefishSpawnPoint.y+1,basefishSpawnPoint.z-20),
            new Vector3(basefishSpawnPoint.x+40,basefishSpawnPoint.y+1,basefishSpawnPoint.z-20),
            new Vector3(basefishSpawnPoint.x,basefishSpawnPoint.y+1,basefishSpawnPoint.z-15),
            new Vector3(basefishSpawnPoint.x+5,basefishSpawnPoint.y+1,basefishSpawnPoint.z-15),
            new Vector3(basefishSpawnPoint.x+10,basefishSpawnPoint.y+1,basefishSpawnPoint.z-15), 
            new Vector3(basefishSpawnPoint.x+15,basefishSpawnPoint.y+1,basefishSpawnPoint.z-15),
            new Vector3(basefishSpawnPoint.x+20,basefishSpawnPoint.y+1,basefishSpawnPoint.z-15), 
            new Vector3(basefishSpawnPoint.x+25,basefishSpawnPoint.y+1,basefishSpawnPoint.z-15),
            new Vector3(basefishSpawnPoint.x+30,basefishSpawnPoint.y+1,basefishSpawnPoint.z-15),
            new Vector3(basefishSpawnPoint.x+35,basefishSpawnPoint.y+1,basefishSpawnPoint.z-15),
            new Vector3(basefishSpawnPoint.x+40,basefishSpawnPoint.y+1,basefishSpawnPoint.z-15),
            new Vector3(basefishSpawnPoint.x,basefishSpawnPoint.y+1,basefishSpawnPoint.z-25),
            new Vector3(basefishSpawnPoint.x+5,basefishSpawnPoint.y+1,basefishSpawnPoint.z-25),
            new Vector3(basefishSpawnPoint.x+10,basefishSpawnPoint.y+1,basefishSpawnPoint.z-25), 
            new Vector3(basefishSpawnPoint.x+15,basefishSpawnPoint.y+1,basefishSpawnPoint.z-25),
            new Vector3(basefishSpawnPoint.x+20,basefishSpawnPoint.y+1,basefishSpawnPoint.z-25), 
            new Vector3(basefishSpawnPoint.x+25,basefishSpawnPoint.y+1,basefishSpawnPoint.z-25),
            new Vector3(basefishSpawnPoint.x+30,basefishSpawnPoint.y+1,basefishSpawnPoint.z-25),
            new Vector3(basefishSpawnPoint.x+35,basefishSpawnPoint.y+1,basefishSpawnPoint.z-25),
            new Vector3(basefishSpawnPoint.x+40,basefishSpawnPoint.y+1,basefishSpawnPoint.z-25),
        };
        lineRunning = 90;
        int j = 0;
        for (int i = 0; i < Plugin.Options.HoverFishCount*Plugin.Options.Multiplier; i++)
        {
            lineRunning = 95;
            if (j == hoverfishSpawnPoints.Length) j = 0;
            lineRunning = 97;
            if (hoverfishesObj == null) hoverfishesObj = new List<GameObject>();
            lineRunning = 99;
            var instantiatedHoverfish = Object.Instantiate(Plugin.HoverFishPrefab, hoverfishSpawnPoints[j],
                _subnauticaLogo.transform.rotation, WorldObject.transform);
            if (!instantiatedHoverfish) instantiatedHoverfish.SetActive(true);
            if (instantiatedHoverfish == null) return;
            lineRunning = 104;
            hoverfishesObj.Add(instantiatedHoverfish);
            lineRunning = 106;
            foreach (var r in instantiatedHoverfish.GetComponentsInChildren<Renderer>(true)) if (r != null) _renderers.Add(r);
            lineRunning = 108;
            foreach (var g in instantiatedHoverfish.GetComponentsInChildren<Graphic>(true)) if (g != null) _graphics.Add(g);
            lineRunning = 110;
            j++;
        }
        //_targetPosition = new Vector3(MainCamera._camera.transform.position.x, MainCamera._camera.transform.position.y, MainCamera._camera.transform.position.z);
        lineRunning = 114;
        _targetScale = Vector3.one*2;
        lineRunning = 116;
        if (WorldObject.name == "NRE") return;
        lineRunning = 118;
        _hoverishObject = hoverfishesObj[0];
        lineRunning = 120;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"Could not initialize. Failed on line {lineRunning}.\nStack trace:\n{e.StackTrace}");
        }
        
    }
    private void UpdateObjectOpacities(float alpha)
    {
        if (!WorldObject) return;

        foreach (var rend in _renderers)
        {
            rend.SetFadeAmount(alpha);
        }
        
        foreach (var graphic in _graphics)
        {
            var col = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
            graphic.color = col;
        }
    }

    private void MoreHoverfish(int num)
    {
        int j = 0;
        for (int i = 0; i < num; i++)
        {
            if (j == hoverfishesObj.Count) j = 0;
            var newHoverfish = Object.Instantiate(Plugin.HoverFishPrefab, hoverfishesObj[j].transform.position, Quaternion.identity, WorldObject.transform);
            newHoverfish.SetActive(true);
            foreach (var applier in WorldObject.GetComponentsInChildren<SkyApplier>(true))
            {
                applier.anchorSky = Skies.Custom;
                applier.customSkyPrefab = _customSky;
            
                applier.Start();
            }
            hoverfishesObj.Add(newHoverfish);
            j++;
        }
    }

    public void LessOrMoreHoverfish(int num)
    {
        if (num == hoverfishesObj.Count) return;
        if (num > hoverfishesObj.Count)
        {
            MoreHoverfish(num - hoverfishesObj.Count);
        }
        if (num < hoverfishesObj.Count)
        {
            LessHoverfish(hoverfishesObj.Count - num);
        }

    }
    private void LessHoverfish(int num)
    {
        int j = hoverfishesObj.Count;
        for (int i = 0; i < num; i++)
        {
            if (i == j) break;
            var hoverfishToDestroy = hoverfishesObj[i];
            hoverfishesObj.Remove(hoverfishToDestroy);
            foreach (var r in hoverfishToDestroy.GetComponentsInChildren<Renderer>(true)) if (_renderers.Contains(r)) _renderers.Remove(r);
            foreach (var g in hoverfishToDestroy.GetComponentsInChildren<Graphic>(true)) if (_graphics.Contains(g))_graphics.Remove(g);
            Object.Destroy(hoverfishToDestroy);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Enabled = true;
        _up = 1;
        foreach (var obj in hoverfishesObj)
        {
            obj.SetActive(true);
        }
        BehaviourUpdateUtils.Register(this);
        _currentFadeInTime = 0;
        _fadingIn = true;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Enabled = false;
        _up = 0;
        foreach (var obj in hoverfishesObj)
        {
            obj.SetActive(false);
        }
        if (!_fadingIn)
            _currentFadeInTime = 0;
        else
            _currentFadeInTime = FadeInTime - _currentFadeInTime;
        _fadingIn = false;
    }

    public override void ManagedUpdate()
    {
        base.ManagedUpdate();
        if (_hoverishObject == null) return;
        if (_up != 0)
        {
            _hoverishObject.transform.position = Vector3.MoveTowards(_hoverishObject.transform.position, _targetPosition, Time.deltaTime*5);
            _hoverishObject.transform.localScale = Vector3.MoveTowards(_hoverishObject.transform.localScale, _targetScale, Time.deltaTime*2);
        }

        if (_hoverishObject.transform.position == _targetPosition )
        {
            switch (_up)
            {
                case 1:
                    _up = 2;
                    _targetPosition = new Vector3(_subnauticaLogo.transform.position.x + 30f,
                        _subnauticaLogo.transform.position.y, _subnauticaLogo.transform.position.z);
                    _targetScale = Vector3.one*2;
                    break;
                case 2:
                    _up = 3;
                    _targetPosition = new Vector3(MainCamera._camera.transform.position.x,
                        MainCamera._camera.transform.position.y, MainCamera._camera.transform.position.z+20f);
                    _targetScale = Vector3.one;
                    break;
                case 3:
                    _up = 4;
                    _targetPosition = new Vector3( _subnauticaLogo.transform.position.x - 30f,
                        _subnauticaLogo.transform.position.y, _subnauticaLogo.transform.position.z);
                    _targetScale = Vector3.one*2;
                    break;
                case 4:
                    _up = 1;
                    _targetPosition = new Vector3(MainCamera._camera.transform.position.x, 
                        MainCamera._camera.transform.position.y, MainCamera._camera.transform.position.z+20f);
                    _targetScale = Vector3.one;
                    break;
                default:
                    _up = 0;
                    break;
            }
        }
        if (!WorldObject)
        {
            BehaviourUpdateUtils.Deregister(this);
            return;
        }
        
        if (_currentFadeInTime < FadeInTime)
        {
            _currentFadeInTime += Time.deltaTime;
            float normalizedProgress = _currentFadeInTime / Mathf.Max(FadeInTime, float.Epsilon);
            UpdateObjectOpacities(_fadingIn ? normalizedProgress : 1 - normalizedProgress);

            if (!_fadingIn && normalizedProgress >= 1)
            {
                WorldObject.SetActive(false);
            }
            else if (_fadingIn && normalizedProgress > 0)
            {
                WorldObject.SetActive(true);
            }
        }
        else if (!_fadingIn)
        {
            BehaviourUpdateUtils.Deregister(this);
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