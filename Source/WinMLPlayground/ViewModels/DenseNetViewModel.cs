﻿using Newtonsoft.Json;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using WinMLPlayground.Core.Helpers;

namespace WinMLPlayground.ViewModels
{
    public class DenseNetViewModel : ViewModelBase
    {
        const string MODEL_PATH = "Assets/DenseNet/densenet121-1.2.onnx";
        const string LABELS_PATH = "Assets/DenseNet/Labels.json";

        Dictionary<int, string> Labels;

        DenseNetModel Model;
        DenseNetInput Input;

        private StorageFile selectedFile;
        public StorageFile SelectedFile
        {
            get => selectedFile;
            set
            {
                selectedFile = value;
                RaisePropertyChanged(nameof(SelectedFile));
                if (value != null)
                    DoPredictionAsync();
            }
        }

        public ObservableCollection<string> Result
        {
            get;
            set;
        } = new ObservableCollection<string>();

        private async Task DoPredictionAsync()
        {
            await LoadAsync();
            await BindAsync();
            await EvaluateAsync();
        }

        private async Task LoadAsync()
        {
            var loadModel = LoadModelAsync();
            var loadLabels = LoadLabelsAsync();
            await Task.WhenAll(loadModel, loadLabels);
        }

        private async Task LoadModelAsync()
        {
            if (Model == null)
            {
                StorageFile modelFile =
                       await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///{MODEL_PATH}"));
                Model = await DenseNetModel.CreateFromStreamAsync(modelFile as IRandomAccessStreamReference);
            }
        }

        private async Task LoadLabelsAsync()
        {
            if (Labels == null)
            {
                var json = await File.ReadAllTextAsync(LABELS_PATH);
                Labels = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
            }
        }

        private async Task BindAsync()
        {
            Input = new DenseNetInput();
            using (IRandomAccessStream stream = await SelectedFile.OpenAsync(FileAccessMode.Read))
            {
                Input.data_0 = await PreProcessAsync(stream);
            }
        }

        private async Task<TensorFloat> PreProcessAsync(IRandomAccessStream stream)
        {
            //ToDo: CenterCrop 224
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied);
            WriteableBitmap innerBitmap = new WriteableBitmap(softwareBitmap.PixelWidth, softwareBitmap.PixelHeight);
            softwareBitmap.CopyToBuffer(innerBitmap.PixelBuffer);

            int imgSize = 224;
            int channelSize = imgSize * imgSize;
            //Normalize to 1, 3, 224, 224
            //(mean =[0.485, 0.456, 0.406], std =[0.229, 0.224, 0.225])

            using (var context = innerBitmap.GetBitmapContext())
            {
                int[] src = context.Pixels;

                var normalized = new float[imgSize * imgSize * 3];

                for (var x = 0; x < imgSize; x++)
                {
                    for (var y = 0; y < imgSize; y++)
                    {
                        var color = innerBitmap.GetPixel(y, x);
                        float r, g, b;

                        r = color.R;
                        g = color.G;
                        b = color.B;

                        r = r / 255f;
                        g = g / 255f;
                        b = b / 255f;

                        r = (r - 0.485f) / 0.229f;
                        g = (g - 0.456f) / 0.224f;
                        b = (b - 0.406f) / 0.225f;

                        var indexChannelR = (x * imgSize) + y;
                        var indexChannelG = indexChannelR + channelSize;
                        var indexChannelB = indexChannelG + channelSize;

                        normalized[indexChannelR] = r;
                        normalized[indexChannelG] = g;
                        normalized[indexChannelB] = b;
                    }
                }
                return TensorFloat.CreateFromArray(new List<long>() { 1, 3, imgSize, imgSize }, normalized);
            }
        }

        private async Task EvaluateAsync()
        {
            Result.Clear();

            var ModelOutput = await Model.EvaluateAsync(Input);

            var resultVector = ModelOutput.fc6_1.GetAsVectorView().ToList();

            var result = MLHelper.GetSoftMaxResult(resultVector);

            var top = result.OrderByDescending(ms => ms.probability).Take(5).ToList();

            top.ForEach(i => Result.Add($"{Labels[i.index]} - {i.probability:P2}"));
        }
    }
}