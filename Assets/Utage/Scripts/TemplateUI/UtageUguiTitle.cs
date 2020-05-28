// UTAGE: Unity Text Adventure Game Engine (c) Ryohei Tokimura
using UnityEngine;
using Utage;
using UtageExtensions;
using System.Collections;


/// <summary>
/// タイトル表示のサンプル
/// </summary>
[AddComponentMenu("Utage/TemplateUI/Title")]
public class UtageUguiTitle : UguiView
{
	/// <summary>スターター</summary>
	public AdvEngineStarter Starter { get { return this.GetComponentCacheFindIfMissing(ref starter); } }
	[SerializeField]
	protected AdvEngineStarter starter;

	/// <summary>メインゲーム画面</summary>
	public UtageUguiMainGame mainGame;

	/// <summary>コンフィグ画面</summary>
	public UtageUguiConfig config;

	/// <summary>セーブデターのロード画面</summary>
	public UtageUguiSaveLoad load;

	///<summary>ダウンロード画面</summary>
	public UtageUguiLoadWait download;

	///<summary>初期タイトル画面の表示</summary>
	public Sprite TitleSprite;
	public SpriteRenderer TitleSpriteRenderer;

	//2周目以降のタイトル画面表示
	public Sprite SecondTitleSprite;
	public SpriteRenderer SecondTitleSpriteRenderer;

	//クリアカウント初期値設定
	public int ClearCount;

	///「はじめから」ボタンが押された
	public virtual void OnTapStart()
	{
		Close();
		mainGame.OpenStartGame();
	}

	///「つづきから」ボタンが押された
	public virtual void OnTapLoad()
	{
		Close();
		load.OpenLoad(this);
	}

	///「コンフィグ」ボタンが押された
	public virtual void OnTapConfig()
	{
		Close();
		config.Open(this);
	}

	///「指定のラベルからスタート」ボタンが押された
	public virtual void OnTapStartLabel(string label)
	{
		Close();
		mainGame.OpenStartLabel(label);
	}

	public override void Open(UguiView prevView)
	{
		if (prevView != null)
		{
			Debug.Log("preview:" + prevView);
			// 前の画面がMainGame
			if (prevView.name == "MainGame")
			{
				// クリア数を加算
				ClearCount++;
				PlayerPrefs.SetInt("ClearCount", ClearCount);
				PlayerPrefs.Save();
			}
		}

		base.Open(prevView);

		// クリア数を読み込み
		ClearCount = PlayerPrefs.GetInt("ClearCount");

		// クリア数を表示
		Debug.Log("clearCount:" + ClearCount);

		if (ClearCount == 0)
		{
			TitleSpriteRenderer.sprite = TitleSprite;
			Debug.Log("未クリアです。");
		}

		if (ClearCount >= 1)
		{
			SecondTitleSpriteRenderer.sprite = SecondTitleSprite;
			Debug.Log("1回以上クリアしました。");
		}
	}

	void Update()
	{
		//エディタ上でDキーが押されたらクリアする
		if (Application.isEditor && Input.GetKeyDown(KeyCode.D))
		{
			PlayerPrefs.SetInt("ClearCount", 0);
			Debug.Log("ClearCountを0に戻しました。");
		}

		//エディタ上でSキーが押されたら二周目にする
		if (Application.isEditor && Input.GetKeyDown(KeyCode.S))
		{
			PlayerPrefs.SetInt("ClearCount", 1);
			Debug.Log("ClearCountを1にしました。");
		}
	}

	protected virtual void OnCloseLoadChapter(string startLabel)
	{
		download.onClose.RemoveAllListeners();
		Close();
		mainGame.OpenStartLabel(startLabel);
	}

}
