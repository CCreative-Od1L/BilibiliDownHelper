using BvDownkr.src.Implement;
using BvDownkr.src.Models;
using BvDownkr.src.Services;
using BvDownkr.src.Utils;
using BvDownkr.src.Views;
using Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BvDownkr.src.ViewModels {
    public class SearchPageVM : NotificationObject {
        private SearchPageModel _model;
        public SearchPageVM() {
            _model = new();
        }
        public string TextContent {
            get => _model.TextContent;
            set {
                _model.TextContent = value;
                RaisePropertyChanged(nameof(TextContent));
            }
        }
        public ICommand TryToSearch => new ReplyCommand<object>(
            (_) => {
                CoreManager.logger.Info(TextContent);
                // VideoService.INSTANCE.ParseUserInput(TextContent);
            },
            true);
            
    }
}
