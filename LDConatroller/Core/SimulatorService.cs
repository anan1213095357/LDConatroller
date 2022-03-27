
using LDConatroller.Models;
using System.Reflection;

namespace LDConatroller.Core
{
    public class SimulatorService : BackgroundService
    {
        private readonly LdCmd ldCmd;

        public SimulatorService(LdCmd ldCmd)
        {
            this.ldCmd = ldCmd;
        }


        //public List<SimulatorModel> Simulators { get; set; } = new();
        public List<Simulator> Simulators { get; set; } = new();


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            


            while (!stoppingToken.IsCancellationRequested)
            {
                var tempList = (await ldCmd.ListSimulatorAsync()).Select(p =>
                {
                    string[] strs = p.Split(",");

                    var simulatorInfo = new SimulatorInfo
                    {
                        ID = Convert.ToInt32(strs[0]),
                        Name = strs[1],
                        TopHwnd = new IntPtr(Convert.ToInt64(strs[2])),
                        BindHwnd = new IntPtr(Convert.ToInt64(strs[3])),
                        IsRun = strs[4],
                        Pid = new IntPtr(Convert.ToInt64(strs[5])),
                        VboxPid = new IntPtr(Convert.ToInt64(strs[6])),

                    };
                    var simulator = new Simulator(ldCmd)
                    {
                        SimulatorInfo = simulatorInfo
                    };
                    return simulator;
                });



                


                foreach (var simulator in Simulators)
                {
                    simulator.CurrScriptTask.Logger +=  s =>
                    {
                        Simulators.First(p => p.SimulatorInfo.ID == simulator.SimulatorInfo.ID).SimulatorInfo.PrintStr = $@"""{s}""";
                    };
                }

                foreach (var item in tempList)
                {

                    var simulator = Simulators.FirstOrDefault(p => p.SimulatorInfo.ID == item.SimulatorInfo.ID);

                    if (simulator != null)
                    {

                        simulator.SimulatorInfo.IsRun = item.SimulatorInfo.IsRun;
                        simulator.SimulatorInfo.TopHwnd = item.SimulatorInfo.TopHwnd;
                        simulator.SimulatorInfo.BindHwnd = item.SimulatorInfo.BindHwnd;
                        simulator.SimulatorInfo.Pid = item.SimulatorInfo.Pid;
                        simulator.SimulatorInfo.VboxPid = item.SimulatorInfo.VboxPid;

                    }
                    else
                    {
                        try
                        {
                            var scriptTasks = Assembly.GetExecutingAssembly().GetTypes().Where(p => p?.GetInterface("IScriptTask")?.Name.Contains(nameof(IScriptTask)) ?? false).Select(p => Activator.CreateInstance(p) as IScriptTask)
                                .ToList() ?? new();

                            item.ScriptTasks = scriptTasks;
                            item.CurrScriptTask = scriptTasks.First();

                            foreach (var script in scriptTasks)
                            {
                                script.Simulator = item;
                            }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

                        Simulators.Add(item);
                    }
                        
                }

                await Task.Delay(1000, stoppingToken);
            }

        }
    }
}
