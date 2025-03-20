using System;
using System.Collections;
using Stateful;
using Stateful.Managers;
using Structs;
using UI;
using UnityEngine;

namespace Garden
{
    public class GardenCamera : MonoBehaviour
    {
        public float speed = 1;
        
        public GardenManager gardenManager;
        public ObjectManager objectManager;

        public TutorialUI introTutorial;
        
        public GameObject plantSeedCanvas;
        public GameObject shopCanvas;
        public GameObject editModeCanvas;
        public GameObject houseCanvas;
        public GameObject dailiesCanvas;
        public GameObject inventoryCanvas;
        public GameObject uiCanvas;
    
        private Camera _mainCamera;
        private Vector2 _previousPosition;
        private double _previousPinchDistance;
        private bool _tapped;
        private bool _draggingEditableObj;
        private EditableObject _tappedEditableObj;
        private BoxCollider _tappedButton;
        
        private const float SCALE_FACTOR = 0.0001f;

        public void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            // If there are no fingers touching the screen, skip update
            if (Input.touchCount <= 0) return;
            
            //If there are multiple fingers touching the screen, skip update
            //Also, if the plant seed screen is open, or the shop screen is open, skip update
            if (Input.touchCount != 1 
                || plantSeedCanvas.activeSelf
                || shopCanvas.activeSelf
                || houseCanvas.activeSelf
                || dailiesCanvas.activeSelf
                || inventoryCanvas.activeSelf 
                || uiCanvas.activeSelf ) return;
            
            //Get the touch input data
            Touch touch = Input.GetTouch(0);
            
            //For each phase the touch is in
            switch (touch.phase)
            {
                //The touch event just started
                case TouchPhase.Began:
                    _previousPosition = touch.position;
                    break;
                
                //The user moved their finger
                case TouchPhase.Moved:
                    Vector2 currentPosition = touch.position;
                    Vector2 delta = currentPosition - _previousPosition;
                    
                    //If the edit screen is open, don't move the camera if the user touched low on their screen (this UI has a scroller)
                    if (editModeCanvas.activeSelf && currentPosition.y < 660)
                        break;

                    _tapped = true;
                    
                    //Do a raycast
                    Ray rayMoved = _mainCamera.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(rayMoved, out RaycastHit hitMoved, 1000))
                    {
                        //If the user tapped on an object with the tag below
                        if (hitMoved.transform.CompareTag("EditableObjectDrag"))
                        {
                            //Drag the editable object on the screen, and don't move the camera (break)
                            RaycastHitDragEditableObject(hitMoved);
                            break;
                        }
                    }
                    
                    if (introTutorial.showTutorial) return;
                    
                    MoveCamera(delta);

                    _previousPosition = currentPosition;
                    break;
                
                //The user hasn't moved their finger (rarely happens, as it is difficult to keep a finger on the exact same XY-coordinate for multiple frames)
                case TouchPhase.Stationary:
                    
                    //Are we in the same tap? The user will leave their finger on the screen for multiple frames. This makes sure that the code
                    //only runs once.
                    if (!_tapped)
                    {
                        if (introTutorial.barrier.activeSelf) return;
                        
                        //Do a raycast
                        Ray ray = _mainCamera.ScreenPointToRay(touch.position);
                        if (Physics.Raycast(ray, out RaycastHit hit, 1000))
                        {
                            //If the edit ui is open
                            if (editModeCanvas.activeSelf)
                            {
                                if (hit.transform.CompareTag("EditableObject"))
                                    RaycastHitEnableEditControlsOnObj(hit);
                                else
                                    RaycastHitEditableObjectControls(hit);
                            }
                            else
                            {
                                if(hit.transform.CompareTag("PlantSpot"))
                                    RaycastHitPlantSpot(hit);
                                else if (hit.transform.CompareTag("House"))
                                    RaycastHitHouse(hit);
                            }
                        }

                        _tapped = true;
                    }
                    break;
                
                //The touch event is over, somehow ended or was interrupted in some way
                default:
                case TouchPhase.Canceled:
                case TouchPhase.Ended:
                    //Reset all the state variables, so we are ready for a new touch
                    _tapped = false;
                    _draggingEditableObj = false;
                            
