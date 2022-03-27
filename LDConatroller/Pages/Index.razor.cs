using LDConatroller.Core;

namespace LDConatroller.Pages
{
    public partial class Index : ISimulatorOperation
    {


        public async Task launchScriptAsync(int id)
        {
            SimulatorService.Simulators.First(p => p.SimulatorInfo.ID == id).Execute();
        }

        public async Task launchSimulatorAsync(int id)
        {
            SimulatorService.Simulators.FirstOrDefault(p => p.SimulatorInfo.ID == id)!.SimulatorInfo.SimulatorStatus = Models.SimulatorStatusEnum.Starting;
            await ldCmd.LaunchAsync(id);
        }

        public async Task StopScriptAsync(int id)
        {
            SimulatorService.Simulators.First(p => p.SimulatorInfo.ID == id).Quit();
        }

        public async Task StopSimulatorAsync(int id)
        {
            SimulatorService.Simulators.FirstOrDefault(p => p.SimulatorInfo.ID == id)!.SimulatorInfo.SimulatorStatus = Models.SimulatorStatusEnum.Stopped;
            await ldCmd.QuitAsync(id);
        }

    }
}
