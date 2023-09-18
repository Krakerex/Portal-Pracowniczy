using Microsoft.AspNetCore.Authorization;

namespace krzysztofb.Authorization
{
    public class OwnershipRequirement : IAuthorizationRequirement
    {
        public int Id { get; }

        public OwnershipRequirement(int id)
        {
            Id = id;
        }
    }
}