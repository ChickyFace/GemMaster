using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;
using Unity.VisualScripting;

public class IdleManager : MonoBehaviour
{
    //public Player player;




    private float wallet;


    //public LayerMask pink;
    //public LayerMask yellow;
    //public LayerMask green;



    //public int gemLayerIndex;
    // Properties to store gemLayerIndex and xScaleOfInitialGem
    public int GemLayerIndex { get; private set; }
    public float XScaleOfInitialGem { get; private set; }


    [HideInInspector]
    public int _yellowGemCount;

    [HideInInspector]
    public int _pinkGemCount;
    [HideInInspector]
    public int _greenGemCount;


    private int greenLayerIndex;
    private int yellowLayerIndex;
    private int pinkLayerIndex;

    
    public float time;


    [HideInInspector]
    public float xScaleOfInitialGem;



    public float initialGreenGemCost;
    public float initialYellowGemCost;
    public float initialPinkGemCost;


    public GameObject PopUpScreen;
    public Button GemsButton;



    public TextMeshProUGUI greenGemCountText;
    public TextMeshProUGUI yellowGemCountText;
    public TextMeshProUGUI pinkGemCountText;

    public TextMeshProUGUI Wallet;



    public static IdleManager instance;
    private bool isCounting = false;



    void Awake()
    {
        // player = FindObjectOfType<Player>();

        PlayerPrefs.DeleteAll();
        if (IdleManager.instance)
            UnityEngine.Object.Destroy(gameObject);

        else
            IdleManager.instance = this;



        wallet = PlayerPrefs.GetFloat("Wallet", 0);
        //xScaleOfInitialGem = PlayerPrefs.GetFloat("XScaleOfInitialGem", xScaleOfInitialGem);
        _pinkGemCount = PlayerPrefs.GetInt("PinkGemCount", 0);
        _yellowGemCount = PlayerPrefs.GetInt("YellowGemCount", 0);
        _greenGemCount = PlayerPrefs.GetInt("GreenGemCount", 0);
       

    }

    public void GemPopUpActivate()

    {

        if (PopUpScreen != null)
        {
            bool isActive = PopUpScreen.activeSelf;

            PopUpScreen.SetActive(!isActive);
        }


    }
    public void UpdateTexts()
    {

        greenGemCountText.text = "Count: " + _greenGemCount.ToString();
        pinkGemCountText.text = "Count: " + _pinkGemCount.ToString();
        yellowGemCountText.text = "Count: " + _yellowGemCount.ToString();

        Wallet.text = wallet.ToString();


    }
     

    private List<GemData> gatheredGems = new List<GemData>();

    // Define a custom class to hold gem data
    private class GemData
    {
    public int gemLayerIndex;
    public float xScaleOfInitialGem;

    public GemData(int layerIndex, float scale)
    {
            gemLayerIndex = layerIndex;
            xScaleOfInitialGem = scale;
    }
    }
    public void SetGemData(int gemLayerIndex, float xScaleOfInitialGem)
    {
        // Create a new instance of GemData and add it to the list

        GemData gemData = new GemData(gemLayerIndex, xScaleOfInitialGem);
        gatheredGems.Add(gemData);
        
        //GemLayerIndex = gemLayerIndex;

        //XScaleOfInitialGem = xScaleOfInitialGem;

    }

