using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Storage.Streams;
using WinMLPlayground.ViewModels.Base;
using WinMLPlayground.WinMLExtensions;

namespace WinMLPlayground.ViewModels
{
    public class DenseNetViewModel
        : ClassificationVMBase<DenseNetModel, DenseNetInput, DenseNetOutput, TensorFloat>
    {
        const string MODEL_PATH = "Assets/DenseNet/densenet121-1.2.onnx";
        const string LABELS_PATH = "Assets/DenseNet/Labels.json";

        public DenseNetViewModel()
            : base (MODEL_PATH, LABELS_PATH)
        { }

        protected override DenseNetInput CreateInput(TensorFloat data)
            => new DenseNetInput
            {
                data_0 = data
            };

        protected override Task<DenseNetOutput> EvaluateAsync(DenseNetModel model, DenseNetInput input)
            => model.EvaluateAsync(input);

        protected override Task<DenseNetModel> GetModelFromStreamAsync(IRandomAccessStreamReference stream)
            => DenseNetModel.CreateFromStreamAsync(stream);

        protected override IReadOnlyList<float> GetResultVector(DenseNetOutput output)
            => output.fc6_1.GetAsVectorView();

        protected override Task<TensorFloat> PreProcessAsync(IRandomAccessStream stream)
            => stream.GetAsDefaultImageNetNormalizedTensorFloat();
    }
}
