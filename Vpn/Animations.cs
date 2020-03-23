using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Vpn
{
    public static class Colors
    {
        public static Brush Offline = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFF0000"));
        public static Brush Connecting = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF00"));
        public static Brush Online = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF00FF3A"));

    }

    class Animations
    {
        MainWindow mainWindow;
        public Animations(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            #region Anims
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = 25F;
                doubleAnimation.To = 100F;
                doubleAnimation.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(2));
                doubleAnimation.AutoReverse = true;
                doubleAnimation.RepeatBehavior = new RepeatBehavior(int.MaxValue);
                mainWindow.BackGroundHeat.Effect.BeginAnimation(System.Windows.Media.Effects.BlurEffect.RadiusProperty, doubleAnimation);
            }
            {
                ScaleTransform scaleTransform = new ScaleTransform();
                mainWindow.BackGroundHeat.RenderTransformOrigin = new System.Windows.Point(0.5F, 0.5F);
                mainWindow.BackGroundHeat.RenderTransform = scaleTransform;
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = 1F;
                doubleAnimation.To = 1.2F;
                doubleAnimation.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(2));
                doubleAnimation.AutoReverse = true;
                doubleAnimation.RepeatBehavior = new RepeatBehavior(int.MaxValue);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, doubleAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, doubleAnimation);
            }
            #endregion
        }
    }
}
