using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pandorum.Authentication
{
    public interface IPartnerInfo
    {
        string Username { get; }
        string Password { get; }
        string DeviceId { get; }
        string DecryptPassword { get; }
        string EncryptPassword { get; }
    }
}
