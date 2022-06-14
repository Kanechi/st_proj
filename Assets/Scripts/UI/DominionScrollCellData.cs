using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj
{
    /// <summary>
    /// �\�����Ă���̈�X�N���[���Z��(�n��Z��)�̃f�[�^
    /// </summary>
    public class DominionScrollCellData
    {
        // �n�惌�R�[�h
        public SfAreaRecord m_areaRecord = null;

        // �n��摜�X�v���C�g
        public Sprite m_areaImageSprite = null;

        public DominionScrollCellData(uint areaId) {

            m_areaRecord = SfAreaRecordTableManager.Instance.Get(areaId);

            // �n��摜�X�v���C�g�̍쐬
        }
    }
}