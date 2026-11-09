using System;
using System.IO;

namespace IPXOvernightProcessor.Actions
{
    public sealed class ConsoleSilencer : IDisposable
    {
        private readonly TextWriter _originalOut;
        private readonly TextWriter _originalErr;

        public ConsoleSilencer()
        {
            _originalOut = Console.Out;
            _originalErr = Console.Error;            

            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }

        public void Dispose()
        {
            Console.SetOut(_originalOut);
            Console.SetError(_originalErr);
        }
    }
}