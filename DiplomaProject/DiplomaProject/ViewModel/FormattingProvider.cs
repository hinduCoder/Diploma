using System.Windows;
using System.Windows.Documents;
using DiplomaProject.Controls;

namespace DiplomaProject.ViewModel
{
    public class FormattingProvider
    {
        private readonly FlowDocument _flowDocument;

        private Block LastBlock { get { return _flowDocument.Blocks.LastBlock; } }

        public FormattingProvider(FlowDocument flowDocument)
        {
            _flowDocument = flowDocument;
        }

        public void AddFormula(TextSelection selection)
        {
            AddBlock(new FormulaBlock {Formula = "f(x)"}, selection);
        }

        public void AddUnorderedList(TextSelection selection)
        {
            AddBlock(new List {ListItems = {new ListItem()}}, selection);
        }

        public void AddOrderedList(TextSelection selection)
        {
            AddBlock(new List {ListItems = {new ListItem()}, MarkerStyle = TextMarkerStyle.Decimal}, selection);
        }

        public void AddDrawing(TextSelection currentSelection) 
        {
            AddBlock(new DrawerBlock(), currentSelection);
        }

        private void AddBlock(Block block, TextSelection selection)
        {
            if (selection == null || selection.End == null)
                AddBlockToEnd(block);
            else
                _flowDocument.Blocks.InsertAfter(selection.End.Paragraph, block);
        }

        private void AddBlockToEnd(Block block)
        {
            _flowDocument.Blocks.Add(block);
        }

    }
}