    private IEnumerator ProcessGatheredGems()
    {
        if (isCounting)
        {
            // Coroutine is already running, do nothing
            yield break;
        }

        isCounting = true;

        int a = gatheredGems.Count;

        
        for (int i = a - 1; i >= 0; i--)
        {

            if (!isCounting)
            {
                // Loop is interrupted, store the current loop index and exit the coroutine
                a = i + 1;
                yield break;
            }

            var gemData = gatheredGems[i];
            int gemLayerIndex = gemData.gemLayerIndex;
            float xScaleOfInitialGem = gemData.xScaleOfInitialGem;
            
            if (gemLayerIndex == LayerMask.NameToLayer("Pink"))
            {
                _pinkGemCount++;
                wallet += (xScaleOfInitialGem * 100 + initialPinkGemCost);
                PlayerPrefs.SetFloat("Wallet", wallet);
                PlayerPrefs.SetInt("PinkGemCount", _pinkGemCount);
            }
            else if (gemLayerIndex == LayerMask.NameToLayer("Green"))
            {
                _greenGemCount++;
                wallet += (xScaleOfInitialGem * 100 + initialGreenGemCost);
                PlayerPrefs.SetFloat("Wallet", wallet);
                PlayerPrefs.SetInt("GreenGemCount", _greenGemCount);
            }
            else if (gemLayerIndex == LayerMask.NameToLayer("Yellow"))
            {
                _yellowGemCount++;
                wallet += (xScaleOfInitialGem * 100 + initialYellowGemCost);
                PlayerPrefs.SetFloat("Wallet", wallet);
                PlayerPrefs.SetInt("YellowGemCount", _yellowGemCount);
            }

            gatheredGems.RemoveAt(i);
            yield return new WaitForSeconds(0.2f);


        }
    }

    public void GetGemMoneyAndCount()
    {
       StartCoroutine(ProcessGatheredGems());
        UpdateTexts();
    }

    //public void GetGemMoneyAndCount()
    //{
    //    foreach (var gemData in gatheredGems)
    //    {
    //        int gemLayerIndex = gemData.gemLayerIndex;
    //        float xScaleOfInitialGem = gemData.xScaleOfInitialGem;

    //        if (gemLayerIndex == LayerMask.NameToLayer("Pink"))
    //        {
    //            _pinkGemCount++;
    //            wallet += (xScaleOfInitialGem * 100 + initialPinkGemCost);
    //            PlayerPrefs.SetFloat("Wallet", wallet);
    //            PlayerPrefs.SetInt("PinkGemCount", _pinkGemCount);
    //        }
    //        else if (gemLayerIndex == LayerMask.NameToLayer("Green"))
    //        {
    //            _greenGemCount++;
    //            wallet += (xScaleOfInitialGem * 100 + initialGreenGemCost);
    //            PlayerPrefs.SetFloat("Wallet", wallet);
    //            PlayerPrefs.SetInt("GreenGemCount", _greenGemCount);
    //        }
    //        else if (gemLayerIndex == LayerMask.NameToLayer("Yellow"))
    //        {
    //            _yellowGemCount++;
    //            wallet += (xScaleOfInitialGem * 100 + initialYellowGemCost);
    //            PlayerPrefs.SetFloat("Wallet", wallet);
    //            PlayerPrefs.SetInt("YellowGemCount", _yellowGemCount);
    //        }
    //    }

    //    // Clear the list after processing all the gathered gem data
    //    gatheredGems.Clear();

    //    UpdateTexts();
    //}


    //    public void GetGemMoneyAndCount()
    //{



    //    // Use GemLayerIndex and XScaleOfInitialGem properties here
    //    if (GemLayerIndex == LayerMask.NameToLayer("Pink"))
    //    {

    //        _pinkGemCount++;
    //        wallet += (XScaleOfInitialGem * 100 + initialPinkGemCost);
    //        PlayerPrefs.SetFloat("Wallet", wallet);
    //        PlayerPrefs.SetInt("PinkGemCount", _pinkGemCount);
    //    }
    //    else if (GemLayerIndex == LayerMask.NameToLayer("Green"))
    //    {
    //        _greenGemCount++;
    //        wallet += (XScaleOfInitialGem * 100 + initialGreenGemCost);
    //        PlayerPrefs.SetFloat("Wallet", wallet);
    //        PlayerPrefs.SetInt("GreenGemCount", _greenGemCount);
    //    }
    //    else if (GemLayerIndex == LayerMask.NameToLayer("Yellow"))
    //    {
    //        _yellowGemCount++;
    //        wallet += (XScaleOfInitialGem * 100 + initialYellowGemCost);
    //        PlayerPrefs.SetFloat("Wallet", wallet);
    //        PlayerPrefs.SetInt("YellowGemCount", _yellowGemCount);
    //    }




