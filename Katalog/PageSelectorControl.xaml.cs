using System;
using System.ComponentModel;
using System.Windows;

namespace Katalog
{
    public partial class PageSelectorControl
    {
        public event EventHandler PageUpdated;

        public bool IsLeftEnabled
        {
            get { return (bool)GetValue(IsLeftEnabledProperty); }
            set { SetValue(IsLeftEnabledProperty, value); }
        }
        public bool IsRightEnabled
        {
            get { return (bool)GetValue(IsRightEnabledProperty); }
            set { SetValue(IsRightEnabledProperty, value); }
        }
        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }
        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        
        
        public static readonly DependencyProperty UpdateCommandProperty =
            DependencyProperty.Register("UpdateCommand", typeof(CommandRelay), typeof(PageSelectorControl), new PropertyMetadata(0));


        public int TotalPages => (int) Math.Ceiling(((double) Count)/((double) PageSize));

        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(PageSelectorControl), new PropertyMetadata(1));
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PageSelectorControl), new PropertyMetadata(100));
        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(int), typeof(PageSelectorControl), new PropertyMetadata(0, OnCountChanged));

        private static void OnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = ((PageSelectorControl)d);
            me.PageUpdated?.Invoke(me.CurrentPage,null);
        }

        public static readonly DependencyProperty IsLeftEnabledProperty =
            DependencyProperty.Register("IsLeftEnabled", typeof(bool), typeof(PageSelectorControl), new PropertyMetadata(true));
        public static readonly DependencyProperty IsRightEnabledProperty =
            DependencyProperty.Register("IsRightEnabled", typeof(bool), typeof(PageSelectorControl), new PropertyMetadata(true));


        private void OnLeftClick(object sender, RoutedEventArgs e)
        {
            if(CurrentPage>0)
                CurrentPage--;
            PageUpdated?.Invoke(CurrentPage,null);
            IsLeftEnabled = CurrentPage > 1;
            IsRightEnabled = CurrentPage < (Count%PageSize);
        }

        private void OnRightClick(object sender, RoutedEventArgs e)
        {
            CurrentPage++;
            PageUpdated?.Invoke(CurrentPage,null);
            IsLeftEnabled = CurrentPage > 1;
            IsRightEnabled = CurrentPage < (Count%PageSize);
        }
        public PageSelectorControl()
        {
            InitializeComponent();
        }

    }
}
