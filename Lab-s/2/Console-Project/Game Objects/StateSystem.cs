namespace Console_Project
{
    public class StateSystem
    {
        public int CurrentState { get; private set; }
        public int StatesAmount { get; private set; }
        public bool IsLoopableStates { get; private set; }

        public StateSystem(int statesAmount, bool isLoopableStates)
        {
            CurrentState = 0;
            StatesAmount = statesAmount;
            IsLoopableStates = isLoopableStates;
        }

        public void NextState(int stepSize = 1)
        {
            var potentialState = CurrentState + stepSize;

            if (potentialState >= StatesAmount)
            {
                CurrentState = IsLoopableStates ? potentialState % StatesAmount : StatesAmount - 1;
            }
            else
            {
                CurrentState = potentialState;
            }
        }

        public void PreviousState(int stepSize = 1)
        {
            var potentialState = CurrentState - stepSize;

            if (potentialState < 0)
            {
                CurrentState = IsLoopableStates ? Math.Abs(potentialState % StatesAmount) : 0;
            }
            else
            {
                CurrentState = potentialState;
            }
        }
    }
}
