using System;
using BepInEx;
using Nautilus.Handlers.TitleScreen;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HoverfishTitleScreen;

public class TitleScreen
{
    public static void Register(BaseUnityPlugin plugin)
    {
        TitleScreenHandler.RegisterTitleScreenObject("DDOH",new TitleScreenHandler.CustomTitleData(
            "DDOH Title Screen", new WorldTitleObjectHandler(SpawnNothing)));
    }

    public static GameObject SpawnNothing()
    {
        var _subnauticaLogo = GameObject.Find("logo");
        if (_subnauticaLogo == null) return new  GameObject("NRE");
        var newVector3 = new Vector3( _subnauticaLogo.transform.position.x - 3f,_subnauticaLogo.transform.position.y, _subnauticaLogo.transform.position.z);
        var hoverfishObject = Object.Instantiate(Plugin.HoverFishPrefab, newVector3, _subnauticaLogo.transform.rotation);
        return hoverfishObject;
    }
}

public class WorldTitleObjectHandler : WorldObjectTitleAddon
{
    private int _up = 0;
    private GameObject _subnauticaLogo;
    private GameObject _hoverishObject;
    private Vector3 _targetPosition;
    public WorldTitleObjectHandler(Func<GameObject> spawnObject, float fadeInTime = 1, params string[] requiredGUIDs) : base(spawnObject, fadeInTime, requiredGUIDs)
    {
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _subnauticaLogo = GameObject.Find("logo");
        if (_subnauticaLogo == null) return;
        _targetPosition = Vector3.zero;
        _hoverishObject = WorldObject;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _up = 1;
    }
    protected override void OnDisable()
    {
        base.OnEnable();
        _up = 0;
    }

    public override void ManagedUpdate()
    {
        base.ManagedUpdate();

        if (_up == 1)
        {
            _hoverishObject.transform.position = Vector3.MoveTowards(_hoverishObject.transform.position, _targetPosition, Time.deltaTime);
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
                    _up = 0;
                    break;
            }
        }
        if (_up == 2)
        {
            _hoverishObject.transform.position = Vector3.MoveTowards(_hoverishObject.transform.position, _targetPosition, Time.deltaTime);
        }
    }
}