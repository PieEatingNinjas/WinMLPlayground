using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Storage.Streams;
using WinMLPlayground.ViewModels.Base;
using WinMLPlayground.WinMLExtensions;

namespace WinMLPlayground.ViewModels
{
    public class AlexNetViewModel
        : ClassificationVMBase<AlexNetModel, AlexNetInput, AlexNetOutput, TensorFloat>
    {
        const string MODEL_PATH = "Assets/AlexNet/alexnet-1.2.onnx";
        const string LABELS_PATH = "Assets/AlexNet/Labels.json";

        public AlexNetViewModel()
            : base(MODEL_PATH, LABELS_PATH)
        { }

        protected override AlexNetInput CreateInput(TensorFloat data)
            => new AlexNetInput
            {
                data_0 = data
            };

        protected override Task<AlexNetOutput> EvaluateAsync(AlexNetModel model, AlexNetInput input)
            => model.EvaluateAsync(input);

        protected override Task<AlexNetModel> GetModelFromStreamAsync(IRandomAccessStreamReference stream)
            => AlexNetModel.CreateFromStreamAsync(stream);

        protected override IReadOnlyList<float> GetResultVector(AlexNetOutput output)
            => output.prob_1.GetAsVectorView();

        protected override Task<TensorFloat> PreProcessAsync(IRandomAccessStream stream)
            => stream.GetAsTensorFloat(224);
    }
}
