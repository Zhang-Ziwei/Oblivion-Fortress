using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebuffLog : MonoBehaviour
{

    public TextMeshProUGUI TimerText;

    public TextMeshProUGUI DescriptionText;

    public Image BuffIcon;

    public Image TimerFill;

    public Buff myBuff;
    public void Init(Buff enemyBuff) {
        myBuff = enemyBuff;

        DescriptionText.text = enemyBuff.BuffName;

        BuffIcon.sprite = enemyBuff.buffIcon;

        TimerText.text = enemyBuff.duration.ToString();

        TimerFill.fillAmount = 1;

        StartCoroutine(TimerCoroutine(enemyBuff.duration));
    }

    private IEnumerator TimerCoroutine(float duration) {
        float timer = duration;
        while (timer > 0) {
            timer -= Time.deltaTime;
            TimerText.text = timer.ToString("F0");
            TimerFill.fillAmount = timer / duration;
            yield return null;
        }
        DebuffLogList.Instance.RemoveBuffItem(this);
    }
}
