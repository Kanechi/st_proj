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
    /// ���O�̐���
    /// </summary>
    /// <param name="createCt">������</param>
    /// <param name="nameList">�e���v���[�g�̖��O</param>
    public void Construct(int createCt, string nameList) {

        var strList = m_builder.LoadName(nameList);

        m_builder.Create(createCt, strList);
    }
}
