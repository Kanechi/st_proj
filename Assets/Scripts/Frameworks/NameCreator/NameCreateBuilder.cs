#define DEBUG_LOG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

/// <summary>
/// ���O�����r���_�[���
/// </summary>
public abstract class NameCreateBuilderBase
{
	protected List<string> m_result = new List<string>();

	public List<string> Result => m_result;

    /// <summary>
    /// Text ��ǂݍ���ŉ��s���Ƃɕ������Ĕz��ɑ��
    /// </summary>
    public List<string> LoadName(Text nameList)
    {
        return nameList.text.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
    }

    /// <summary>
    /// string ��ǂݍ���ŉ��s���Ƃɕ������Ĕz��ɑ��
    /// </summary>
    public List<string> LoadName(string nameList)
    {
        return nameList.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
    }

    public abstract void Create(int createCt, List<string> nameList);
}

public class NameCreateNatural : NameCreateBuilderBase
{
	List<string> strBody = null;
	List<string> strFooter = new List<string>();
	string strRet = "";
	string strRet2 = "";
	string strRet3 = "";
	int lngRet = 0;
	System.Random r = new System.Random();

	private void Generate() {

		int j = 0;

		//�����_���Ŏ擾�����ʒu����S�������āA�����܂Ŏ擾����������̍Ō�̕����ƍ��v���镶����ƍ�������B
		do
		{
			if (strRet == "")
			{
				lngRet = r.Next(strFooter.Count);
				strRet = strFooter[lngRet];
			}
			else
			{
				lngRet = r.Next(strBody.Count);

				for (j = lngRet; j < strBody.Count; j++)
				{
					if (strBody[j].Substring(strBody[j].Length - 1, 1) == strRet.Substring(0, 1))
					{
						strRet = strBody[j] + strRet.Substring(1, strRet.Length - 1);
						return;
					}
				}

				for (j = 0; j < lngRet; j++)
				{
					if (strBody[j].Substring(strBody[j].Length - 1, 1) == strRet.Substring(0, 1))
					{
						strRet = strBody[j] + strRet.Substring(1, strRet.Length - 1);
						return;
					}
				}
				return;
			}

			//8�����𒴂����炻���ŏI������B
		} while (strRet.Length <= 9);
	}

	// �֎~����
	private string[] m_fc = new string[] { "�@", "�B", "�D", "�F", "�H", "��", "�b", "��", "��", "��", "�[", "��" };

	public override void Create(int createCt, List<string> nameList)
    {
		strBody = nameList.ToList();

		//3�������Ƃ̃u���b�N���X�g�����B
		strRet = "";
		strRet2 = "";
		for (int i = 0; i < strBody.Count; i++)
		{
			string stringBody = strBody[i];

			if (stringBody.Length <= 2)
			{
				strRet2 = strRet2 + " " + stringBody;
			}
			else
			{

				for (int j = 1; j <= stringBody.Length - 2; j++)
				{

					strRet3 = stringBody.Substring(j - 1, 3);// Mid(stringBody, j, 3);

					//�ŏ��̕����Ƃ��ė��Ă͂����Ȃ����́B
					var firstChar = strRet3.Substring(0, 1);//Mid(strRet3, 1, 1);
					var lastChar = strRet3.Substring(strRet3.Length - 1, 1);//Mid(strRet3, strRet3.Length, 1);

					if ("�@�B�D�F�H���b�������[��".IndexOf(firstChar) != -1)
					{
						
					}
					//�Ō�̕����Ƃ��ė��Ă͂����Ȃ����́B
					else if ("�b".IndexOf(lastChar) != -1)
					{
						//�K��3�������Ɓu�W���b�N�v�̂悤�Ɏ��Ȃ����̂��o�Ă���̂ł��̏ꍇ��O�I��4�����ŏ����B
						strRet3 = stringBody.Substring(j - 1, 4);//Mid(stringBody, j, 4);

						if (j == (stringBody.Length - 3))
						{
							strRet2 = strRet2 + " " + strRet3;
						}
						else
						{
							strRet = strRet + " " + strRet3;
						}

					}
					else if (j == (stringBody.Length - 2))
					{
						strRet2 = strRet2 + " " + strRet3;

					}
					else
					{
						strRet = strRet + " " + strRet3;
					}
				}
			}
		}

		strBody = new List<string>(strRet.Split(' '));
		strFooter = new List<string>(strRet2.Split(' '));
		strBody.Remove("");
		strFooter.Remove("");

		var result = new List<string>();

		for (int i = 1; i <= createCt; i++)
		{
			strRet = "";

			Generate();



			

			bool finalFlag = true;
			while (finalFlag)
			{
				// �ŏI�`�F�b�N
				var firstChar = strRet.Substring(0, 1);

				bool isHit = false;
				foreach (var c in m_fc) {
					if (c == firstChar) {
						isHit = true;
						break;
					}
				}

				// �ꕶ���ڂɋ֎~�����������Ă�����폜
				if (isHit)
				{
					strRet = strRet.Substring(1, strRet.Length - 1);
				}
				else
				{
					finalFlag = false;
				}
			}

			result.Add(strRet);
		}

#if DEBUG_LOG
		for (int i = 0; i < createCt; ++i) {
			Debug.Log(result[i]);
		}
#endif

		m_result.AddRange(result);
	}
}

/// <summary>
/// �ϊ������������̂Ō������񂪂قڂ킩��Ȃ����A�����Ӗ����Ă���̂���������Â炢�̂�
/// �{���ɓK���ɖ��O��t���������ɗ��p
/// </summary>
public class NameCreateCustom : NameCreateBuilderBase
{
	private List<string> m_charaList = new List<string>();

