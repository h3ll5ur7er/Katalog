#define WPF //Comment this line if used in universal app, uncomment for wpf

using System.Windows;
using System.Windows.Controls;

namespace Katalog
{
    public static class GridExtention
    {
        public static string GetCompactRowDefinitions(DependencyObject d)
        {
            return (string)d.GetValue(CompactRowDefinitionsProperty);
        }

        public static void SetCompactRowDefinitions(DependencyObject d, string v)
        {
            d.SetValue(CompactRowDefinitionsProperty, v);
        }

        public static string GetCompactColumnDefinitions(DependencyObject d)
        {
            return (string)d.GetValue(CompactColumnDefinitionsProperty);
        }

        public static void SetCompactColumnDefinitions(DependencyObject d, string v)
        {
            d.SetValue(CompactColumnDefinitionsProperty, v);
        }

        public static readonly DependencyProperty CompactRowDefinitionsProperty =
            DependencyProperty.RegisterAttached("CompactRowDefinitions", typeof(string), typeof(Grid),
#if WPF
                new FrameworkPropertyMetadata("*", OnCompactRowDefinitionsChanged)
#else
                PropertyMetadata.Create("*", OnCompactRowDefinitionsChanged)
#endif
                );

        public static readonly DependencyProperty CompactColumnDefinitionsProperty =
            DependencyProperty.RegisterAttached("CompactColumnDefinitions", typeof(string), typeof(Grid),
#if WPF
                new FrameworkPropertyMetadata("*", OnCompactColumnDefinitionsChanged)
#else
                PropertyMetadata.Create("*", OnCompactColumnDefinitionsChanged)
#endif
                );

        private static void OnCompactColumnDefinitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var g = (Grid)d;
            var s = (string)g.GetValue(CompactColumnDefinitionsProperty);

            g.ColumnDefinitions.Clear();
            foreach (var c in s.Split(' '))
            {
                if (c.ToLower() == "auto")
                {
                    g.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                }
                else if (c.Contains("*"))
                {
                    var cs = c.Replace("*", "");
                    if (string.IsNullOrWhiteSpace(cs))
                        cs = "1";
                    g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(double.Parse(cs), GridUnitType.Star) });
                }
                else
                {
                    double cd;
                    g.ColumnDefinitions.Add(double.TryParse(c, out cd)
                        ? new ColumnDefinition { Width = new GridLength(cd) }
                        : new ColumnDefinition { Width = new GridLength() });
                }
            }
        }

        private static void OnCompactRowDefinitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var g = (Grid)d;
            var s = (string)g.GetValue(CompactRowDefinitionsProperty);

            g.RowDefinitions.Clear();
            foreach (var c in s.Split(' '))
            {
                if (c.ToLower() == "auto")
                    g.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                else if (c.Contains("*"))
                {
                    var cs = c.Replace("*", "");
                    if (string.IsNullOrWhiteSpace(cs))
                        cs = "1";
                    g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(double.Parse(cs), GridUnitType.Star) });
                }
                else
                {
                    double cd;
                    g.RowDefinitions.Add(double.TryParse(c, out cd)
                        ? new RowDefinition { Height = new GridLength(cd) }
                        : new RowDefinition { Height = new GridLength() });
                }
            }
        }
    }
}