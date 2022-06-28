using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class NameListObject : SerializedMonoBehaviour
{
    // テンプレートの名前のカテゴリ
    public enum eCreateNameCategory : int {
        Engel,          // 天使系
        Arm,            // 武器
        CzechF,         // チェコ女
        CzechM,         // チェコ男
        Demon,          // 悪魔
        Dowarf,         // ドワーフ
        EnglishF,       // イギリス女
        EnglishLand,    // イギリス土地名
        EnglishL,       // イギリス苗字
        EnglishM,       // イギリス男
        FinlandF,
        FinlandL,
        FinlandM,
        FranceF,
        FranceLand,
        FranceL,
        FranceM,
        GermanF,
        GermanLand,
        GermanL,
        GermanM,
        ItaliaF,
        ItaliaLand,
        ItaliaL,
        ItaliaM,
        OrandaF,
        OrandaL,
        OrandaM,
        Ore,
        Russia,
        RussiaF,
        RussiaL,
        RussiaM,
        SpainF,
        SpainL,
        SpainM,
        SwedenF,
        SwedenL,
        SwedenM,
    }


    [SerializeField]
    private TextAsset[] m_nameTemplateList;
    
    // 名前の取得
    public string Get(eCreateNameCategory category) => m_nameTemplateList[(int)category].text;

}
