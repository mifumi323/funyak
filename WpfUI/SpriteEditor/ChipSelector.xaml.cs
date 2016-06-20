using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MifuminSoft.funyak.View.Resource;

namespace MifuminSoft.funyak.UI.SpriteEditor
{
    /// <summary>
    /// Spriteを編集のため表示・選択します
    /// </summary>
    public partial class ChipSelector : UserControl
    {
        /// <summary>
        /// 表示・選択するSpriteを取得または設定します
        /// </summary>
        [Description("表示・選択するSpriteを取得または設定します")]
        public Sprite Source
        {
            get { return (Sprite)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(Sprite), typeof(ChipSelector), new PropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));

        /// <summary>
        /// 選択中の項目を取得または設定します
        /// </summary>
        [Description("選択中の項目を取得または設定します")]
        public KeyValuePair<string, SpriteChipInfo>? SelectedItem
        {
            get { return (KeyValuePair<string, SpriteChipInfo>?)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(KeyValuePair<string, SpriteChipInfo>?), typeof(ChipSelector), new PropertyMetadata(null, new PropertyChangedCallback(OnSelectionChanged)));

        /// <summary>
        /// 選択中のチップを取得します
        /// </summary>
        [Description("選択中のチップを取得します")]
        public SpriteChipInfo SelectedChip
        {
            get { return SelectedItem?.Value; }
        }

        /// <summary>
        /// 選択中のキーを取得します
        /// </summary>
        [Description("選択中のキーを取得します")]
        public string SelectedKey
        {
            get { return SelectedItem?.Key; }
        }

        /// <summary>
        /// 項目の表示色を取得または設定します
        /// </summary>
        [Description("項目の表示色を取得または設定します")]
        public Color ChipColor
        {
            get { return (Color)GetValue(ChipColorProperty); }
            set { SetValue(ChipColorProperty, value); }
        }
        public static readonly DependencyProperty ChipColorProperty =
            DependencyProperty.Register(nameof(ChipColor), typeof(Color), typeof(ChipSelector), new PropertyMetadata(Colors.Black, new PropertyChangedCallback(OnSourceChanged)));

        /// <summary>
        /// 選択中の項目の表示色を取得または設定します
        /// </summary>
        [Description("選択中の項目の表示色を取得または設定します")]
        public Color SelectedChipColor
        {
            get { return (Color)GetValue(SelectedChipColorProperty); }
            set { SetValue(SelectedChipColorProperty, value); }
        }
        public static readonly DependencyProperty SelectedChipColorProperty =
            DependencyProperty.Register(nameof(SelectedChipColor), typeof(Color), typeof(ChipSelector), new PropertyMetadata(Colors.Red, new PropertyChangedCallback(OnSourceChanged)));

        /// <summary>
        /// 選択中の項目が別の項目に変化したときに発生します
        /// </summary>
        public event EventHandler SelectionChanged;

        public ChipSelector()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 表示を更新します
        /// </summary>
        public void Refresh()
        {
            canvas.Children.RemoveRange(1, canvas.Children.Count - 1);
            var source = Source;
            var selectedKey = SelectedKey;
            var chipStroke = new SolidColorBrush(ChipColor);
            var selectedChipStroke = new SolidColorBrush(SelectedChipColor);
            var chipFill = new SolidColorBrush(Colors.Transparent);
            var selectedChipFill = new SolidColorBrush(Colors.Transparent);
            if (source != null)
            {
                source.SetToRectangle(background, "", 0, 0, 1.0);
                foreach (var item in source.Chip)
                {
                    var key = item.Key;
                    var chip = item.Value;
                    var selected = key == selectedKey;
                    var rectangle = new Rectangle()
                    {
                        Stroke = selected ? selectedChipStroke : chipStroke,
                        Fill = selected ? selectedChipFill : chipFill,
                        Width = chip.SourceWidth,
                        Height = chip.SourceHeight,
                        Tag = (KeyValuePair<string, SpriteChipInfo>?)item,
                    };
                    rectangle.MouseDown += Chip_MouseDown;
                    canvas.Children.Add(rectangle);
                    Canvas.SetLeft(rectangle, chip.SourceLeft);
                    Canvas.SetTop(rectangle, chip.SourceTop);
                    Panel.SetZIndex(rectangle, selected ? 2 : 1);
                }
            }
            else
            {
                background.Fill = null;
            }
        }

        private void Chip_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedItem = (sender as Rectangle)?.Tag as KeyValuePair<string, SpriteChipInfo>?;
            e.Handled = true;
        }

        private void background_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedItem = null;
            e.Handled = true;
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChipSelector)?.Refresh();
        }

        private static void OnSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chipSelector = d as ChipSelector;
            if (chipSelector == null) return;
            chipSelector.Refresh();
            chipSelector.SelectionChanged?.Invoke(chipSelector, EventArgs.Empty);
        }
    }
}
