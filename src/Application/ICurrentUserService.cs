﻿namespace EGID.Application
{
    public interface ICurrentUserService
    {
        /// <summary>
        ///     Current user id.
        /// </summary>
        string UserId { get; }

        /// <summary>
        ///     Returns true if the current request is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }
    }
}
