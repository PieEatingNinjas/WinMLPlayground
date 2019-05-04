using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Storage.Streams;
using WinMLPlayground.ViewModels.Base;
using WinMLPlayground.WinMLExtensions;

namespace WinMLPlayground.ViewModels
{
    public class ShuffleNetViewModel
        : ClassificationVMBase<ShuffleNetModel, ShuffleNetInput, ShuffleNetOutput, TensorFloat>
    {
        const string MODEL_PATH = "Assets/ShuffleNet/shufflenet-1.2.onnx";
        const string LABELS_PATH = "Assets/ShuffleNet/Labels.json";

        public ShuffleNetViewModel()
            : base(MODEL_PATH, LABELS_PATH)
        { }

        protected override Task<ShuffleNetModel> GetModelFromStreamAsync(IRandomAccessStreamReference stream)
            => ShuffleNetModel.CreateFromStreamAsync(stream);

        protected override ShuffleNetInput CreateInput(TensorFloat data)
            => new ShuffleNetInput
            {
                gpu_00data_0 = data
            };

        protected override Task<TensorFloat> PreProcessAsync(IRandomAccessStream stream)
            => stream.GetAsDefaultImageNetNormalizedTensorFloat();

        protected override Task<ShuffleNetOutput> EvaluateAsync(ShuffleNetModel model, ShuffleNetInput input)
            => model.EvaluateAsync(input);

        protected override IReadOnlyList<float> GetResultVector(ShuffleNetOutput output)
            => output.gpu_00softmax_1.GetAsVectorView();
    }
}
