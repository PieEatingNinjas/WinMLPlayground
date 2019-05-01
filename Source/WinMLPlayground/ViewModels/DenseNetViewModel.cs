using Newtonsoft.Json;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Storage;
using Windows.Storage.Streams;
using WinMLPlayground.Core.Helpers;
using WinMLPlayground.WinMLExtensions;

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
            return await stream.NormalizeImageForImageNetAsync(224);
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
