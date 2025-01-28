using System;
using Stateful.Managers;
using Structs;
using UnityEngine;

namespace Garden
{
    public class GardenCamera : MonoBehaviour
    {
        public float speed = 1;
        public GameObject plantSeedCanvas;
        public GameObject editModeCanvas;
        public bool uiOpen;
        public PlantableSpot lastSelectedGardenSpot;
    
        private Vector2 _previousPosition;
        private double _previousPinchDistance;
        private bool _tapped;
        private EditableObject _tappedEditableObj;

        public ObjectManager objectManager;
        
        private const float SCALE_FACTOR = 0.0001f;
        private Camera _mainCamera;

        public void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                if (Input.touchCount != 1 || plantSeedCanvas.activeSelf)
                {
                    
                }
                else
                {
                    Touch touch = Input.GetTouch(0);
            
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            _previousPosition = touch.position;
                            
                            break;
                        case TouchPhase.Moved:
                            Vector2 currentPosition = touch.position;
                            Vector2 delta = currentPosition - _previousPosition;
                            
                            if (editModeCanvas.activeSelf && currentPosition.y < 660)
                                break;
                            
                            Vector3 movement = new((delta.y + delta.x) * speed * SCALE_FACTOR, 0f,
                                (delta.y - delta.x) * speed * SCALE_FACTOR);
                            
                            Ray rayMoved = _mainCamera.ScreenPointToRay(touch.position);
                            if (Physics.Raycast(rayMoved, out RaycastHit hitMoved, 1000))
                            {
                                if (hitMoved.transform.CompareTag("EditableObjectDrag"))
                                {
                                    Transform objectToMove = hitMoved.transform.parent.parent.parent;
                                    Vector3 distanceToFinger = hitMoved.point - hitMoved.transform.position;

                                    Quaternion originalRotation = objectToMove.rotation;
                                    objectToMove.rotation = Quaternion.identity;
                                    objectToMove.Translate(new Vector3(distanceToFinger.x - distanceToFinger.y, 0, -distanceToFinger.x - distanceToFinger.y));
                                    objectToMove.rotation = originalRotation;
                                    
                                    EditableObject objectHit = objectToMove.gameObject.GetComponent<EditableObject>();
                                    objectManager.ObjectChanged(objectHit);
                                    
                                    break;
                                }
                            }

                            transform.Translate(movement);

                            switch (transform.position.x)
                            {
                                case > 3f:
                                    transform.Translate(new Vector3(3f - transform.position.x, 0f, 0f));
                                    break;
                                case < -0.25f:
                                    transform.Translate(new Vector3(-0.25f - transform.position.x, 0f, 0f));
                                    break;
                            }
                    
                            switch (transform.position.z)
                            {
                                case > 2.6f:
                                    transform.Translate(new Vector3(0f, 0f, 2.6f - transform.position.z));
                                    break;
                                case < -0.6f:
                                    transform.Translate(new Vector3(0f, 0f, -0.6f - transform.position.z));
                                    break;
                            }
                    
                            _previousPosition = currentPosition;
                            break;
                        case TouchPhase.Stationary:
                            if (!_tapped)
                            {
                                Ray ray = _mainCamera.ScreenPointToRay(touch.position);
                                
                                if (Physics.Raycast(ray, out RaycastHit hit, 1000))
                                {
                                    if (!editModeCanvas.activeSelf && hit.transform.CompareTag("PlantSpot"))
                                    {
                                        PlantableSpot spot = hit.transform.GetComponent<PlantableSpot>();
                                        lastSelectedGardenSpot = spot;
                                        
                                        if(spot.state == GrowState.Vacant)
                                            plantSeedCanvas.SetActive(true);

                                        if (spot.needsWater)
                                        {
                                            spot.UserPoppedWaterPopup();
                                            GardenSpotManager.OnPlantWater(spot);
                                        }

                                        if (spot.harvestable)
                                        {
                                            spot.UserHarvestedPlant();
                                            GardenSpotManager.OnPlantHarvested(spot);
                                        }
                                    }
                                    
                                    if (editModeCanvas.activeSelf && hit.transform.CompareTag("EditableObject"))
                                    {
                                        EditableObject obj = hit.transform.GetComponent<EditableObject>();
                                        obj.editControls.SetActive(true);
                                        
                                        _tappedEditableObj = obj;
                                    }
                                    else
                                    {
                                        if (hit.transform.CompareTag("EditableObjectRot"))
                                        {
                                            EditableObject obj = hit.transform.parent.parent.parent.GetComponent<EditableObject>();
                                            obj.transform.Rotate(Vector3.up, 90);
                                            obj.editControls.transform.Rotate(Vector3.up, -90);
                                            objectManager.ObjectChanged(obj);
                                        }
                                        else if (hit.transform.CompareTag("EditableObjectRot"))
                                        {
                                            EditableObject obj = hit.transform.parent.parent.parent.GetComponent<EditableObject>();
                                            objectManager.DeleteObject(obj);
                                        }
                                        else if (!hit.transform.CompareTag("EditableObjectDrag") && _tappedEditableObj != null)
                                        {
                                            _tappedEditableObj.editControls.SetActive(false);
                                            _tappedEditableObj = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (_tappedEditableObj != null)
                                    {
                                        _tappedEditableObj.editControls.SetActive(false);
                                        _tappedEditableObj = null;
                                            
                                    }
                                }

                                _tapped = true;
                            }
                            break;
                        case TouchPhase.Ended:
                            _tapped = false;
                            break;
                        default:
                        case TouchPhase.Canceled:
                            break;
                    }
                }
            }
        }
    }
}