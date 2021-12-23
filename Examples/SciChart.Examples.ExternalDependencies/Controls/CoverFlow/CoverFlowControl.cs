// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CoverFlowControl.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using SciChart.Charting.Common.Helpers;

namespace SciChart.Examples.ExternalDependencies.Controls.CoverFlow
{
    public delegate void SelectedItemChangedEvent(CoverFlowEventArgs e);

    public class CoverFlowControl : ListBox, INotifyPropertyChanged
    {
        #region DependencyProperties

        public new static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(CoverFlowControl), new PropertyMetadata(null, SelectedItemPropertyChanged));

        public static readonly DependencyProperty SelectionColorProperty = DependencyProperty.Register("SelectionColor", typeof(Color), typeof(CoverFlowControl), new PropertyMetadata(Colors.Transparent, OnValuesChanged));

        public static readonly DependencyProperty SingleItemDurationProperty = DependencyProperty.Register("SingleItemDuration", typeof(Duration), typeof(CoverFlowControl), new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(600)), OnValuesChanged));

        public static readonly DependencyProperty PageDurationProperty = DependencyProperty.Register("PageDuration", typeof(Duration), typeof(CoverFlowControl), new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(900)), OnValuesChanged));

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(double), typeof(CoverFlowControl), new PropertyMetadata(.7d, OnValuesChanged));

        public static readonly DependencyProperty HideOverlappedSideItemsProperty = DependencyProperty.Register("HideOverlappedSideItems", typeof(bool), typeof(CoverFlowControl), new PropertyMetadata(false));

        public static readonly DependencyProperty SpaceBetweenItemsProperty = DependencyProperty.Register("SpaceBetweenItems", typeof(double), typeof(CoverFlowControl), new PropertyMetadata(60d, OnValuesChanged));

        public static readonly DependencyProperty SpaceBetweenSelectedItemAndItemsProperty = DependencyProperty.Register("SpaceBetweenSelectedItemAndItems", typeof(double), typeof(CoverFlowControl), new PropertyMetadata(140d, OnValuesChanged));

        public static readonly DependencyProperty NextPrevSelectionOnlyProperty = DependencyProperty.Register("NextPrevSelectionOnly", typeof(bool), typeof(CoverFlowControl), new PropertyMetadata(false));

        public static readonly DependencyProperty ZDistanceProperty = DependencyProperty.Register("ZDistance", typeof(double), typeof(CoverFlowControl), new PropertyMetadata(0d, OnValuesChanged));

        public static readonly DependencyProperty RotationAngleProperty = DependencyProperty.Register("RotationAngle", typeof(double), typeof(CoverFlowControl), new PropertyMetadata(0d, OnValuesChanged));

        public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(CoverFlowControl), new PropertyMetadata(new CubicEase()));

        #endregion

        #region CLR Properties

        public Color SelectionColor
        {
            get => (Color)GetValue(SelectionColorProperty);
            set => SetValue(SelectionColorProperty, value);
        }

        public Duration SingleItemDuration
        {
            get => (Duration)GetValue(SingleItemDurationProperty);
            set => SetValue(SingleItemDurationProperty, value);
        }

        public Duration PageDuration
        {
            get => (Duration)GetValue(PageDurationProperty);
            set => SetValue(PageDurationProperty, value);
        }

        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public bool HideOverlappedSideItems
        {
            get => (bool)GetValue(HideOverlappedSideItemsProperty);
            set => SetValue(HideOverlappedSideItemsProperty, value);
        }

        public double SpaceBetweenItems
        {
            get => (double)GetValue(SpaceBetweenItemsProperty);
            set => SetValue(SpaceBetweenItemsProperty, value);
        }

        public double SpaceBetweenSelectedItemAndItems
        {
            get => (double)GetValue(SpaceBetweenSelectedItemAndItemsProperty);
            set => SetValue(SpaceBetweenSelectedItemAndItemsProperty, value);
        }

        public bool NextPrevSelectionOnly
        {
            get => (bool)GetValue(NextPrevSelectionOnlyProperty);
            set => SetValue(NextPrevSelectionOnlyProperty, value);
        }

        public double ZDistance
        {
            get => (double)GetValue(ZDistanceProperty);
            set => SetValue(ZDistanceProperty, value);
        }

        public double RotationAngle
        {
            get => (double)GetValue(RotationAngleProperty);
            set => SetValue(RotationAngleProperty, value);
        }

        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        #endregion

        private int _selectedIndex;
        private int _prevSelectedIndex;

        private bool _isZIndexAscending;
        private bool _wasSelected;

        private ItemsPresenter _itemsPresenter;
        private readonly ObservableCollection<CoverFlowItemControl> _items;
        private Dictionary<object, CoverFlowItemControl> _objectToItemContainer;

        private Duration _animationDuration;
        private readonly ICommand _nextItemCommand;
        private readonly ICommand _previousItemCommand;

        public event SelectedItemChangedEvent SelectedItemChanged;
        public event EventHandler SelectedItemClick;
        public event PropertyChangedEventHandler PropertyChanged;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                IndexSelected(value, false);
                
                if (_items.Count > 0)
                {
                    _selectedIndex = value;
                }
            }
        }

        public new object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        private void IndexSelected(int index, bool mouseClick)
        {
            _items[SelectedIndex].IsItemSelected = false;
            _items[index].IsItemSelected = true;

            IndexSelected(index, mouseClick, true);
        }

        private void IndexSelected(int index, bool mouseClick, bool layoutChildren)
        {
            if (_items.Count > 0)
            {
                _selectedIndex = index;
                _isZIndexAscending = _prevSelectedIndex <= _selectedIndex;
                _prevSelectedIndex = _selectedIndex;

                var item = _items.Count > 0 ? _items[SelectedIndex].Content : null;

                SetValue(SelectedItemProperty, item);

                if (layoutChildren)
                    InvalidateArrange();

                var args = new CoverFlowEventArgs
                {
                    Index = index,
                    Item = _items[index].Content,
                    MouseClick = mouseClick
                };

                SelectedItemChanged?.Invoke(args);

                if (PropertyChanged != null)
                    OnPropertyChanged("SelectedIndex");
            }
        }

        public CoverFlowControl()
        {
            DefaultStyleKey = typeof(CoverFlowControl);

            _items = new ObservableCollection<CoverFlowItemControl>();
            _items.CollectionChanged += (s, e) => InvalidateArrange();
            _animationDuration = SingleItemDuration;

            _wasSelected = true;
            _prevSelectedIndex = int.MaxValue;

            Loaded += (s, e) =>
            {
                var item = GetItemContainerForObject(SelectedItem);
                if (item != null)
                {
                    var index = _items.IndexOf(item);
                    if (index >= 0 && _wasSelected)
                    {
                        int centralButtonIndex = _items.Count / 2;
                        IndexSelected(centralButtonIndex, false);
                        _wasSelected = false;
                    }
                }
            };
            
            _previousItemCommand = new ActionCommand(PreviousItem);
            _nextItemCommand = new ActionCommand(NextItem);

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _itemsPresenter = (ItemsPresenter)GetTemplateChild("ItemsPresenter");
        }

        public ICommand PreviousItemCommand => _previousItemCommand;

        public ICommand NextItemCommand => _nextItemCommand;

        protected CoverFlowItemControl GetItemContainerForObject(object key)
        {
            var item = key as CoverFlowItemControl;
            if (item == null && key != null)
            {
                ObjectToItemContainer.TryGetValue(key, out item);
            }
            return item;
        }

        protected Dictionary<object, CoverFlowItemControl> ObjectToItemContainer => _objectToItemContainer ?? (_objectToItemContainer = new Dictionary<object, CoverFlowItemControl>());

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CoverFlowItemControl {SelectionColor = SelectionColor};
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is CoverFlowItemControl;
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            ObjectToItemContainer.Remove(item);

            if (element is CoverFlowItemControl coverFlowItem)
            {
                _items.Remove(coverFlowItem);
                UnsubscribeCoverFlowItemEvents(coverFlowItem);
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is CoverFlowItemControl coverFlowItem)
            {
                ObjectToItemContainer[item] = coverFlowItem;

                if (!_items.Contains(coverFlowItem))
                {
                    _items.Add(coverFlowItem);
                    SubscribeCoverFlowItemEvents(coverFlowItem);
                }
            }
        }

        private void SubscribeCoverFlowItemEvents(CoverFlowItemControl coverFlowItem)
        {
            UnsubscribeCoverFlowItemEvents(coverFlowItem);

            if (coverFlowItem != null)
            {
                coverFlowItem.ItemSelected += CoverFlowItemSelected;
                coverFlowItem.SizeChanged += CoverFlowItemSizeChanged;
            }
        }

        private void UnsubscribeCoverFlowItemEvents(CoverFlowItemControl coverFlowItem)
        {
            if (coverFlowItem != null)
            {
                coverFlowItem.ItemSelected -= CoverFlowItemSelected;
                coverFlowItem.SizeChanged -= CoverFlowItemSizeChanged;
            }
        }

        private void CoverFlowItemSelected(object sender, EventArgs e)
        {
            if (sender is CoverFlowItemControl item)
            {
                var index = _items.IndexOf(item);

                if (index >= 0)
                {
                    if (NextPrevSelectionOnly && Math.Abs(index - SelectedIndex) > 1)
                    {
                        return;
                    }

                    if (index != SelectedIndex)
                    {
                        IndexSelected(index, true);
                    }
                    else
                    {
                        var handler = SelectedItemClick;
                        handler?.Invoke(this, new CoverFlowEventArgs
                        {
                            Index = index,
                            Item = _items[index].Content,
                            MouseClick = false
                        });
                    }
                }
            }
        }

        private void CoverFlowItemSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is CoverFlowItemControl item)
            {
                var index = _items.IndexOf(item);
                LayoutChild(item, index);
            }
        }

        protected void LayoutChild(CoverFlowItemControl item, int index)
        {
            var halfWidth = _itemsPresenter.ActualWidth / 2;
            var relativeIndex = index - SelectedIndex;
            var position = GetRelativePosition(relativeIndex);

            var x = halfWidth + (relativeIndex * SpaceBetweenItems + SpaceBetweenSelectedItemAndItems * position) - item.ActualWidth / 2;
            var zIndex = _items.Count + index * (_isZIndexAscending ? 1 : -1);
            var scale = position == 0 ? 1 : Scale;

            bool useAnimation;

            if ((x + item.ActualWidth < 0 || x > _itemsPresenter.ActualWidth) &&
                (item.X + item.ActualWidth < 0 || item.X > _itemsPresenter.ActualWidth) &&
                !(x + item.ActualWidth < 0 && item.X > _itemsPresenter.ActualWidth) &&
                !(item.X + item.ActualWidth < 0 && x > _itemsPresenter.ActualWidth))
            {
                useAnimation = false;

                if (HideOverlappedSideItems) 
                    item.Visibility = Visibility.Hidden;
            }
            else
            {
                useAnimation = true;

                if (HideOverlappedSideItems)
                    item.Visibility = Visibility.Visible;
            }
            
            item.SetValues(x, zIndex, RotationAngle * position, scale, _animationDuration, EasingFunction, useAnimation);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);

            if (_itemsPresenter != null)
            {
                var visibleArea = new RectangleGeometry();
                var clip = new Rect(0, 0, _itemsPresenter.ActualWidth, _itemsPresenter.ActualHeight);

                foreach (CoverFlowItemControl item in _items)
                {
                    item.Height = _itemsPresenter.ActualHeight;
                }

                visibleArea.Rect = clip;
                _itemsPresenter.Clip = visibleArea;

                double halfWidth = _itemsPresenter.ActualWidth / 2;
                for (int index = 0; index < _items.Count; index++)
                {
                    var relativeIndex = index - SelectedIndex;
                    var position = GetRelativePosition(relativeIndex);

                    var item = _items[index];
                    var itemCenter = halfWidth + (relativeIndex * SpaceBetweenItems + SpaceBetweenSelectedItemAndItems * position);

                    item.X = itemCenter - item.ActualWidth / 2;
                    item.YRotation = RotationAngle * position;
                    item.Scale = position == 0 ? 1 : Scale;

                    LayoutChild(item, index);
                }
            }

            return size;
        }

        private int GetRelativePosition(int relativeIndex)
        {
            var position = 0;

            if (relativeIndex < 0)
            {
                position = -1;
            }
            else if (relativeIndex > 0)
            {
                position = 1;
            }

            return position;
        }

        #region Navigation Methods

        public void First()
        {
            if (_items.Count != 0)
            {
                _animationDuration = PageDuration;
                SelectedIndex = 0;
            }
        }

        public void Last()
        {
            if (_items.Count != 0)
            {
                _animationDuration = PageDuration;
                SelectedIndex = _items.Count - 1;
            }
        }

        public void NextItem()
        {
            if (SelectedIndex < _items.Count - 1)
            {
                _animationDuration = SingleItemDuration;
                SelectedIndex++;
            }
        }

        public void PreviousItem()
        {
            if (SelectedIndex > 0)
            {
                _animationDuration = SingleItemDuration;
                SelectedIndex--;
            }
        }

        public void NextPage()
        {
            if (SelectedIndex != _items.Count - 1)
            {
                _animationDuration = PageDuration;
                int i = GetPageCount();

                SelectedIndex = SelectedIndex + i >= _items.Count ? _items.Count - 1 : SelectedIndex + i;
            }
        }

        public void PreviousPage()
        {
            if (SelectedIndex != 0)
            {
                _animationDuration = PageDuration;
                int i = GetPageCount();

                SelectedIndex = SelectedIndex - i < 0 ? 0 : SelectedIndex - i;
            }
        }

        #endregion

        protected int GetPageCount()
        {
            double halfWidth = _itemsPresenter.ActualWidth / 2;
            halfWidth -= SpaceBetweenSelectedItemAndItems;
            return (int)(halfWidth / SpaceBetweenItems);
        }

        public void UpdatePositions(object value)
        {
            var item = GetItemContainerForObject(value);
            if (item != null)
            {
                int index = _items.IndexOf(item);
                LayoutChild(item, index);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static void OnValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CoverFlowControl coverFlowControl)
            {
                coverFlowControl.InvalidateArrange();
            }
        }

        private static void SelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var coverFlow = (CoverFlowControl)d;
            coverFlow.ObjectToItemContainer.TryGetValue(args.NewValue, out var item);

            if (item != null)
            {
                var newIndex = coverFlow._items.IndexOf(item);
                if (coverFlow.SelectedIndex != newIndex)
                {
                    coverFlow.SelectedIndex = newIndex;
                }
            }
        }
    }
}