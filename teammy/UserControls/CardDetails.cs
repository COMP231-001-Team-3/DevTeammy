using System;
using System.Windows.Media;

namespace teammy.UserControls
{
    public class CardDetails
    {
        public string FullName { get; set; }
        public Color ProfileBack { get; set; }
        public bool SelectorVisible { get; set; }
        public bool Selected { get; set; }
        public bool IsVisible { get; set; }
        public bool IsInputVisible { get; set; }
        public string InputName { get; set; } = string.Empty;

        //Colors for team cards
        Color[] backColors = new Color[] { Colors.Red, Colors.Blue, Colors.Orange, Colors.Aqua, Colors.BlueViolet, Colors.Gold, Colors.Brown, Colors.Coral, Colors.Gold, Colors.SaddleBrown, Colors.Salmon, Colors.CornflowerBlue, Colors.RoyalBlue, Colors.RosyBrown, Colors.Yellow, Colors.YellowGreen, Colors.GreenYellow, Colors.Indigo };

        public CardDetails(string name, bool isVisible, bool selectVisible, bool selected)
        {
            FullName = name;
            SelectorVisible= selectVisible; 
            Selected = selected;
            IsVisible = isVisible;
            Random rd = new Random();
            ProfileBack = backColors[rd.Next(0, backColors.Length)];
        }
    }
}
