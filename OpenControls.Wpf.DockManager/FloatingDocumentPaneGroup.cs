using System;
using System.Windows;

namespace OpenControls.Wpf.DockManager
{
    internal class FloatingDocumentPaneGroup : FloatingPane, IActiveDocument
    {
        internal FloatingDocumentPaneGroup() : base(new DocumentContainer())
        {
            IViewContainer.SelectionChanged += IViewContainer_SelectionChanged;
            (IViewContainer as DocumentContainer).HideCommandsButton();
        }

        private IViewModel PrevSelectedDocument;
        private void IViewContainer_SelectionChanged(object sender, EventArgs e)
        {
            FloatingViewModel floatingViewModel = DataContext as FloatingViewModel;
            System.Diagnostics.Trace.Assert(floatingViewModel != null);

            floatingViewModel.Title = Application.Current.MainWindow.Title + " - " + IViewContainer.URL;


            int i = IViewContainer.SelectedIndex;
            if (i >= 0)
            {
                IViewModel SelectedDocument = IViewContainer.GetIViewModel(i);

                SelectedDocument.isSelected = true;
                if (PrevSelectedDocument != null)
                    PrevSelectedDocument.isSelected = false;
                PrevSelectedDocument = SelectedDocument;
            }
        }

        bool IActiveDocument.IsActive
        {
            get
            {
                return IViewContainer.IsActive;
            }
            set
            {
                IViewContainer.IsActive = value;
            }
        }
    }
}
