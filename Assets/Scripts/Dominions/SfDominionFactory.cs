using System.Collections;
using System.Collections.Generic;
using TGS;
using UnityEngine;

namespace sfproj
{

    /// <summary>
    /// �̈搶���H��  ���
    /// 2022/06/08
    ///     �����ނ����݂���킯�ł͂Ȃ��̂ŕ�����K�v�͂Ȃ�����
    ///     �ꉞ�e���v���[�g�����Ă���
    /// </summary>
    public abstract class SfDominionFactoryBase
    {
        public SfDominion Create(uint uniqueId, int territoryIndex)
        {
            var record = CreateRecord();

            // ���j�[�N ID �̐ݒ�
            record.Id = uniqueId;

            // �̈於�̐���
            record.Name = CreateName();

            // �e���g���C���f�b�N�X�̐ݒ�
            record.TerritoryIndex = territoryIndex;

            // �C�ɗאڂ��Ă��邩�ǂ����̃t���O��ݒ�
            record.m_neighboursOceanFlag = CheckAdjastingOceanTerrain(territoryIndex);

            return record;
        }

        // �̈惌�R�[�h�̍쐬
        protected abstract SfDominion CreateRecord();

        // �̈於�̍쐬
        protected abstract string CreateName();

        /// <summary>
        /// �C�ɗאڂ��Ă��邩�`�F�b�N
        /// �אڂ��Ă���e���g���ɂP�ł���\��������ΊC
        /// </summary>
        /// <returns>true...�C�ɗאڂ��Ă���</returns>
        private bool CheckAdjastingOceanTerrain(int territoryIndex)
        {
            return (TerrainGridSystem.instance.territories[territoryIndex].neighbourVisible == false);
        }
    }

    /// <summary>
    /// �̈惌�R�[�h�����H��
    /// </summary>
    public abstract class SfDominionCreateFactory : SfDominionFactoryBase
    {
        protected override SfDominion CreateRecord()
        {
            return new SfDominion();
        }
    }

    /// <summary>
    /// �̈搶���H��
    /// </summary>
    public class SfDominionFactory : SfDominionCreateFactory
    {
        // ���̕�����n��ɂ���ĕω�������H
        protected override string CreateName()
        {
            return "test";
        }


    }

    /// <summary>
    /// �̈�H��Ǘ�
    /// </summary>
    public class SfDominionFactoryManager : Singleton<SfDominionFactoryManager>
    {

        /// <summary>
        /// �̈惌�R�[�h�̍쐬
        /// </summary>
        /// <param name="territoryIndex"></param>
        /// <returns></returns>
        public SfDominion Create(int territoryIndex)
        {

            // �ЂƂ܂� SfDominionFactory �����Ȃ�
            var factory = new SfDominionFactory();

            // ���j�[�N ID �̍쐬
            uint uniqueId = SfConstant.CreateUniqueId(ref SfDominionTableManager.Instance.m_uniqueIdList);

            // �̈惌�R�[�h���쐬
            var record = factory.Create(uniqueId, territoryIndex);

            return record;
        }
    }

}