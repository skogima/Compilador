using ICSharpCode.AvalonEdit.Document;

namespace Compiler
{
    public class MainWindowViewModel : BaseViewModel
    {
        private AnaLexico analexico = new AnaLexico();
        public string LexicoResult { get; set; }
        public RelayCommand AnalisarCommand { get; set; }
        public TextDocument CodeDocument { get; set; }

        public MainWindowViewModel()
        {
            CodeDocument = new TextDocument();
            AnalisarCommand = new RelayCommand(Analisar);
        }

        private void Analisar()
        {
            if (!string.IsNullOrEmpty(CodeDocument.Text))
                LexicoResult = analexico.Analisar(CodeDocument.Text) ?? "Erro";
        }
    }
}
