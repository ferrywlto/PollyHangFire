namespace PollyHangFire;

public class SimulatedJob {
    private static int _callCount;
    public int DoSomething() {
        _callCount += 1;
        if(_callCount % 5 != 0) throw new Exception("Simulated error");

        Console.WriteLine("Some lengthy operations...");
        Task.Delay(1000);

        return _callCount;
    }
}
