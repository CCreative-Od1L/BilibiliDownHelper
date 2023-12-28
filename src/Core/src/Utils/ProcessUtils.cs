using System.Diagnostics;

namespace Core.Utils {
    public class ProcessUtils {
        public static void StartProcess(Process process) {
            process.Start();    
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }
        public static bool KillProcess(Process process) {
            try {
                process.WaitForExit(10 * 10000);
                process.Kill();
            } catch (Exception) {
                return false;
            }
            return true;
        }
        public static async Task<bool> AsyncKillProcess(Process process) {
            await Task.Run(() => {
                process.WaitForExit(30 * 1000);
                process.Kill();
            });
            return true;
        }
    }
}