	/// <summary>
	/// �����_�������J�n
	/// </summary>
	public override void Create(int createCt, List<string> nameList)
	{

		for (int i = 0; i < nameList.Count; ++i)
		{

			var name = nameList[i];


			if (name.Length <= 3)
			{
				LessThanThreeCharaSplit(name);
			}
			else if (name.Length == 4)
			{
				FourCharaSplit(name);
			}
			else if (name.Length == 5)
			{
				FiveCharaSplit(name);
			}
		}

#if false
		foreach(var chara in charaList_) {
			Console.WriteLine(chara);
		}
#endif

#if false
		// ���O���̐ݒ� (3 �������� 8 ����)
		Random rnd = new System.Random(); 
		int nameCount = rnd.Next(3, 8);
		Console.WriteLine("name count : {0}", nameCount);
#endif

		//================================
		// ���O�̍쐬
		//================================
		var customNameList = new List<string>();

		var rnd = new System.Random(System.Environment.TickCount);


		for (int i = 0; i < createCt; i++)
		{
			int nameCount = rnd.Next(3, 8);

			// true...���O����
			var customName = "";
			bool comp = false;
			while (comp == false)
			{

				int index = rnd.Next(m_charaList.Count);
				customName += m_charaList[index];
				if (customName.Length >= nameCount)
					comp = true;
			}

			customNameList.Add(customName);
		}





		// ���X�g�ɗ��Ă͂����Ȃ��������ǂ���
		for (int j = 0; j < customNameList.Count; ++j)
		{

			var customName = customNameList[j];

			if ("�b".IndexOf(customName[customName.Length - 1]) != -1)
			{

				// �b ���폜�����ۂɂP�����Ȃ當�����폜����
				if (customName.Length <= 2)
				{
					// �Ȃ̂ŁA����Q�����Ȃ�폜
					customNameList.Remove(customName);
				}
				else
				{
					// �R�����ȏ� �b �����폜
					customName.Remove(customName.Length - 1, 1);
					// ���X�g�ɏ㏑��
					customNameList[j] = customName;
				}
			}
		}




#if DEBUG_LOG
		for (int i = 0; i < createCt; i++)
		{
			Debug.Log(customNameList[i]);
		}
#endif

		m_result.AddRange(customNameList);
	}

	/// <summary>
	/// �R�����ȉ��̖��O�̕���
	/// </summary>
	private void LessThanThreeCharaSplit(string name)
	{
		// �R�����̏ꍇ�͋�؂邱�ƂȂ����̂܂܎��t��
		m_charaList.Add(name);
	}

	/// <summary>
	/// �S�����̖��O�̕���
	/// </summary>
	private void FourCharaSplit(string name)
	{
		// ��⃊�X�g
		var candidateList = new List<string>();

		for (int j = 0; j < 2; ++j)
		{

			// ���O���Q������؂�ŕ���
			var str = name.Substring(j * 2, 2);

			candidateList.Add(str);
		}

		// �P�����ڂɗ��Ă͂����Ȃ��������ǂ���
		if ("�@�B�D�F�H���b�������[��".IndexOf(candidateList[1][0]) != -1)
		{
			candidateList[0] += candidateList[1][0];

			candidateList.Remove(candidateList[1]);
		}



		foreach (var candidate in candidateList)
			m_charaList.Add(candidate);
	}

	/// <summary>
	/// �T�����̖��O�̕���
	/// "�A�D�X�g��","���F�X�g��""�A�[�i�b��"�r���H�[�� �X���[�C�� ���B�g�D�� �t���f�B��
	/// </summary>
	private void FiveCharaSplit(string name)
	{

		// ��⃊�X�g
		var candidateList = new List<string>();

		// �ʏ�� 3-2 �ɕ��� �S�����ڂɗ��Ă͂����Ȃ�����������ꍇ�́@2-3 �ɕ�����
		if ("�@�B�D�F�H���b�������[��".IndexOf(name[3]) != -1)
		{
			// 2-3 �ɕ���
			var str = name.Substring(0, 2);
			candidateList.Add(str);
			str = name.Substring(2, 3);

			// �ꕶ���ڂɗ��Ă͂����Ȃ����������邩�ǂ���
			if ("�@�B�D�F�H���b�������[��".IndexOf(str[0]) != -1)
			{
				//Console.WriteLine(str);
			}
			else
			{
				candidateList.Add(str);
			}
		}
		else
		{
			// 3-2 �ɕ���
			var str = name.Substring(0, 3);
			candidateList.Add(str);
			str = name.Substring(3, 2);
			candidateList.Add(str);
		}

		foreach (var candidate in candidateList)
			m_charaList.Add(candidate);

	}

}

class NameCreateOrigin : NameCreateBuilderBase
{
	private List<string> charaList_ = new List<string>();

	public override void Create(int createCt, List<string> nameList)
    {
		int count = nameList.Count;

		int randomCount = count / createCt;

		List<string> selectedNameList = new List<string>();

		for (int i = 0; i < createCt; ++i)
		{
			var rnd = new System.Random();
			int nameCount = rnd.Next(randomCount * i, randomCount * i + randomCount);

			if (nameCount >= nameList.Count)
			{
				nameCount--;
			}

			selectedNameList.Add(nameList[nameCount]);
		}

#if DEBUG_LOG
		for (int i = 0; i < createCt; i++)
		{
			Debug.Log(selectedNameList[i]);
		}
#endif

		m_result.AddRange(selectedNameList);
	}
}