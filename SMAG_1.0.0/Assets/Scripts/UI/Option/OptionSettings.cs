using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

public class OptionSettings : MonoBehaviour
{
    public GameObject currentCanvas;
    public GameObject nextCanvas;
    private BannerView bannerView;
    


    void Start()
    {
        #if UNITY_ANDROID
            string appId = "ca-app-pub-8947757057986921~3514556253";
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-8947757057986921~3514556253";
        #else
            string adUnitId = "unexpected_platform";
        #endif
            MobileAds.Initialize(initStatus =>
            {
                RequestBanner();
            });
    }

    void Update()
    {
    }

    private void RequestBanner()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-8947757057986921/8383739552";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-8947757057986921/8383739552";
        #else
            string adUnitId = "unexpected_platform";
        #endif
            adUnitId = "ca-app-pub-3940256099942544/6300978111";
            // Create a 320x50 banner at the top of the screen.
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
            AdRequest request = new AdRequest.Builder().Build();
            this.bannerView.LoadAd(request);
    }

    void OnEnable()
    {
        //Debug.Log("OptionSettings OnEnable");
        if (bannerView != null)
            bannerView.Show();
    }

    void OnDisable()
    {
        //Debug.Log("OptionSettings OnDisable");
        if (bannerView != null)
            bannerView.Hide();
    }

    private void OnDestroy()
    {
        //Debug.Log("OptionSettings OnDestroy");
        bannerView.Destroy();
    }

    public void LogOut()
    {
        currentCanvas.SetActive(false);
        nextCanvas.SetActive(true);
    }
    
    public void Exit()
    {
        Application.Quit();
    }
    
    public void WoongjinURL()
    {
        Application.OpenURL(Utils.WJTB_URI);
    }
}
