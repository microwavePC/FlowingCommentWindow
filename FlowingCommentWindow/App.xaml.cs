using CommandLine;
using FlowingCommentWindow.Enums;
using FlowingCommentWindow.Extensions;
using FlowingCommentWindow.Parameters;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Windows;
using System.Windows.Media;

namespace FlowingCommentWindow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            #region コマンドラインで指定されたオプションをパース

            var parsedResult = Parser.Default.ParseArguments<Options>(e.Args);
            Options? options = null;

            switch (parsedResult.Tag)
            {
                case ParserResultType.Parsed:
                    // パース成功
                    options = parsedResult.Value;
                    break;
                default:
                    // パース失敗
                    throw new ArgumentException("コマンドラインオプションまたはその値に誤りがあります。");
            }

            string defaultFont = options.DefaultFont;
            int defaultFontSize = options.DefaultFontSize;
            string wholeComment = options.Comment;

            #endregion

            #region コメント内で指定されたオプションをパース

            string[] commentParts = wholeComment.Split("\\?");
            string commentParam = commentParts[1];
            Uri dummyUri = new Uri("http://www.example.com?" + commentParam);
            NameValueCollection qParams = HttpUtility.ParseQueryString(dummyUri.Query);

            // フォントの種類
            string font = qParams.Get("font") ?? defaultFont;

            // フォントサイズ
            FontSize size;
            if (!Enum.TryParse(qParams.Get("size").ToTitleCase(), out size))
            {
                size = FontSize.Medium;
            }

            // コメントの位置
            Pos pos;
            if (!Enum.TryParse(qParams.Get("pos").ToTitleCase(), out pos))
            {
                pos = Pos.Random;
            }

            // コメントのスピード
            Speed speed;
            if (!Enum.TryParse(qParams.Get("speed").ToTitleCase(), out speed))
            {
                speed = Speed.Medium;
            }

            // コメントの色
            Brush color = Brushes.White;
            if (qParams.Get("color") != null)
            {
                string colStr = qParams.Get("color").ToTitleCase()!;
                var bConv = new BrushConverter();
                SolidColorBrush? brush = bConv.ConvertFromString(colStr) as SolidColorBrush;
                if (brush != null)
                {
                    color = brush;
                }
            }

            // コメント本体
            string comment = commentParts[0];

            #endregion

            var commentWindow = new MainWindow(font, defaultFontSize, size, color, pos, speed, comment);
            commentWindow.Show();
        }
    }
}
