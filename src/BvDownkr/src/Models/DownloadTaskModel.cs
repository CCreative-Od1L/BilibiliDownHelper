using BvDownkr.src.Entries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BvDownkr.src.Models
{
    public class DownloadTaskModel {
        public ObservableCollection<BilibiliDownloadTaskEntry> TasksEntries { get; set; } = [];
    }
}
