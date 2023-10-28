using ONGAnimaisAPI.Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Application.Extensions
{
    public static class ListExtension
    {
        public static bool AnyEquals<TVO>(this IList<TVO> valueObjects) where TVO : ValueObject
        {
            for (int i = 0; i < valueObjects.Count(); i++)
            {
                for (int j = i + 1; j < valueObjects.Count(); j++)
                {
                    if (valueObjects[i].Equals(valueObjects[j]))
                        return true;
                }
            }

            return false;
        }
    }
}
