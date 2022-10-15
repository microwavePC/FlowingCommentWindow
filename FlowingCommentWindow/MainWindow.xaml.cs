using FlowingCommentWindow.Components;
using FlowingCommentWindow.Enums;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace FlowingCommentWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool _isShown = false;
        Random _random = new Random();

        readonly int _baseSpeed = 16;

        int _defaultFontSize = 108;
        int _fontSize = 108;
        int _speed = 16;
        Pos? _position = null;

        public MainWindow(string font, int defaultFontSize, FontSize size, Brush color, Brush? marginColor, Pos position, Speed speed, string comment)
        {
            InitializeComponent();

            this.Left = (int)SystemParameters.VirtualScreenWidth;
            this.Top = _random.Next(0, (int)(SystemParameters.VirtualScreenHeight - _fontSize));

            _defaultFontSize = defaultFontSize;

            // モニターの解像度に応じてコメント移動のベース速度を調整
            double monitorFullWidth = SystemParameters.VirtualScreenWidth;
            _baseSpeed = (int)(monitorFullWidth / 136);

            // コメント文章設定
            this.CommentLabel.Text = comment;

            // コメントサイズ設定
            switch (size)
            {
                case Enums.FontSize.Big:
                    _fontSize = (int)(_defaultFontSize * 1.5);
                    break;
                case Enums.FontSize.Medium:
                    _fontSize = _defaultFontSize;
                    break;
                case Enums.FontSize.Small:
                    _fontSize = (int)(_defaultFontSize * 0.5);
                    break;
                default:
                    _fontSize = _defaultFontSize;
                    break;
            }
            this.CommentLabel.FontSize = _fontSize;

            // コメントフォント設定
            if (!string.IsNullOrWhiteSpace(font))
            {
                this.CommentLabel.FontFamily = new FontFamily(font);
            }

            // コメント色設定
            this.CommentLabel.Foreground = color;

            // コメント縁色設定
            if (marginColor != null)
            {
                this.CommentLabel.SetValue(Adorning.StrokeProperty, marginColor!);
            }
            else
            {
                this.CommentLabel.SetValue(Adorning.StrokeThicknessProperty, 0.0);
            }

            // スピード設定
            switch (speed)
            {
                case Speed.Fast:
                    _speed = _baseSpeed * 2;
                    break;
                case Speed.Medium:
                    _speed = _baseSpeed;
                    break;
                case Speed.Slow:
                    _speed = _baseSpeed / 2;
                    break;
                default:
                    _speed = _baseSpeed;
                    break;
            }

            // 位置設定（レンダリング未実施のこのタイミングでは引数の保管のみ実施）
            _position = position;
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (_isShown)
            {
                return;
            }

            _isShown = true;

            // 位置の調整はレンダリング完了後のこのタイミングで行う。
            int verticalMargin = 24;
            double textTop;
            switch (_position)
            {
                case Pos.Ue:
                    textTop = verticalMargin;
                    break;
                case Pos.Naka:
                    textTop = SystemParameters.VirtualScreenHeight / 2 - this.ActualHeight / 2;
                    break;
                case Pos.Shita:
                    textTop = SystemParameters.VirtualScreenHeight - this.ActualHeight - verticalMargin;
                    break;
                case Pos.Random:
                default:
                    textTop = _random.Next(0, (int)(SystemParameters.VirtualScreenHeight - this.ActualHeight));
                    break;
            }
            this.Top = textTop;
            this.Width = this.CommentLabel.ActualWidth;

            moveComment();
        }

        private void moveComment()
        {
            this.Left = this.Left - _speed;

            if (this.Left + this.ActualWidth < 0)
            {
                this.Close();
            }
            else
            {
                Thread.Sleep(20);  // 次の移動処理まで20ms待機
                moveComment();
            }
        }

        #region コメントをマウスで触れなくするためのコード

        protected override void OnActivated(EventArgs e)
        {
            // base.OnActivated(e);

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            base.Owner.Activate();
        }

        #endregion
    }
}
