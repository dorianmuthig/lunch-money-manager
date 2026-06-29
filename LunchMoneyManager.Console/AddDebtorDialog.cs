using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace LunchMoneyManager.Console
{
    public class AddDebtorDialog: Dialog<string>
    {
        private TextField input;
        public AddDebtorDialog()
        {
            input = new TextField { Width = Dim.Fill() - 12, X = Pos.AnchorEnd() };
            Add(new Label { Text = "Enter Name:" });
            Add(input);
            AddButton(new Button() { Text = "_Cancel" });
            AddButton(new Button() { Text = "_Ok" });
        }
        protected override bool OnAccepting(CommandEventArgs args)
        {
            if (base.OnAccepting(args))
            {
                return true;
            }
            Result = input.Text;
            RequestStop();
            return false;
        }
    }
}
