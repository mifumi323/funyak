using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MifuminSoft.funyak.Core.Tests;

namespace WPFTests
{
    /// <summary>
    /// NormalUnitTest.xaml の相互作用ロジック
    /// </summary>
    public partial class NormalUnitTest : Page
    {
        public class TestCase
        {
            public Type TestClass { get; set; }
            public MethodInfo TestMethod { get; set; }

            public enum Result
            {
                None,
                Success,
                Failure,
                Error,
            }
            public Result TestResult { get; set; } = Result.None;

            public Exception TestException { get; set; }

            public override string ToString() => $"{TestClass.Name}.{TestMethod.Name}({TestResult})";
        }

        public IList<TestCase> TestCaseList { get; set; } = new List<TestCase>();

        public NormalUnitTest() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var assembly = Assembly.GetAssembly(typeof(MapTest));
            var testClasses = assembly.GetTypes().Where(t => !t.IsAbstract && t.GetCustomAttribute<TestClassAttribute>() != null);
            foreach (var testClass in testClasses)
            {
                var testMethods = testClass.GetMethods().Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null);
                foreach (var testMethod in testMethods)
                {
                    TestCaseList.Add(new TestCase()
                    {
                        TestClass = testClass,
                        TestMethod = testMethod,
                    });
                }
            }
            lbTestCase.ItemsSource = TestCaseList;
        }

        private void btnTestSelected_Click(object sender, RoutedEventArgs e) => Test(lbTestCase.SelectedItems.Cast<TestCase>());

        private void btnTestAll_Click(object sender, RoutedEventArgs e) => Test(TestCaseList);

        private void Test(IEnumerable<TestCase> testCaseList)
        {
            Task.Run(async () =>
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    btnTestSelected.IsEnabled = false;
                    btnTestAll.IsEnabled = false;
                });
                foreach (var testCase in testCaseList)
                {
                    try
                    {
                        var testObject = Activator.CreateInstance(testCase.TestClass);
                        testCase.TestMethod.Invoke(testObject, new object[0]);
                        testCase.TestResult = TestCase.Result.Success;
                        testCase.TestException = null;
                    }
                    catch (AssertFailedException e)
                    {
                        testCase.TestResult = TestCase.Result.Failure;
                        testCase.TestException = e;
                    }
                    catch (Exception e)
                    {
                        testCase.TestResult = TestCase.Result.Error;
                        testCase.TestException = e;
                    }
                    await Dispatcher.InvokeAsync(() =>
                    {
                        lbTestCase.Items.Refresh();
                    });
                }
                await Dispatcher.InvokeAsync(() =>
                {
                    btnTestSelected.IsEnabled = true;
                    btnTestAll.IsEnabled = true;
                });
            });
        }
    }
}
