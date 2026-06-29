using LunchMoneyManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using Terminal.Gui.App;
using Terminal.Gui.Drawing;
using Terminal.Gui.Drivers;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace LunchMoneyManager.Console
{
    public class AddDebtDialog: Dialog<Transaction>
    {
        private TextValidateField moneyInput;
        private DropDownList debtorInput;
        private TextView referenceInput;
        public List<KeyValuePair<Guid, string>> Debtors;

        public AddDebtDialog()
        {
            Width = Dim.Percent(80);
            Height = Dim.Percent(90);
            X = Pos.Center();
            Y = Pos.Center();
            
            Add(new Label { Text = "Debtor:" });
            this.Initialized += async (_, _) =>
             {
                 // screw it, either it's broken, or I can't figure out how to pass an object into a view by reference
                 try
                 {
                     List<Debtor> debtors = await LunchMoneyManager.Library.LunchMoneyManager.Instance.GetDebtors();
                     Debtors = new List<KeyValuePair<Guid, string>>();
                     foreach (Debtor debtor in debtors)
                     {
                         Debtors.Add(new KeyValuePair<Guid, string>(debtor.Id, debtor.Name));
                     }
                     debtorInput.Source = new ListWrapper<KeyValuePair<Guid, string>>(new ObservableCollection<KeyValuePair<Guid, string>>(Debtors));
                 }
                 catch (Exception e)
                 {
                     MessageBox.ErrorQuery(Application.Instance, e.GetType().ToString(), e.Message, "OK");
                 }
             };
            debtorInput = new DropDownList
            {
                ReadOnly = true,
                // Source = new ListWrapper<Debtor>(new ObservableCollection<Debtor>(Debtors)),
                Y = 2,
                Width = Dim.Fill()
            };
            debtorInput.Y = 2;
            debtorInput.Width = Dim.Fill();
            Add(debtorInput);
            Add(new Label { Text = "Amount:", Y = 4 });
            moneyInput = new TextValidateField { Provider = new TextRegexProvider("\\d+([,\\.]\\d\\d?)?") };
            moneyInput.Y = 6;
            moneyInput.Width = Dim.Fill();
            Add(moneyInput);
            Add(new Label { Text = "Reference:", Y = 8 });
            referenceInput = new TextView
            {
                ReadOnly = false,
                Multiline = true,
                ScrollBars = true,
                TabKeyAddsTab = false,
                Y = 10,
                Width = Dim.Fill(),
                Height = 8,
                BorderStyle = LineStyle.HeavyDotted
            };
            Add(referenceInput);
            AddButton(new Button() { Text = "_Cancel" });
            AddButton(new Button() { Text = "_Ok" });
        }

        protected override bool OnAccepting(CommandEventArgs args)
        {
            if (base.OnAccepting(args))
            {
                return true;
            }
            Result = new Transaction 
            {
                Debtor = new Debtor { Id = new Guid(debtorInput.Value.TrimStart('[').TrimEnd(']').Split(',')[0]) },
                Amount = float.Parse(moneyInput.Value.Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat),
                InterestRate = 0, //todo: we're not bothering with interest right now
                Reference = referenceInput.Text
            };
            RequestStop();
            return false;
        }
    }
}
