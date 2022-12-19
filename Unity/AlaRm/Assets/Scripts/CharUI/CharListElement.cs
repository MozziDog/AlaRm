using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharListElement : MonoBehaviour
{
    [SerializeField] int charCode = 00;
    [ReadOnly] bool isOwned = true;

    [SerializeField] Image thumbnail = null;
    [SerializeField] Image statusIcon = null;

    [Space]

    [SerializeField] Sprite icon_nowUsing;
    [SerializeField] Sprite icon_owned;
    [SerializeField] Sprite icon_notOwned;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckOwned()
    {
        bool isOwned = SaveManager.instance.saveData.characterOwn[charCode];
        this.isOwned = isOwned;
        if (isOwned)
            statusIcon.sprite = icon_owned;
        else
            statusIcon.sprite = icon_notOwned;
        //if(SaveManager.instance.saveData.characterCode == this.charCode)
        //    statusIcon.sprite = icon_nowUsing;
    }

    public void OnClickElement()
    {
        if(SaveManager.instance.saveData.characterCode == this.charCode)
        {
            Debug.Log("이미 선택중인 캐릭터임");
        }
        else if(isOwned)
        {
            WebRequestManager.instance.callbackSuccess += OnCharacterChangeSuccess;
            WebRequestManager.instance.callbackFailure += OnCharacterChangeFailure;
            StartCoroutine(WebRequestManager.instance.API_change_character(charCode));
        }
        else
        {
            WebRequestManager.instance.callbackSuccess += OnCharacterBuySuccess;
            WebRequestManager.instance.callbackFailure += OnCharacterBuyFailure;
            StartCoroutine(WebRequestManager.instance.API_character(charCode));
        }
    }

    public void OnCharacterChangeSuccess()
    {
        SaveManager.instance.saveData.characterCode = this.charCode;
        SaveManager.instance.Save();
        GameManager.instance.SpawnCharacter();
        AndroidPluginLoader.Instance.ShowToast("캐릭터 교체");
    }

    public void OnCharacterChangeFailure()
    {
        AndroidPluginLoader.Instance.ShowToast("캐릭터 교체 실패");
    }

    public void OnCharacterBuySuccess()
    {
        GetComponentInParent<CharListUI>().Refresh();
        SaveManager.instance.saveData.characterCode = this.charCode;
        GameManager.instance.SpawnCharacter();
        this.statusIcon.sprite = icon_owned;
    }

    public void OnCharacterBuyFailure()
    {
        AndroidPluginLoader.Instance.ShowToast("캐릭터 구매 실패");
    }
}
