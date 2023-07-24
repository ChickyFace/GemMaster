//using UnityEngine;
//using DG.Tweening;
//using System.Collections.Generic;

//public class InteractionController : MonoBehaviour
//{
//    public Transform player; // Reference to the player or camera

//    // Layer mask to define which objects are interactable
//    public LayerMask interactableLayer;



//    // Y-axis offset between stacked objects in the backpack
//    public float stackOffset = 2f;

//    // Backpack object on the player's back to gather the interactable objects
//    public Transform backpack;

//    // List to store the gathered objects in the backpack
//    private List<Transform> gatheredObjects = new List<Transform>();

//    // Function to move the object to the backpack
//    private void MoveToBackpack(Transform targetObject)
//    {
//        //// Calculate the position behind the player based on the number of gathered objects
//        int gatheredCount = gatheredObjects.Count+1;
//        Vector3 backsidePosition = backpack.localPosition;

//        //// Adjust the Y-axis position based on the number of gathered objects
//        backsidePosition =new Vector3(backpack.transform.localPosition.x, backpack.transform.localPosition.y * (gatheredCount * stackOffset), backpack.transform.localPosition.z);
//        // Set the object as a child of the backpack //ilkparennt yap sonra movement//dolocalmove
//        targetObject.SetParent(backpack);

//        float duration = 0.5f;
//        // Use DOTween to move the object to the backpack's position
//        targetObject.DOLocalMove(backsidePosition, duration).SetEase(Ease.InBounce);
//        targetObject.DOScale(new Vector3(0.5f, 0.5f, 0.5f), duration).SetEase(Ease.Linear);

        

//        // Add the object to the gatheredObjects list
//        gatheredObjects.Add(targetObject);
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        // Check if the other object is on the interactable layer
//        if (((1 << other.gameObject.layer) & interactableLayer) != 0)
//        {
//            other.gameObject.GetComponent<Collider>().enabled = false;
//            // Move the other object to the backpack
//            MoveToBackpack(other.transform);
//        }
//    }
//}
