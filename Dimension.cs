namespace TickspeedChallenge
{
    class Dimension
    {
        static int[] _startCosts = { 2, 3, 5, 13, 10, 14, 19, 25 };
        static int[] _stepCost = { 3, 4, 5, 6, 8, 10, 12, 15 };

        public int DimNumber { get; private set; }
        public int StartCost { get; private set; }
        public int StepCost { get; private set; }
        public int CurrentCost { get; private set; }
        public int StepsCount { get; private set; }
        public int VisibleStepsCount { get; private set; }

        public int NextCost
        {
            get { return CurrentCost + StepCost; }
        }

        public Dimension(int dimNumber)
        {
            DimNumber = dimNumber;
            StartCost = _startCosts[DimNumber - 1];
            StepCost = _stepCost[DimNumber - 1];
            CurrentCost = StartCost;
            StepsCount = 0;
        }

        public void Step(bool visible)
        {
            CurrentCost += StepCost;
            StepsCount++;

            if(visible)
                VisibleStepsCount++;
        }
    }
}