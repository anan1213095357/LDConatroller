using LDConatroller.Core;

namespace LDConatroller.Models
{
    public class SimulatorInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public IntPtr TopHwnd { get; set; }
        public IntPtr BindHwnd { get; set; }
        public string IsRun { get; set; } = "";
        public IntPtr Pid { get; set; }
        public IntPtr VboxPid { get; set; }
        public bool Selected { get; set; } = false;
        public SimulatorStatusEnum SimulatorStatus { get; set; } = SimulatorStatusEnum.Stopped;
        public string? PrintStr { get; set; }

    }


    public enum SimulatorStatusEnum
    {
        Starting,
        Running,
        Stopped
    }

}
