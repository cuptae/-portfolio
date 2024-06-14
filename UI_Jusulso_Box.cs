using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Jusulso_Box : MonoBehaviour
{
    public int id;
    public string steam_id;
    public int box_type;
    public int stage_id;
    public DateTime? open_start_time;
    public bool is_open;
    public DateTime created_at;
    public DateTime updated_at;

    public bool openable = false;

    public void SetData(OwnBoxData data)
    {
        id = data.id;
        steam_id = data.steam_id;
        box_type = data.box_type;
        stage_id = data.stage_id;
        open_start_time = data.open_start_time;
        is_open = data.is_open;
        created_at = data.created_at;
        updated_at = data.updated_at;
    }

}
