using System;
using System.Collections;
using System.Collections.Generic;
using Mib;
using Mib.Data;
using Mib.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

[PopupPath("Popup_Ranking")]
public class Popup_Ranking : PopupBase
{
    [SerializeField] private UiRankElement uiElement;
    [SerializeField] private UiRankElement mine;

    private List<UiRankElement> listElements;

    private void Awake()
    {
        listElements = new List<UiRankElement>() { uiElement };
    }

    protected override void OnAwake()
    {
        
    }

    public override void OnOpen()
    {
        listElements.ForEach(x => x.gameObject.SetActive(false));
        mine.gameObject.SetActive(false);
        
        BattleManager.GetRankList(list =>
        {
            int idx = 0;
            foreach (Dictionary<string, object> value in list)
            {
                Debug.Log(value["Rank"]);
                Debug.Log(value["RotationCount"]);
                Debug.Log(value["RecordId"]);
                Debug.Log(value["Score"]);
                Debug.Log(value["ExtraData"]);

                var extra = JsonUtility.FromJson<BattleManager.Rank>(value["ExtraData"].ToString());
                
                if(listElements.Count <= idx)
                    listElements.Add(Instantiate(uiElement, uiElement.transform.parent));
                listElements[idx].SetUi((int) value["Rank"],  (int)((System.Double) value["Score"]), 
                    extra.stage, extra.nick);
                listElements[idx].gameObject.SetActive(true);
                
                idx++;

                if (extra.nick == BattleManager.nick)
                {
                    mine.SetUi((int) value["Rank"],  (int)((System.Double) value["Score"]), 
                        extra.stage, extra.nick);
                    mine.gameObject.SetActive(true);
                }
            }
        });
    }

    protected override void OnClose()
    {
            
        SceneLoader.ChangeScene(Constant.TitleScene).Forget();
    }
}
