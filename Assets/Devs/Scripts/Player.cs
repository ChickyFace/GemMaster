﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    //player
    public Transform player;
    //for movement
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private FixedJoystick fixedJoystick;

    private bool isWalking; //animasyon için
    private bool isMovingToKuyumcu = false; // kutuya gemleri atarken trigger exit ve enter da durdurmak için



    //from Gridmanagager
    private GridManager gridManager;
    private Tweener movingBackScaleTween;
    private Transform gemReSpawn;

    //Offsetler ve transform
    public LayerMask interactableLayer;
    public float stackOffset ;
    public float stackOffsetkasa ;
    public Transform backpack;
    public Transform kasa;
    private List<Transform> gatheredObjects = new List<Transform>();


    void Update()
    {
        HandleMovement();

    }

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleMovement()
    {

        //JoyStick
        Vector3 direction = Vector3.forward * fixedJoystick.Vertical + Vector3.right * fixedJoystick.Horizontal;
        transform.position += direction * moveSpeed * Time.deltaTime;



        //InputSystem
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        transform.position += moveDir * moveDistance;

        // yürüme animasyonu için
        isWalking = (moveDir != Vector3.zero) || (direction != Vector3.zero);

        // yürürken yüz rotasyonu için.
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * rotateSpeed);



    }
    private void OnTriggerEnter(Collider other)
    {
        

        if (((1 << other.gameObject.layer) & interactableLayer) != 0)
        {

           
            //İdle list edilen data burdan gidiyor
            other.gameObject.GetComponent<Collider>().enabled = false;
            gemReSpawn = other.transform;
            IdleManager.instance.SetGemData(other.gameObject.layer, gemReSpawn.localScale.x);
            
          
            // Move the other object to the backpack
            MoveToBackpack(other.transform);
            
        }

        if (IsKuyumcu(other))
        {
            //Debug.Log("triggerEenter");
            StartCoroutine(MoveToKuyumcu());
            return;
        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (IsKuyumcu(other))
        {
            isMovingToKuyumcu = false;
            //Debug.Log("triggerExit");
            return;
        }
    }
    private bool IsKuyumcu(Collider other)
    {
        return other.gameObject.layer ==
            LayerMask.NameToLayer("Kuyumcu");

    

    }
    private void MoveToBackpack(Transform targetObject)
    {


        //// Calculate the position behind the player based on the number of gathered objects
        int gatheredCount = gatheredObjects.Count + 1;
        Vector3 backsidePosition = backpack.localPosition;

        //// Adjust the Y-axis position based on the number of gathered objects
        backsidePosition = new Vector3(backpack.localPosition.x, backpack.localPosition.y * (gatheredCount * stackOffset), backpack.localPosition.z);
        // Set the object as a child of the backpack //ilkparennt yap sonra movement//dolocalmove


        
        Vector3 gemReSpawnOffset = new Vector3(0f, gridManager.floatGridSize.x / 4, 0f);
        //Debug.Log(gemReSpawnOffset);
        //gem tekrar çağırıldığında offseti bir daha eklemesin diye çıkarttık.
        StartCoroutine(gridManager.SpawnGemWithDelay((gemReSpawn.position- gemReSpawnOffset), gemReSpawnOffset));


        //IdleManager.instance.GemLayerIndex = targetObject.gameObject.layer;
        //dleManager.instance.GetGemMoneyAndCount(targetObject.gameObject.layer);
        //gemLayers = targetObject.gameObject.layer;
        //IdleManager.instance.gemLayerIndex = gemLayers;
        //Debug.Log("movetobackpack " + gemLayers);

        //targetObject objenin parent ını tut bu yeni olusturmak istedigim grid olcak sonra burda olusturma fonksiyonuu calıstır.

        targetObject.SetParent(backpack);
        



        float duration = 0.5f;
        DOTween.Kill(targetObject);
        // Use DOTween to move the object to the backpack's position
        targetObject.DOLocalMove(backsidePosition, duration).OnUpdate(() =>
        {
            
            movingBackScaleTween = targetObject.DOScale(new Vector3(0.3f, 0.3f, 0.3f), duration);
        });

        // Add the object to the gatheredObjects list
        gatheredObjects.Add(targetObject);
        //gemStackCount = gatheredObjects.Count;
        
       

    }
    private IEnumerator MoveToKuyumcu()
    {
        if (isMovingToKuyumcu)
        {
            // Coroutine is already running, do nothing
            yield break;
        }

        isMovingToKuyumcu = true;

        int a = gatheredObjects.Count;

        int b = kasa.childCount -3; 
        //kasanın üstüne atarken offset icin
        

        Vector3 kasaPosition = kasa.localPosition;


        for (int i = a-1; i >= 0; i--)
        {
            
            if (!isMovingToKuyumcu)
            {
                // Loop is interrupted, store the current loop index and exit the coroutine
                a = i+1;
                yield break;
            }

            Transform child = gatheredObjects[i];
            kasaPosition = new Vector3(kasa.localPosition.x, (kasa.localPosition.y * (a - i - 1 + b) * stackOffsetkasa) + 2, kasa.localPosition.z);

            // Set the object as a child of the kasa
            child.SetParent(kasa);



            // Use DOJump to throw the object to the kasa position
            child.DOJump(
                endValue: kasaPosition,
                jumpPower: 3,
                numJumps: 1,
                duration: 0.25f
            ).OnComplete(() =>
            {

                IdleManager.instance.GetGemMoneyAndCount();
            
                //IdleManager.
                //para verecek 
                //gem count artacak

            });

            gatheredObjects.RemoveAt(i);

            // Wait for a short delay before moving the next object
            yield return new WaitForSeconds(0.2f);
        }

       
        
    }
   




}


