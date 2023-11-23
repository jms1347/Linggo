using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds;
using System;

public class AdmobManager : MonoBehaviour
{
    InterstitialAd frontAD;
    RewardedAd rebirthRewardedAd;
    RewardedAd cardChangeRewardedAd;
    int retryCount = 0;
    //const string frontID = "ca-app-pub-3940256099942544/1033173712";
    const string frontID = "ca-app-pub-3819330341227143/9853527457";
    //const string rewardID = "ca-app-pub-3940256099942544/5224354917";
    const string rewardID = "ca-app-pub-3819330341227143/9605760228";

    protected void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {            
            // This callback is called once the MobileAds SDK is initialized.
        });

    }
    
    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Interstitial ad paid {0} {1}.");
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
    public void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Rewarded ad paid {0} {1}.");
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
    #region 리워드 광고(환생)
    //환생 리워드 광고
    public void ShowRebirthRewardedAd()
    {
        if (rebirthRewardedAd != null && rebirthRewardedAd.CanShowAd())
        {
            rebirthRewardedAd.Show((Reward reward) =>
            {
                GameController.Inst.SettingRebirth();
            });
        }
    }

    
    public void LoadRebirthRewardAd()
    {
        if (rebirthRewardedAd != null)
        {
            rebirthRewardedAd.Destroy();
            rebirthRewardedAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rewardID, adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  if (retryCount < 3)
                  {
                      Debug.LogError("Rewarded ad failed to load on attempt " + retryCount + " with error: " + error);
                      retryCount++;
                      LoadRebirthRewardAd();
                  }
                  else
                  {
                      Debug.LogError("Rewarded ad failed to load after 3 retries. Giving up.");
                  }
                  Debug.LogError("Rewarded ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }
              retryCount = 0;
              Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

              rebirthRewardedAd = ad;
              RewardedAdEvents(rebirthRewardedAd);
          });
    }

    #endregion

    #region 리워드 광고(카드 체인지)
    public void ShowCardChangeRewardedAd()
    {
        if (cardChangeRewardedAd != null && cardChangeRewardedAd.CanShowAd())
        {
            cardChangeRewardedAd.Show((Reward reward) =>
            {
                SkillCardController.Inst.ChangeCardRewardAD();
            });
        }
    }

    //카드 체인지 리워드 광고
    public void LoadCardChangeRewardAd()
    {
        if (cardChangeRewardedAd != null)
        {
            cardChangeRewardedAd.Destroy();
            cardChangeRewardedAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rewardID, adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  if (retryCount < 3)
                  {
                      Debug.LogError("Rewarded ad failed to load on attempt " + retryCount + " with error: " + error);
                      retryCount++;
                      LoadCardChangeRewardAd();
                  }
                  else
                  {
                      Debug.LogError("Rewarded ad failed to load after 3 retries. Giving up.");
                  }
                  Debug.LogError("Rewarded ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }
              retryCount = 0;
              Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

              cardChangeRewardedAd = ad;
              RewardedAdEvents(cardChangeRewardedAd);
          });
    }


    #endregion
    #region 전면 광고
    public void ShowInterstitialAd()
    {
        if (frontAD != null && frontAD.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            frontAD.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (frontAD != null)
        {
            frontAD.Destroy();
            frontAD = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        InterstitialAd.Load(frontID, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    if (retryCount < 3)
                    {
                        Debug.LogError("Interstitial ad failed to load on attempt " + retryCount + " with error: " + error);
                        retryCount++;
                        LoadInterstitialAd();
                    }
                    else
                    {
                        Debug.LogError("Interstitial ad failed to load after 3 retries. Giving up.");
                    }
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }
                retryCount = 0;
                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                frontAD = ad;
                RegisterReloadHandler(frontAD);
            });
    }
    public void GameOver()
    {
        LoadInterstitialAd();
    }

    //public void HandleOnAdClosed(object sender, EventArgs args)
    //{
    //    //print("전면광고 닫기");
    //    GameController.Inst.GameOver();
    //    //RequestFrontAD();
    //}
    #endregion

    #region 버린 리워드 광고(주석)
    ////파밍카드 리워드 광고
    //public void LoadFarmingCardRewardAd()
    //{
    //    //RequestRewardAD();

    //    StartCoroutine(ShowRewardADCour());

    //    IEnumerator ShowRewardADCour()
    //    {
    //        //while (!this.rewardedAd.IsLoaded())
    //        //{
    //        //    print("리워드광고 로딩 아직 안됨");
    //        //    yield return null;
    //        //}
    //        yield return new WaitUntil(() => this.rewardedAd.IsLoaded());

    //        print("리워드광고 로딩 됨");

    //        this.rewardedAd.Show();
    //    }
    //    this.rewardedAd.OnUserEarnedReward += (sender, e) =>
    //    {
    //        print("파밍카드 보상 광고 성공");
    //        SkillCardController.Inst.OnPopUpUI();

    //        //RequestRewardAD();
    //    };
    //}

    ////더블골드 리워드 광고
    //public void LoadDoubleGoldRewardAd()
    //{
    //    //RequestRewardAD();

    //    StartCoroutine(ShowRewardADCour());

    //    IEnumerator ShowRewardADCour()
    //    {
    //        //while (!this.rewardedAd.IsLoaded())
    //        //{
    //        //    print("리워드광고 로딩 아직 안됨");
    //        //    yield return null;
    //        //}
    //        yield return new WaitUntil(() => this.rewardedAd.IsLoaded());

    //        print("리워드광고 로딩 됨");

    //        this.rewardedAd.Show();
    //    }
    //    this.rewardedAd.OnUserEarnedReward += (sender, e) =>
    //    {
    //        Time.timeScale = 1;

    //        print("더블골드 보상 광고 성공");
    //        GameController.Inst.DoubleGold();

    //        //RequestRewardAD();
    //    };
    //}
    #endregion
}
