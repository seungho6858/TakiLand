using System.Collections;
using System.Collections.Generic;
using Mib;
using Mib.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[PopupPath("TutoPage")]
public class TutorialPopup : PopupBase
{
    [SerializeField]
    private Button _button;
    
    protected override void OnAwake()
    {
        _button.onClick.AddListener(ChangeScene);
    }

    public override void OnOpen()
    {
    }

    protected override void OnClose()
    {
    }

    private void ChangeScene()
    {
        SoundManager.PlaySound("Menu_Select_00");
        SceneLoader.ChangeScene(Constant.BattleScene).Forget();
    }
}
