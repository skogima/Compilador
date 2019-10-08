namespace Compiler
{
    public class MainWindowViewModel : BaseViewModel
    {
        private AnaLexico analexico = new AnaLexico();
        public string Instruction { get; set; }
        public string LexicoResult { get; set; }
        public RelayCommand AnalisarCommand { get; set; }

        public MainWindowViewModel()
        {
            AnalisarCommand = new RelayCommand(Analisar);
        }

        private void Analisar()
        {
            if (!string.IsNullOrEmpty(Instruction))
                LexicoResult = analexico.Analisar(Instruction) ?? "Erro";
        }
    }
}
