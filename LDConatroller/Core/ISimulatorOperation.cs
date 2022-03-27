namespace LDConatroller.Core
{
    public interface ISimulatorOperation
    {
        public Task launchSimulatorAsync(int id);
        public Task StopSimulatorAsync(int id);
        public Task launchScriptAsync(int id);
        public Task StopScriptAsync(int id);


    }
}
