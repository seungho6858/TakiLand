using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Ranking : MonoBehaviour
{
    [SerializeField]
    private UiRankElement uiElement;

    private List<UiRankElement> listElements;

    private void OnEnable()
    {
        listElements.ForEach(x => x.gameObject.SetActive(false));
        
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
                    extra.stage);
                listElements[idx].gameObject.SetActive(true);
                
                idx++;
            }
        });
    }

    private void Awake()
    {
        listElements = new List<UiRankElement>() { uiElement };
    }
}
