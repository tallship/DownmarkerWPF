using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace MarkPad.Document
{
    public partial class DocumentView
    {
        public DocumentView()
        {
            InitializeComponent();
            Loaded += DocumentViewLoaded;
        }

        private void DocumentViewLoaded(object sender, RoutedEventArgs e)
        {
            using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream("MarkPad.Syntax.Markdown.xshd"))
            using (var reader = new XmlTextReader(stream))
            {
                Document.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }

            var editCommandBindings = Document.TextArea.DefaultInputHandler.Editing.CommandBindings;
            foreach (var binding in editCommandBindings.Where(binding => binding.Command == ICSharpCode.AvalonEdit.AvalonEditCommands.IndentSelection))
            {
                editCommandBindings.Remove(binding);
                break;
            }

            Document.TextArea.KeyDown += TextArea_KeyDown;
        }

        void TextArea_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.B && Keyboard.Modifiers == ModifierKeys.Control)
            {
                BoldSelectedArea();
                e.Handled = true;
            }

            else if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (e.Key == Key.I)
                {
                    ItalicsSelectedArea();
                    e.Handled = true;
                }
            }
        }

        private string RetrieveSelectedArea()
        {
            var textArea = this.Document.TextArea;
            if (textArea.Selection.IsEmpty)
            {
                var line = textArea.Document.GetLineByOffset(textArea.Caret.Offset);
                textArea.Selection = textArea.Selection.StartSelectionOrSetEndpoint(line.Offset, line.Offset + line.Length);
            }

            return textArea.Selection.GetText(textArea.Document);
        }

        public void BoldSelectedArea()
        {
            string selectedText = RetrieveSelectedArea();

            if ((selectedText.StartsWith("**") && selectedText.EndsWith("**")) ||
                (selectedText.StartsWith("***") && selectedText.EndsWith("***")))
                Document.TextArea.Selection.ReplaceSelectionWithText(Document.TextArea, selectedText.Replace("**", ""));
            else Document.TextArea.Selection.ReplaceSelectionWithText(Document.TextArea, "**" + selectedText + "**");

        }

        public void ItalicsSelectedArea()
        {
            var selectedText = RetrieveSelectedArea();

            if (selectedText.StartsWith("***") &&
                selectedText.EndsWith("***"))
            {
                Document.TextArea.Selection.ReplaceSelectionWithText(Document.TextArea, selectedText.Replace("***", "**"));
            }
            else if (selectedText.StartsWith("*") &&
                selectedText.EndsWith("*") &&
                !selectedText.StartsWith("**") &&
                !selectedText.EndsWith("**"))
            {
                Document.TextArea.Selection.ReplaceSelectionWithText(Document.TextArea, selectedText.Replace("*", ""));
            }
            else Document.TextArea.Selection.ReplaceSelectionWithText(Document.TextArea, "*" + selectedText + "*");
        }
    }
}
