using BvDownkr.src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BvDownkr.src.ViewModels {
    public class QRCodeLoginVM {
        private QRCodeLoginModel _model;
        public QRCodeLoginVM() {
            _model = new();
        }

        public QRCodeLoginVM(QRCodeLoginModel model) {
            _model = model;
        }

    }
}
