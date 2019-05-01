# WinMLPlayground
UWP Project for trying out WinML with different ONNX models

## Models
* SqueezeNet
* ResNet
* AlexNet (ONNX model not included as is exceeds GitHub's max file size. You can download it from [Azure AI Gallery]( https://gallery.azure.ai/Model/AlexNet-1-2-2))
* ShuffleNet
* ZfNet (ONNX model not included as is exceeds GitHub's max file size. You can download it from [Azure AI Gallery]( https://gallery.azure.ai/Model/ZFNet-1-2-2))
* DenseNet

## DISCLAIMER
This sample project demonstrates how to do image classification using different ONNX models on WinML. Currently this is still work in progress, there is no guarantee that any of this code (like preprocessing) is 100% as it should be. Over time this will get validated and will be updated in this repository. That being said, all of the models integrated in this sample give correct results for the sample images and some manual tests (although the probability might be lower than expected).
