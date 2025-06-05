using QuickOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemType _type = ItemType.Other;

    private Outline _outline;

    public string Name { get; private set; }

    public bool IsInteractable => true;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        Name = ItemNameGetter.GetItemName(_type);
        DisableOutline();
    }

    public void DisableOutline()
    {
        _outline.enabled = false;
    }

    public void EnableOutline()
    {
        _outline.enabled = true;
    }

    public ItemType GetItemType()
    {
        return _type;
    }

    public void Interact()
    {
        
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

public static class ItemNameGetter
{
    public static string GetItemName(ItemType type)
    {
        switch (type)
        {
            case ItemType.Other:
                return "Name not found";
            case ItemType.Cup:
                return "Cup";
            case ItemType.Lid:
                return "Lid";
            case ItemType.CoffeeCapsule:
                return "Coffee Capsule";
            case ItemType.CoffeeCup:
                return "Coffee Cup";
            default:
                return "Item";
        }
    }
}