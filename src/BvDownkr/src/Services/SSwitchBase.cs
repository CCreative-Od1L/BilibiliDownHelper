using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BvDownkr.src.Services {
    public class SSwitchBase {
        private readonly List<string> configure = [];
        private readonly List<string> installed = [];
        private bool isDone = false;
        private readonly Action? Done;
        public SSwitchBase(string[] classNames, Action action) {
            LoadConfigure(classNames);
            Done = action;
        }
        protected void LoadConfigure(string[] classNames) {
            configure.AddRange(classNames);
        }
        public void OpenSwitch(string className) {
            if (isDone) { return; }

            if (configure.Contains(className) && !installed.Contains(className)) {
                installed.Add(className);
            }
            if (installed.Count == configure.Count) {
                Done?.Invoke();
                isDone = true;
            }
        }
    }
}
