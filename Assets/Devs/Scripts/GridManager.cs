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
    //public GameObject[] gridPrefabs; for random color grid genarate
    public GameObject DefaultGrid;
    
    //private Player playerCs;

    //[SerializeField] private LayerMask[] gridLayerMasks;

    public GameObject[] gemPrefabs;
    //public GameObject GreenGem;
    //public GameObject YellowGem;
    //public GameObject PinkGem;

    private Tweener spawnScaleTween;

    public Vector3 floatGridSize;

    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;

    public Vector3 tilePosition2;

    //long PinkGemBB = 0;
    //long YellowGemBB = 0;
    //long GreenGemBB = 0;

    public int N;
    public int M;

    void Start()
    {
        GenerateTile();
        //playerCs = FindObjectOfType<Player>();
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


                /*if (LayerMask.LayerToName(tile.layer) == "Pink")
                {
                    //PinkGemBB = SetGridState(PinkGemBB, r, M, c); BitBoards of Grids
                    //PrintBB("PinkGem", PinkGemBB);

                    // Set the initial scale of the GreenGem to zero before animating it to the desired size using DOTween.
                    PinkGem.transform.localScale = Vector3.zero;

                    // Instantiate your object at the generated tile position.
                    Vector3 objectOffset = new Vector3(0f, 1f, 0f); // Adjust the offset for your object's desired position relative to the tile.
                    GameObject newObject = Instantiate(PinkGem, tilePosition + objectOffset, Quaternion.identity);

                    // Use DOTween to animate the scale of the GreenGem from zero to the desired size over the specified duration.
                    float duration = 5.0f; // You can adjust the duration as needed.
                    newObject.transform.DOScale(Vector3.one, duration);
                }
                else if (LayerMask.LayerToName(tile.layer) == "Green")
                {
                    // Set the initial scale of the GreenGem to zero before animating it to the desired size using DOTween.
                    GreenGem.transform.localScale = Vector3.zero;

                    // Instantiate your object at the generated tile position.
                    Vector3 objectOffset = new Vector3(0f, 1f, 0f); // Adjust the offset for your object's desired position relative to the tile.
                    GameObject newObject = Instantiate(GreenGem, tilePosition + objectOffset, Quaternion.identity);

                    // Use DOTween to animate the scale of the GreenGem from zero to the desired size over the specified duration.
                    float duration = 5.0f; // You can adjust the duration as needed.
                    newObject.transform.DOScale(Vector3.one, duration);

                }
                else if (LayerMask.LayerToName(tile.layer) == "Yellow")
                {

                    // Set the initial scale of the GreenGem to zero before animating it to the desired size using DOTween.
                    YellowGem.transform.localScale = Vector3.zero;

                    // Instantiate your object at the generated tile position.
                    Vector3 objectOffset = new Vector3(0f, 1f, 0f); // Adjust the offset for your object's desired position relative to the tile.
                    GameObject newObject = Instantiate(YellowGem, tilePosition + objectOffset, Quaternion.identity);

                    // Use DOTween to animate the scale of the GreenGem from zero to the desired size over the specified duration.
                    float duration = 5.0f; // You can adjust the duration as needed.
                    newObject.transform.DOScale(Vector3.one, duration);

                }*/ // instantiate gem at same color tile accordingly Grid Layer
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

            if (gemObject.transform.localScale.x >= GemSize.x / 4 &&
                gemObject.transform.localScale.y >= GemSize.y / 4 &&
                gemObject.transform.localScale.z >= GemSize.z / 4)
            {


                gemObject.GetComponent<Collider>().enabled = true;


                //{ pickable = true}

            }

        });
    }
    /* void PrintBB(string name, long BB)
     {
         Debug.Log(name + ":" + Convert.ToString(BB, 2).PadLeft(64, '0'));
     }*/ // pringting grids bitboard binary numbers

    /*long SetGridState(long bitboard, int row, int width, int col)
    {

        long newBit = 1L << (row * width + col);
        return (bitboard |= newBit);
    } */  // Settint with bitshifting grid's states

    /* bool GetGridState(long bitboard, int row, int width, int col)
     {
         long mask = 1L << (row * width + col);
         return ((bitboard & mask) != 0);
     }*/ // Get GridState bool*/

    

}