                    if(_tappedButton != null)
                        _tappedButton.size = new Vector3(1, 1, 0.1f);
                    break;
            }
        }

        

        /// <summary>
        /// Opens the house UI.
        /// </summary>
        /// <param name="hitInfo">The raycast hit information.</param>
        private void RaycastHitHouse(RaycastHit hitInfo)
        {
            if(GameStateManager.CurrentState.LevelUpTime == DateTime.MinValue && GameStateManager.CurrentState.HouseLevel != HouseUpgrades.MaxLevel)
                houseCanvas.SetActive(true);
        }

        /// <summary>
        /// Performs the actions of the editable object controls.
        /// </summary>
        /// <param name="hit">The raycast hit information.</param>
        private void RaycastHitEditableObjectControls(RaycastHit hit)
        {
            if (hit.transform.CompareTag("EditableObjectRot"))
            {
                EditableObject obj = hit.transform.parent.parent.parent.GetComponent<EditableObject>();
                obj.transform.Rotate(Vector3.up, 90);
                obj.editControls.transform.Rotate(Vector3.up, -90);
                objectManager.ObjectChanged(obj);
            }
            else if (hit.transform.CompareTag("EditableObjectDel"))
            {
                EditableObject obj = hit.transform.parent.parent.parent.GetComponent<EditableObject>();

                int cost = EditableObjectCost.GetCostByType(obj.type);
                GameStateManager.CurrentState.Coins = Math.Min(GameStateManager.CurrentState.Coins + cost, GameStateManager.CurrentState.CoinCap);
                FirebaseManager.TelemetryRecordCoinsGenerated(cost);
                
                Destroy(obj.gameObject);
                objectManager.DeleteObject(obj);
            }
        }

        /// <summary>
        /// Opens the UI containing the buttons to edit the object when it is tapped.
        /// </summary>
        /// <param name="hit">The raycast hit information.</param>
        private void RaycastHitEnableEditControlsOnObj(RaycastHit hit)
        {
            if(_tappedEditableObj != null)
                _tappedEditableObj.editControls.SetActive(false);
                                        
            EditableObject obj = hit.transform.GetComponent<EditableObject>();
            obj.editControls.SetActive(true);
                                        
            _tappedEditableObj = obj;
        }

        /// <summary>
        /// Performs updates on plant spots when they are tapped.
        /// </summary>
        /// <param name="hit">The raycast hit information.</param>
        private void RaycastHitPlantSpot(RaycastHit hit)
        {
            PlantableSpot spot = hit.transform.GetComponent<PlantableSpot>();
            gardenManager.SetSelectedSpot(spot);
            Seed seed = Seeds.FromID(spot.seedID);
            int neededWater = seed.Water;
            int neededEnergy = seed.Energy;
          
            int currentWater = GameStateManager.CurrentState.Water;
            int currentEnergy = GameStateManager.CurrentState.Energy;    

            if(spot.state == GrowState.Vacant){
                
                plantSeedCanvas.SetActive(true);
            }

            if (spot.needsWater)
            {
               
                if(currentEnergy<neededEnergy || currentWater<neededWater){
                
                    if(currentWater<neededWater){
                       
                            spot.textField.text = "Not enough water";
                            StartCoroutine(RemoveTextAfterDelay(spot));
                    }
                    else if (currentEnergy<neededEnergy){
                        
                            spot.textField.text = "Not enough energy";
                            StartCoroutine(RemoveTextAfterDelay(spot));
                    }
                    return;
                }
                
                GameStateManager.CurrentState.Energy -= 15;
                FirebaseManager.TelemetryRecordEnergySpent(15);
                
                GameStateManager.CurrentState.Water -= 15;
                spot.UserPoppedWaterPopup();
                GardenManager.OnPlantWater(spot);
            }

            if (spot.harvestable)
            {
                if(currentEnergy<neededEnergy){
                    Debug.Log("Not enough energy");
                    return;
                }
                GameStateManager.CurrentState.Energy -= 15;
                FirebaseManager.TelemetryRecordEnergySpent(15);
                spot.UserHarvestedPlant();
                GardenManager.OnPlantHarvested(spot);
            }
        }
        IEnumerator RemoveTextAfterDelay(PlantableSpot spot)
        {
            yield return new WaitForSeconds(2f);
            spot.textField.text = "";
        }
        /// <summary>
        /// Moves the GardenCamera according to the delta.
        /// </summary>
        /// <param name="delta">Interframe delta of the tap location on screen.</param>
        private void MoveCamera(Vector2 delta)
        {
            Vector3 movement = new((delta.y + delta.x) * speed * SCALE_FACTOR, 0f,
                (delta.y - delta.x) * speed * SCALE_FACTOR);

            if (_draggingEditableObj)
                return;
                            
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
        }

        /// <summary>
        /// Performs updates on editable objects when their drag button was tapped.
        /// </summary>
        /// <param name="hitMoved">The raycast hit information.</param>
        private void RaycastHitDragEditableObject(RaycastHit hitMoved)
        {
            _draggingEditableObj = true;
            _tappedButton = hitMoved.transform.GetComponent<BoxCollider>();
            _tappedButton.size = new Vector3(10, 10, 0.1f);
            Transform objectToMove = hitMoved.transform.parent.parent.parent;
            Vector3 distanceToFinger = hitMoved.point - hitMoved.transform.position;

            Quaternion originalRotation = objectToMove.rotation;
            objectToMove.rotation = Quaternion.identity;
            objectToMove.Translate(new Vector3(distanceToFinger.x - distanceToFinger.y, 0, -distanceToFinger.x - distanceToFinger.y));
            objectToMove.rotation = originalRotation;
                                    
            EditableObject objectHit = objectToMove.gameObject.GetComponent<EditableObject>();
                                    
            if (objectHit.type == EditableObjectType.Spot)
            {
                gardenManager.ObjectChanged(objectHit);
            }
            else
            {
                objectManager.ObjectChanged(objectHit);
            }
        }
    }
}