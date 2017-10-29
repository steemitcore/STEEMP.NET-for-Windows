using Newtonsoft.Json;

namespace STEEM.Operations.Post
{
    /// <summary>
    /// Vote up post
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class UpVoteOperation : VoteOperation
    {
        public UpVoteOperation(string voter, string author, string permlink)
            : base(voter, author, permlink, 10000)
        {
        }
    }
}