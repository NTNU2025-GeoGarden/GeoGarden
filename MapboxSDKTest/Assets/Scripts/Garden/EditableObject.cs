using System;
using Structs;
using UnityEngine;

namespace Garden
{
    public class EditableObject : MonoBehaviour
    {
        public int ObjectID;
        
        public GameObject treeHolder;
        public GameObject fenceHolder;
        public GameObject spotHolder;
        public GameObject lanternHolder;
        public GameObject bannerHolder;
        public GameObject cartHolder;
        public GameObject pileHolder;

        public GameObject deleteButton;
        public GameObject rotationButton;
        public GameObject editControls;
        public PlantableSpot spot;
        public EditableObjectType type;

        private BoxCollider _boxCollider;

        public void Start()
        {
            _boxCollider = GetComponent<BoxCollider>();
            switch (type)
            {
                case EditableObjectType.Tree:
                    _boxCollider.center = new Vector3(0, 0.7f, 0);
                    _boxCollider.size   = new Vector3(0.47f, 1.52f, 0.73f);
                    treeHolder.SetActive(true);
                    break;
                case EditableObjectType.Fence:
                    _boxCollider.center = new Vector3(0, 0.16f, 0);
                    _boxCollider.size   = new Vector3(1.1f, 0.7f, 0.34f);
                    fenceHolder.SetActive(true);
                    break;
                case EditableObjectType.Spot:
                    _boxCollider.center = new Vector3(0, 0.32f, 0);
                    _boxCollider.size   = new Vector3(1.1f, 0.07f, 0.94f);
                    spotHolder.SetActive(true);
                    deleteButton.SetActive(false);
                    rotationButton.SetActive(false);
                    break;
                case EditableObjectType.Lantern:
                    _boxCollider.center = new Vector3(0, 0.7f, 0);
                    _boxCollider.size   = new Vector3(0.47f, 1.52f, 0.73f);
                    lanternHolder.SetActive(true);
                    break;
                case EditableObjectType.Banner:
                    _boxCollider.center = new Vector3(0, 0.61f, 0);
                    _boxCollider.size   = new Vector3(0.38f, 0.57f, 0.12f);
                    bannerHolder.SetActive(true);
                    break;
                case EditableObjectType.Cart:
                    _boxCollider.center = new Vector3(0, 0.28f, 0);
                    _boxCollider.size   = new Vector3(0.93f, 0.6f, 1.38f);
                    cartHolder.SetActive(true);
                    break;
                case EditableObjectType.WoodPile:
                    _boxCollider.center = new Vector3(0, 0.11f, 0);
                    _boxCollider.size   = new Vector3(0.93f, -0.11f, 1.38f);
                    pileHolder.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}