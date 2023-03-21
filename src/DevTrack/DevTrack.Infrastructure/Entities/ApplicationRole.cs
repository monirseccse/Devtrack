﻿using Microsoft.AspNetCore.Identity;

namespace DevTrack.Infrastructure.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
            : base()
        {
        }

        public ApplicationRole(string roleName)
            : base(roleName)
        {
        }
    }
}
