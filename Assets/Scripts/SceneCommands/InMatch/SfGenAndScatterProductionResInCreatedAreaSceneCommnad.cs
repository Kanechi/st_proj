using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sfproj
{
    /// <summary>
    /// �쐬���ꂽ�n��ɐ��Y�����𐶐����U�z����
    /// </summary>
    public class SfGenAndScatterProductionResInCreatedAreaSceneCommnad : SfInMatchSceneCommandBase
    {
        public override void Update(SfInMatchGameSceneController scene)
        {
            var areaList = SfAreaTableManager.Instance.Table.RecordList;

            foreach (var area in areaList)
            {
                if (area.AreaGroupType != eAreaGroupType.Normal)
                    continue;

                // ���Y�����������������邩������
                int count = UnityEngine.Random.Range(1, SfConfigController.Instance.MaxAreaProductionResourceItemCt);

                var builder = new SfProductionResourceItemBuilder(area);
                var director = new SfItemGenDirector<SfProductionResourceItem>(builder);

                for (int i = 0; i < count; ++i)
                {
                    director.Construct();

                    var item = director.GetResult();

                    // �n��̐��Y���� ID ���X�g�ɓo�^
                    area.ProductionResourceItemIdList.Add(item.Id);
                }
            }

            var list = SfProductionResourceItemTableManager.Instance.Table.RecordList;

            scene.Invoker.ChangeSceneState(eSceneState.CreateKingdomInWorld);
        }
    }
}