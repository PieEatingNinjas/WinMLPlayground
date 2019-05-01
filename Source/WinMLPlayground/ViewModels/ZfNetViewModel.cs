using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using Windows.Storage.Streams;
using WinMLPlayground.ViewModels.Base;
using WinMLPlayground.WinMLExtensions;

namespace WinMLPlayground.ViewModels
{
    public class ZfNetViewModel
        : ClassificationVMBase<ZfNetModel, ZfNetInput, ZfNetOutput, ImageFeatureValue>
    {
        const string MODEL_PATH = "Assets/ZfNet/zfnet512-1.2.onnx";
        const string LABELS_PATH = "Assets/ZfNet/Labels.json";

        public ZfNetViewModel()
            : base(MODEL_PATH, LABELS_PATH)
        { }

        protected override ZfNetInput CreateInput(ImageFeatureValue data)
            => new ZfNetInput
            {
                gpu_00data_0 = data
            };

        protected override Task<ZfNetOutput> EvaluateAsync(ZfNetModel model, ZfNetInput input)
            => model.EvaluateAsync(input);

        protected override Task<ZfNetModel> GetModelFromStreamAsync(IRandomAccessStreamReference stream)
            => ZfNetModel.CreateFromStreamAsync(stream);

        protected override IReadOnlyList<float> GetResultVector(ZfNetOutput output)
            => output.gpu_00softmax_1.GetAsVectorView();

        protected override Task<ImageFeatureValue> PreProcessAsync(IRandomAccessStream stream)
            => stream.GetAsImageFeatureValue();
    }
}
