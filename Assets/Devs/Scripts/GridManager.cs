using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using DG.Tweening;
using System.Drawing;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    
    public GameObject DefaultGrid;

    public GameObject[] gemPrefabs;

    private Tweener spawnScaleTween;

    // grid ve gemlerin size oranı ve offset icin
    public Vector3 floatGridSize; 



    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;


    //GridTile içinde oluştuıulacak NxM grid sayısı
    public int N;
    public int M;

    void Start()
    {
        GenerateTile();
    }
    private void GenerateTile()
    {
        Vector3 objectPosition = transform.position; // Get the position of the object.
        DefaultGrid.transform.localScale = floatGridSize / 10; // Change DefaultGrid Scale
        /*for (int i = 0; i < gridPrefabs.Length; i++)
        {
            gridPrefabs[i].transform.localScale = floatGridSize;
        } */ // Change the GridPrefab Scales

        for (int r = 0; r < N; r++)
        {
            for (int c = 0; c < M; c++)
            {
                // Calculate the position for the tile based on the row (r) and column (c) values and the spacing.
                Vector3 tilePosition = new Vector3(objectPosition.x + c * floatGridSize.x, objectPosition.y, objectPosition.z + r * floatGridSize.z);
                // int randomGrid = UnityEngine.Random.Range(0, gridPrefabs.Length); for randam grid
                GameObject tile = Instantiate(DefaultGrid, tilePosition, Quaternion.identity);

                // Randomly invoke the SpawnGem method with a delay between minSpawnDelay and maxSpawnDelay.
                float randomDelay = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);


                Vector3 gemOffset = new Vector3(0f, floatGridSize.x / 4, 0f);
                StartCoroutine(SpawnGemWithDelay((tile.transform.position), gemOffset));
                
            }
        }
    }

    public IEnumerator SpawnGemWithDelay(Vector3 gemTargetPosition , Vector3 gemTargetOffset)

    {
        // Randomly wait for a delay before spawning a gem on the tile.
        float randomDelay = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(randomDelay);

        int randomGem = UnityEngine.Random.Range(0, gemPrefabs.Length);

        //gemTargetPosition.y = 0;
        Vector3 spawnPosition = gemTargetPosition + gemTargetOffset;
        //Debug.Log(gemTargetPosition + " : " + gemTargetOffset);
            

        GameObject gemObject = Instantiate(gemPrefabs[randomGem], spawnPosition , Quaternion.identity);

        // Set the initial scale of the gem to zero.
        gemObject.transform.localScale = Vector3.zero;
        gemObject.GetComponent<Collider>().enabled = false;
        

        float scalingDuration = 5f; // Adjust the duration as needed.
        Vector3 GemSize = new Vector3(floatGridSize.x / 3, floatGridSize.x / 3, floatGridSize.z / 3);

        spawnScaleTween = gemObject.transform.DOScale(GemSize, scalingDuration).OnUpdate(() =>
        {

            //Case de gemler 0.25 den sonra alınabilir olsun dediği için 1- 0.25 oranını kullandım, büyüyecekleri max noktanın 1/4'ünde colliderları aktif oluyor

            if (gemObject.transform.localScale.x >= GemSize.x / 4 &&
                gemObject.transform.localScale.y >= GemSize.y / 4 &&
                gemObject.transform.localScale.z >= GemSize.z / 4)
            {


                gemObject.GetComponent<Collider>().enabled = true;


                //{ pickable = true}

            }

        });
    }
 

}
