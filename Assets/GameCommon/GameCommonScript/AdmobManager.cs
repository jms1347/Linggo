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


    #region ������ ����(ȯ��)
    public void RequestRebirthRewardAD()
    {
        print("�����層�� �̴�(ȯ��)");
        this.rebirthRewardedAd = new RewardedAd(rewardIDrebirth);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rebirthRewardedAd.LoadAd(request);
    }



    //ȯ�� ������ ����
    public void LoadRebirthRewardAd()
    {
        StartCoroutine(ShowRewardADCour());

        IEnumerator ShowRewardADCour()
        {
            while (!this.rebirthRewardedAd.IsLoaded())
            {
                print("�����層�� �ε� ���� �ȵ�");
                yield return null;
            }
            yield return new WaitUntil(()=>this.rebirthRewardedAd.IsLoaded());            
            print("�����層�� �ε� ��");
            this.rebirthRewardedAd.Show();
        }
        this.rebirthRewardedAd.OnUserEarnedReward += (sender, e) =>
        {
            GameController.Inst.SettingRebirth();
            RequestRebirthRewardAD();
            print("ȯ�� ���� ���� ����");
        };
        this.rebirthRewardedAd.OnAdFailedToLoad += (sender, e) =>
        {
            print("�����層�� �ε� ����_���û");
            RequestRebirthRewardAD();
        };

        this.rebirthRewardedAd.OnAdClosed += (sender, e) =>
         {
             RequestRebirthRewardAD();

         };
    }
    #endregion

    #region ������ ����(ī�� ü����)
    public void RequestCardChangeRewardAD()
    {
        print("�����層�� �̴�(ī��ü��)");
        this.cardChangeRewardedAd = new RewardedAd(rewardIDcardChagne);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.cardChangeRewardedAd.LoadAd(request);
    }

    //ī�� ü���� ������ ����
    public void LoadCardChangeRewardAd()
    {
        StartCoroutine(ShowRewardADCour());

        IEnumerator ShowRewardADCour()
        {
            while (!this.cardChangeRewardedAd.IsLoaded())
            {
                print("�����層�� �ε� ���� �ȵ�");
                yield return null;
            }

            yield return new WaitUntil(() => this.cardChangeRewardedAd.IsLoaded());
            print("�����層�� �ε� ��");
            this.cardChangeRewardedAd.Show();
        }
        this.cardChangeRewardedAd.OnUserEarnedReward += (sender, e) =>
        {
            print("ī��ü���� ���� ���� ����");
            SkillCardController.Inst.ChangeCardRewardAD();
            RequestCardChangeRewardAD();
        };
        this.cardChangeRewardedAd.OnAdFailedToLoad += (sender, e) =>
        {
            print("�����層�� �ε� ����_���û");
            RequestCardChangeRewardAD();
        };
        this.cardChangeRewardedAd.OnAdClosed += (sender, e) =>
        {
            RequestCardChangeRewardAD();

        };
    }


    #endregion
    #region ���� ����
    private void RequestFrontAD()
    {
        print("���鱤�� �̴�");

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
                print("���鱤�� �ε� ���� �ȵ�");
                yield return null;
            }
            yield return new WaitUntil(() => this.frontAD.IsLoaded());
            print("���鱤�� �ε� ��");
            this.frontAD.Show();
        }
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        print("���鱤�� �ݱ�");
        GameController.Inst.GameOver();
        RequestFrontAD();
    }
    #endregion

    #region ���� ������ ����(�ּ�)
    ////�Ĺ�ī�� ������ ����
    //public void LoadFarmingCardRewardAd()
    //{
    //    //RequestRewardAD();

    //    StartCoroutine(ShowRewardADCour());

    //    IEnumerator ShowRewardADCour()
    //    {
    //        //while (!this.rewardedAd.IsLoaded())
    //        //{
    //        //    print("�����層�� �ε� ���� �ȵ�");
    //        //    yield return null;
    //        //}
    //        yield return new WaitUntil(() => this.rewardedAd.IsLoaded());

    //        print("�����層�� �ε� ��");

    //        this.rewardedAd.Show();
    //    }
    //    this.rewardedAd.OnUserEarnedReward += (sender, e) =>
    //    {
    //        print("�Ĺ�ī�� ���� ���� ����");
    //        SkillCardController.Inst.OnPopUpUI();

    //        //RequestRewardAD();
    //    };
    //}

    ////������ ������ ����
    //public void LoadDoubleGoldRewardAd()
    //{
    //    //RequestRewardAD();

    //    StartCoroutine(ShowRewardADCour());

    //    IEnumerator ShowRewardADCour()
    //    {
    //        //while (!this.rewardedAd.IsLoaded())
    //        //{
    //        //    print("�����層�� �ε� ���� �ȵ�");
    //        //    yield return null;
    //        //}
    //        yield return new WaitUntil(() => this.rewardedAd.IsLoaded());

    //        print("�����層�� �ε� ��");

    //        this.rewardedAd.Show();
    //    }
    //    this.rewardedAd.OnUserEarnedReward += (sender, e) =>
    //    {
    //        Time.timeScale = 1;

    //        print("������ ���� ���� ����");
    //        GameController.Inst.DoubleGold();

    //        //RequestRewardAD();
    //    };
    //}
    #endregion
}
