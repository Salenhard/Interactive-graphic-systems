namespace Console_Project
{
    partial class InputsController
    {
        public static event EventHandler? OnLMBClick;
        public static event EventHandler? OnLeftPress;
        public static event EventHandler? OnRightPress;

        public static void InvokeLMBClick(object sender, EventArgs? eventArgs)
        {
            eventArgs ??= EventArgs.Empty;
            OnLMBClick?.Invoke(sender, eventArgs);
        }

        public static void InvokeLeftPress(object sender, EventArgs? eventArgs)
        {
            eventArgs ??= EventArgs.Empty;
            OnLeftPress?.Invoke(sender, eventArgs);
        }

        public static void InvokeRightPress(object sender, EventArgs? eventArgs)
        {
            eventArgs ??= EventArgs.Empty;
            OnRightPress?.Invoke(sender, eventArgs);
        }
    }
}
