﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PlayNANOO;
using PlayNANOO.Interfacaes;
using PlayNANOO.ChatServer;
using PlayNANOO.ChatServer.Models;
using PlayNANOO.CloudCode;
using PlayNANOO.SimpleJSON;

public class PlayNANOOExample : MonoBehaviour, IChatListener
{
    Plugin plugin;
    ChatClient chatClient;

    string accountToken;

    void Start()
    {
        plugin = Plugin.GetInstance();

        // Guest SignIn
        plugin.AccountManagerV20240401.GuestSignIn((status, errorCode, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["access_token"].ToString());
                accountToken = values["access_token"].ToString();
            }
            else
            {
                if (values["ErrorCode"].ToString() == "30007")
                {
                    Debug.Log(values["WithdrawalKey"].ToString());
                }
                else if (values["ErrorCode"].ToString() == "70002")
                {
                    Debug.Log(values["BlockKey"].ToString());
                    BlockReason(values["BlockKey"].ToString());
                }
            }
        });    }

    public void AppleToken(string token)
    {
        
    }

    public void ChangeApiKey()
    {
        string gameId = string.Empty;
        string serviceKey = string.Empty;
        string secretKey = string.Empty;

        plugin.ServiceSetting(gameId, serviceKey, secretKey);
    }

    public void OnConntected()
    {
        chatClient.Channels();
    }

    public void OnChannels(ChatChannelModel[] channels)
    {
        if (channels.Length > 0)
        {
            foreach (ChatChannelModel channel in channels)
            {
                Debug.Log(channel.channel);
                Debug.Log(channel.count.ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service();
        }

        plugin.AccountCheckDuplicate(OnCheckAccountDuplicate);
    }

    void OnCheckAccountDuplicate(bool isDuplicate)
    {
        if (isDuplicate)
        {
            Debug.LogError("Duplicate connection has been detected.");
        }
    }

    void PopupWindowCallback(string code)
    {
        Debug.Log("PopupWindowCallback");
        Debug.Log(code);
    }

    public void OpenView()
    {
        plugin.OpenView("https://www.playnanoo.com", (response) => {
            Debug.Log("Close Callback");
        });
    }

    public void OpenHelpDeskGuestMode()
    {
        plugin.ClearHelpDeskOptional();
        plugin.SetHelpDeskOptional("OptionTest1", "ValueTest1");
        plugin.SetHelpDeskOptional("OptionTest2", "ValueTest2");
        plugin.OpenHelpDeskGuestMode((response) => {
            Debug.Log("Close Callback");
        });
    }

    #region GameLog
    public void GameLog()
    {
        PlayNANOO.GameLog.LogParams logParams = new PlayNANOO.GameLog.LogParams();
        logParams.Add("Level1", Random.Range(0, 100000).ToString());
        logParams.Add("Level2", Random.Range(0, 100000).ToString());
        logParams.Add("Level3", Random.Range(0, 100000).ToString());
        logParams.Add("Level4", Random.Range(0, 100000).ToString());

        string tableCode = string.Empty;
        plugin.GameLog.Save(tableCode, logParams, (state, message, jsonString, dictionary) => {
            Debug.Log(jsonString);
        });
    }
    #endregion

    #region Accounts
    public void AccountLink()
    {
        string userID = string.Empty;
        plugin.AccountManagerV20240401.CustomLink(userID, Configure.PN_ACCOUNT_LINK, (status, errorCode, jsonString, values) =>
        {
            Debug.Log(values["access_token"].ToString());
            Debug.Log(values["refresh_token"].ToString());
            Debug.Log(values["uuid"].ToString());
            Debug.Log(values["openID"].ToString());
            Debug.Log(values["nickname"].ToString());
            Debug.Log(values["linkedID"].ToString());
            Debug.Log(values["linkedType"].ToString());
            Debug.Log(values["country"].ToString());
        });
    }

    public void AccountGuest()
    {
        plugin.AccountManagerV20240401.GuestSignIn((status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["access_token"].ToString());
                Debug.Log(values["refresh_token"].ToString());
                Debug.Log(values["uuid"].ToString());
                Debug.Log(values["openID"].ToString());
                Debug.Log(values["nickname"].ToString());
                Debug.Log(values["linkedID"].ToString());
                Debug.Log(values["linkedType"].ToString());
                Debug.Log(values["country"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void AccountEmailSignIn()
    {
        string email = string.Empty;
        string password = string.Empty;
        plugin.AccountManagerV20240401.EmailSignIn(email, password, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["access_token"].ToString());
                Debug.Log(values["refresh_token"].ToString());
                Debug.Log(values["uuid"].ToString());
                Debug.Log(values["openID"].ToString());
                Debug.Log(values["nickname"].ToString());
                Debug.Log(values["linkedID"].ToString());
                Debug.Log(values["linkedType"].ToString());
                Debug.Log(values["country"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void AccountEmailSignUp()
    {
        string email = string.Empty;
        string password = string.Empty;
        plugin.AccountManagerV20240401.EmailSignUp(email, password, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["access_token"].ToString());
                Debug.Log(values["refresh_token"].ToString());
                Debug.Log(values["uuid"].ToString());
                Debug.Log(values["openID"].ToString());
                Debug.Log(values["nickname"].ToString());
                Debug.Log(values["linkedID"].ToString());
                Debug.Log(values["linkedType"].ToString());
                Debug.Log(values["country"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void AccountNicknameExists()
    {
        string nickname = string.Empty;
        plugin.AccountManagerV20240401.NicknameExists(nickname, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["status"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void AccountNicknameGet()
    {
        plugin.AccountManagerV20240401.NicknameGet((status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["nickname"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void AccountNicknamePut()
    {
        string nickname = string.Empty;
        plugin.AccountManagerV20240401.NicknamePut(nickname, true, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["nickname"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void AccountTokenInfo()
    {
        string accessToken = string.Empty;
        plugin.AccountManagerV20240401.TokenInfo(accessToken, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["access_token"].ToString());
                Debug.Log(values["refresh_token"].ToString());
                Debug.Log(values["uuid"].ToString());
                Debug.Log(values["openID"].ToString());
                Debug.Log(values["nickname"].ToString());
                Debug.Log(values["linkedID"].ToString());
                Debug.Log(values["linkedType"].ToString());
                Debug.Log(values["country"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void AccountTokenStatus()
    {
        string accessToken = string.Empty;
        plugin.AccountManagerV20240401.TokenStatus(accessToken, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["status"].ToString());
            }
            else
            {
                Debug.Log("Fail");
                Debug.Log(jsonString.ToString());
            }
        });
    }

    public void AccountTokenRefresh()
    {
        string refreshToken = string.Empty;
        plugin.AccountManagerV20240401.TokenRefresh(refreshToken, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["access_token"].ToString());
                Debug.Log(values["refresh_token"].ToString());
                Debug.Log(values["uuid"].ToString());
                Debug.Log(values["openID"].ToString());
                Debug.Log(values["nickname"].ToString());
                Debug.Log(values["linkedID"].ToString());
                Debug.Log(values["linkedType"].ToString());
                Debug.Log(values["country"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void AccountTokenSingIn()
    {
        string accessToken = string.Empty;
        plugin.AccountManagerV20240401.TokenSignIn(accessToken, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["access_token"].ToString());
                Debug.Log(values["refresh_token"].ToString());
                Debug.Log(values["uuid"].ToString());
                Debug.Log(values["openID"].ToString());
                Debug.Log(values["nickname"].ToString());
                Debug.Log(values["linkedID"].ToString());
                Debug.Log(values["linkedType"].ToString());
                Debug.Log(values["country"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void AccountTokenSingOut()
    {
        string accessToken = string.Empty;
        plugin.AccountManagerV20240401.TokenSignOut(accessToken, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(jsonString);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void AccountSocialChange()
    {
        string accessToken = string.Empty;

        plugin.AccountManagerV20240401.SocialChange(accessToken, Configure.PN_ACCOUNT_APPLE_ID, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["access_token"].ToString());
                Debug.Log(values["refresh_token"].ToString());
                Debug.Log(values["uuid"].ToString());
                Debug.Log(values["openID"].ToString());
                Debug.Log(values["nickname"].ToString());
                Debug.Log(values["linkedID"].ToString());
                Debug.Log(values["linkedType"].ToString());
                Debug.Log(values["country"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void AccountBan()
    {
        int period = 0;
        string service = "inbox,ranking";
        string reason = "Test";
        string accessToken = string.Empty;

        plugin.AccountManagerV20240401.Ban(accessToken, period, service, reason, (status, errorCode, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
                Debug.Log(values["status"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    #region Server Time
    public void ServerTime()
    {
        plugin.ServerTime((state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["timezone"]);
                Debug.Log(dictionary["timestamp"]);
                Debug.Log(dictionary["ISO_8601_date"]);
                Debug.Log(dictionary["date"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    #region Coupon
    public void Coupon()
    {
        plugin.CouponManagerV20240301.Use("COUPON_CODE", (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
                foreach (PlayNANOO.Coupon.v20240301.ItemValueModel item in (PlayNANOO.Coupon.v20240301.ItemValueModel[])values["Items"])
                {
                    Debug.Log(item.item_code);
                    Debug.Log(item.item_count);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    #region Inbox

    public void InboxItems()
    {
        string tableCode = "TABLE_CODE";

        plugin.InboxManager.Items(tableCode, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["Type"]);
                    Debug.Log(value["ItemKey"]);
                    Debug.Log(value["ExpireSec"]);

                    PlayNANOO.Inbox.ItemValueModel[] items = value["Items"] as PlayNANOO.Inbox.ItemValueModel[];
                    foreach (PlayNANOO.Inbox.ItemValueModel item in items)
                    {
                        Debug.Log(item.item_code);
                        Debug.Log(item.item_count);
                    }

                    PlayNANOO.Inbox.MessageValueModel[] messages = value["Messages"] as PlayNANOO.Inbox.MessageValueModel[];
                    foreach (PlayNANOO.Inbox.MessageValueModel message in messages)
                    {
                        Debug.Log(message.language);
                        Debug.Log(message.title);
                        Debug.Log(message.content);
                    }
                }
            }
            else
            {
                Debug.Log("Fail");
            }

        });
    }

    public void InboxCount()
    {
        string tableCode = "TABLE_CODE";

        plugin.InboxManager.Count(tableCode, (status, error, jsonString, values) =>
        {
            Debug.Log(values["Count"]);
        });
    }

    public void InboxShow()
    {
        string itemKey = "ITEM_KEY";

        plugin.InboxManager.Show(itemKey, (status, error, jsonString, values) =>
        {
            PlayNANOO.Inbox.ItemValueModel[] items = values["Items"] as PlayNANOO.Inbox.ItemValueModel[];
            foreach (PlayNANOO.Inbox.ItemValueModel item in items)
            {
                Debug.Log(item.item_code);
                Debug.Log(item.item_count);
            }

            PlayNANOO.Inbox.MessageValueModel[] messages = values["Messages"] as PlayNANOO.Inbox.MessageValueModel[];
            foreach (PlayNANOO.Inbox.MessageValueModel message in messages)
            {
                Debug.Log(message.language);
                Debug.Log(message.title);
                Debug.Log(message.content);
            }
        });
    }

    public void InboxConsumeItem()
    {
        string itemKey = "ITEM_KEY";

        plugin.InboxManager.ConsumeItem(itemKey, (status, error, jsonString, values) =>
        {
            PlayNANOO.Inbox.ItemValueModel[] items = values["Items"] as PlayNANOO.Inbox.ItemValueModel[];
            foreach (PlayNANOO.Inbox.ItemValueModel item in items)
            {
                Debug.Log(item.item_code);
                Debug.Log(item.item_count);
            }
        });
    }

    public void InboxConsumeMultiItem()
    {
        List<string> itemKeys = new List<string>();
        itemKeys.Add("ITEM_KEY_1");
        itemKeys.Add("ITEM_KEY_2");

        plugin.InboxManager.ConsumeMultiItem(itemKeys, (status, error, jsonString, values) =>
        {
            PlayNANOO.Inbox.ItemValueModel[] items = values["Items"] as PlayNANOO.Inbox.ItemValueModel[];
            foreach (PlayNANOO.Inbox.ItemValueModel item in items)
            {
                Debug.Log(item.item_code);
                Debug.Log(item.item_count);
            }
        });
    }

    public void InboxSendItem()
    {
        string tableCode = "TABLE_CODE";

        List<PlayNANOO.Inbox.ItemValueModel> items = new List<PlayNANOO.Inbox.ItemValueModel>();
        List<PlayNANOO.Inbox.MessageValueModel> messages = new List<PlayNANOO.Inbox.MessageValueModel>();

        items.Add(new PlayNANOO.Inbox.ItemValueModel { item_code = "GOLD", item_count = 100 });
        items.Add(new PlayNANOO.Inbox.ItemValueModel { item_code = "GOLD", item_count = 20 });

        messages.Add(new PlayNANOO.Inbox.MessageValueModel { language = Configure.PN_LANG_CODE_DEFAULT, title = "TEST", content = "TEST" });
        messages.Add(new PlayNANOO.Inbox.MessageValueModel { language = Configure.PN_LANG_CODE_KO, title = "TEST", content = "TEST" });

        plugin.InboxManager.SendItem(tableCode, items, messages, 10, (status, error, jsonString, values) =>
        {
            Debug.Log(jsonString);
        });
    }

    public void InboxSendItemPlayer()
    {
        string tableCode = "TABLE_CODE";
        string playerId = "PLAYER_ID";

        List<PlayNANOO.Inbox.ItemValueModel> items = new List<PlayNANOO.Inbox.ItemValueModel>();
        List<PlayNANOO.Inbox.MessageValueModel> messages = new List<PlayNANOO.Inbox.MessageValueModel>();

        items.Add(new PlayNANOO.Inbox.ItemValueModel { item_code = "GOLD", item_count = 100 });
        items.Add(new PlayNANOO.Inbox.ItemValueModel { item_code = "GOLD", item_count = 20 });

        messages.Add(new PlayNANOO.Inbox.MessageValueModel { language = Configure.PN_LANG_CODE_DEFAULT, title = "TEST", content = "TEST" });
        messages.Add(new PlayNANOO.Inbox.MessageValueModel { language = Configure.PN_LANG_CODE_KO, title = "TEST", content = "TEST" });

        plugin.InboxManager.SendItemPlayer(tableCode, playerId, items, messages, 10, (status, error, jsonString, values) =>
        {
            Debug.Log(jsonString);
        });
    }
    #endregion

    #region Cloud Data(Storage)
    public void StorageSave()
    {
        plugin.Storage.Save("TEST_KEY", "TEST_KEY_VALUE", false, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void StorageMultiSave()
    {
        PlayNANOO.Storage.MultiSaveParam multiSaveParam = new PlayNANOO.Storage.MultiSaveParam();
        multiSaveParam.Items.Add(new PlayNANOO.Storage.MultiSaveParamValue { StorageKey = "Storage1", StorageValue = "StorageValue1", IsPrivate = true });
        multiSaveParam.Items.Add(new PlayNANOO.Storage.MultiSaveParamValue { StorageKey = "Storage2", StorageValue = "StorageValue2", IsPrivate = false });

        plugin.Storage.MultiSave(multiSaveParam, (status, error, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void StorageLoad()
    {
        plugin.Storage.Load("TEST_KEY", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["value"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void StoragePlayerDataLoad()
    {
        plugin.Storage.PlayerDataLoad("TEST_KEY", "TEST_KEY_VALUE", (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["StorageKey"]);
                Debug.Log(values["StorageValue"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void StorageMultiLoad()
    {
        PlayNANOO.Storage.MultiLoadParam multiLoadParam = new PlayNANOO.Storage.MultiLoadParam();
        multiLoadParam.Items.Add(new PlayNANOO.Storage.MultiLoadParamValue { StorageKey = "TEST_KEY" });
        multiLoadParam.Items.Add(new PlayNANOO.Storage.MultiLoadParamValue { StorageKey = "TEST_KEY" });

        plugin.Storage.MultiLoad(multiLoadParam, (status, error, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["PlayerId"]);
                    Debug.Log(value["StorageKey"]);
                    Debug.Log(value["StorageValue"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void StorageMultiPlayerDataLoad()
    {
        PlayNANOO.Storage.MultiLoadParam multiLoadParam = new PlayNANOO.Storage.MultiLoadParam();
        multiLoadParam.Items.Add(new PlayNANOO.Storage.MultiLoadParamValue { PlayerId = "PLAYER_TEST_ID", StorageKey = "TEST_KEY" });
        multiLoadParam.Items.Add(new PlayNANOO.Storage.MultiLoadParamValue { PlayerId = "PLAYER_TEST_ID", StorageKey = "TEST_KEY" });

        plugin.Storage.MultiPlayerDataLoad(multiLoadParam, (status, error, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["PlayerId"]);
                    Debug.Log(value["StorageKey"]);
                    Debug.Log(value["StorageValue"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void StorageLoadPublic()
    {
        plugin.Storage.LoadPublic("TEST_KEY", (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["StorageKey"]);
                Debug.Log(values["StorageValue"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void StorageDelete()
    {
        string storageKey = "TEST_KEY";
        plugin.Storage.Delete(storageKey, (status, message, jsonString, dictionary) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    #region Ranking
    public void Ranking()
    {
        plugin.Ranking("RANKING_CODE", 50, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                ArrayList list = (ArrayList)dictionary["list"];
                foreach (Dictionary<string, object> rank in list)
                {
                    Debug.Log(rank["score"]);
                    Debug.Log(rank["data"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void RankingRange()
    {
        plugin.RankingRange("RANKING_CODE", 1, 10, (status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach(Dictionary<string, object> item in (ArrayList)values["items"])
                {
                    Debug.Log(item["ranking"]);
                    Debug.Log(item["uuid"]);
                    Debug.Log(item["nickname"]);
                    Debug.Log(item["score"]);
                    Debug.Log(item["data"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void RankingPreviousRange()
    {
        plugin.RankingPreviousRange("RANKING_CODE", 1, 1, 10, (status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> item in (ArrayList)values["items"])
                {
                    Debug.Log(item["ranking"]);
                    Debug.Log(item["uuid"]);
                    Debug.Log(item["nickname"]);
                    Debug.Log(item["score"]);
                    Debug.Log(item["data"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void RankingRecord()
    {
        plugin.RankingRecord("RANKING_CODE", 100, "TEST_PLAYER_RANKING_VALUE", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void RankingPersonal()
    {
        plugin.RankingPersonal("RANKING_CODE", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["ranking"]);
                Debug.Log(dictionary["data"]);
                Debug.Log(dictionary["total_player"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void RankingSeason()
    {
        plugin.RankingSeasonInfo("RANKING_CODE", (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["season"]);
                Debug.Log(dictionary["expire_sec"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void RankingDelete()
    {
        string tableCode = string.Empty;

        plugin.RankingManager.Delete(tableCode, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
                Debug.Log(values["status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void RankingPlayerDataEdit()
    {
        string tableCode = string.Empty;
        string playerData = string.Empty;

        plugin.RankingManager.PlayerDataEdit(tableCode, playerData, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
                Debug.Log(values["status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    #region Leaderboard
    public void LeaderboardTableCreate()
    {
        PlayNANOO.LeaderBoard.v20240301.TableManage tableManage = new PlayNANOO.LeaderBoard.v20240301.TableManage();
        tableManage.name = "ROTATION"; 
        tableManage.rotation = PlayNANOO.LeaderBoard.v20240301.Rotaion.ROTATION; 
        tableManage.recordType = PlayNANOO.LeaderBoard.v20240301.RecordType.HIGHSCORE;
        tableManage.recordSortType = PlayNANOO.LeaderBoard.v20240301.SortType.DESC;
        tableManage.recordPriority = false; 
        tableManage.rotationDetail = new PlayNANOO.LeaderBoard.v20240301.TableManageRotationDetail();
        tableManage.rotationDetail.period = 10;
        tableManage.rotationDetail.timeOffset = 32400;
        tableManage.rotationDetail.date = "2024-03-01"; 
        tableManage.rotationDetail.time = "15:00"; 

        plugin.LeaderboardManagerV20240301.TableCreate(tableManage, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
                Debug.Log(values["TableId"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardTableEdit()
    {
        PlayNANOO.LeaderBoard.v20240301.TableManage tableManage = new PlayNANOO.LeaderBoard.v20240301.TableManage();
        tableManage.uid = "TABLE_CODE"; 
        tableManage.recordType = PlayNANOO.LeaderBoard.v20240301.RecordType.HIGHSCORE;
        tableManage.recordSortType = PlayNANOO.LeaderBoard.v20240301.SortType.ASC;
        tableManage.recordPriority = false; 

        plugin.LeaderboardManagerV20240301.TableEdit(tableManage, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardTableDelete()
    {
        string tableCode = "TABLE_CODE"; 
        plugin.LeaderboardManagerV20240301.TableDelete(tableCode, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardTableShow()
    {
        string tableCode = "TABLE_CODE"; 
        plugin.LeaderboardManagerV20240301.TableShow(tableCode, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
                Debug.Log(values["Rotation"]);
                Debug.Log(values["RotationCount"]);
                Debug.Log(values["RotationTimeLeft"]);
                Debug.Log(values["RecordType"]);
                Debug.Log(values["RecordSortType"]);
                Debug.Log(values["TotalIds"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardTableTotalPlayerCount()
    {
        string tableCode = "TABLE_CODE";
        plugin.LeaderboardManagerV20240301.TotalCount(tableCode, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
                Debug.Log(values["TotalIds"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardTableTotalPlayerCountPrev()
    {
        string tableCode = "TABLE_CODE"; 
        int rotationCount = 1;
        plugin.LeaderboardManagerV20240301.TotalCountPrev(tableCode, rotationCount, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
                Debug.Log(values["TotalIds"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardPlayerShow()
    {
        string tableCode = "TABLE_CODE"; 
        string recordId = "PLAYER-0001"; 
        plugin.LeaderboardManagerV20240301.Show(tableCode, recordId, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
                Debug.Log(values["Rank"]);
                Debug.Log(values["RotationCount"]);
                Debug.Log(values["RecordId"]);
                Debug.Log(values["Score"]);
                Debug.Log(values["ExtraData"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardPlayerShowPrev()
    {
        string tableCode = "TABLE_CODE";
        string recordId = "PLAYER-0001";
        int rotationCount = 1;
        plugin.LeaderboardManagerV20240301.ShowPrev(tableCode, rotationCount, recordId, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
                Debug.Log(values["Rank"]);
                Debug.Log(values["RotationCount"]);
                Debug.Log(values["RecordId"]);
                Debug.Log(values["Score"]);
                Debug.Log(values["ExtraData"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardPlayerRange()
    {
        string tableCode = "TABLE_CODE"; 
        plugin.LeaderboardManagerV20240301.Range(tableCode, 1, 100, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["Rank"]);
                    Debug.Log(value["RotationCount"]);
                    Debug.Log(value["RecordId"]);
                    Debug.Log(value["Score"]);
                    Debug.Log(value["ExtraData"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardPlayerRangePrev()
    {
        string tableCode = "TABLE_CODE";
        int rotationCount = 1;
        plugin.LeaderboardManagerV20240301.RangePrev(tableCode, rotationCount, 1, 100, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["Rank"]);
                    Debug.Log(value["RotationCount"]);
                    Debug.Log(value["RecordId"]);
                    Debug.Log(value["Score"]);
                    Debug.Log(value["ExtraData"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardPlayerRecord()
    {
        string tableCode = "TABLE_CODE"; 
        string recordId = "PLAYER-0001"; 
        double score = 100;
        string extraData = "Data";
        plugin.LeaderboardManagerV20240301.Record(tableCode, recordId, score, extraData, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardPlayerEdit()
    {
        string tableCode = "TABLE_CODE";
        string recordId = "PLAYER-0001";
        string extraData = "DataTest";
        plugin.LeaderboardManagerV20240301.Edit(tableCode, recordId, extraData, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardPlayerEditPrev()
    {
        string tableCode = "TABLE_CODE";
        string recordId = "PLAYER-0001";
        string extraData = "DataTest";
        int rotationCount = 1;
        plugin.LeaderboardManagerV20240301.EditPrev(tableCode, recordId, rotationCount, extraData, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void LeaderboardPlayerDelete()
    {
        string tableCode = "TABLE_CODE";
        string recordId = "PLAYER-0001";

        plugin.LeaderboardManagerV20240301.Delete(tableCode, recordId, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    #region In App Purchase
    public void IapReceiptionAndroid()
    {
        plugin.IAP.Android("RECEIPT", (status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["UserID"]);
                Debug.Log(values["PackageName"]);
                Debug.Log(values["OrderID"]);
                Debug.Log(values["ProductID"]);
                Debug.Log(values["Currency"]);
                Debug.Log(values["Price"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void IapReceiptioniOS()
    {
        plugin.IAP.IOS("RECEIPT", "PRODUCT_ID", "CURRENCY", 100, (status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["UserID"]);
                Debug.Log(values["PackageName"]);
                Debug.Log(values["OrderID"]);
                Debug.Log(values["ProductID"]);
                Debug.Log(values["Currency"]);
                Debug.Log(values["Price"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void IapReceiptionOneStoreKR()
    {
        plugin.ReceiptVerificationOneStoreKR("PRODUCT_ID", "PURCHASE_ID", "RECEIPT", "CURRENCY", 100, true, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["package"]);
                Debug.Log(dictionary["product_id"]);
                Debug.Log(dictionary["order_id"]);
                Debug.Log("Issue Item");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    #region Cache Data
    public void CacheExists()
    {
        string cacheKey = "TEST001";
        plugin.CacheExists(cacheKey, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["value"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void CacheGet()
    {
        string cacheKey = "TEST001";
        plugin.CacheGet(cacheKey, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["value"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void CacheMultiGet()
    {
        ArrayList cacheKeys = new ArrayList();
        cacheKeys.Add("TEST001");
        cacheKeys.Add("TEST002");
        cacheKeys.Add("TEST003");

        plugin.CacheMultiGet(cacheKeys, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)dictionary["values"])
                {
                    Debug.Log(value["key"]);
                    Debug.Log(value["value"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void CacheSet()
    {
        string cacheKey = "TEST001";
        string cacheValue = "TESTValue";
        int cacheTTL = 3600;
        plugin.CacheSet(cacheKey, cacheValue, cacheTTL, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void CacheIncrby()
    {
        string cacheKey = "TEST002";
        int cacheValue = 100;
        int cacheTTL = 3600;
        plugin.CacheIncrby(cacheKey, cacheValue, cacheTTL, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["value"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void CacheDecrby()
    {
        string cacheKey = "TEST003";
        int cacheValue = 100;
        int cacheTTL = 3600;
        plugin.CacheDecrby(cacheKey, cacheValue, cacheTTL, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(dictionary["value"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void CacheDelete()
    {
        string cacheKey = "TEST003";
        plugin.CacheDel(cacheKey, (state, message, rawData, dictionary) => {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    #region Currency
    public void CurrencyInit()
    {
        plugin.CurrencyManager.Init((status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                PlayNANOO.Currency.ItemModel[] items = values["Items"] as PlayNANOO.Currency.ItemModel[];
                foreach (PlayNANOO.Currency.ItemModel item in items)
                {
                    Debug.Log(item.currency_code);
                    Debug.Log(item.currency_amount);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void CurrencyAll()
    {
        plugin.CurrencyAll((status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach(Dictionary<string, object> item in (ArrayList)values["items"])
                {
                    Debug.Log(item["currency"]);
                    Debug.Log(item["amount"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void CurrencySet()
    {
        List<PlayNANOO.Currency.ItemModel> currencyItems = new List<PlayNANOO.Currency.ItemModel>();
        currencyItems.Add(new PlayNANOO.Currency.ItemModel { currency_code = "CURRENCY_CODE", currency_amount = 100 });
        currencyItems.Add(new PlayNANOO.Currency.ItemModel { currency_code = "CURRENCY_CODE", currency_amount = 100 });

        plugin.CurrencyManager.Put(currencyItems, (status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                PlayNANOO.Currency.ItemModel[] items = values["Items"] as PlayNANOO.Currency.ItemModel[];
                foreach (PlayNANOO.Currency.ItemModel item in items)
                {
                    Debug.Log(item.currency_code);
                    Debug.Log(item.currency_amount);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void CurrencyCharge()
    {
        List<PlayNANOO.Currency.ItemModel> currencyItems = new List<PlayNANOO.Currency.ItemModel>();
        currencyItems.Add(new PlayNANOO.Currency.ItemModel { currency_code = "CURRENCY_CODE", currency_amount = 100 });
        currencyItems.Add(new PlayNANOO.Currency.ItemModel { currency_code = "CURRENCY_CODE", currency_amount = 100 });

        plugin.CurrencyManager.Charge(currencyItems, (status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                PlayNANOO.Currency.ItemModel[] items = values["Items"] as PlayNANOO.Currency.ItemModel[];
                foreach (PlayNANOO.Currency.ItemModel item in items)
                {
                    Debug.Log(item.currency_code);
                    Debug.Log(item.currency_amount);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void CurrencyGet()
    {
        plugin.CurrencyGet("AS", (status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["amount"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void CurrencySubtract()
    {
        plugin.CurrencySubtract("CURRENCY_CODE", 1000, (status, errorMessage, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["amount"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    #region CloudeCode
    public void CloudCodeExecution()
    {
        string tableCode = string.Empty;
        string functionName = string.Empty;

        var parameters = new CloudCodeExecution()
        {
            TableCode = tableCode,
            FunctionName = functionName,
            FunctionArguments = new { InputValue1 = "InputValue1", InputValue2 = "InputValue2", InputValue3 = "InputValue3" }
        };

        plugin.CloudCode.Run(parameters, (state, message, rawData, dictionary) =>
        {
            if (state.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(rawData);
                //PlayNANOO.SimpleJSON.JSONNode node = PlayNANOO.SimpleJSON.JSONNode.Parse(dictionary["Result"].ToString());
                //Debug.Log(node["Function"]["Name"].Value);
                //Debug.Log(node["Function"]["Version"].Value);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    #region Friends
    public void FriendInfo()
    {
        string friendCode = string.Empty;
        plugin.FriendManager.Info(friendCode, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["RelationshipCount"].ToString());
                Debug.Log(values["PendingCount"].ToString());
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void FriendAll()
    {
        string friendCode = string.Empty;
        plugin.FriendManager.Search(friendCode, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(jsonString);
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["RelationshipCode"]);
                    Debug.Log(value["UserId"]);
                    Debug.Log(value["Nickname"]);
                    Debug.Log(value["Timezone"]);
                    Debug.Log(value["AccessSeconds"]);
                    //Debug.Log(value["ExtraData"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void FriendReadyAll()
    {
        string friendCode = string.Empty;
        plugin.FriendManager.SearchPending(friendCode, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(jsonString);
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["RelationshipCode"]);
                    Debug.Log(value["UserId"]);
                    Debug.Log(value["Nickname"]);
                    Debug.Log(value["Timezone"]);
                    Debug.Log(value["AccessSeconds"]);
                }
            }
            else
            {
                Debug.Log("실패");
            }
        });
    }

    public void FriendRequest()
    {
        string friendCode = string.Empty;
        string friendOpenID = "AUIAPO";

        plugin.FriendManager.Request(friendCode, friendOpenID, false, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("성공");
            }
            else
            {
                Debug.Log("실패");
            }
        });
    }

    public void FriendAccept()
    {
        string friendCode = string.Empty;
        string friendRelationshipCode = string.Empty;

        plugin.FriendManager.Accept(friendCode, friendRelationshipCode, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("성공");
            }
            else
            {
                Debug.Log("실패");
            }
        });
    }

    public void FriendDelete()
    {
        string friendCode = string.Empty;
        string friendRelationshipCode = string.Empty;

        plugin.FriendManager.Delete(friendCode, friendRelationshipCode, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log("성공");
            }
            else
            {
                Debug.Log("실패");
            }
        });
    }

    public void FriendRandomSearch()
    {
        string friendCode = string.Empty;
        int limit = 10;

        plugin.FriendManager.SearchRandom(friendCode, limit, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(jsonString);
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["OpenId"]);
                    Debug.Log(value["UserId"]);
                    Debug.Log(value["Nickname"]);
                    Debug.Log(value["AccessSeconds"]);
                    Debug.Log(value["InDate"]);
                }
            }
            else
            {
                Debug.Log("실패");
            }
        });
    }
    #endregion

    #region BlockReason
    public void BlockReason(string blockKey)
    {
        plugin.BlockManager.Reason(blockKey, (status, errorCode, jsonSTring, values) =>
        {
            Debug.Log(jsonSTring);
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["Reason"]);
                    Debug.Log(((string[])value["Services"])[0]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    #region Chat
    public void ChatBlockPlayers()
    {
        plugin.Chat.BlockPlayers((status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["BlockUserId"]);
                    Debug.Log(value["BlockUserName"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void ChatBlockPlayerAdd()
    {
        plugin.Chat.BlockPlayerAdd("TESTUSER", "TESTNAME", (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }

        });
    }

    public void ChatBlockPlayerRemove()
    {
        plugin.Chat.BlockPlayerRemove("TESTUSER", (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void ChatChannels()
    {
        plugin.Chat.Channels((status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["Channel"]);
                    Debug.Log(value["Count"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    #region Event logs
    public void LogWrite()
    {
        var messages = new PlayNANOO.Monitor.LogMessages();
        messages.Add(Configure.PN_LOG_DEBUG, "Message1");
        messages.Add(Configure.PN_LOG_INFO, "Message2");
        messages.Add(Configure.PN_LOG_ERROR, "Message3");

        plugin.LogWrite(new PlayNANOO.Monitor.LogWrite()
        {
            EventCode = "TEST_LOG_20210607001",
            EventMessages = messages
        });
    }
    #endregion

    #region Guild
    public void GuildSearch()
    {
        string tableCode = "Guild_TableCode";

        plugin.Guild.Search(tableCode, PlayNANOO.Guild.SortCondition.RANDOM, PlayNANOO.Guild.SortType.DESC, 10, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["TableCode"]);
                    Debug.Log(value["Uid"]);
                    Debug.Log(value["Name"]);
                    Debug.Log(value["Point"]);
                    Debug.Log(value["MasterUuid"]);
                    Debug.Log(value["MasterNickname"]);
                    Debug.Log(value["Country"]);
                    Debug.Log(value["MemberCount"]);
                    Debug.Log(value["MemberLimit"]);
                    Debug.Log(value["AutoJoin"]);
                    Debug.Log(value["ExtraData"]);
                    Debug.Log(value["InDate"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildCreate()
    {
        string tableCode = "Guild_TableCode";
        string name = "Guild_Name";

        plugin.Guild.Create(tableCode, name, PlayNANOO.Guild.PointType.DELETE, true, 10, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Uid"]);
                Debug.Log(values["Name"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildExists()
    {
        string tableCode = "Guild_TableCode";
        string name = "Guild_Name";

        plugin.Guild.Exists(tableCode, name, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildEdit()
    {
        string tableCode = "Guild_TableCode";
        string uid = "Guild_UniqueId";
        string name = "Guild_Name";

        plugin.Guild.Edit(tableCode, uid, name, PlayNANOO.Guild.PointType.DELETE, false, 100, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildDelete()
    {
        string tableCode = "Guild_TableCode";
        string uid = "Guild_UniqueId";

        plugin.Guild.Delete(tableCode, uid, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildMemberSearch()
    {
        string tableCode = "Guild_TableCode";
        string uid = "Guild_UniqueId";

        plugin.Guild.MemberSearch(tableCode, uid, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["Uuid"]);
                    Debug.Log(value["Nickname"]);
                    Debug.Log(value["Grade"]);
                    Debug.Log(value["Point"]);
                    Debug.Log(value["ExtraData"]);
                    Debug.Log(value["LastLoginDate"]);
                    Debug.Log(value["InDate"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildMemberRequestSearch()
    {
        string tableCode = "Guild_TableCode";
        string uid = "Guild_UniqueId";

        plugin.Guild.MemberRequest(tableCode, uid, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["Uuid"]);
                    Debug.Log(value["Nickname"]);
                    Debug.Log(value["ExtraData"]);
                    Debug.Log(value["LastLoginDate"]);
                    Debug.Log(value["InDate"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildMemberApprove()
    {
        string tableCode = "Guild_TableCode";
        string uid = "Guild_UniqueId";
        string uuid = "UserUniqueId";

        plugin.Guild.MemberApprove(tableCode, uid, uuid, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildMemberBan()
    {
        string tableCode = "Guild_TableCode";
        string uid = "Guild_UniqueId";
        string uuid = "UserUniqueId";

        plugin.Guild.MemberBan(tableCode, uid, uuid, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildMemberReject()
    {
        string tableCode = "Guild_TableCode";
        string uid = "Guild_UniqueId";
        string uuid = "UserUniqueId";

        plugin.Guild.MemberReject(tableCode, uid, uuid, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildMemberChangeGrade()
    {
        string tableCode = "Guild_TableCode";
        string uid = "Guild_UniqueId";
        string uuid = "UserUniqueId";

        plugin.Guild.MemberChangeGrade(tableCode, uid, uuid, 1, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildMemberChangeMaster()
    {
        string tableCode = "Guild_TableCode";
        string uid = "Guild_UniqueId";
        string uuid = "UserUniqueId";

        plugin.Guild.MemberChangeMaster(tableCode, uid, uuid, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildPersonalRequest()
    {
        string tableCode = "Guild_TableCode";
        string uid = "Guild_UniqueId";

        plugin.Guild.PersonalRequest(tableCode, uid, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildPersonalRequestSearch()
    {
        string tableCode = "Guild_TableCode";

        plugin.Guild.PersonalSearch(tableCode, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                foreach (Dictionary<string, object> value in (ArrayList)values["Items"])
                {
                    Debug.Log(value["TableCode"]);
                    Debug.Log(value["Uid"]);
                    Debug.Log(value["Name"]);
                    Debug.Log(value["Country"]);
                    Debug.Log(value["Point"]);
                    Debug.Log(value["MemberCount"]);
                    Debug.Log(value["MemberLimit"]);
                    Debug.Log(value["AutoJoin"]);
                    Debug.Log(value["ExtraData"]);
                    Debug.Log(value["InDate"]);
                    Debug.Log(value["RequestInDate"]);
                }
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildPersonalCancel()
    {
        string tableCode = "Guild_TableCode";
        string uid = "Guild_UniqueId";

        plugin.Guild.PersonalCancel(tableCode, uid, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildPersonalInfo()
    {
        string tableCode = "Guild_TableCode";

        plugin.Guild.Personal(tableCode, (status, error, jsonString, values) =>
        {
            Debug.Log(values["GuildUid"]);
            Debug.Log(values["GuildName"]);
            Debug.Log(values["GuildPoint"]);
            Debug.Log(values["GuildMemberCount"]);
            Debug.Log(values["GuildMemberLimit"]);
            Debug.Log(values["GuildExtraData"]);
            Debug.Log(values["MemberGrade"]);
            Debug.Log(values["MemberPoint"]);
            Debug.Log(values["MemberInDate"]);
        });
    }

    public void GuildPersonalAddPoint()
    {
        string tableCode = "Guild_TableCode";

        plugin.Guild.PersonalPoint(tableCode, 100, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildPersonalWithdraw()
    {
        string tableCode = "Guild_TableCode";

        plugin.Guild.PersonalWithdraw(tableCode, (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    public void GuildPersonalEdit()
    {
        string tableCode = "exithero-guild-455373F5";

        plugin.Guild.PersonalEdit(tableCode, "asdf", (status, error, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                Debug.Log(values["Status"]);
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }

    #endregion

    #region REMOTE CONFIG
    public void RemoteConfig()
    {
        string tableCode = "RemoteConfig_TableCode";

        plugin.RemoteConfig.Init(tableCode, (isSuccess) =>
        {
            if (isSuccess)
            {
                Debug.Log("Success");
                Debug.Log(plugin.RemoteConfig.GetString(tableCode, "VariableKey", string.Empty));
                Debug.Log(plugin.RemoteConfig.GetJson(tableCode, "VariableKey", string.Empty));
                Debug.Log(plugin.RemoteConfig.GetInteger(tableCode, "VariableKey", 0));
                Debug.Log(plugin.RemoteConfig.GetFloat(tableCode, "VariableKey", 0));
                Debug.Log(plugin.RemoteConfig.GetDouble(tableCode, "VariableKey", 0));
                Debug.Log(plugin.RemoteConfig.GetBool(tableCode, "VariableKey", false));
            }
            else
            {
                Debug.Log("Fail");
            }
        });
    }
    #endregion

    public void OnError(ChatErrorModel error)
    {
        Debug.LogError(error.code);
        Debug.LogError(error.message);
        throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(ChatInfoModel chatInfo)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnSubscribed(ChatInfoModel chatInfo)
    {
        throw new System.NotImplementedException();
    }

    public void OnPublicMessage(ChatInfoModel chatInfo, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(ChatInfoModel chatInfo, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnNotifyMessage(ChatInfoModel chatInfo, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerOnline(ChatPlayerModel[] players)
    {
        throw new System.NotImplementedException();
    }
}
