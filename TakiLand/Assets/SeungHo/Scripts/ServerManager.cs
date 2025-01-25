using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayNANOO;

public class ServerManager : MonoBehaviour
{
    public static ServerManager instance { get; private set; }

    private static string UserUniqueID
    {
        get;
        set;
    }
    
    private Plugin plugin { get; set; }
    public Plugin GetPlugin() => plugin;
    
    public void LeaderboardPlayerRange(System.Action<ArrayList> callBack)
    {
        plugin.LeaderboardManagerV20240301.Range("takiland-leaderboard-2B3CF595-9E3E7FD1", 0, 100, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                callBack.Invoke((ArrayList)values["Items"]);
            }
            else
            {
                callBack.Invoke(null);
                Debug.LogError($"Get Ranking Fail : {errorCode}");
            }
        });
    }

    public void LeaderboardPlayerRecord(long score, string extraData, string nickName,
        System.Action<bool> callBack)
    {
        //string recordId = ServerManager.UserUniqueID; // 고유한 값으로 지정

        string recordId = nickName;
        
        plugin.LeaderboardManagerV20240301.Record("takiland-leaderboard-2B3CF595-9E3E7FD1", recordId,
            score, extraData, (status, errorCode, jsonString, values) =>
            {
                if (status.Equals(Configure.PN_API_STATE_SUCCESS) ||
                    errorCode.Equals("27000")) // 더 낮은 값으로 시도하면 27000 뜨는듯
                {
                    callBack.Invoke(true);
                }
                else
                {
                    callBack.Invoke(false);
                }
            });
    }
    
    private void Start()
    {
        AccountGuestSignIn(b =>
        {
            if(b)
                Debug.Log("Login!");
        });
    }

    public void AccountGuestSignIn(System.Action<bool> success)
    {
        // Guest SignIn
        plugin.AccountGuestSignIn((status, errorCode, jsonString, values) =>
        {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                UserUniqueID = values["uuid"].ToString();
                success.Invoke(true);
            }
            else
            {
            }
        });
    }
    
    public void AccountNicknamePut(string nickname, System.Action<bool> callBack)
    {
        plugin.AccountNickanmePut(nickname, true, (status, errorCode, jsonString, values) => {
            if (status.Equals(Configure.PN_API_STATE_SUCCESS))
            {
                callBack.Invoke(true);
            }
            else
            {
                callBack.Invoke(false);
                Debug.Log(errorCode);
            }
        });
    }
    
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            plugin = Plugin.GetInstance();
            DontDestroyOnLoad(plugin.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
}
