namespace LDConatroller.Core
{
    public interface IScriptTask
    {
        delegate void LoggerEvent(string msg);
        public string ScriptTaskName { get; set; }
        public Simulator Simulator { get; set; }
        public Task Main();

        public event LoggerEvent Logger;

    }
}
