using System;
using System.Collections.Generic;
using System.Linq;

namespace WinMLPlayground.Core.Helpers
{
    public static class MLHelper
    {
        public static IEnumerable<(int index, float probability)> GetSoftMaxResult(IReadOnlyList<float> resultVector)
        {
            var z_exp = resultVector.Select(r => Math.Exp(r));

            var sum_z_exp = z_exp.Sum();

            var softmax = z_exp.Select(i => i / sum_z_exp);

            for (int i = 0; i < softmax.Count(); i++)
            {
                yield return (index: i, probability: resultVector.ElementAt(i));
            }
        }
    }
}
