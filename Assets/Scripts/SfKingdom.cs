using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// ����
    /// �ۑ����
    /// �Q�[���J�n���Ɉ�x�����쐬
    /// </summary>
    [Serializable]
    public class SfKingdomRecord
    {
        [Serializable]
        public class SfKingdomDetail
        {
            // ���� ID
            public uint m_id = 0;
            // ������
            public string m_name = "";
            // �����̐F
            public Color m_color = Color.white;

            // �̈� ID ���X�g
            public List<uint> m_sfDominionIdList = new List<uint>();
        }
    }

    /// <summary>
    /// ����
    /// �X�N���[���r���[���^�b�v������ǂ����ɊȈՃE�B���h�E�Ƃ��ĕ\��
    /// </summary>
    public class SfKingdom : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}