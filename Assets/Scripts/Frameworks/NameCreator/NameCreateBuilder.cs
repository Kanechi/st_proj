#define DEBUG_LOG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

/// <summary>
/// 名前生成ビルダー基底
/// </summary>
public abstract class NameCreateBuilderBase
{
	protected List<string> m_result = new List<string>();

	public List<string> Result => m_result;

    /// <summary>
    /// Text を読み込んで改行ごとに分解して配列に代入
    /// </summary>
    public List<string> LoadName(Text nameList)
    {
        return nameList.text.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
    }

    /// <summary>
    /// string を読み込んで改行ごとに分解して配列に代入
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

		//ランダムで取得した位置から全検索して、ここまで取得した文字列の最後の文字と合致する文字列と合成する。
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

			//8文字を超えたらそこで終了する。
		} while (strRet.Length <= 9);
	}

	// 禁止文字
	private string[] m_fc = new string[] { "ァ", "ィ", "ゥ", "ェ", "ォ", "ヵ", "ッ", "ャ", "ュ", "ョ", "ー", "ン" };

	public override void Create(int createCt, List<string> nameList)
    {
		strBody = nameList.ToList();

		//3文字ごとのブロックリストを作る。
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

					//最初の文字として来てはいけないもの。
					var firstChar = strRet3.Substring(0, 1);//Mid(strRet3, 1, 1);
					var lastChar = strRet3.Substring(strRet3.Length - 1, 1);//Mid(strRet3, strRet3.Length, 1);

					if ("ァィゥェォヵッャュョーン".IndexOf(firstChar) != -1)
					{
						
					}
					//最後の文字として来てはいけないもの。
					else if ("ッ".IndexOf(lastChar) != -1)
					{
						//必ず3文字だと「ジャック」のように取れないものが出てくるのでその場合例外的に4文字で処理。
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
				// 最終チェック
				var firstChar = strRet.Substring(0, 1);

				bool isHit = false;
				foreach (var c in m_fc) {
					if (c == firstChar) {
						isHit = true;
						break;
					}
				}

				// 一文字目に禁止文字が入っていたら削除
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
/// 変換率が激しいので元文字列がほぼわからないが、何を意味しているのかも分かりづらいので
/// 本当に適当に名前を付けたい時に利用
/// </summary>
public class NameCreateCustom : NameCreateBuilderBase
{
	private List<string> m_charaList = new List<string>();

	/// <summary>
	/// ランダム生成開始
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
		// 名前数の設定 (3 文字から 8 文字)
		Random rnd = new System.Random(); 
		int nameCount = rnd.Next(3, 8);
		Console.WriteLine("name count : {0}", nameCount);
#endif

		//================================
		// 名前の作成
		//================================
		var customNameList = new List<string>();

		var rnd = new System.Random(System.Environment.TickCount);


		for (int i = 0; i < createCt; i++)
		{
			int nameCount = rnd.Next(3, 8);

			// true...名前完成
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





		// ラストに来てはいけない文字かどうか
		for (int j = 0; j < customNameList.Count; ++j)
		{

			var customName = customNameList[j];

			if ("ッ".IndexOf(customName[customName.Length - 1]) != -1)
			{

				// ッ を削除した際に１文字なら文字を削除する
				if (customName.Length <= 2)
				{
					// なので、現状２文字なら削除
					customNameList.Remove(customName);
				}
				else
				{
					// ３文字以上 ッ だけ削除
					customName.Remove(customName.Length - 1, 1);
					// リストに上書き
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
	/// ３文字以下の名前の分割
	/// </summary>
	private void LessThanThreeCharaSplit(string name)
	{
		// ３文字の場合は区切ることなくそのまま取り付け
		m_charaList.Add(name);
	}

	/// <summary>
	/// ４文字の名前の分割
	/// </summary>
	private void FourCharaSplit(string name)
	{
		// 候補リスト
		var candidateList = new List<string>();

		for (int j = 0; j < 2; ++j)
		{

			// 名前を２文字区切りで分割
			var str = name.Substring(j * 2, 2);

			candidateList.Add(str);
		}

		// １文字目に来てはいけない文字かどうか
		if ("ァィゥェォヵッャュョーン".IndexOf(candidateList[1][0]) != -1)
		{
			candidateList[0] += candidateList[1][0];

			candidateList.Remove(candidateList[1]);
		}



		foreach (var candidate in candidateList)
			m_charaList.Add(candidate);
	}

	/// <summary>
	/// ５文字の名前の分割
	/// "アゥストリ","ヴェストリ""アーナッル"ビヴォール スラーイン ヴィトゥル フンディン
	/// </summary>
	private void FiveCharaSplit(string name)
	{

		// 候補リスト
		var candidateList = new List<string>();

		// 通常は 3-2 に分け ４文字目に来てはいけない文字がある場合は　2-3 に分ける
		if ("ァィゥェォヵッャュョーン".IndexOf(name[3]) != -1)
		{
			// 2-3 に分割
			var str = name.Substring(0, 2);
			candidateList.Add(str);
			str = name.Substring(2, 3);

			// 一文字目に来てはいけない文字があるかどうか
			if ("ァィゥェォヵッャュョーン".IndexOf(str[0]) != -1)
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
			// 3-2 に分割
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