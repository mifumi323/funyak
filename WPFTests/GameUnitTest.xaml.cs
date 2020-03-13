using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Core.Tests;
using MifuminSoft.funyak.Core.Tests.GameTest;
using MifuminSoft.funyak.View;
using MifuminSoft.funyak.View.MapObject;
using MifuminSoft.funyak.View.Resource;

namespace WPFTests
{
    /// <summary>
    /// GameUnitTest.xaml の相互作用ロジック
    /// </summary>
    public partial class GameUnitTest : Page
    {
        class TestCase
        {
            public Type TestClass { get; set; }

            public enum Result
            {
                None,
                Success,
                Failure,
                Error,
            }
            public Result TestResult { get; set; } = Result.None;

            public Exception TestException { get; set; }

            public override string ToString() => $"{TestClass.Name}({TestResult})";
        }

        private readonly IList<TestCase> testCaseList = new List<TestCase>();

        class TestProcess
        {
            public TestCase TestCase { get; private set; }

            public GameTestBase TestObject { get; private set; }

            public int Frames { get; private set; }

            public TestProcess(TestCase testCase) => TestCase = testCase;

            public void Initialize()
            {
                try
                {
                    Frames = 0;
                    TestCase.TestException = null;
                    TestCase.TestResult = TestCase.Result.None;

                    TestObject = (GameTestBase)Activator.CreateInstance(TestCase.TestClass);
                    TestObject.Initialize();
                }
                catch (AssertFailedException ex)
                {
                    TestCase.TestResult = TestCase.Result.Failure;
                    TestCase.TestException = ex;
                }
                catch (Exception ex)
                {
                    TestCase.TestResult = TestCase.Result.Error;
                    TestCase.TestException = ex;
                }
            }

            public void OnFrame()
            {
                try
                {
                    if (Frames >= TestObject.TimeoutFrames)
                    {
                        if (TestObject.FailOnTimeout)
                        {
                            Assert.Fail("テストが規定時間以内に成功しませんでした。");
                        }
                        else
                        {
                            TestCase.TestResult = TestCase.Result.Success;
                            return;
                        }
                    }
                    TestObject.OnFrame();
                    if (TestObject.IsSuccess())
                    {
                        TestCase.TestResult = TestCase.Result.Success;
                        return;
                    }
                    Frames++;
                }
                catch (AssertFailedException ex)
                {
                    TestCase.TestResult = TestCase.Result.Failure;
                    TestCase.TestException = ex;
                }
                catch (Exception ex)
                {
                    TestCase.TestResult = TestCase.Result.Error;
                    TestCase.TestException = ex;
                }
            }

            public void Terminate()
            {
                try
                {
                    TestObject.Terminate();
                }
                catch (AssertFailedException ex)
                {
                    TestCase.TestResult = TestCase.Result.Failure;
                    TestCase.TestException = ex;
                }
                catch (Exception ex)
                {
                    TestCase.TestResult = TestCase.Result.Error;
                    TestCase.TestException = ex;
                }
            }
        }

        private readonly IList<TestProcess> testProcessList = new List<TestProcess>();

        TestProcess executingTestProcess = null;

        MapView mapView = null;
        readonly Sprite resource;
        int resultFrame = 0;
        const int ResultFrameMax = 60;

        public GameUnitTest()
        {
            InitializeComponent();

            resource = SpriteReader.Read(@"Assets\main.png");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var assembly = Assembly.GetAssembly(typeof(MapTest));
            var testClasses = assembly.GetTypes().Where(t => !t.IsAbstract && t.GetCustomAttribute<TestClassAttribute>() != null && t.IsSubclassOf(typeof(GameTestBase)));
            foreach (var testClass in testClasses)
            {
                testCaseList.Add(new TestCase()
                {
                    TestClass = testClass,
                });
            }
            lbTestCase.ItemsSource = testCaseList;

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            if (executingTestProcess == null || resultFrame >= ResultFrameMax)
            {
                if (testProcessList.Count > 0)
                {
                    executingTestProcess = testProcessList[0];
                    testProcessList.RemoveAt(0);

                    executingTestProcess.Initialize();
                    canvas.Children.Clear();
                    var map = executingTestProcess.TestObject.Map;
                    mapView = new MapView(map, new MapObjectViewFactory()
                    {
                        MainMapObjectResourceSelector = a => resource,
                    })
                    {
                        Canvas = canvas,
                        FocusTo = map.FindMapObject("main") ?? map.GetMapObjects().FirstOrDefault()
                    };
                    lblResult.Content = null;
                    lbTestCase.Items.Refresh();
                    resultFrame = 0;
                }
            }
            if (executingTestProcess != null)
            {
                if (executingTestProcess.TestCase.TestResult == TestCase.Result.None)
                {
                    executingTestProcess.OnFrame();
                    if (executingTestProcess.TestCase.TestResult != TestCase.Result.None)
                    {
                        executingTestProcess.Terminate();
                        lblResult.Content = executingTestProcess.TestCase.TestResult;
                        lbTestCase.Items.Refresh();
                    }
                }
                else
                {
                    resultFrame++;
                }
                mapView.Update(sliderScale.Value);
            }
        }

        private void BtnTestSelected_Click(object sender, RoutedEventArgs e) => Test(lbTestCase.SelectedItems.Cast<TestCase>());

        private void BtnTestAll_Click(object sender, RoutedEventArgs e) => Test(testCaseList);

        private void Test(IEnumerable<TestCase> testCaseList)
        {
            foreach (var testCase in testCaseList)
            {
                testProcessList.Add(new TestProcess(testCase));
            }
        }
    }
}
