using System.Collections.Generic;

namespace Structs
{
    public enum EditableObjectType
    {
        Tree,
        Fence,
        Spot,
        DirtPatch,
        Lantern,
        Banner,
        Cart,
        WoodPile
    }

    public static class EditableObjectCost
    {
        private static readonly Dictionary<EditableObjectType, int> _cost = new()
        {
            { EditableObjectType.Tree, 10 },
            { EditableObjectType.Fence, 15 },
            { EditableObjectType.DirtPatch, 5 },
            { EditableObjectType.Lantern, 20 },
            { EditableObjectType.Banner, 15 },
            { EditableObjectType.Cart, 40 },
            { EditableObjectType.WoodPile, 10 }
        };

        public static int GetCostByType(EditableObjectType type)
        {
            return _cost[type];
        }
    }
}