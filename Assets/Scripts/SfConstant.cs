using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace sfproj {

    /// <summary>
    /// 大陸タイプ
    /// </summary>
    public enum eLandType : int {
        OneLand,
        TwoLand,
        FourLand,
    };

    /// <summary>
    /// Spectral Force Constant
    /// </summary>
    public class SfConstant {

        /// <summary>
        /// ユニーク ID の作成
        /// </summary>
        /// <param name="uniqueIdList"></param>
        /// <returns></returns>
        static public uint CreateUniqueId(ref HashSet<uint> uniqueIdList)
        {
            uint id = 0;

            if (uniqueIdList.Count > 0)
            {
                id = uniqueIdList.Max();
                id++;
            }
            else
            {
                id = 1;
            }

            uniqueIdList.Add(id);

            return id;
        }

        static public int WeightedPick(int[] list) {

            int totalWeight = 0;
            int pick = 0;

            for (int i = 0; i < list.Length; ++i)
            {
                totalWeight += list[i];
            }

            int rnd = (int)(UnityEngine.Random.value * totalWeight);

            for (int i = 0; i < list.Length; ++i)
            {
                if (rnd < list[i])
                {
                    pick = i;
                    break;
                }
                rnd -= list[i];
            }

            return pick;
        }
    }
}