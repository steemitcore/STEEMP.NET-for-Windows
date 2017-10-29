using STEEM.Helpers;
using Newtonsoft.Json;

namespace STEEM.Operations.Post
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseOperation
    {
        [SerializeHelper.MessageOrder(10)]
        public abstract OperationType Type { get; }
        
        public abstract string TypeName { get; }
    }
}