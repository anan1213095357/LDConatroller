using LDConatroller.Models;

namespace LDConatroller.Core
{
    public class SimulatorBase : ILDCmdOperation, IScriptsOperation
    {

        public SimulatorBase(LdCmd ldCmd)
        {
            this.ldCmd = ldCmd;
        }
        public SimulatorInfo SimulatorInfo { get; set; } = new();

        public List<IScriptTask> ScriptTasks { get; set; } = new();
        public IScriptTask CurrScriptTask { get; set; }

        private Task? task;

        CancellationTokenSource tokenSource = new();
        private readonly LdCmd ldCmd;

        public async Task ClickAsync(int x, int y)
        {
            await ldCmd.TapAsync(SimulatorInfo.ID, x, y);
        }
        public async Task KeyeventAsync(string key)
        {
            await ldCmd.KeyeventAsync(SimulatorInfo.ID, key);
        }

        public async Task MoveToAsync(int x, int y)
        {
            await ldCmd.TapAsync(SimulatorInfo.ID, x, y);
        }
        public async Task StartAppAsync(string packageName)
        {
            await ldCmd.StartAppAsync(SimulatorInfo.ID, packageName);
        }

        public void Execute()
        {

            CurrScriptTask.Main();
            task = Task.Run(() => CurrScriptTask.Main(), tokenSource.Token);
        }
        public void Quit()
        {
            if (task == null)
                throw new NotImplementedException("没有脚本正在运行！");
            tokenSource.Cancel();
        }

    }
}
