using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Data;
using MifuminSoft.funyak.Input;

namespace MifuminSoft.funyak.Core.Tests.GameTest
{
    public abstract class GameTestBase
    {
        /// <summary>
        /// テストに使うマップファイルのパス
        /// </summary>
        public string MapFilePath { get; protected set; }

        /// <summary>
        /// テストの規定時間(フレーム数単位)
        /// このフレーム数経過してもテスト成功しなければ失敗とみなします。
        /// </summary>
        public int TimeOutFrames { get; protected set; }

        /// <summary>
        /// テストを行います。
        /// 派生クラスで、TestMethod属性を付けたメソッドから呼び出してください。
        /// </summary>
        public virtual void Run()
        {
            Initialize();
            try
            {
                for (int i = 0; i < TimeOutFrames; i++)
                {
                    OnFrame();
                    if (IsSuccess())
                    {
                        // テスト成功！
                        return;
                    }
                }
                Assert.Fail("テストが規定時間以内に完了しませんでした。");
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
            var map = MapReader.FromString(File.ReadAllText(MapFilePath), new MapReaderOption()
            {
                Input = new NullInput(),
            });
        }

        /// <summary>
        /// 1フレームごとの処理を行います。
        /// </summary>
        public virtual void OnFrame()
        {
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
