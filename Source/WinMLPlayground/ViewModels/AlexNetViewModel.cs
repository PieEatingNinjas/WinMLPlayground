﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prism.Windows.Mvvm;
using Windows.AI.MachineLearning;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;
using WinMLPlayground.Core.Helpers;

namespace WinMLPlayground.ViewModels
{
    public class AlexNetViewModel : ViewModelBase
    {
        const string MODEL_PATH = "Assets/AlexNet/alexnet-1.2.onnx";
        const string LABELS_PATH = "Assets/AlexNet/Labels.json";

        Dictionary<int, string> Labels;

        AlexNetModel Model;
        AlexNetInput Input;

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
                Model = await AlexNetModel.CreateFromStreamAsync(modelFile as IRandomAccessStreamReference);
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
            Input = new AlexNetInput();
            using (IRandomAccessStream stream = await SelectedFile.OpenAsync(FileAccessMode.Read))
            {
                Input.data_0 = await PreProcessAsync(stream);
            }
        }

        private async Task<ImageFeatureValue> PreProcessAsync(IRandomAccessStream stream)
        {
            SoftwareBitmap softwareBitmap;
            // Create the decoder from the stream 
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

            // Get the SoftwareBitmap representation of the file in BGRA8 format
            softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

            VideoFrame videoFrame = VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);

            return ImageFeatureValue.CreateFromVideoFrame(videoFrame);
        }

        private async Task EvaluateAsync()
        {
            Result.Clear();

            var ModelOutput = await Model.EvaluateAsync(Input);

            var resultVector = ModelOutput.prob_1.GetAsVectorView();

            var result = MLHelper.GetSoftMaxResult(resultVector);

            var top = result.OrderByDescending(ms => ms.probability).Take(5).ToList();

            top.ForEach(i => Result.Add($"{Labels[i.index]} - {i.probability:P2}"));
        }
    }
}
