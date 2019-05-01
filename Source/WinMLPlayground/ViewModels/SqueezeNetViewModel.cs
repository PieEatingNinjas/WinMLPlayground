using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Storage.Streams;
using WinMLPlayground.ViewModels.Base;
using WinMLPlayground.WinMLExtensions;

namespace WinMLPlayground.ViewModels
{
    public class SqueezeNetViewModel
        : ClassificationVMBase<SqueezeNetModel, SqueezeNetInput, SqueezeNetOutput, ImageFeatureValue>
    {
        const string MODEL_PATH = "Assets/SqueezeNet/squeezenet1.2.onnx";
        const string LABELS_PATH = "Assets/SqueezeNet/Labels.json";

        public SqueezeNetViewModel()
            : base(MODEL_PATH, LABELS_PATH)
        { }

        protected override Task<SqueezeNetModel> GetModelFromStreamAsync(IRandomAccessStreamReference stream)
            => SqueezeNetModel.CreateFromStreamAsync(stream);

        protected override SqueezeNetInput CreateInput(ImageFeatureValue data)
            => new SqueezeNetInput
            {
                data_0 = data
            };

        protected override Task<ImageFeatureValue> PreProcessAsync(IRandomAccessStream stream)
            => stream.GetAsImageFeatureValue();

        protected override Task<SqueezeNetOutput> EvaluateAsync(SqueezeNetModel model, SqueezeNetInput input)
            => model.EvaluateAsync(input);

        protected override IReadOnlyList<float> GetResultVector(SqueezeNetOutput output)
            => output.softmaxout_1.GetAsVectorView();
    }
}
