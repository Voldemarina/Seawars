using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace Seawars.Infrastructure.Validation
{
    public static class Validator
    {
        public static bool NotNullElementsExist(params string[] elements) => elements.ToList().Exists(x => string.IsNullOrWhiteSpace(x));
        public static bool NotNullElementsExist(params int[] elements) => elements.ToList().Exists(x => x > 1);
        public static bool DoesTheGameExist(int[] ExitstingId, int CurrentId) => ExitstingId.Contains(CurrentId);
    }
}
