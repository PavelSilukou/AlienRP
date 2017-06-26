/* 
* ***** BEGIN GPL LICENSE BLOCK*****

* Copyright © 2017 Pavel Silukou

* This file is part of AlienRP.

* AlienRP is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* AlienRP is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with AlienRP.  If not, see<http://www.gnu.org/licenses/>.

* ***** END GPL LICENSE BLOCK*****
*/

using System;
using System.Windows;
using System.Windows.Controls;

namespace AlienRP.Elements
{
    public partial class MarqueeTextBlock : UserControl
    {
        int animationBeforeSeconds = 2;
        int animationSeconds = 2;
        int animationAfterSeconds = 2;

        public MarqueeTextBlock()
        {
            InitializeComponent();

            marqueeGrid.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            marqueeGrid.Arrange(new Rect(marqueeGrid.DesiredSize));

            this.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            this.Arrange(new Rect(this.DesiredSize));

            SetAnimationDuration();

            marqueeTextStoryboard.Begin(marqueeText, true);
        }

        private void SetAnimationDuration()
        {
            marqueeTextStoryboard.Stop();

            animationBefore.Duration = TimeSpan.FromSeconds(animationBeforeSeconds);

            animation.Duration = TimeSpan.FromSeconds(animationSeconds);
            animation.BeginTime = TimeSpan.FromSeconds(animationBeforeSeconds);

            animationAfter.Duration = TimeSpan.FromSeconds(animationAfterSeconds);
            animationAfter.BeginTime = TimeSpan.FromSeconds(animationBeforeSeconds + animationSeconds);

            marqueeTextStoryboard.Begin();
        }

        public void SetText(string text)
        {
            marqueeText.Text = text;

            marqueeText.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            marqueeText.Arrange(new Rect(marqueeText.DesiredSize));

            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            marqueeTextStoryboard.Stop();

            if ((this.ActualWidth - marqueeText.ActualWidth) < 0)
            {
                animation.From = 0;
                animation.To = -marqueeText.ActualWidth + this.ActualWidth;

                animationAfter.From = -marqueeText.ActualWidth + this.ActualWidth;
                animationAfter.To = -marqueeText.ActualWidth + this.ActualWidth;
            }
            else
            {
                animation.From = 0;
                animation.To = 0;

                animationAfter.From = 0;
                animationAfter.To = 0;
            }

            marqueeTextStoryboard.Begin();
        }
    }
}
