using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using static UIStatus;
using UnityEditor.Tilemaps;
using Steamworks;

public class UI_JusulsoPossesionSlot : UI_Base
{
    public bool hasBox;
    public UI_Jusulso_Box box;
    public Sprite itemImage;
    public Image slotImage;
    private Button slotButton;
    public List<UI_JusulsoProgressBox> progressSlot;
    private UI_Jusulso jusulso;

    public Text boxId;
    // Start is called before the first frame update
    void Start()
    {
        jusulso = FindObjectOfType<UI_Jusulso>();
        progressSlot = jusulso.progressList;
        for (int i = 0; i < progressSlot.Count; i++)
        {
            progressSlot[i] = jusulso.progressList[i];
        }

        slotButton = GetComponentInChildren<Button>();
        slotButton.onClick.AddListener(ClickSlot);

    }
    public void ClickSlot()
    {
        if (box == null)
        {
            DebugManager.Instance.PrintDebug("슬롯에 아이템 프리팹이 없습니다");
        }
        else
        {
            RequestBoxOpenStart();
        }
    }
    public void SetItem(UI_Jusulso_Box newbox)
    {
        if (newbox != null)
        {
            box = newbox;
            box.transform.SetParent(transform);
            slotImage.sprite = itemImage;
            hasBox = true;
            boxId.text = box.id.ToString();
            SetImageAlpha();
        }
        else if (newbox == null)
        {
            box = null;
            hasBox = false;
            SetImageAlpha();
            boxId.text = " ";
        }
    }
    public void MoveItemToProgressBox()
    {
        for (int i = 0; i < progressSlot.Count; i++)
        {
            if (!progressSlot[i].hasBox)
            {
                progressSlot[i].SetItem(box);
                box.transform.SetParent(progressSlot[i].transform);
                SetItem(null);
                break;
            }
            else
            {
                DebugManager.Instance.PrintDebug("진행 슬롯이 다 차있습니다");
            }
        }
    }
    //public void test()
    //{
    //    debug.logerror("box.id:" + box.id
    //        + " box.steam_id" + box.steam_id
    //        + " box.box_type" + box.box_type
    //        + " box.stage_id" + box.stage_id
    //        + " box.box_open_start_time" + box.open_start_time
    //        + " box.is_open" + box.is_open
    //        + " box.created_at" + box.created_at
    //        + " box.updated_at" + box.updated_at);
    //}
    public void SetImageAlpha()
    {
        if (!hasBox)
        {
            slotImage.color = new Color(0, 0, 0, 0);
        }
        else
        {
            slotImage.color = new Color(1, 1, 1, 1);
        }
    }

    async void RequestBoxOpenStart()
    {
        for (int i = 0; i < progressSlot.Count; i++)
        {
            if (!progressSlot[i].hasBox)
            {
                BoxOpenStart boxOpen = await APIManager.Instance.BoxOpenStart(box.id);
                if (boxOpen.statusCode == 200)
                {
                    box.open_start_time = boxOpen.data.open_start_time;
                    MoveItemToProgressBox();
                    jusulso.BoxSort();
                    DebugManager.Instance.PrintDebug("박스 열기 시작!");
                    break;
                }
            }
            else
            {
                DebugManager.Instance.PrintDebug("슬롯이 차있습니다");
            }
        }

    }

}
