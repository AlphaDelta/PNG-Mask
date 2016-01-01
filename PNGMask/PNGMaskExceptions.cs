using System;
using System.Collections.Generic;
using System.Text;

namespace PNGMask
{
    public class PNGMaskException : Exception { public PNGMaskException(string msg) : base(msg) { } };
    public class NotEnoughSpaceException : PNGMaskException { public NotEnoughSpaceException(string msg) : base(msg) { } };
    public class InvalidPasswordException : PNGMaskException { public InvalidPasswordException(string msg = "Invalid password") : base(msg) { } };
}
