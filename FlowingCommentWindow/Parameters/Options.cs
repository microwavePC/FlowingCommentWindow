using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowingCommentWindow.Parameters
{
    public class Options
    {
        [Option('f', "default-font", Required = false, HelpText = "デフォルトのフォント。ユーザーがコメント内のパラメーターでフォントを指定していなかった場合、このパラメーターで指定されたフォントが使用される。")]
        public string DefaultFont { get; set; } = string.Empty;

        [Option('s', "default-font-size", Required = false, HelpText = "デフォルトのフォントサイズ。このパラメーターで指定された数値が標準のフォントサイズとなる。")]
        public int DefaultFontSize { get; set; } = 108;

        [Option('c', "comment", Required = true, HelpText = "コメント本体。")]
        public string Comment { get; set; } = "<コメント文字列がプログラムに渡されていません。>";
    }
}
