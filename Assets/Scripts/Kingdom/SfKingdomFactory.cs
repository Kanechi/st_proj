using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{

    /// <summary>
    /// ���������H����
    /// </summary>
    public abstract class SfKingdomFactoryBase
    {
        public SfKingdom Create(int uniqueId)
        {
            var record = CreateRecord();

            // ���j�[�N ID �ݒ�
            record.Id = uniqueId;

            // �������ݒ�
            CreateName(record);

            // �����J���[�ݒ�
            SettingColor(record);

            // �����̍����ǂ����̃t���O�̐ݒ�
            SettingSelfFlag(record);

            return record;
        }

        // ���R�[�h����
        protected abstract SfKingdom CreateRecord();

        // ����������
        protected abstract void CreateName(SfKingdom record);
        // �����J���[�̐ݒ�
        protected abstract void SettingColor(SfKingdom record);

        protected abstract void SettingSelfFlag(SfKingdom record);
    }

    /// <summary>
    /// ���������H��
    /// </summary>
    public abstract class SfKingdomFactory : SfKingdomFactoryBase
    {
        protected override SfKingdom CreateRecord()
        {
            return new SfKingdom();
        }
    }

    /// <summary>
    /// �����̐���
    /// �����̐����̓Q�[���J�n�O�ɐݒ肵�����ڂ�ݒ�
    /// </summary>
    public class SfSelfKingdomFactory : SfKingdomFactory
    {
        // ����������
        protected override void CreateName(SfKingdom record)
        {
            record.Name = SfConfigController.Instance.KingdomName;
        }

        // �����J���[�̐ݒ�
        protected override void SettingColor(SfKingdom record)
        {
            record.Color = SfConfigController.Instance.KingdomColor;
        }

        // �����̍����ǂ����̃t���O
        protected override void SettingSelfFlag(SfKingdom record)
        {
            record.SelfFlag = true;
        }
    }


    // ���̑��̍��̃����_������
    // ������x�̍������O�ɍ쐬���Ă����Ċ���U�邾����
    // �Ƃǂ߂邩�A���ׂĂO����쐬���邩�E�E�E
    public class SfOtherKingdomFactory : SfKingdomFactory
    {
        // ����������
        protected override void CreateName(SfKingdom record)
        {
            // �����_������
            record.Name = "test";
        }

        // �����J���[�̐ݒ�
        protected override void SettingColor(SfKingdom record)
        {
            record.Color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 0.6f);
        }

        // �����̍����ǂ����̃t���O
        protected override void SettingSelfFlag(SfKingdom record)
        {
            record.SelfFlag = false;
        }
    }

    /// <summary>
    /// �����H��Ǘ�
    /// </summary>
    public class SfKingdomFactoryManager : Singleton<SfKingdomFactoryManager>
    {
        public SfKingdom Create(bool selfKingdom)
        {
            SfKingdomFactoryBase factory = null;

            if (selfKingdom)
            {
                factory = new SfSelfKingdomFactory();
            }
            else
            {
                factory = new SfOtherKingdomFactory();
            }

            // �������R�[�h�̐���
            var record = factory.Create((int)SfConstant.CreateUniqueId(ref SfKingdomTableManager.Instance.m_uniqueIdList));

            return record;
        }
    }
}