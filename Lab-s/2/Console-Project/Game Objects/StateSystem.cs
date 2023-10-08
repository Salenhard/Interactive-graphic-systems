namespace Console_Project
{
    public class StateSystem
    {
        public int CurrentState { get; private set; }
        public int StatesAmount { get; private set; }
        public bool IsLoopableStates { get; private set; }

        public StateSystem(int statesAmount, int startState = 0, bool isLoopableStates = false)
        {
            if (startState >= statesAmount || startState < 0)
            {
                throw new IndexOutOfRangeException(
                    $"{nameof(startState)} was out of states amount range"
                );
            }

            CurrentState = startState;
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
                CurrentState = IsLoopableStates
                    ? StatesAmount - Math.Abs(potentialState % StatesAmount)
                    : 0;
            }
            else
            {
                CurrentState = potentialState;
            }
        }

        public void SetFirstState() => CurrentState = 0;

        public void SetLastState() => CurrentState = StatesAmount - 1;
    }
}
