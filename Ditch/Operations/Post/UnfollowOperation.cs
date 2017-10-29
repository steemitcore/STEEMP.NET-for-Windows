﻿using STEEM.Operations.Enums;

namespace STEEM.Operations.Post
{
    /// <summary>
    /// Unfollow some author
    /// </summary>
    public class UnfollowOperation : UnFollowOperation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="author"></param>
        /// <param name="requiredPostingAuths"></param>
        /// <returns></returns>
        public UnfollowOperation(string login, string author, params string[] requiredPostingAuths)
            : base(login, author, new FollowType[0], requiredPostingAuths) { }
    }
}