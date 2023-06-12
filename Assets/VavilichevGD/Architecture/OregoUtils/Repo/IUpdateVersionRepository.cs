using System.Collections;
using Orego.Util;

namespace Orego.Util
{
    public interface IUpdateVersionRepository
    {
        IEnumerator OnUpdateVersion(Reference<bool> isUpdated);
    }
}