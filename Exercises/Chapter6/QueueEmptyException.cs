using System;

namespace Homework
{
    public class QueueEmptyException : Exception
    {
        public QueueEmptyException(string message) : base(message) { }
    }
}