    //    UpdateTexts();
    //}

    /*
          //// Get the gem type based on the layer of the collided gem object
            //string gemType = LayerMask.LayerToName(gemLayerIndex);
            //Debug.Log(gemLayerIndex);

            //// Update gem count and wallet based on the gem type
            //switch (gemType)
            //{
            //    case "Pink":
            //        _pinkGemCount++;
            //        wallet += (xScaleOfInitialGem * 100 + initialPinkGemCost);
            //        PlayerPrefs.SetFloat("Wallet", wallet);
            //        PlayerPrefs.SetInt("PinkGemCount", _pinkGemCount);
            //        break;

            //    case "Green":
            //        _greenGemCount++;
            //        wallet += (xScaleOfInitialGem * 100 + initialGreenGemCost);
            //        PlayerPrefs.SetFloat("Wallet", wallet);
            //        PlayerPrefs.SetInt("GreenGemCount", _greenGemCount);
            //        break;

            //    case "Yellow":
            //        _yellowGemCount++;
            //        wallet += (xScaleOfInitialGem * 100 + initialYellowGemCost);
            //        PlayerPrefs.SetFloat("Wallet", wallet);
            //        PlayerPrefs.SetInt("YellowGemCount", _yellowGemCount);


            //        break;
            //}
            //if (gemLayerIndex == LayerMask.NameToLayer("Pink"))
            //{
            //    _pinkGemCount++;

            //    wallet += (xScaleOfInitialGem * 100 + initialPinkGemCost);
            //    PlayerPrefs.SetFloat("Wallet", wallet);
            //    PlayerPrefs.SetInt("PinkGemCount", _pinkGemCount);

            //}

            //else if (gemLayerIndex == LayerMask.NameToLayer("Green"))
            //{
            //    _greenGemCount++;

            //    wallet += (xScaleOfInitialGem * 100 + initialGreenGemCost);
            //    PlayerPrefs.SetFloat("Wallet", wallet);
            //    PlayerPrefs.SetInt("GreenGemCount", _greenGemCount);
            //}
            //else if (gemLayerIndex == LayerMask.NameToLayer("Yellow"))
            //{
            //    _yellowGemCount++;

            //    wallet += (xScaleOfInitialGem * 100 + initialYellowGemCost);
            //    PlayerPrefs.SetFloat("Wallet", wallet);
            //    PlayerPrefs.SetInt("YellowGemCount", _yellowGemCount);


            //if (((1 << gemLayerIndex) & pink) != 0)
            //{
            //    _pinkGemCount++;

            //    wallet += (xScaleOfInitialGem * 100 + initialPinkGemCost);
            //    PlayerPrefs.SetFloat("Wallet", wallet);
            //    PlayerPrefs.SetInt("PinkGemCount", _pinkGemCount);

            //}
            //if (((1 << gemLayerIndex) & green) != 0)
            //{
            //    _greenGemCount++;

            //    wallet += (xScaleOfInitialGem * 100 + initialGreenGemCost);
            //    PlayerPrefs.SetFloat("Wallet", wallet);
            //    PlayerPrefs.SetInt("GreenGemCount", _greenGemCount);

            //}
            //if (((1 << gemLayerIndex) & yellow) != 0)
            //{
            //    _yellowGemCount++;

            //    wallet += (xScaleOfInitialGem * 100 + initialYellowGemCost);
            //    PlayerPrefs.SetFloat("Wallet", wallet);
            //    PlayerPrefs.SetInt("YellowGemCount", _yellowGemCount);

            //}*/







}
