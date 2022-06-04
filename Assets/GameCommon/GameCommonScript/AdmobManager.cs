using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdmobManager : MonoBehaviour
{
    private InterstitialAd frontAD;
    private RewardedAd rebirthRewardedAd;
    private RewardedAd cardChangeRewardedAd;

    const string frontID = "ca-app-pub-3819330341227143/5753851917";
    const string rewardIDcardChagne = "ca-app-pub-3819330341227143/2943570331";
    const string rewardIDrebirth = "ca-app-pub-3819330341227143/6771134705";


    private void Start()
    {
        MobileAds.Initialize((initStatus) => {
            RequestRebirthRewardAD();
            RequestCardChangeRewardAD();
            RequestFrontAD();
        });
        
    }


    #region 리워드 광고(환생)
    public void RequestRebirthRewardAD()
    {
        print("리워드광고 이닛(환생)");
        this.rebirthRewardedAd = new RewardedAd(rewardIDrebirth);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rebirthRewardedAd.LoadAd(request);
    }



    //환생 리워드 광고
    public void LoadRebirthRewardAd()
    {
        StartCoroutine(ShowRewardADCour());

        IEnumerator ShowRewardADCour()
        {
            while (!this.rebirthRewardedAd.IsLoaded())
            {
                print("리워드광고 로딩 아직 안됨");
                yield return null;
            }
            yield return new WaitUntil(()=>this.rebirthRewardedAd.IsLoaded());            
            print("리워드광고 로딩 됨");
            this.rebirthRewardedAd.Show();
        }
        this.rebirthRewardedAd.OnUserEarnedReward += (sender, e) =>
        {
            GameController.Inst.SettingRebirth();
            RequestRebirthRewardAD();
            print("환생 보상 광고 성공");
        };
        this.rebirthRewardedAd.OnAdFailedToLoad += (sender, e) =>
        {
            print("리워드광고 로드 실패_재요청");
            RequestRebirthRewardAD();
        };

        this.rebirthRewardedAd.OnAdClosed += (sender, e) =>
         {
             RequestRebirthRewardAD();

         };
    }
    #endregion

    #region 리워드 광고(카드 체인지)
    public void RequestCardChangeRewardAD()
    {
        print("리워드광고 이닛(카드체읹)");
        this.cardChangeRewardedAd = new RewardedAd(rewardIDcardChagne);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.cardChangeRewardedAd.LoadAd(request);
    }

    //카드 체인지 리워드 광고
    public void LoadCardChangeRewardAd()
    {
        StartCoroutine(ShowRewardADCour());

        IEnumerator ShowRewardADCour()
        {
            while (!this.cardChangeRewardedAd.IsLoaded())
            {
                print("리워드광고 로딩 아직 안됨");
                yield return null;
            }

            yield return new WaitUntil(() => this.cardChangeRewardedAd.IsLoaded());
            print("리워드광고 로딩 됨");
            this.cardChangeRewardedAd.Show();
        }
        this.cardChangeRewardedAd.OnUserEarnedReward += (sender, e) =>
        {
            print("카드체인지 보상 광고 성공");
            SkillCardController.Inst.ChangeCardRewardAD();
            RequestCardChangeRewardAD();
        };
        this.cardChangeRewardedAd.OnAdFailedToLoad += (sender, e) =>
        {
            print("리워드광고 로드 실패_재요청");
            RequestCardChangeRewardAD();
        };
        this.cardChangeRewardedAd.OnAdClosed += (sender, e) =>
        {
            RequestCardChangeRewardAD();

        };
    }


    #endregion
    #region 전면 광고
    private void RequestFrontAD()
    {
        print("전면광고 이닛");

        // Initialize an InterstitialAd.
        this.frontAD = new InterstitialAd(frontID);
        // Called when the ad is closed.
        this.frontAD.OnAdClosed += HandleOnAdClosed;
        // Create an empty ad request.
        AdRequest request2 = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.frontAD.LoadAd(request2);
    }

    public void GameOver()
    {
        //RequestFrontAD();

        StartCoroutine(ShowFronADCour());

        IEnumerator ShowFronADCour()
        {
            while (!this.frontAD.IsLoaded())
            {
                print("전면광고 로딩 아직 안됨");
                yield return null;
            }
            yield return new WaitUntil(() => this.frontAD.IsLoaded());
            print("전면광고 로딩 됨");
            this.frontAD.Show();
        }
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        print("전면광고 닫기");
        GameController.Inst.GameOver();
        RequestFrontAD();
    }
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
