using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UI_JusulsoProgressBox : UI_Base
{
    public bool hasBox = false;
    public UI_Jusulso_Box box;
    public Image slotImage;
    public Sprite itemImage;
    public TextMeshProUGUI time;
    public TextMeshProUGUI itemId;
    private Button slotButton;
    private UI_Jusulso jusulso;
    private TimeSpan openTime;
    private TimeSpan duringTime;
    // Start is called before the first frame update
    void Start()
    {
        jusulso = FindObjectOfType<UI_Jusulso>();
        slotButton = GetComponentInChildren<Button>();
        slotButton.onClick.AddListener(BoxOpen);
        if (box != null)
        {
            slotImage.sprite = itemImage;
            hasBox = true;
            time.gameObject.SetActive(true);
        }
        else
        {
            hasBox = false;
            time.gameObject.SetActive(false);
        }
        SetImageAlpha();
    }
    public void SetItem(UI_Jusulso_Box newBox)
    {
        box = newBox;
        TimeSet();
        box.transform.SetParent(transform);
        slotImage.sprite = itemImage;
        hasBox = true;
        time.gameObject.SetActive(true);
        SetImageAlpha();
        hasBox = true;
        StartCoroutine(UpdateTimer());
        itemId.text = box.id.ToString();
        if (hasBox)
        {
            DebugManager.Instance.PrintDebug("박스가 이미 차 있습니다.");
        }
    }
    private void SetImageAlpha()
    {
        if (box == null)
        {
            itemId.text = " ";
            time.gameObject.SetActive(false);
            slotImage.color = new Color(0, 0, 0, 0);
        }
        else
        {
            slotImage.sprite = itemImage;
            hasBox = true;
            time.gameObject.SetActive(true);
            slotImage.color = new Color(1, 1, 1, 1);
        }
    }

    public void BoxOpen()
    {
        if (box.openable)
        {
            RequestBoxOpen();
        }
        else if (!box.openable)
        {
            UI_Jusulso_Boxopen_Popup boxopen_Popup = Util.UILoad<UI_Jusulso_Boxopen_Popup>(Define.UiPrefabsPath + "/UI_Jusulso_Boxopen_Popup");
            GameObject boxopen_popup_gamObject = Instantiate(boxopen_Popup.gameObject);
            boxopen_popup_gamObject.GetComponent<UI_Jusulso_Boxopen_Popup>().jusulsoProgressBox = this;
            boxopen_popup_gamObject.SetActive(true);
        }
    }

    public async void RequestBoxOpen()
    {
        if (box != null)
        {
            BoxOpen boxOpen = await APIManager.Instance.BoxOpen(box.id);
            if (boxOpen.data.reward1 != null)
            {
                BoxReward(boxOpen.data.reward1.item);
            }
            if (boxOpen.data.reward2 != null)
            {
                BoxReward(boxOpen.data.reward2.item);
            }
            if (boxOpen.data.reward3 != null)
            {
                BoxReward(boxOpen.data.reward3.item);
            }
            if (boxOpen.data.reward4 != null)
            {
                BoxReward(boxOpen.data.reward4.item);
            }
            box = null;
            hasBox = false;
            SetImageAlpha();
            jusulso.RefreshBox();
        }
    }
    public void TimeSet()
    {
        if (box != null)
        {
            if (box.box_type == 1)
            {
                DebugManager.Instance.PrintDebug("박스타입 1");
                openTime = new TimeSpan(0, 1, 0);
            }
            else if (box.box_type == 2)
            {
                DebugManager.Instance.PrintDebug("박스타입 2");
                openTime = new TimeSpan(0, 0, 0);
            }
        }
    }
    public IEnumerator UpdateTimer()
    {
        TimeSet();
        if (box.open_start_time != null)
        {
            TimeSpan timeDifference = jusulso.currentTime - (DateTime)box.open_start_time;
            if (timeDifference.TotalSeconds <= 3)
            {
                timeDifference = new TimeSpan(0, 0, 0);
            }
            duringTime = openTime - timeDifference;
            DebugManager.Instance.PrintDebug(jusulso.currentTime + "-" + box.open_start_time + "=" + timeDifference);
            DebugManager.Instance.PrintDebug(openTime + "-" + timeDifference + "=" + duringTime);
        }

        int h = duringTime.Hours;
        int m = duringTime.Minutes;
        int s = duringTime.Seconds;
        while (true)
        {
            s--;
            if (s < 0)
            {
                m--;
                if (m >= 0)
                {
                    s = 59;
                }
            }
            if (m < 0)
            {
                h--;
                if (h >= 0)
                {
                    m = 59;
                }
            }
            time.text = string.Format("남은시간 {0:00}:{1:00}:{2:00}", h, m, s);
            if (h + m + s <= 0)
            {
                box.openable = true;
                time.text = "상자 열기";
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    public void BoxReward(string item)
    {
        UI_JusulsoReward reward = Util.UILoad<UI_JusulsoReward>(Define.UiPrefabsPath + "/UI_JusulsoReward");
        GameObject rewardGameObject = Instantiate(reward.gameObject);
        rewardGameObject.GetComponent<UI_JusulsoReward>().SetItemImage(item);
        rewardGameObject.SetActive(true);
        DebugManager.Instance.PrintDebug("박스 보상");
    }
}
