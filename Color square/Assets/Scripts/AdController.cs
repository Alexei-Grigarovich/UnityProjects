﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour
{
    private static string rewVideoPlacementId = "rewardedVideo";
    private static string videoPlacementId = "video";
    private string gameId = "3722877";

    private static bool isRewVideo = false;
    private static bool rewVideoIsFinished = false;
    private static bool adIsFinished = false;
    public static bool isNoAds = false;

    public bool testMode;

    void Start()
    {
        if (Advertisement.isSupported) Advertisement.Initialize(gameId, testMode);

        PurchaseManager.OnPurchaseNonConsumable += PurchaseManager_OnPurchaseNonConsumable;

        StartCoroutine(checkBuyStatesOfNoAds());
    }

    private void PurchaseManager_OnPurchaseNonConsumable(UnityEngine.Purchasing.PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == "noads_") isNoAds = true;
    }

    private static void showAd(string placementId)
    {
        adIsFinished = false;
        if (isRewVideo) rewVideoIsFinished = false;

        Advertisement.Show(placementId, new ShowOptions { resultCallback = handleShowResult });
    }

    private static void handleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            if (isRewVideo) rewVideoIsFinished = true;           
        }
        adIsFinished = true;
    }

    public static bool lastRewAdIsFinished()
    {
        return rewVideoIsFinished;
    }

    public static bool lastAdIsFinished()
    {
        return adIsFinished;
    }

    public static int showRewardedVideoAd(bool checkNoAds)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return -3;
        if (isNoAds && checkNoAds) return -2;
        isRewVideo = true;
        if (Advertisement.IsReady(rewVideoPlacementId)) {
            showAd(rewVideoPlacementId);
            return 0;
        }
        else return -1;
    }

    public static int showVideoAd()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return -3;
        if (isNoAds) return -2;
        isRewVideo = false;
        if (Advertisement.IsReady(videoPlacementId))
        {
            showAd(videoPlacementId);
            return 0;
        }
        else return -1;
    }

    private IEnumerator checkBuyStatesOfNoAds()
    {
        yield return new WaitUntil(() => PurchaseManager.IsInitialized());

        if (PurchaseManager.CheckBuyState("noads_")) isNoAds = true;
        else isNoAds = false;
    }
}
