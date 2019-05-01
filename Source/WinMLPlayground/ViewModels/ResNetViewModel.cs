using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Storage.Streams;
using WinMLPlayground.ViewModels.Base;
using WinMLPlayground.WinMLExtensions;

namespace WinMLPlayground.ViewModels
{
    public class ResNetViewModel
        : ClassificationVMBase<ResNet50Model, ResNet50Input, ResNet50Output, TensorFloat>
    {
        const string MODEL_PATH = "Assets/ResNet/resnet50-1.2.onnx";
        const string LABELS_PATH = "Assets/ResNet/Labels.json";

        public ResNetViewModel()
            : base(MODEL_PATH, LABELS_PATH)
        { }

        protected override Task<ResNet50Model> GetModelFromStreamAsync(IRandomAccessStreamReference stream)
            => ResNet50Model.CreateFromStreamAsync(stream);

        protected override ResNet50Input CreateInput(TensorFloat data)
            => new ResNet50Input
            {
                gpu_00data_0 = data
            };

        protected override Task<TensorFloat> PreProcessAsync(IRandomAccessStream stream)
            => stream.NormalizeImageForImageNetAsync(224);

        protected override Task<ResNet50Output> EvaluateAsync(ResNet50Model model, ResNet50Input input)
            => model.EvaluateAsync(input);

        protected override IReadOnlyList<float> GetResultVector(ResNet50Output output)
            => output.gpu_00softmax_1.GetAsVectorView();
    }
}
