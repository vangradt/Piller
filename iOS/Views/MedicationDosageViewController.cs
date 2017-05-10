using System;
using MvvmCross.iOS.Views;
using Piller.ViewModels;
using Piller.iOS.Common;
using Piller.Resources;
using MvvmCross.Binding.BindingContext;
using UIKit;
using Piller.iOS.Common.Dialog;
using System.Collections.Generic;

namespace Piller.iOS.Views
{
    public class MedicationDosageViewController : MvxTableViewController<MedicationDosageViewModel>
    {

        ElementTableSource tableSource;
        SingleLineEditElement drugNameElement;
        UIBarButtonItem saveButton;

        public MedicationDosageViewController() : base(UITableViewStyle.Grouped)
        {
           
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            tableSource = new ElementTableSource(createEditForm(), this.TableView);
            this.TableView.RowHeight = UITableView.AutomaticDimension;
            this.TableView.EstimatedRowHeight = 44f;
            this.TableView.Source = tableSource;

            this.saveButton = new UIBarButtonItem (UIKit.UIBarButtonSystemItem.Save);
            this.NavigationItem.RightBarButtonItem = saveButton;

            setBindings();

        }

        private FormDefinition createEditForm()
        {
            //drugNameElement = new SingleLineEditElement { Title = AppResources.MedicationDosageView_MedicationName };
            var drugEntrySection = new Section(String.Empty, new List<Element> { drugNameElement });

            var rootElement = new FormDefinition(new List<Section> { drugEntrySection });
       
            return rootElement;
        }

        private void setBindings()
        {
  
            var bindingSet = this.CreateBindingSet<MedicationDosageViewController, MedicationDosageViewModel>();

            bindingSet.Bind(drugNameElement)
                      .For(element => element.Value)
                      .To(viewModel => viewModel.MedicationName)
                      .TwoWay();

            bindingSet.Bind (this.saveButton).To (vm => vm.Save);
            
            bindingSet.Apply();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.tableSource.Dispose();
        }
         
    }
}
