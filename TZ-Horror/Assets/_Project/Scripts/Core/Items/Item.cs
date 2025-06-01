using QuickOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IOutlinable
{
    [SerializeField] private ItemType _type = ItemType.Other;

    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    public void DisableOutline()
    {
        _outline.SetOutlineWidth(0);
    }

    public void EnableOutline()
    {
        _outline.SetOutlineWidth(3);
    }

    public ItemType GetItemType()
    {
        return _type;
    }
}

public enum ItemType
{
    Other,
    Cup,
    Lid,
    CoffeeCapsule,
    CoffeeCup
}