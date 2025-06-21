using FluentFrame.ViewModel.Shell.Message;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace FluentFrame.UI.Shell.Controls;


[TemplatePart(Name = PART_Toggle, Type = typeof(ToggleButton))]
[TemplatePart(Name = PART_Popup, Type = typeof(Popup))]
[TemplatePart(Name = PART_ItemsHost, Type = typeof(ItemsControl))]
internal class MessagePopupBox  : Control
{
    private const string PART_Toggle = "PART_Toggle";
    private const string PART_Popup = "PART_Popup";
    private const string PART_ItemsHost = "PART_ItemsHost";

    static MessagePopupBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MessagePopupBox),
            new FrameworkPropertyMetadata(typeof(MessagePopupBox)));
    }


    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(ObservableCollection<MessageItemViewModel>),
        typeof(MessagePopupBox ),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


    public static readonly DependencyProperty CountProperty = DependencyProperty.Register(
        nameof(Count),
        typeof(int),
        typeof(MessagePopupBox ),
        new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


    public static readonly DependencyProperty IsPopupOpenProperty = DependencyProperty.Register(
        nameof(IsPopupOpen),
        typeof(bool),
        typeof(MessagePopupBox),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public ObservableCollection<MessageItemViewModel> ItemsSource
    {
        get => (ObservableCollection<MessageItemViewModel>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public int Count
    {
        get => (int)GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public bool IsPopupOpen
    {
        get => (bool)GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild(PART_Toggle) is ToggleButton toggle)
        {
            toggle.Checked += (s, e) => IsPopupOpen = true;
            toggle.Unchecked += (s, e) => IsPopupOpen = false;
        }

        if (GetTemplateChild(PART_Popup) is Popup popup)
        {
            popup.SetBinding(Popup.IsOpenProperty, new Binding(nameof(IsPopupOpen)) { Source = this });
        }

        if (GetTemplateChild(PART_ItemsHost) is ItemsControl items)
        {
            items.SetBinding(ItemsControl.ItemsSourceProperty, new Binding(nameof(ItemsSource)) { Source = this });
        }
    }
}
