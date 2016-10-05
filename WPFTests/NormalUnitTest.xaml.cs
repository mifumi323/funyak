using System.Linq;
using System.Reflection;
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
        public NormalUnitTest()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var assembly = Assembly.GetAssembly(typeof(MapTest));
            var testClasses = assembly.GetTypes().Where(t => !t.IsAbstract && t.GetCustomAttribute<TestClassAttribute>() != null);
            foreach (var testClass in testClasses)
            {
                var testMethods = testClass.GetMethods().Where(m => m.GetCustomAttribute<TestMethodAttribute>() != null);
                foreach (var testMethod in testMethods)
                {
                    lbTestCase.Items.Add(string.Format("{0}.{1}", testClass.Name, testMethod.Name));
                }
            }
        }
    }
}
