
namespace DrunkDial
{
    using System;
    using System.Windows.Input;

    public class DeleteContactCommand : ICommand
    {
        public void Execute(object parameter)
        {
            ContactGridViewModel.Instance.RemoveContactById(parameter as string);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
