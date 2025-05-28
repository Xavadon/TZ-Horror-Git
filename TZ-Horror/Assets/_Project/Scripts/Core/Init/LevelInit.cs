using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInit : MonoBehaviour
{
    [SerializeField] private ItemInfo _itemInfo;
    [SerializeField] private PlayerController _player;

    private void Awake()
    {
        _itemInfo.Construct();
        _player.Construct(_itemInfo);
    }
}
