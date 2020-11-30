using System;
using System.Collections.Generic;
using SadPumpkin.Util.Context;

namespace SadPumpkin.Util.LookupTree.Nodes
{
    public abstract class EvaluatorBase_Boolean : IEvaluator
    {
        public bool ExpectedResult = true;

        public abstract bool Execute(IContext bb);

        public static bool GetResult(bool expectedResult, bool inValue)
        {
            return expectedResult == inValue;
        }
    }

    public enum IntOperationType
    {
        Equals = 0,
        LessThan = 1,
        LessThanOrEqualTo = 2,
        BetweenInclusive = 3,
        BetweenExclusive = 4,
        GreaterThan = 5,
        GreaterThanOrEqualTo = 6,
    }

    public abstract class EvaluatorBase_Int : IEvaluator
    {
        public int MinExpectedResult = 0;
        public int MaxExpectedResult = 0;
        public IntOperationType Operation = IntOperationType.Equals;

        public abstract bool Execute(IContext bb);

        public static bool GetResult(int minExpectedResult, int maxExpectedResult, IntOperationType opType, int inValue)
        {
            switch (opType)
            {
                case IntOperationType.Equals:
                    return inValue == minExpectedResult;
                case IntOperationType.LessThan:
                case IntOperationType.GreaterThan:
                case IntOperationType.BetweenExclusive:
                    return inValue > minExpectedResult && inValue < maxExpectedResult;
                case IntOperationType.LessThanOrEqualTo:
                case IntOperationType.GreaterThanOrEqualTo:
                case IntOperationType.BetweenInclusive:
                    return inValue >= minExpectedResult && inValue <= maxExpectedResult;
                default:
                    throw new InvalidOperationException($"Unhandled IntOperationType of {opType} in GetResult()!");
            }
        }
    }

    public enum StringOperationType
    {
        Equals = 0,
        StartsWith = 1,
        Contains = 2,
        ContainedIn = 3
    }

    public abstract class EvaluatorBase_String : IEvaluator
    {
        public string ExpectedValue = string.Empty;
        public StringOperationType Operation = StringOperationType.Contains;

        public abstract bool Execute(IContext bb);

        public static bool GetResult(string expectedValue, StringOperationType opType, string inValue)
        {
            if (inValue is null)
                return false;
            
            switch (opType)
            {
                case StringOperationType.Equals:
                    return inValue.Equals(expectedValue, StringComparison.InvariantCultureIgnoreCase);
                case StringOperationType.StartsWith:
                    return inValue.StartsWith(expectedValue, StringComparison.InvariantCultureIgnoreCase);
                case StringOperationType.Contains:
                    return inValue.Contains(expectedValue);//, StringComparison.InvariantCultureIgnoreCase);
                case StringOperationType.ContainedIn:
                    return expectedValue.Contains(inValue);//, StringComparison.InvariantCultureIgnoreCase);
                default:
                    throw new InvalidOperationException($"Unhandled StringOperationType of {opType} in GetResult()!");
            }
        }
    }
    
    public enum SetOperationType
    {
        Overlaps = 0,     // expectedValues.Overlaps(inValues)
        SupersetOf = 1,   // expectedValues.IsSupersetOf(inValues)
        EqualTo = 2,      // expectedValues.SetEquals(inValues)
        SubsetOf = 3,     // expectedValues.IsSubsetOf(inValues)
    }
    
    public abstract class EvaluatorBase_List<T> : IEvaluator
    {
        public readonly HashSet<T> ExpectedValues = new HashSet<T>();
        public SetOperationType Operation = SetOperationType.Overlaps;
        public bool ExpectedResult = true;

        public abstract bool Execute(IContext bb);

        public static bool GetResult(HashSet<T> expectedValues, SetOperationType opType, bool expectedResult, IEnumerable<T> inValues)
        {
            switch (opType)
            {
                case SetOperationType.Overlaps:
                    return expectedResult == expectedValues.Overlaps(inValues);
                case SetOperationType.SupersetOf:
                    return expectedResult == expectedValues.IsSupersetOf(inValues);
                case SetOperationType.EqualTo:
                    return expectedResult == expectedValues.SetEquals(inValues);
                case SetOperationType.SubsetOf:
                    return expectedResult == expectedValues.IsSubsetOf(inValues);
                default:
                    throw new ArgumentOutOfRangeException(nameof(opType), opType, null);
            }
        }

        public static bool GetResult(HashSet<T> expectedValues, SetOperationType opType, bool expectedResult, T inValue)
        {
            switch (opType)
            {
                case SetOperationType.Overlaps:
                case SetOperationType.SupersetOf:
                    return expectedResult == expectedValues.Contains(inValue);
                case SetOperationType.EqualTo:
                case SetOperationType.SubsetOf:
                    return expectedResult == (expectedValues.Count == 1 && expectedValues.Contains(inValue));
                default:
                    throw new ArgumentOutOfRangeException(nameof(opType), opType, null);
            }
        }
    }

    public enum DateTimeOperationType
    {
        Before = 0,
        Between = 1,
        After = 2
    }
    
    public abstract class EvaluatorBase_DateTime : IEvaluator
    {
        public long MinExpectedResult = 0;
        public long MaxExpectedResult = 0;
        public DateTimeOperationType Operation = DateTimeOperationType.After;

        public abstract bool Execute(IContext bb);

        public static bool GetResult(long minExpectedResult, long maxExpectedResult, DateTime inValue)
        {
            long ticks = inValue.Ticks;
            return ticks > minExpectedResult && ticks < maxExpectedResult;
        }
    }
}