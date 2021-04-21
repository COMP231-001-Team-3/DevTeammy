using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Media;
using teammy;

namespace teammyTest
{
    [TestClass]
    public class UnitTest1
    {      

        [TestMethod]
        public void TestTeamClick()
        {
            CardBox testCard = new CardBox();
            testCard.CardClick += new RoutedEventHandler(test_CardClick);
            
        }

        private void test_CardClick(object sender, RoutedEventArgs e)
        {
            return;
        }
    }
}
