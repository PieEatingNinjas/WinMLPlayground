// This file was automatically generated by VS extension Windows Machine Learning Code Generator v3
// from model file alexnet-1.2.onnx
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
    
    public sealed class AlexNetInput
    {
        public TensorFloat data_0; // shape(1,3,224,224)
    }
    
    public sealed class AlexNetOutput
    {
        public TensorFloat prob_1; // shape(1,1000)
    }
    
    public sealed class AlexNetModel
    {
        private LearningModel model;
        private LearningModelSession session;
        private LearningModelBinding binding;
        public static async Task<AlexNetModel> CreateFromStreamAsync(IRandomAccessStreamReference stream)
        {
            AlexNetModel learningModel = new AlexNetModel();
            learningModel.model = await LearningModel.LoadFromStreamAsync(stream);
            learningModel.session = new LearningModelSession(learningModel.model);
            learningModel.binding = new LearningModelBinding(learningModel.session);
            return learningModel;
        }
        public async Task<AlexNetOutput> EvaluateAsync(AlexNetInput input)
        {
            binding.Bind("data_0", input.data_0);
            var result = await session.EvaluateAsync(binding, "0");
            var output = new AlexNetOutput();
            output.prob_1 = result.Outputs["prob_1"] as TensorFloat;
            return output;
        }
    }
}

