using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class NameListObject : SerializedMonoBehaviour
{
    // �e���v���[�g�̖��O�̃J�e�S��
    public enum eCreateNameCategory : int {
        Engel,          // �V�g�n
        Arm,            // ����
        CzechF,         // �`�F�R��
        CzechM,         // �`�F�R�j
        Demon,          // ����
        Dowarf,         // �h���[�t
        EnglishF,       // �C�M���X��
        EnglishLand,    // �C�M���X�y�n��
        EnglishL,       // �C�M���X�c��
        EnglishM,       // �C�M���X�j
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
    
    // ���O�̎擾
    public string Get(eCreateNameCategory category) => m_nameTemplateList[(int)category].text;

}
