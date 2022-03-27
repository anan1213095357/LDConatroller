using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace LDConatroller
{
    public class LdCmd
    {
        private string simulatorPath;

        SemaphoreSlim slimlock = new(1, 1);

        public LdCmd(string path)
        {
            if (path != "")
            {
                simulatorPath = path;
                CleanAsync().Wait();
            }
            else
                throw new ArgumentException();
        }

        public async Task<string> CleanAsync()
        {
            return await ImplementCmdAsync(
                string.Format("{0}dnconsole globalsetting --fps 10 --audio 0  --fastplay 1 --cleanmode 1", simulatorPath));
        }
        /// <summary>
        /// 清理应用数据
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <param name="packageName">App包名</param>
        /// <returns></returns>
        public async Task<string> ClearAppDataAsync(int index, string packageName)
        {
            return await ImplementCmdAsync(
                string.Format("{0}ld -s {1} pm clear {2}", simulatorPath, index, packageName));
        }
        /// <summary>
        /// 安装应用
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <param name="packagePath">模拟器路径</param>
        /// <returns></returns>
        public async Task<string> InstallAPPAsync(int index, string packagePath)
        {

            return await ImplementCmdAsync(
                string.Format("{0}dnconsole installapp --index {1} --filename {2}", simulatorPath, index, packagePath));
        }
        /// <summary>
        /// 卸载应用
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <param name="packageName">APP包名</param>
        /// <returns></returns>
        public async Task<string> UnInstallAppAsync(int index, string packageName)
        {
            return await ImplementCmdAsync(
                string.Format("{0}dnconsole uninstallapp --index {1} --filename {2}", simulatorPath, index, packageName));
        }
        /// <summary>
        /// 启动应用
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <param name="value">包名/Activity类名</param>
        /// <returns></returns>
        public async Task<string> StartAppAsync(int index, string packageName)
        {
            return await ImplementCmdAsync(
                string.Format("{0}dnconsole.exe runapp --index {1} --packagename {2}", simulatorPath, index, packageName));
        }
        /// <summary>
        /// 模拟鼠标点击
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <param name="value">点击位置 x y</param>
        /// <returns></returns>
        public async Task<string> TapAsync(int index, int x, int y)
        {
            return await ImplementCmdAsync($"{simulatorPath}ld -s {index} input tap {x} {y}");
        }
        /// <summary>
        /// 模拟按键
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <param name="value">按键值</param>
        /// <returns></returns>
        public async Task<string> KeyeventAsync(int index, string value)
        {
            return await ImplementCmdAsync(
                string.Format("{0}dnconsole action --index {1} --key call.keyboard --value {2}", simulatorPath, index, value));
        }
        /// <summary>
        /// 打开模拟器
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <returns></returns>
        public async Task<string> LaunchAsync(int index)
        {

            return await ImplementCmdAsync(
                string.Format("{0}dnconsole launch --index {1}", simulatorPath, index));
        }
        /// <summary>
        /// 退出指定模拟器
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <returns></returns>
        public async Task<string> QuitAsync(int index)
        {

            return await ImplementCmdAsync(
                string.Format("{0}dnconsole quit --index {1}", simulatorPath, index));
        }
        /// <summary>
        /// 重启模拟器
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <param name="packageName">启动后并打开 packagename 应用, null 表示不打开任何应用</param>
        /// <returns></returns>
        public async Task<string> RebootAsync(int index, string packageName = null)
        {
            if (packageName == null)
            {
                return await ImplementCmdAsync(
                    string.Format("{0}dnconsole action --index {1} --key call.reboot --value null",
                    simulatorPath, index));
            }
            return await ImplementCmdAsync(
                string.Format("{0}dnconsole action --index {1} --key call.reboot --value {2}",
                simulatorPath, index, packageName));
        }
        /// <summary>
        /// CPU优化
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <param name="rate">0~100</param>
        /// <returns></returns>
        public async Task<string> DowncpuAsync(int index, int rate)
        {

            return await ImplementCmdAsync(
                string.Format("{0}dnconsole downcpu {1} --rate {2}",
                simulatorPath, index, rate));

        }
        /// <summary>
        /// 新增模拟器
        /// </summary>
        /// <param name="name">模拟器名字 默认为NULL</param>
        /// <returns></returns>
        public async Task<string> AddSimulatorAsync(string name = null)
        {
            if (name == null)
            {
                return await ImplementCmdAsync(string.Format("{0}dnconsole add", simulatorPath));
            }
            return await ImplementCmdAsync(string.Format("{0}dnconsole add --name {1}", simulatorPath, name));
        }
        /// <summary>
        /// 复制模拟器
        /// </summary>
        /// <param name="index">要复制的模拟器序号</param>
        /// <param name="name">模拟器名字  默认为NULL</param>
        /// <returns></returns>
        public async Task<string> CopySimulatorAsync(int index, string name = null)
        {

            if (name == null)
            {
                return await ImplementCmdAsync(string.Format("{0}dnconsole copy --from {1}", simulatorPath, index));
            }
            return await ImplementCmdAsync(string.Format("{0}dnconsole copy --name {1} --from {2}", simulatorPath, name, index));
        }
        /// <summary>
        /// 删除模拟器
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <returns></returns>
        public async Task<string> RemoveSimulatorAsync(int index)
        {

            return await ImplementCmdAsync(string.Format("{0}dnconsole remove --index {1}", simulatorPath, index));
        }
        /// <summary>
        /// 模拟器一件排序,在模拟器自带的多开器配置排序规则
        /// </summary>
        /// <returns></returns>
        public async Task<string> SortWndAsync()
        {
            return await ImplementCmdAsync(string.Format("{0}dnconsole sortWnd", simulatorPath));
        }
        /// <summary>
        /// 获取界面控件类名
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <returns></returns>
        public async Task<string> GetAndroidClassAsync(int index)
        {
            try
            {
                string ret = await ImplementCmdAsync(string.Format("{0}ld -s {1} dumpsys window | findstr mCurrentFocus", simulatorPath, index));
                return Regex.Match(ret, @"Window{[^>]*? [^>]*? (?<text>[^<]*)}").Groups["text"].Value;
            }
            catch { return ""; }

        }
        /// <summary>
        /// 查看已创建的模拟器状态 PS：依次是：索引,标题,顶层窗口句柄,绑定窗口句柄,是否进入android,进程PID,VBox进程PID
        /// </summary>
        /// <returns>返回List集合</returns>
        public async Task<List<string>> ListSimulatorAsync()
        {
            List<string> list = new List<string>();
            foreach (Match item in Regex.Matches(await ImplementCmdAsync(string.Format("{0}dnconsole list2", simulatorPath)), @"\d,[^>]*?,[0-9]{1,8},[0-9]{1,8},\d,[^>]*?[0-9]{1,8},[^>]*?[0-9]{1,8}"))
            {
                list.Add(item.Value);
            }
            return list;
        }
        /// <summary>
        /// 查看已创建的模拟器状态 PS：依次是：索引,标题,顶层窗口句柄,绑定窗口句柄,是否进入android,进程PID,VBox进程PID
        /// </summary>
        /// <returns>返回List集合</returns>
        public async Task<string> List2Async()
        {
            string str = "";
            foreach (Match item in Regex.Matches(await ImplementCmdAsync(string.Format("{0}dnconsole list2", simulatorPath)), @"\d,[^>]*?,[0-9]{1,8},[0-9]{1,8},\d,[^>]*?[0-9]{1,8},[^>]*?[0-9]{1,8}"))
            {
                str += item;
            }
            return str;
        }
        /// <summary>
        /// 设置模拟器分辨率
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <param name="value">分辨率w,h,dpi</param>
        /// <returns></returns>
        public async Task<string> SetResolutionAsync(int index, string value)
        {
            return await ImplementCmdAsync(string.Format("{0}dnconsole modify --index {1} --resolution {2}",
                simulatorPath,
                index,
                value));
        }
        /// <summary>
        /// 还原模拟器备份
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <param name="path">备份文件路径包括文件名和扩展名</param>
        /// <returns></returns>
        public async Task<string> ResToreAsync(int index, string path)
        {
            return await ImplementCmdAsync(string.Format("{0}dnconsole restore --index {1} --file {2}",
                simulatorPath,
                index,
                path));
        }
        /// <summary>
        /// 模拟器备份
        /// </summary>
        /// <param name="index">模拟器序号</param>
        /// <param name="path">备份文件路径包括文件名和扩展名</param>
        /// <returns></returns>
        public async Task<string> BackUpAsync(int index, string path)
        {
            return await ImplementCmdAsync(string.Format("{0}dnconsole backup --index {1} --file {2}",
                simulatorPath,
                index,
                path));
        }
        /// <summary>
        /// 执行CMD窗口信息
        /// </summary>
        /// <param name="value">模拟器命令</param>
        /// <returns>执行后的信息</returns>
        private async Task<string> ImplementCmdAsync(string value)
        {
            await slimlock.WaitAsync();
            Process p = new();
            //设置要启动的应用程序
            p.StartInfo.FileName = "cmd.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            //输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p.StartInfo.CreateNoWindow = true;
            //启动程序
            p.Start();
            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(value);
            p.StandardInput.WriteLine("exit");
            p.StandardInput.AutoFlush = true;
            //获取输出信息
            string strOuput = await p.StandardOutput.ReadToEndAsync();
            //等待程序执行完退出进程
            await p.WaitForExitAsync();
            p.Close();
            slimlock.Release();
            return strOuput;
        }
    }
}
