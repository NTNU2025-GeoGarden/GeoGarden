﻿using System;
using Structs;
using UnityEngine;

namespace Garden
{
    public class EditableObject : MonoBehaviour
    {
        public int ObjectID;

        public GardenCamera gardenCamera;
        public GameObject treeHolder;
        public GameObject fenceHolder;
        public GameObject spotHolder;
        public GameObject lanternHolder;
        public GameObject bannerHolder;
        public GameObject cartHolder;
        public GameObject pileHolder;
        public GameObject dirtHolder;

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
                    editControls.transform.localPosition = new Vector3(0, -1, 0);
                    break;
                case EditableObjectType.Spot:
                    _boxCollider.center = new Vector3(0, 0.32f, 0);
                    _boxCollider.size   = new Vector3(0.9f, 0.3f, 0.9f);
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
                    editControls.transform.localPosition = new Vector3(0, -0.6f, 0.2f);
                    bannerHolder.SetActive(true);
                    break;
                case EditableObjectType.Cart:
                    _boxCollider.center = new Vector3(0, 0.28f, 0);
                    _boxCollider.size   = new Vector3(0.93f, 0.6f, 1.38f);
                    editControls.transform.localPosition = new Vector3(0, -1, 0);
                    cartHolder.SetActive(true);
                    break;
                case EditableObjectType.WoodPile:
                    _boxCollider.center = new Vector3(0, 0.11f, 0);
                    _boxCollider.size   = new Vector3(0.93f, -0.11f, 1.38f);
                    editControls.transform.localPosition = new Vector3(0, -1, 0);
                    pileHolder.SetActive(true);
                    break;
                case EditableObjectType.DirtPatch:
                    _boxCollider.center = new Vector3(0, -0.35f, 0);
                    _boxCollider.size   = new Vector3(0.93f, -0.11f, 1.38f);
                    editControls.transform.localPosition = new Vector3(0, -1, 0);
                    dirtHolder.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Update()
        {
            if(!gardenCamera.editModeCanvas.activeSelf)
                editControls.SetActive(false);

            _boxCollider.enabled = gardenCamera.editModeCanvas.activeSelf;
        }
    }
}