using System.Collections.Generic;
using UnityEngine;

public abstract class AGenerator : MonoBehaviour
{
    public Stack<FractalState> previousGenerations = new Stack<FractalState>();
    public FractalState currentState;

    public int maxIterations;
    protected float iterationInverse;

    public abstract void Generate();
    public abstract void Generate(FractalState state);
    public void AddToHistory()
    {
            FractalState state = currentState.Clone();
            previousGenerations.Push(state);;
    }
    public void AddToHistory(FractalState state)
    {
        MainThreadDispatcher.RunOnMainThread(() => {
            FractalState state = currentState.Clone();

            previousGenerations.Push(state); ;
        });
    }
    public void ChangeMaxIteration(int maxIterations)
    {
        this.maxIterations = maxIterations;
        currentState.iterationsUsed = maxIterations;
    }
    public abstract void Undo();
}
