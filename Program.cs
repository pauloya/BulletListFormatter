using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Css.Dom;
using AngleSharp.Html.Dom;


namespace BulletListFormatter
{
    /// <summary>
    /// https://chat.openai.com/chat/b6894f2e-b60b-43dc-bb18-e1b5f8493b38
    /// </summary>
    class Program
    {
        [STAThread]
        static async Task Main(string[] args)
        {
            string htmlContent = GetClipboardAsHtml();
            if (string.IsNullOrWhiteSpace(htmlContent))
            {
                string msg = "Could not get the HTML from the clipboard!";
                Console.Error.WriteLine(msg);
                throw new Exception(msg);
            }
            string formattedList = await ProcessBulletList(htmlContent);

            Console.WriteLine(formattedList);

            SetClipboardText(formattedList);
            Console.WriteLine("The formatted list has been copied to the clipboard.");
        }

        static string GetClipboardAsHtml()
        {
            string result = string.Empty;
            var thread = new System.Threading.Thread(() =>
            {
                result = Clipboard.GetText(TextDataFormat.Html);
            });

            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
            thread.Join();
            return result;
        }

        static void SetClipboardText(string text)
        {
            var thread = new System.Threading.Thread(() =>
            {
                Clipboard.SetText(text);
            });

            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
            thread.Join();
        }


        static async Task<string> ProcessBulletList(string html)
        {
            var config = Configuration.Default.WithCss();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(html));

            var sb = new StringBuilder();
            ProcessList(document.DocumentElement, sb);

            return sb.ToString();
        }


        static void ProcessList(IElement element, StringBuilder sb)
        {
            foreach (var child in element.Children)
            {
                if (child.NodeName == "LI")
                {
                    int indentationLevel = ComputeIndentationFromMarginLeft(child);
                    sb.Append(new string('\t', indentationLevel));
                    sb.Append("- ");
                    sb.AppendLine(child.TextContent.Trim());
                }
                else if (child.NodeName == "SPAN")
                {
                    sb.AppendLine(child.TextContent.Trim());
                }
                else
                {
                    ProcessList(child, sb);
                }
            }
        }


        private static int ComputeIndentationFromMarginLeft(IElement element)
        {
            var style = element.ComputeCurrentStyle();
            var marginLeftStr = style.GetMarginLeft();

            var marginLeft = decimal.Parse(marginLeftStr.TrimEnd("px".ToCharArray()));

            //18 24 = 1
            //54 72 = 2
            //90 120 = 3

            int result = (int)((marginLeft / 48) + 0.5m);
            return result;

        }
    }
}
