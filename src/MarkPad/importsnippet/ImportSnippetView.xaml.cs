using System.Windows.Input;

namespace MarkPad.ImportSnippet
{
    public partial class ImportSnippetView
    {
        public ImportSnippetView()
        {
            InitializeComponent();
        }

        private void DragMoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Pressed && e.MiddleButton != MouseButtonState.Pressed)
                DragMove();
        }

    }
}
