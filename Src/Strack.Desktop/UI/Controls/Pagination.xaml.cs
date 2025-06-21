using Common.Extension;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Strack.Desktop.UI.Controls;

public partial class Pagination : UserControl
{
    public static readonly DependencyProperty FirstProperty = DependencyProperty.Register(
        nameof(First),
        typeof(int),
        typeof(Pagination),
        new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnUpdatePageNumber));
    public static readonly DependencyProperty LastProperty = DependencyProperty.Register(
       nameof(Last),
       typeof(int),
       typeof(Pagination),
       new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnUpdatePageNumber));
    public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register(
        nameof(Current),
        typeof(int),
        typeof(Pagination),
        new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnUpdatePageNumber));


    public static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register(
        nameof(ItemsCount),
        typeof(int),
        typeof(Pagination),
        new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnUpdatePageNumber), x =>
        {
            if (x is not int count) return false;
            if (count < 3) return false; // 中间页码数量不能小于3
            if(count % 2 == 0) return false; // 中间页码数量必须为奇数
            return true;
        });
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(ObservableCollection<RadioButton>),
        typeof(Pagination),
        new PropertyMetadata(new ObservableCollection<RadioButton>()));

    
    /// <summary>
    /// 首页
    /// </summary>
    public int First
    {
        get => (int)GetValue(FirstProperty);
        set => SetValue(FirstProperty, value);
    }
    /// <summary>
    /// 尾页
    /// </summary>
    public int Last
    {
        get => (int)GetValue(LastProperty);
        set => SetValue(LastProperty, value);
    }
    /// <summary>
    /// 当前页
    /// </summary>
    public int Current
    {
        get => (int)GetValue(CurrentProperty);
        set => SetValue(CurrentProperty, value);
    }

    /// <summary>
    /// 中间页面的数量
    /// </summary>
    public int ItemsCount
    {
        get => (int)GetValue(ItemsCountProperty);
        set => SetValue(ItemsCountProperty, value);
    }
    /// <summary>
    /// 中间页码集合
    /// </summary>
    public ObservableCollection<RadioButton> ItemsSource
    {
        get => (ObservableCollection<RadioButton>)GetValue(ItemsSourceProperty);
        private set => SetValue(ItemsSourceProperty, value);
    }

    public Pagination()
    {
        InitializeComponent();
    }

    private void OnPrevButtonClicked(object sender, RoutedEventArgs e)
    {
        if (Current - 1 > First) Current--;
        else Current = First;
    }
    private void OnNextButtonClicked(object sender, RoutedEventArgs e)
    {
        if (Current + 1 < Last) Current++;
        else Current = Last;
    }

    private void OnPrevPagesButtonClick(object sender, RoutedEventArgs e)
    {
        if (Current - ItemsCount > First) Current -= ItemsCount;
        else Current = First;
    }
    private void OnNextPagesButtonClick(object sender, RoutedEventArgs e)
    {
        if (Current + ItemsCount < Last) Current += ItemsCount;
        else Current = Last;
    }


    private void OnFirstPageClick(object sender, RoutedEventArgs e)
    {
        Current = 1;
    }
    private void OnLastPageClick(object sender, RoutedEventArgs e)
    {
        Current = Last;
    }
    private void OnGotoPageKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        if (sender is not TextBox textBox) return;

        if (int.TryParse(textBox.Text, out int pageNumber) && pageNumber >= First && pageNumber <= Last)
        {
            Current = pageNumber;
            textBox.Text = string.Empty;
        }
    }


    /// <summary>
    /// 选择页码
    /// </summary>
    /// <param name="pageNumber"></param>
    private void OnPageNumberChecked(int pageNumber)
    {
        if (pageNumber < First) pageNumber = First;
        if (pageNumber > Last) pageNumber = Last;

        //选择页码
        FirstRadioButton.IsChecked = pageNumber == First;
        LastRadioButton.IsChecked = pageNumber == Last;


        var offset = (ItemsCount - 1) / 2;
        var start = Math.Max(pageNumber - offset, First + 1);
        var end = Math.Min(start + ItemsCount, Last);

        List<RadioButton> pageButtons = new List<RadioButton>();
        for (int i = start; i < end; i++)
        {
            if (i == First || i == Last) continue; // 跳过首尾页码
            int curPageNumber = i;

            var btn = new RadioButton() { IsChecked = curPageNumber == pageNumber, Content = curPageNumber };
            btn.Click += delegate
            {
                OnPageNumberChecked(curPageNumber);
            };
            pageButtons.Add(btn);
        }

        ItemsSource = pageButtons.ToObservableCollection();
        Current = pageNumber;
    }

    private static void OnUpdatePageNumber(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Pagination pagination) return;
        pagination.OnPageNumberChecked(pagination.Current);
    } 
}