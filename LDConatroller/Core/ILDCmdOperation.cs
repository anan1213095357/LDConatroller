namespace LDConatroller.Core
{
    public interface ILDCmdOperation
    {
        public Task ClickAsync(int x, int y);
        public Task MoveToAsync(int x, int y);
        public Task KeyeventAsync(string key);
        public Task StartAppAsync(string packageName);

    }
}
