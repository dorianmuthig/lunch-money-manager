using LunchMoneyManager.Library;
using LunchMoneyManager.Library.Models;
using System.Collections.ObjectModel;
using Terminal.Gui.App;
using Terminal.Gui.Views;
using static Terminal.Gui.Views.SpinnerStyle;

namespace LunchMoneyManager.Console
{
    public partial class MainView
    {
        private List<Debtor> debtors;

        public MainView()
        {
            
            InitializeComponent();
            debtorListView.Initialized += async (_, _) =>
            {
                try
                {
                    debtors = await LunchMoneyManager.Library.LunchMoneyManager.Instance.GetDebtors(); //todo: this retrieves only the first 10 by default! It would be good to get a data binding with pagination in the dropdown control... but this is too much work for now
                    debtorListView.Source = new ListWrapper<Debtor>(new ObservableCollection<Debtor>(debtors));
                }
                catch (Exception e)
                {
                    MessageBox.ErrorQuery(Application.Instance, e.GetType().ToString(), e.Message, "OK");
                }
            };
            addDebtorButton.Accepted += async (_, _) =>
            {
                try
                {
                    AddDebtorDialog addDebtorDialog = new AddDebtorDialog();
                    Application.Run(addDebtorDialog); //this.Prompt<AddDebtorDialog, string>(); // broken upstream, back to legacy
                    string? newDebtor = addDebtorDialog.Result;
                    if (!string.IsNullOrEmpty(newDebtor))
                    {
                        Guid debtorId = await LunchMoneyManager.Library.LunchMoneyManager.Instance.CreateDebtor(newDebtor);
                        //todo: refresh debtor list
                        debtors = await LunchMoneyManager.Library.LunchMoneyManager.Instance.GetDebtors();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.ErrorQuery(Application.Instance, e.GetType().ToString(), e.Message, "OK");
                }
            };
            addDebtButton.Accepted += async (_, _) =>
            {
                try
                {                    
                    if (debtors.Count < 1)
                    {
                        throw new ArgumentOutOfRangeException("You must add a debtor first... 😅");
                    }
                    AddDebtDialog addDebtDialog = new AddDebtDialog();
                    //addDebtDialog.Debtors = debtors;
                    Application.Run(addDebtDialog); //this.Prompt<AddDebtDialog, Transaction>(addDebtDialog); // see above
                    Transaction? newTransaction = addDebtDialog.Result;
                    //todo: it's not good to reuse the Transaction class type because hidden or unused fields get exposed this way, but I'm lazy
                    if (newTransaction != null)
                    {
                        await LunchMoneyManager.Library.LunchMoneyManager.Instance.AddDebt(newTransaction.Debtor.Id, newTransaction.Reference, newTransaction.Amount, newTransaction.InterestRate);
                        //todo: refresh transaction list and tally
                    }
                }
                catch (Exception e)
                {
                    MessageBox.ErrorQuery(Application.Instance, e.GetType().ToString(), e.Message, "OK");
                }
            };
            repayDebtButton.Accepted += async (_, _) =>
            {
                try
                {
                    throw new NotImplementedException("Debts can't be repaid yet. Yay for those who borrowed? 🤭");
                }
                catch (Exception e)
                {
                    MessageBox.ErrorQuery(Application.Instance, e.GetType().ToString(), e.Message, "OK");
                }
            };
            quitButton.Accepted += (_, _) =>
            {
                try
                {
                    Application.Instance.RequestStop();
                }
                catch (Exception e)
                {
                    MessageBox.ErrorQuery(Application.Instance, e.GetType().ToString(), e.Message, "OK");
                }
            };

        }
    }
}
