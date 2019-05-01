// This file was automatically generated by VS extension Windows Machine Learning Code Generator v3
// from model file shufflenet-1.2.onnx
// Warning: This file may get overwritten if you add add an onnx file with the same name
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.AI.MachineLearning;
namespace WinMLPlayground
{
    
    public sealed class ShuffleNetInput
    {
        public TensorFloat gpu_00data_0; // shape(1,3,224,224)
    }
    
    public sealed class ShuffleNetOutput
    {
        public TensorFloat gpu_00softmax_1; // shape(1,1000)
    }
    
    public sealed class ShuffleNetModel
    {
        private LearningModel model;
        private LearningModelSession session;
        private LearningModelBinding binding;
        public static async Task<ShuffleNetModel> CreateFromStreamAsync(IRandomAccessStreamReference stream)
        {
            ShuffleNetModel learningModel = new ShuffleNetModel();
            learningModel.model = await LearningModel.LoadFromStreamAsync(stream);
            learningModel.session = new LearningModelSession(learningModel.model);
            learningModel.binding = new LearningModelBinding(learningModel.session);
            return learningModel;
        }
        public async Task<ShuffleNetOutput> EvaluateAsync(ShuffleNetInput input)
        {
            binding.Bind("gpu_0/data_0", input.gpu_00data_0);
            var result = await session.EvaluateAsync(binding, "0");
            var output = new ShuffleNetOutput();
            output.gpu_00softmax_1 = result.Outputs["gpu_0/softmax_1"] as TensorFloat;
            return output;
        }
    }
}

