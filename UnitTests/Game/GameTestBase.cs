﻿using System.IO;
using NUnit.Framework;
using MifuminSoft.funyak.Data;
using MifuminSoft.funyak.Input;

namespace MifuminSoft.funyak.UnitTests.Game
{
    /// <summary>
    /// ゲーム画面上で動くテストを行います。
    /// 基本的に、派生クラスで1クラスにつき1テストを実装してください。
    /// </summary>
    public abstract class GameTestBase
    {
        /// <summary>
        /// テストに使うマップファイルのパス
        /// </summary>
        public string MapFilePath { get; protected set; }

        /// <summary>
        /// テスト中のマップ
        /// </summary>
        public Map Map { get; protected set; }

        /// <summary>
        /// テスト用の入力
        /// </summary>
        public IInput Input { get; protected set; }

        /// <summary>
        /// テストの規定時間(フレーム数単位)
        /// このフレーム数が経過した時点でテストを終了します。
        /// </summary>
        public int TimeoutFrames { get; protected set; }

        /// <summary>
        /// テストの規定時間が経過しても結果が出なかった場合に失敗にする場合true
        /// falseの場合、テストの規定時間が経過した時点で失敗でなければ成功とみなされます。
        /// </summary>
        public bool FailOnTimeout { get; protected set; }

        /// <summary>
        /// テストを行います。
        /// 派生クラスで、Test属性を付けたメソッドから呼び出してください。
        /// </summary>
        public virtual void Run()
        {
            Initialize();
            try
            {
                for (int i = 0; i < TimeoutFrames; i++)
                {
                    OnFrame();
                    if (IsSuccess())
                    {
                        // テスト成功！
                        return;
                    }
                }
                if (FailOnTimeout) Assert.Fail("テストが規定時間以内に成功しませんでした。");
            }
            finally
            {
                Terminate();
            }
        }

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        public virtual void Initialize()
        {
            if (Input == null) Input = NullInput.Instance;
            Map = MapReader.FromString(File.ReadAllText(MapFilePath), new MapReaderOption()
            {
                Input = Input,
            });
        }

        /// <summary>
        /// 1フレームごとの処理を行います。
        /// </summary>
        public virtual void OnFrame()
        {
            Input.Update();
            Map.Update();
        }

        /// <summary>
        /// テスト成功しているかどうかを判定します。
        /// 派生クラスで、テスト成功ならtrue、継続中ならfalseを返す処理を作成してください。
        /// テスト失敗の場合は、falseを返さず、Assertクラスを使用して例外をスローしてください。
        /// </summary>
        /// <returns>テスト成功であればtrue</returns>
        public abstract bool IsSuccess();

        /// <summary>
        /// 終了処理を行います。
        /// </summary>
        public virtual void Terminate()
        {
        }
    }
}
