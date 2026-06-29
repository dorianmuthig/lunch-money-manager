using LunchMoneyManager.Console;
using LunchMoneyManager.Library;
using Terminal.Gui.App;
using Terminal.Gui.Views;

LunchMoneyManager.Library.LunchMoneyManager lunchMoney = LunchMoneyManager.Library.LunchMoneyManager.Instance;

Application.Init();

try
{
    Application.Run(new MainView());
}
catch (Exception e)
{
    ConsoleColor foregroundColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(e.GetType().ToString());
    Console.ForegroundColor = foregroundColor;
    Console.WriteLine(string.Concat(e.Message, Environment.NewLine));
    Console.WriteLine(e.StackTrace);
}
finally
{
    Application.Shutdown();
}