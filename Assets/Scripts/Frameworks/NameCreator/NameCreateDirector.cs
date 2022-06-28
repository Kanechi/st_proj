using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameCreateDirector
{
    private NameCreateBuilderBase m_builder = null;

    public NameCreateDirector(NameCreateBuilderBase builder) => m_builder = builder;

    public List<string> Result => m_builder.Result;

    /// <summary>
    /// 名前の生成
    /// </summary>
    /// <param name="createCt">生成数</param>
    /// <param name="nameList">テンプレートの名前</param>
    public void Construct(int createCt, string nameList) {

        var strList = m_builder.LoadName(nameList);

        m_builder.Create(createCt, strList);
    }
}
