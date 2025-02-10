using Stateful;
using UnityEngine;

namespace UI
{
    public class FirebaseStatusIcon : MonoBehaviour
    {
        public GameObject cross;
        public GameObject check;

        public void Update()
        {
            cross.SetActive(!FirebaseManager.FirebaseAvailable);
            check.SetActive(FirebaseManager.FirebaseAvailable);
        }
    }
}
