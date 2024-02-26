using BvDownkr.src.Implement;
using BvDownkr.src.Models;
using BvDownkr.src.Services;
using BvDownkr.src.Utils;
using Core.BilibiliApi.Video.Model;
using Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BvDownkr.src.ViewModels {
    public class VideoDescPageVM : NotificationObject {
        private readonly VideoDescModel _model;
        public VideoDescPageVM() {
            _model = new();

            VideoService.INSTANCE.AddAfterGetVBInfoAction(UpateUIAction);
        }
        public ImageSource? VideoCover {
            get => _model.VideoCover;
            set {
                _model.VideoCover = value;
                RaisePropertyChanged(nameof(VideoCover));
            }
        }
        public ImageSource? OwnerAvatar {
            get => _model.OwnerAvatar;
            set {
                _model.OwnerAvatar = value;
                RaisePropertyChanged(nameof(OwnerAvatar));
            }
        }
        public string Title {
            get => _model.Title;
            set {
                _model.Title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }
        public string Desc {
            get => _model.Desc;
            set {
                _model.Desc = value;
                RaisePropertyChanged(nameof(Desc));
            }
        }
        public string Owner {
            get => _model.Owner;
            set {
                _model.Owner = value;
                RaisePropertyChanged(nameof(Owner));
            }
        }
        private void UpateUIAction(VideoBaseInfoData videoBaseInfoData) {
            Title = videoBaseInfoData.Title;
            Desc = videoBaseInfoData.GetVideoDesc();
            Owner = videoBaseInfoData.Owner.Name;
            Task updateImageTask = new(async () => {
                var (isGetCoverSucc, coverRawData) = await WebClient.RequestData(videoBaseInfoData.CoverUrl);
                var (isGetOwnerAvatarSucc, ownerAvatarRawData) = await WebClient.RequestData(videoBaseInfoData.Owner.Face);

                PageManager.VideoDescPage.Dispatcher.Invoke(() => {
                    if (isGetCoverSucc) {
                        VideoCover = UIMethod.GetBitmapSource(coverRawData);
                    }
                    if (isGetOwnerAvatarSucc) {
                        OwnerAvatar = UIMethod.GetBitmapSource(ownerAvatarRawData);
                    }
                });
            });
            updateImageTask.Start();
        }
    }
}
