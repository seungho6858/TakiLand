using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiRankElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI stage;

    public void SetUi(int rank, long gold, int stage)
    {
        this.rank.text = $"{rank}";
        this.gold.text = $"{gold}";
        this.stage.text = $"{stage}";
    }
}
