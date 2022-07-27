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
    public class SfAreaWithinDominionScrollCellData
    {
        // �n�惌�R�[�h
        public SfArea m_areaRecord = null;

        // �n��w�i�摜�X�v���C�g
        public Sprite m_areaBgImageSprite = null;

        // �n��摜�X�v���C�g
        public Sprite m_areaImageSprite = null;

        // true...�J��ς�  false...���J��
        public bool IsDevelopment = false;


        // �f�[�^���Q�Ƃ��Ă���Z��
        public SfAreaWithinDominionScrollCell Cell { get; set; } = null;
        
        // true...�I��(�t�H�[�J�X)����Ă���
        public bool IsSelected { get; set; } = false;

        public SfAreaWithinDominionScrollCellData(SfArea record) {

            m_areaRecord = record;

            // �n��w�i�摜�X�v���C�g
            m_areaBgImageSprite = AssetManager.Instance.Get<Sprite>(m_areaRecord.AreaGroupType.ToEnumString());

            // �n��摜�X�v���C�g�̍쐬
            m_areaImageSprite = AssetManager.Instance.Get<Sprite>(m_areaRecord.AreaType.ToEnumString());

            // �J��ς݂��ǂ����̃`�F�b�N
            if (record.AreaDevelopmentState == eAreaDevelopmentState.Completed)
                IsDevelopment = true;
        }
    }
}