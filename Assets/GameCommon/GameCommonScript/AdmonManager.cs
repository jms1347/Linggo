using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmonManager : MonoBehaviour
{
    public bool isTestMode;
    void Start()
    {
        var requestConfiguration = new RequestConfiguration
          .Builder()
          .SetTestDeviceIds(new List<string>() { "1DF7B7CC05014E8", "934A6C36C6B6EC4541C45D32AF3FB", "6F78F3702764B9C7"}) // test Device ID
          .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);
        MobileAds.Initialize((initStatus) => { });
        //LoadBannerAd();
        //LoadFrontAd();
        //LoadRewardAd();
    }

    //void Update()
    //{
    //    FrontAdsBtn.interactable = frontAd.IsLoaded();
    //    RewardAdsBtn.interactable = rewardAd.IsLoaded();
    //}

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }



    #region 배너 광고
    const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    const string bannerID = "";
    BannerView bannerAd;


    void LoadBannerAd()
    {
        bannerAd = new BannerView(isTestMode ? bannerTestID : bannerID,
            AdSize.SmartBanner, AdPosition.Bottom);
        bannerAd.LoadAd(GetAdRequest());
        ToggleBannerAd(false);
    }

    public void ToggleBannerAd(bool b)
    {
        if (b) bannerAd.Show();
        else bannerAd.Hide();
    }
    #endregion



    #region 전면 광고
    const string frontTestID = "ca-app-pub-3940256099942544/1033173712";
    const string frontID = "ca-app-pub-3819330341227143/9853527457";
    InterstitialAd frontAd;


    void LoadFrontAd()
    {
        frontAd = new InterstitialAd(isTestMode ? frontTestID : frontID);
        frontAd.LoadAd(GetAdRequest());
        frontAd.OnAdClosed += (sender, e) =>
        {
            print("전면광고 성공");
            Time.timeScale = 1;
            GameController.Inst.GameOver();
        };
    }

    public void ShowFrontAd()
    {
        LoadFrontAd();
        frontAd.Show();
        
    }
    #endregion



    #region 리워드 광고
    const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string rewardID = "ca-app-pub-3819330341227143/9605760228";
    RewardedAd rewardAd;


    void LoadRewardAd()
    {
        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) =>
        {
            print("리워드 광고 성공");
        };
    }
    public void ShowRewardAd()
    {
        LoadRewardAd();
        rewardAd.Show();

    }
    //환생 리워드 광고
    void LoadRebirthRewardAd()
    {
        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) =>
        {
            print("환생 보상 광고 성공");
            GameController.Inst.SettingRebirth();
        };
    }
    public void ShowRebirthRewardAd()
    {
        LoadRebirthRewardAd();
        rewardAd.Show();
    }


    //카드 체인지 리워드 광고
    void LoadCardChangeRewardAd()
    {
        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) =>
        {
            print("카드체인지 보상 광고 성공");
            SkillCardController.Inst.ChangeCardRewardAD();
        };
    }
    public void ShowCardChangeRewardAd()
    {
        LoadCardChangeRewardAd();
        rewardAd.Show();
    }

    //파밍카드 리워드 광고
    void LoadFarmingCardRewardAd()
    {
        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) =>
        {
            print("파밍카드 보상 광고 성공");
            SkillCardController.Inst.OnPopUpUI();
        };
    }
    public void ShowFarmingCardRewardAd()
    {
        LoadFarmingCardRewardAd();
        rewardAd.Show();
    }

    //더블골드 리워드 광고
    void LoadDoubleGoldRewardAd()
    {

        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) =>
        {
            Time.timeScale = 1;

            print("더블골드 보상 광고 성공");
            GameController.Inst.DoubleGold();
        };
    }
    public void ShowDoubleGoldRewardAd()
    {
        LoadDoubleGoldRewardAd();
        rewardAd.Show();
    }
    #endregion
}
