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
    }
}