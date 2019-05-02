using Newtonsoft.Json;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using WinMLPlayground.Core.Helpers;

namespace WinMLPlayground.ViewModels.Base
{
    public abstract class ClassificationVMBase<TModel, TInput, TOutput, TInputData>
        : ViewModelBase, IClassificationViewModel
    {
        readonly string ModelPath;
        readonly string LabelsPath;

        TModel Model;
        TInput Input;

        protected Dictionary<int, string> Labels { get; private set; }

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

        private bool _IsLoading;

        public bool IsLoading
        {
            get { return _IsLoading; }
            set { _IsLoading = value; RaisePropertyChanged(nameof(_IsLoading)); }
        }

        public ClassificationVMBase(string modelPath, string labelsPath)
        {
            ModelPath = modelPath;
            LabelsPath = labelsPath;
        }

        private async Task DoPredictionAsync()
        {
            try
            {
                IsLoading = true;
                await LoadAsync();
                await BindAsync();
                await EvaluateAsync();
            }
            catch (Exception)
            {
            }
            finally
            {
                IsLoading = false;
            }
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
                       await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///{ModelPath}"));
                Model = await GetModelFromStreamAsync(modelFile as IRandomAccessStreamReference);
            }
        }

        protected abstract Task<TModel> GetModelFromStreamAsync(IRandomAccessStreamReference stream);

        private async Task LoadLabelsAsync()
        {
            if (Labels == null)
            {
                var json = await File.ReadAllTextAsync(LabelsPath);
                Labels = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
            }
        }

        private async Task BindAsync()
        {
            using (IRandomAccessStream stream = await SelectedFile.OpenAsync(FileAccessMode.Read))
            {
                var data = await PreProcessAsync(stream);
                Input = CreateInput(data);
            }
        }

        protected abstract TInput CreateInput(TInputData data);

        protected abstract Task<TInputData> PreProcessAsync(IRandomAccessStream stream);

        private async Task EvaluateAsync()
        {
            Result.Clear();

            var ModelOutput = await EvaluateAsync(Model, Input);

            var resultVector = GetResultVector(ModelOutput);

            var result = PostProcessResultVector(resultVector);

            var top = result.OrderByDescending(ms => ms.probability).Take(5).ToList();

            top.ForEach(i => Result.Add($"{Labels[i.index]} - {i.probability:P2}"));
        }

        protected virtual IEnumerable<(int index, float probability)> PostProcessResultVector(IReadOnlyList<float> resultVector)
            => MLHelper.GetSoftMaxResult(resultVector);

        protected abstract Task<TOutput> EvaluateAsync(TModel model, TInput input);

        protected abstract IReadOnlyList<float> GetResultVector(TOutput output);
    }
}
